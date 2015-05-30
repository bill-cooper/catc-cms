
if(get-module CnqEnvironment){
	Write-Host "Removing CnqEnvironment"
    remove-module CnqEnvironment | Out-Null
}

if(!(get-module CnqEnvironment)){
	Write-Host "Importing CnqEnvironment"
	Import-Module $PSScriptRoot\CnqEnvironment.psm1
}

New-CnqEnvironment