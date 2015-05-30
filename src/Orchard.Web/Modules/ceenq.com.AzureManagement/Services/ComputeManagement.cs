using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ceenq.com.AzureManagement.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Models;
using ceenq.com.Core.Tenants;
using Hyak.Common;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Network;
using Microsoft.WindowsAzure.Management.Network.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Notify;
using Component = Orchard.Component;

namespace ceenq.com.AzureManagement.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Compute.DefaultServerManagement")]
    public class ComputeManagement : Component, IServerManagement
    {
        private readonly IAssetStorageCredentialsProvider _assetStorageCredentials;
        private readonly ITenantContextProvider _tenantContextProvider;
        private readonly IOrchardServices _orchardServices;
        private readonly IAccountContext _accountContext;
        private readonly INotifier _notifier;
        private readonly Lazy<CertificateCloudCredentials> _cloudCredentials;
        private readonly Lazy<AzureSettingsRecord> _settings;
        public ComputeManagement(IAssetStorageCredentialsProvider assetStorageCredentials, ITenantContextProvider tenantContextProvider, IOrchardServices orchardServices, IAccountContext accountContext, INotifier notifier)
        {
            _assetStorageCredentials = assetStorageCredentials;
            _tenantContextProvider = tenantContextProvider;
            _orchardServices = orchardServices;
            _accountContext = accountContext;
            _notifier = notifier;
            _settings = new Lazy<AzureSettingsRecord>(() =>
            {
                var coreSettings = _orchardServices.WorkContext.CurrentSite.As<CoreSettingsPart>();
                var settings = new AzureSettingsRecord();
                using (var coreContext = _tenantContextProvider.ContextFor(coreSettings.ParentTenant))
                {
                    var accountManager = coreContext.Resolve<IAccountManager>();
                    var currentAccount = accountManager.GetAccountByName(_accountContext.Account);
                    var parentSettings = currentAccount.As<AzureSettingsPart>();
                    settings.DataCenterRegion = parentSettings.DataCenterRegion;
                    settings.SubscriptionId = parentSettings.SubscriptionId;
                    settings.Base64EncodedCertificate = parentSettings.Base64EncodedCertificate;
                    settings.RoutingServerImageName = parentSettings.RoutingServerImageName;
                    settings.RoutingServerAdminUserName = parentSettings.RoutingServerAdminUserName;
                    settings.RoutingServerAdminPassword = parentSettings.RoutingServerAdminPassword;
                }
                return settings;
            });
            _cloudCredentials = new Lazy<CertificateCloudCredentials>(() => new CertificateCloudCredentials(_settings.Value.SubscriptionId, new X509Certificate2(Convert.FromBase64String(_settings.Value.Base64EncodedCertificate))));
        }

        public ServerInfo Create(ServerOperationParameters parameters)
        {
            var networkClient = new NetworkManagementClient(_cloudCredentials.Value);
            var computeClient = new ComputeManagementClient(_cloudCredentials.Value);

            // create hosted service to host VM.
            if (!computeClient.HostedServices.CheckNameAvailability(parameters.Name).IsAvailable)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be created.  A cloud service with the name {0} already exists.", parameters.Name));
                throw new OrchardFatalException(T("A cloud service with the name {0} already exists.  VM creation failed", parameters.Name));
            }


            //Create Reserved IP Address
            var ipName = string.Format("{0}-rip", parameters.Name);
            try
            {
                networkClient.ReservedIPs.Create(new NetworkReservedIPCreateParameters
                {
                    Location = _settings.Value.DataCenterRegion,
                    Label = EncodeToBase64(ipName),
                    Name = ipName
                });
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be created.  Creation of Reserved IP Address failed.", parameters.Name));
                throw new OrchardFatalException(T("Creation of Reserved IP Address failed for {0}.  VM creation failed", parameters.Name), e);
            }

            //Create Cloud Service to host VM Deployment
            try
            {
                computeClient.HostedServices.Create(new HostedServiceCreateParameters
                {
                    ServiceName = parameters.Name,
                    Location = _settings.Value.DataCenterRegion,
                    Label = EncodeToBase64(parameters.Name),
                });
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be created.  Failure during the creation of cloud service {0}.", parameters.Name));
                throw new OrchardFatalException(T("Failure during the creation of cloud service {0}.  VM creation failed", parameters.Name), e);
            }

            //Create VM Deployment
            try
            {
                computeClient.VirtualMachines.CreateDeployment(parameters.Name, new VirtualMachineCreateDeploymentParameters
                {
                    Name = parameters.Name,
                    Label = parameters.Name,
                    Roles = new List<Role> {  
                            new Role {
                            RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                            RoleName = parameters.Name,
                            Label = parameters.Name,
                            RoleSize = VirtualMachineRoleSize.ExtraSmall,
                            ConfigurationSets = new List<ConfigurationSet>(){
                                    new ConfigurationSet
                                    {
                                        ConfigurationSetType = ConfigurationSetTypes.LinuxProvisioningConfiguration,
                                        HostName = parameters.Name,
                                        AdminUserName = _settings.Value.RoutingServerAdminUserName,
                                        AdminPassword = _settings.Value.RoutingServerAdminPassword,
                                        UserName = _settings.Value.RoutingServerAdminUserName,
                                        UserPassword = _settings.Value.RoutingServerAdminPassword,
                                        DisableSshPasswordAuthentication = false
                                    },
                                    new ConfigurationSet
                                    {
                                        ConfigurationSetType = ConfigurationSetTypes.NetworkConfiguration,
                                        InputEndpoints = new BindingList<InputEndpoint>
                                            {
                                                new InputEndpoint {LocalPort = 22, Port = 22, Name = "SSH", Protocol = "tcp"},
                                                new InputEndpoint {LocalPort = 80, Port = 80, Name = "HTTP", Protocol = "tcp"},
                                                new InputEndpoint {LocalPort = 443, Port = 443, Name = "HTTPS", Protocol = "tcp"}
                                            }
                                    }
                                },
                                OSVirtualHardDisk = new OSVirtualHardDisk
                                {
                                    MediaLink = GetVhdUri(string.Format("{0}.blob.core.windows.net/vhds", _assetStorageCredentials.Username), parameters.Name, parameters.Name),
                                    SourceImageName = GetSourceImageNameByFamliyName(computeClient.VirtualMachineOSImages, _settings.Value.RoutingServerImageName)
                                },
                                ProvisionGuestAgent = true
                            } 
                        },
                    DeploymentSlot = DeploymentSlot.Production,
                    ReservedIPName = ipName
                });
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be created.  Failure during the VM creation for {0}.", parameters.Name));
                throw new OrchardFatalException(T("Failure during the VM creation for {0}.", parameters.Name), e);
            }


            //TODO: add give up for this
            HostedServiceGetDetailedResponse hostService;
            do
            {
                System.Threading.Thread.Sleep(2000);
                hostService = computeClient.HostedServices.GetDetailed(parameters.Name);
                Console.WriteLine("Status: {0}", hostService.Deployments[0].Status);
            } while (hostService.Deployments[0].Status != DeploymentStatus.Running);

            var ipaddress = hostService.Deployments[0].VirtualIPAddresses[0].Address;
            var uri = hostService.Deployments[0].Uri;

            return new ServerInfo()
            {
                IpAddress = ipaddress,
                Host = uri.DnsSafeHost,
                AdminUsername = _settings.Value.RoutingServerAdminUserName,
                AdminPassword = _settings.Value.RoutingServerAdminPassword
            };

        }

        public void Delete(ServerOperationParameters parameters)
        {
            var computeClient = new ComputeManagementClient(_cloudCredentials.Value);

            try
            {
                var deploy = computeClient.Deployments.GetByName(parameters.Name, parameters.Name);
                if (deploy == null)
                {
                    _notifier.Warning(T("There is no deployed routing server named {0}.", parameters.Name));
                    return;
                }
                var ipName = deploy.ReservedIPName;
                computeClient.Deployments.DeleteByName(parameters.Name, parameters.Name, true);
                _notifier.Information(T("VM deployment deleted for {0}.", parameters.Name));
                computeClient.HostedServices.Delete(parameters.Name);
                _notifier.Information(T("Cloud Service deleted for {0}.", parameters.Name));

                if (!string.IsNullOrWhiteSpace(ipName))
                {
                    var networkClient = new NetworkManagementClient(_cloudCredentials.Value);
                    networkClient.ReservedIPs.Delete(ipName);
                    _notifier.Information(T("Reserved IP Address {0} has been deleted for {1}.", ipName, parameters.Name));
                }
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be deleted.  Failure during the deletion of {0}.", parameters.Name));
                throw new OrchardFatalException(T("Failure during the deletion of routing server {0}.", parameters.Name), e);
            }
        }

        public void PowerOn(ServerOperationParameters parameters)
        {
            if (IsPoweredOn(parameters))
            {
                _notifier.Warning(T("The Routing Server with name {0} is already powered on.", parameters.Name));
                return;
            }
            try
            {
                var computeClient = new ComputeManagementClient(_cloudCredentials.Value);
                computeClient.VirtualMachines.Start(parameters.Name, parameters.Name, parameters.Name);
                _notifier.Information(T("The Routing Server with name {0} has been powered on.", parameters.Name));
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be powered on.", parameters.Name));
                throw new OrchardFatalException(T("Failure while attempting to power on VM for {0}.", parameters.Name), e);
            }
        }

        public void PowerOff(ServerOperationParameters parameters)
        {
            if (!IsPoweredOn(parameters))
            {
                _notifier.Warning(T("The Routing Server with name {0} is already powered off.", parameters.Name));
                return;
            }
            try
            {
                var computeClient = new ComputeManagementClient(_cloudCredentials.Value);
                computeClient.VirtualMachines.Shutdown(parameters.Name, parameters.Name, parameters.Name, new VirtualMachineShutdownParameters(){PostShutdownAction = PostShutdownAction.StoppedDeallocated});
                _notifier.Information(T("The Routing Server with name {0} has been powered off.", parameters.Name));
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be powered off.", parameters.Name));
                throw new OrchardFatalException(T("Failure while attempting to power off VM for {0}.", parameters.Name), e);
            }
        }

        public void Reboot(ServerOperationParameters parameters)
        {
            try
            {
                var computeClient = new ComputeManagementClient(_cloudCredentials.Value);
                computeClient.VirtualMachines.Restart(parameters.Name, parameters.Name, parameters.Name);
                _notifier.Information(T("The Routing Server with name {0} has been restarted.", parameters.Name));
            }
            catch (Exception e)
            {
                _notifier.Error(T("The Routing Server with name {0} could not be restared.", parameters.Name));
                throw new OrchardFatalException(T("Failure while attempting to restart VM for {0}.", parameters.Name), e);
            }
        }

        public bool IsPoweredOn(ServerOperationParameters parameters)
        {
            try
            {
                var computeClient = new ComputeManagementClient(_cloudCredentials.Value);
                var cloudService = computeClient.HostedServices.GetDetailed(parameters.Name);
                var powerState = cloudService.Deployments[0].RoleInstances[0].PowerState;
                return powerState == RoleInstancePowerState.Started;
            }
            catch (Exception e)
            {
                _notifier.Error(T("The power state of Routing Server with name {0} could not be determined.", parameters.Name));
                throw new OrchardFatalException(T("Failure while attempting to retrieve power state of VM for {0}.", parameters.Name), e);
            }
        }

        private string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes
                = Encoding.ASCII.GetBytes(toEncode);
            string returnValue
                = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        private string GetSourceImageNameByFamliyName(IVirtualMachineOSImageOperations operation, string imageFamliyName)
        {
            var disk = operation.List().FirstOrDefault(o => o.ImageFamily == imageFamliyName);
            if (disk != null)
            {
                return disk.Name;
            }
            throw new Exception(string.Format("Can't find {0} OS image in current subscription", imageFamliyName));
        }

        private Uri GetVhdUri(string blobcontainerAddress, string cloudServiceName, string vmName, bool cacheDisk = false, bool https = false)
        {
            var now = DateTime.UtcNow;
            var dateString = now.Year + "-" + now.Month + "-" + now.Day;

            var address = string.Format("http{0}://{1}/{2}-{3}-{4}-{5}-650.vhd", https ? "s" : string.Empty, blobcontainerAddress, cloudServiceName, vmName, cacheDisk ? "-CacheDisk" : string.Empty, dateString);
            return new Uri(address);
        }

    }
}