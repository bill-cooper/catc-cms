
Set-Variable -Name websitePhysicalPath -Value "$PSScriptRoot\src\Orchard.Web" -Option Constant

function New-CnqEnvironment{
    [cmdletbinding()]
    param()
    process{

		Import-Module WebAdministration

		$instanceName  = Read-Host "Provide an Name for this core instance"
		$dnsName = $instanceName + ".ncnq.io"
		New-IISSite -HostName $dnsName -PhysicalPath $websitePhysicalPath
		
		$awsDetails = Get-AwsDetails
		$subscription = Set-CurrentSubscription
		$mgmtcert = Get-ManagementCertificate
		
		$datacenter = Set-CurrentDatacenter
		$databaseServerDetails = New-SubscriptionDatabaseServer -Datacenter $datacenter
		$storageAccountDetails = New-SubscriptionStorage -Datacenter $datacenter -InstanceName $instanceName
		
		$instance = New-CoreInstance -InstanceName $instanceName -AwsDetails $awsDetails -AzureSubscription $subscription -DatabaseServerDetails $databaseServerDetails -StorageAccountDetails $storageAccountDetails -Datacenter $datacenter -MgmtCertificate $mgmtcert
		
		
		Write-Host "You local environment is ready to go"
		Start-Process -FilePath ("https://" + $instance.DnsName)
    } 
}

function New-SubscriptionStorage{
	param($Datacenter, $InstanceName)
	Write-Host "Creating a new Storage Account for your environment..."
	Write-Host 
	
	$storageAccountName = "store" + $InstanceName

	New-AzureStorageAccount -StorageAccountName $storageAccountName -Location $Datacenter 
	Write-Host "Storage Account $storageAccountName Created Successfully"
	Write-Host 
	$storageAccountKey = (Get-AzureStorageKey -StorageAccountName $storageAccountName).Primary

	$storageAccountDetails = New-Object PsObject -Property @{
		Location = $datacenter
		StorageAccountName = $storageAccountName
		StorageAccountKey = $storageAccountKey
	}
	return $storageAccountDetails
}
function New-SubscriptionDatabaseServer{
	param($Datacenter)
	Write-Host "Creating a new Sql Database Server for your environment..."
	Write-Host 

	$administratorLogin  = Read-Host "Provide an Administrator Username for the new Sql Server Instance"
	$administratorLoginPassword  = Read-Host "Provide an Administrator Password for the new Sql Server Instance"

	Write-Host 
	Write-Host "Creating Sql Database Server..."
	$databaseServer = New-AzureSqlDatabaseServer -AdministratorLogin $administratorLogin -AdministratorLoginPassword $administratorLoginPassword -Location $Datacenter
	Write-Host ("Sql Database Server " + $databaseServer.ServerName + " Created Successfully")
	$clientFirewallRuleName = "ClientIPAddress_" + [DateTime]::UtcNow
    Write-Host ("Creating client firewall rule '$clientFirewallRuleName' for Sql Database Server " + $databaseServer.ServerName)
	$ipAddress = (Invoke-WebRequest ip.bsd-unix.net).Content.split("`n")[4]
    New-AzureSqlDatabaseServerFirewallRule -ServerName $databaseServer.ServerName -RuleName $clientFirewallRuleName -StartIpAddress $ipAddress -EndIpAddress $ipAddress | Out-Null  
	Write-Host 
	$databaseServerDetails = New-Object PsObject -Property @{
		Location = $datacenter
		Login = $administratorLogin
		Password = $administratorLoginPassword
		DatabaseServer = $databaseServer
	}
	return $databaseServerDetails
}
function Get-ManagementCertificate{
	param()
	$certificate = [Environment]::GetEnvironmentVariable("AzureMgmtCert","User")
	if($certificate -eq $null){
		$certificate  = Read-Host "Provide Base64 Azure Management Certificate. This is in your publish profile"
		[Environment]::SetEnvironmentVariable("AzureMgmtCert",$certificate,"User")
	}


	return $certificate
}
function Get-AwsDetails{
	param()
	$accessKey = [Environment]::GetEnvironmentVariable("AwsAccessKey","User")
	if($accessKey -eq $null){
		$accessKey  = Read-Host "Provide your AWS Access Key"
		[Environment]::SetEnvironmentVariable("AwsAccessKey",$accessKey,"User")
	}
	
	$secretKey = [Environment]::GetEnvironmentVariable("AwsSecretKey","User")
	if($secretKey -eq $null){
		$secretKey  = Read-Host "Provide your AWS Secret Key"
		[Environment]::SetEnvironmentVariable("AwsSecretKey",$secretKey,"User")
	}

	$hostedZoneId = [Environment]::GetEnvironmentVariable("AwsHostedZoneId","User")
	if($hostedZoneId -eq $null){
		$hostedZoneId  = Read-Host "Provide the Route53 Hosted Zone Id to target"
		[Environment]::SetEnvironmentVariable("AwsHostedZoneId",$hostedZoneId,"User")
	}

	

	$awsDetails = New-Object PsObject -Property @{
		AccessKey = $accessKey 
		SecretKey = $secretKey 
		HostedZoneId = $hostedZoneId
	}

	return $awsDetails
}
function Set-CurrentSubscription {
	
    param()
	
	Import-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Azure.psd1" 

	$azureModule = Get-Module Azure
	if($azureModule -eq $null){
		Write-Error "The Azure powershell module is not imported into your powershell session.  Please resolve this problem before continuing"
		Write-Host "Aborting...."
		exit
	}
	$importSubscription = $true
	$subscriptions = $()
	$subscriptions = Get-AzureSubscription
	if($subscriptions.Count -eq 0)
	{
		Write-Host "No subscriptions registered in your Azure powershell settings."
		Write-Host
	}
	else{
		Write-Host "Following subscription(s) found in your Azure powershell settings:"
		Write-Host
		foreach($subscription in $subscriptions){
			Write-Host $subscription.SubscriptionName
		}
		Write-Host
		$doSubscriptionImport = Read-Host 'Add additional subscriptions into your Azure powershell settings. [y/n]'
		if($doSubscriptionImport -ne "y" -and $doSubscriptionImport -ne "n"){
			Write-Error "The input value was not understood."
			Write-Host "Aborting...."
			exit
		} elseif($doSubscriptionImport -eq "n"){
			$importSubscription = $false
		}
	}
	Write-Host
	Write-Host
	if($importSubscription){
		$hasAzureSubscriptionFile = Read-Host 'Do you have an Azure Publish Settings file for the subscription that you are targeting [y/n]'
		if($hasAzureSubscriptionFile -eq "y"){
			Write-Host "Ok, no need to download publish settings."
		} elseif($hasAzureSubscriptionFile -eq "n"){
			Write-Host "Ok, you will be forwarded to an azure publish profile download page via web browser...."
			Start-Sleep -Seconds 4
			Get-AzurePublishSettingsFile
			Write-Host "Publish settings files contain security credentials.  After downloading your publish settings file, you may want to move it to a suitable location."
		} else{
			Write-Error "The input value was not understood."
			Write-Host "Aborting...."
			exit
		}

		$publishSettingsFilePath = Read-Host 'Please provide the full folder path of the publish settings file for the subscription that you are targeting.'

		Write-Host "Importing publish settings file $publishSettingsFilePath"
		try{
			Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFilePath
		}catch{
			Write-Host "Aborting...."
			exit
		}
	}
	#refresh subscriptions collection
	$subscriptions = Get-AzureSubscription

	$subscriptionName = ""

	if($subscriptions.Count -gt 1)
	{
		$subscriptionNames = ""
		foreach($subscription in $subscriptions){
			if($subscriptionNames -ne ""){
				$subscriptionNames += ", "
			}
			$subscriptionNames += $subscription.SubscriptionName
		}
		$subscriptionName = Read-Host "What is the name of the subscription that you are targeting? ($subscriptionNames)"
	}else{
		$subscriptionName = $subscriptions[0].SubscriptionName
	}

	$targetSubscription = Get-AzureSubscription | where {$_.SubscriptionName -eq $subscriptionName}
	if($targetSubscription -eq $null){
			Write-Error "Could not find a subscription named: $targetSubscription"
			Write-Host "Aborting...."
			exit
	}
	Write-Host
	Write-Host ($targetSubscription.SubscriptionName + " will be used as the target azure subscription.")
	Write-Host
	
	Select-AzureSubscription -SubscriptionName $targetSubscription.SubscriptionName

	return $targetSubscription
}
function Set-CurrentDatacenter {
	
    param()
	Write-Host "Azure Datacenter Regions:"
	Get-AzureLocation | foreach-object {write-host $_.Name}
	Write-Host 
	$datacenter  = Read-Host "Which Azure Datacenter you are targeting?"

	Write-Host
	Write-Host ($datacenter + " will be used as the target datacenter when previsioning supporting infrastructure.")
	Write-Host

	return $datacenter
}
function Remove-AzureSubscriptions{
	
    param()
	$doRemoveSubscriptionRegistrations = Read-Host 'Would you like to unregister your azure subscritions from powershell [y/n]'
	if($doRemoveSubscriptionRegistrations -eq "y"){
		$subscriptions = Get-AzureSubscription
		Write-Host "Ok, you will be prompted for confirmation on each individual registed subscription."
		foreach($subscription in $subscriptions){
			Remove-AzureSubscription $subscription.SubscriptionName
		}
	} elseif($doRemoveSubscriptionRegistrations -eq "n"){
		Write-Host "Ok, all azure subscriptions will remained registered in powershell."
	} else{
		Write-Error "The input value was not understood."
		Write-Host "Aborting...."
		exit
	}
}
function New-CoreInstance{
    param($InstanceName, $AwsDetails, $AzureSubscription, $DatabaseServerDetails, $StorageAccountDetails, $Datacenter, $MgmtCertificate)
	
	$appDataDir = join-path $websitePhysicalPath "app_data"

	if(!(Test-Path -Path $appDataDir) ){
        'Creating Add_Data directory at [{0}]' -f $appDataDir | Write-Verbose
        mkdir $appDataDir | Out-Null
    }
	
	if((Get-ChildItem $appDataDir | Measure-Object).Count -gt 0){
		$clearAppDataFolder = Read-Host 'It appears that Orchard has already been initialized.  The app_data folder must be empty for the setup to proceed.  Clear the app_data folder [y/n]'
		if($clearAppDataFolder -eq "y"){
			Write-Host "Ok, clearing app_data folder..."
			Write-Host "running iisreset to ensure no usage locks on this folder."
			invoke-command -scriptblock {iisreset}
			Remove-Item -Recurse -Force $appDataDir
			mkdir $appDataDir | Out-Null

		} elseif($clearAppDataFolder -eq "n"){
			Write-Host "The app_data folder must be empty for the setup to proceed."
			Write-Host "Aborting...."
			exit
		} else{
			Write-Error "The input value was not understood."
			Write-Host "Aborting...."
			exit
		}
	}


	$adminUsername = Read-Host "Provide a core instance administrator username"
	$adminPassword = Read-Host "Provide a core instance administrator password"

	$routingServerAdminUsername = Read-Host "Provide a default Routing Server administrator username"
	$routingServerAdminPassword = Read-Host "Provide a default Routing Server administrator password"


	[string]$dnsName = $InstanceName + ".ncnq.io"
	$ipAddress = (Invoke-WebRequest ip.bsd-unix.net).Content.split("`n")[4]


	Write-Host "Core Instance Name: $InstanceName"
	Write-Host "Dns Name: $dnsName"
	Write-Host "Public IP Address: $ipAddress"

	Write-Host
	Write-Host "Setting up your core instance.  This will take a few seconds...."

	

	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe setup /SiteName:core-system /AdminUsername:'$adminUsername' /AdminPassword:'$adminPassword' /DatabaseProvider:SqlCe /Recipe:coresystem")

	Write-Host "Core instance setup complete...."
	Write-Host

	Write-Host "Initializing Core Settings...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe coresettings /AccountDomain:'$dnsName'")
	Write-Host "Core Settings initialization complete."
	Write-Host

	Write-Host "Initializing Site Settings...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe site setting set baseurl /BaseUrl:'https://$dnsName'")
	Write-Host "Site Settings initialization complete."
	Write-Host

	$accessKey = $AwsDetails.AccessKey
	$secretKey= $AwsDetails.SecretKey
	$hostedZoneId = $AwsDetails.HostedZoneId

	Write-Host "Initializing Aws Settings...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe awssettings /AccessKey:'$accessKey' /SecretAccessKey:'$secretKey' /HostedZoneId:'$hostedZoneId'")
	Write-Host "Aws Settings initialization complete."
	Write-Host

	Write-Host "Create Route53 A Record for core instance...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe route53 createarecord /Host:'$dnsName' /IpAddress:'$ipAddress'")
	Write-Host "Route53 A Record creation complete...."
	Write-Host

	$accountWildcard = '*.' + $dnsName
	Write-Host "Create Route53 CNAME for core instance to provide DNS support for accounts...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe route53 createcname /Host:'$accountWildcard' /ToAddress:'$dnsName'")
	Write-Host "Route53 CNAME creation complete...."
	Write-Host
	
	
	$subscriptionId = $AzureSubscription.SubscriptionId
	$databaseServer = $DatabaseServerDetails.DatabaseServer.ServerName
	$databaseServerLogin = $DatabaseServerDetails.Login
	$databaseServerPassword = $DatabaseServerDetails.Password
	
	Write-Host "Initializing Azure settings...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe azuresettings /SubscriptionId:'$subscriptionId' /SqlServerDatabaseName:'$databaseServer' /SqlServerDatabaseUsername:'$databaseServerLogin' /SqlServerDatabasePassword:'$databaseServerPassword' /RoutingServerAdminUserName:'$routingServerAdminUsername' /RoutingServerAdminPassword:'$routingServerAdminPassword' /DataCenterRegion:'$Datacenter' /Base64EncodedCertificate:'$MgmtCertificate'")
	Write-Host "Azure settings initialization complete."
	Write-Host

	$storageAccountName = $StorageAccountDetails.StorageAccountName
	$storageAccountKey = $StorageAccountDetails.StorageAccountKey 

	Write-Host "Initializing Azure Storage Account Settings...."
	invoke-expression -command ("$websitePhysicalPath\bin\orchard.exe azurestoragesettings /StorageAccount:'$storageAccountName' /StorageKey:'$storageAccountKey'")
	Write-Host "Azure Storage Account settings initialization complete."
	Write-Host

	Write-Host "Core instance setup complete"
	Write-Host
	Write-Host

	$instance = New-Object PsObject -Property @{DnsName = $dnsName}

	return $instance

}
function New-IISSite {
    param($HostName, $PhysicalPath)

	New-AppPool -Name $HostName -RuntimeVersion "v4.0" -PipelineMode "Integrated"

	if (Test-Path "IIS:\Sites\$HostName") {
		Write-Host "Web site $HostName already exists. No action taken." -ForegroundColor Cyan 
	}
	else {

			if(Find-HostEntry -hostname $HostName){
				Write-Host "host file entry $HostName already exists. No action taken." -ForegroundColor Cyan
			}else{
				Write-Host "Creating host entry $HostName"
				Add-HostEntry( "127.0.0.1", $HostName) 
				Write-Host "Host entry $HostName created successfully"
			}

			Write-Host "Creating site $HostName"
			$webSite = New-Website -Name $HostName -PhysicalPath $PhysicalPath -ApplicationPool $ApplicationPool
			Write-Host "$HostName created successfully"

			Write-Host "Creating bindings for $HostName"
			Set-HttpAndHttpsBinding -websiteName $HostName -ip "*"
			Write-Host "Bindings created successfully"
			$webSite.serverAutoStart = $true
			Write-Host "Site: $HostName created successfully"
	}	
}
function New-AppPool($Name, $Identity = "NetworkService", $RuntimeVersion, $PipelineMode) {
	if (Test-Path "IIS:\AppPools\$Name") {
		Write-Host "Application pool $Name already exists. No action taken."-ForegroundColor Cyan 
	} 
	else {
		Write-Host "Creating application pool $Name"
		$appPool = New-WebAppPool -Name $Name
		Write-Host "Application pool $Name created successfully"
		$appPool.managedRuntimeVersion = $RuntimeVersion
		$appPool.managedPipelineMode = $PipelineMode
		$appPool.processModel.identityType = $Identity
		$appPool | Set-Item
	}
}
function Set-HttpAndHttpsBinding($websiteName, $ip){
    Set-HttpBinding $websiteName $ip
    Set-HttpsBinding $websiteName $ip
}
function Set-HttpBinding($websiteName, $ip, $port = 80) { 
	Remove-Bindings -ip $ip -port $port -protocol http
	Write-Host "Creating binding $ip\:$port\:$websiteName for site $websiteName"
    New-WebBinding -Name $websiteName -IP $ip -Port $port -Protocol http
	Write-Host "Binding $ip\:$port\:$websiteName for site $websiteName successfully created"
}
function Set-HttpsBinding($websiteName, $ip, $port = 443) {
	$binding = $ip + ":" + $port + ":"
	Remove-Bindings -ip $ip -port $port -protocol https
	Write-Host "Creating self signed cert for $websiteName"
	New-SelfSignedCert -certcn $websiteName -password "1ma_Ceenq"  -certfilepath ("c:\" + $websiteName + ".pfx")
	Write-Host "self signed cert created successfully"
	$certificate = Get-ChildItem cert:\LocalMachine\MY | Where-Object {$_.Subject -match ("CN=" + $websiteName)}  | select -first 1
	$thumb = $certificate.Thumbprint.ToString()
	Write-Host "Creating binding $ip\:$port\:$websiteName for site $websiteName"
	New-WebBinding -Name $websiteName -IP $ip -Port 443 -Protocol https
	Write-Host "Binding $ip\:$port\:$websiteName for site $websiteName successfully created"
	$binding = Get-WebBinding -Name $websiteName -IPAddress $ip -Port 443 -Protocol https
	$binding.AddSslCertificate($thumb, "MY")
	Write-Host "Cert $thumb added to binding $ip\:$port\:$websiteName for site $websiteName"
}
function Remove-Bindings($ip, $port, $protocol) {
	$bindexpression = $ip + ":" + $port + ":"
	$sites = Get-ChildItem IIS:\Sites
	foreach ($site in $sites) {


		try{
		  # Remove existing binding form site
		  if ($null -ne (Get-WebBinding -Name $site.name | where-object {$_.protocol -eq $protocol})) {
			$RemoveWebBinding = Remove-WebBinding -Name $site.name -Port $port -Protocol $protocol
			Write-Output ("Removed existing binding for site " + $site.name + " port:" + $port+ " protocol:" + $protocol)
		  }
		  # Remove existing binding in SSLBindings store
		  If (Test-Path "IIS:\SslBindings\0.0.0.0!$port") {
			$RemoveSSLBinding = Remove-Item -path "IIS:\SSLBindings\0.0.0.0!$port"
			Write-Output ("Removed existing SSL binding for site " + $site.name + " port:" + $port+ " protocol:" + $protocol)
		  }
		}
		catch{
			Write-Warning ("Unable to remove existing binding for site " + $site.name + " port:" + $port+ " protocol:" + $protocol)
		}



		#$bindings = $site.bindings.Collection
		#foreach ($binding in $bindings) {
		#	if($binding.bindinginformation -like ($bindexpression + "*")){
		#		Write-Host ("Removing binding " + $binding.bindinginformation + " from site " + $site.name )
		#		$hostHeader = $binding.bindinginformation.Replace($bindexpression, "")
		#		if($hostHeader -eq $null -or $hostHeader -eq ""){
		#			Remove-WebBinding -Name $site.name -IPAddress * -Port $port -Protocol $protocol
		#		}else{
		#			Remove-WebBinding -Name $site.name -IPAddress * -Port $port -Protocol $protocol -HostHeader $hostHeader
		#		}
		#		
		#	}
		#}
	}
}
function List-Bindings($ip, $port, $protocol) {
	$bindexpression = $ip + ":" + $port + ":"
	$sites = Get-ChildItem IIS:\Sites
	foreach ($site in $sites) {
		$bindings = $site.bindings.Collection
		foreach ($binding in $bindings) {
			if($binding.bindinginformation -like ($bindexpression + "*")){
				Write-Host ("Removing binding " + $binding.bindinginformation + " from site " + $site.name )
				$hostHeader = $binding.bindinginformation.Replace($bindexpression, "")
				Remove-WebBinding -Name $site.name -IPAddress * -Port $port -Protocol $protocol -HostHeader $hostHeader
			}
		}
	}
}
function New-SelfSignedCert( $certcn, $password, $certfilepath ) {

	#Check if the certificate name was used before 
	$thumbprintA=(dir cert:\localmachine\My -recurse | where {$_.Subject -match "CN=" + $certcn} | Select-Object -Last 1).thumbprint 

	if ($thumbprintA.Length -gt 0) { 
		Write-Warning  "A Cert with cn $certcn already exists.  Using this cert and continuing..."
		$cert=(dir cert:\localmachine\My -recurse | where {$_.Subject -match "CN=" + $certcn} | Select-Object -Last 1) 
		return $thumbprintA
	} else { 
		$thumbprintA = New-SelfSignedCertificate -DnsName $certcn -CertStoreLocation cert:\LocalMachine\My |ForEach-Object{ $_.Thumbprint} 
	} 

	#If generated successfully 
	if ($thumbprintA.Length -gt 0) { 
		$thumbprintB=(dir cert:\localmachine\My -recurse | where {$_.Subject -match "CN=" + $certcn} | Select-Object -Last 1).thumbprint 

		#If new cert installed sucessfully with the same thumbprint 
		if($thumbprintA -eq $thumbprintB ) { 
			Write-Host "$certcn installed into LocalMachine\My successfully with thumprint $thumbprintA"  -ForegroundColor Cyan 
			$mypwd = ConvertTo-SecureString -String $password -force –asplaintext  
			Write-Host "Exporting Certificate as .pfx file" -ForegroundColor Cyan 
			Export-PfxCertificate -FilePath $certfilepath -Cert cert:\localmachine\My\$thumbprintA -Password $mypwd
			Write-Host "Importing Certificate to LocalMachine\Root" -ForegroundColor Cyan 
			Import-PfxCertificate -FilePath $certfilepath -Password $mypwd -CertStoreLocation cert:\LocalMachine\Root -Exportable #exportable could pose a security risk
			return $thumbprintA
		} else { 
			Write-Host "Thumbprint is not the same between new cert and installed cert." -ForegroundColor Cyan 
		} 
	} else { 
		Write-Host "$certcn was not created." -ForegroundColor Cyan 
		return $null
	} 
}
function Find-HostEntry( $hostname ) {
	$fileContent = Get-Content C:\Windows\system32\drivers\etc\hosts
	$containsHostName = $fileContent | %{$_ -match $hostname}
	if($containsHostName -contains $true){
		return $true
	}else{
		return $false
	}
}
function Add-HostEntry( $ip, $hostname) {
	$entry = "`r`n" + $ip + "`t`t" + $hostname
	$entry | Out-File -encoding UTF8 -append C:\Windows\system32\drivers\etc\hosts
}
function Remove-HostEntry( $hostname ) {
	$fileContent = Get-Content C:\Windows\system32\drivers\etc\hosts 
	$newLines = @() 
	foreach ($line in $fileContent){ 
		$bits = [regex]::Split($line, "\t+") 
		if ($bits.count -eq 2) { 
			if ($bits[1] -ne $hostname){
				$newLines += $line 
			} 
		} else { 
			$newLines += $line 
		} 
	} 
	# Write file 
	Clear-Content C:\Windows\system32\drivers\etc\hosts  
	foreach ($line in $newLines){ 
		$line | Out-File -encoding ASCII -append $filename 
	} 
}