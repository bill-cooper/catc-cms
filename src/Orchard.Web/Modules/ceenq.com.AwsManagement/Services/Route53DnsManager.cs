using System;
using Amazon.Route53;
using Amazon.Route53.Model;
using ceenq.com.AwsManagement.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Infrastructure.Dns;
using ceenq.com.Core.Models;
using ceenq.com.Core.Tenants;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Logging;

namespace ceenq.com.AwsManagement.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Dns.DefaultDnsManager")]
    public class Route53DnsManager :Component, IDnsManager
    {
        private readonly ITenantContextProvider _tenantContextProvider;
        private readonly IOrchardServices _orchardServices;
        private readonly IAccountContext _accountContext;
        public Route53DnsManager(ITenantContextProvider tenantContextProvider, IOrchardServices orchardServices, IAccountContext accountContext)
        {
            _tenantContextProvider = tenantContextProvider;
            _orchardServices = orchardServices;
            _accountContext = accountContext;
        }

        public void CreateARecord(string host, string ipAddress)
        {
            var coreSettings = _orchardServices.WorkContext.CurrentSite.As<CoreSettingsPart>();
            var settings = new AwsSettingsRecord();
            using (var coreContext = _tenantContextProvider.ContextFor(coreSettings.ParentTenant))
            {
                var accountManager = coreContext.Resolve<IAccountManager>();
                var currentAccount = accountManager.GetAccountByName(_accountContext.Account);
                var parentSettings = currentAccount.As<AwsSettingsPart>();
                settings.HostedZoneId = parentSettings.HostedZoneId;
                settings.AccessKey = parentSettings.AccessKey;
                settings.SecretAccessKey = parentSettings.SecretAccessKey;
            }

            CreateARecord(host, ipAddress, settings.AccessKey, settings.SecretAccessKey, settings.HostedZoneId);
        }

        public void CreateARecord(string host, string ipAddress, string accessKey, string secretAccessKey, string hostedZone)
        {
            var client = new AmazonRoute53Client(accessKey, secretAccessKey, Amazon.RegionEndpoint.USEast1);

            var batch = new ChangeBatch()
            {
                Comment = "adding an A record for a client application",
                Changes =
                {
                     new Change()
                     {
                         Action = ChangeAction.CREATE,
                         ResourceRecordSet = new ResourceRecordSet()
                         {
                             Name = host,
                             Type = "A",
                             TTL = 60,
                             ResourceRecords =
                             {
                                 new ResourceRecord() {Value = ipAddress}
                             }
                         }
                     }
                }
            };
            try
            {
                client.ChangeResourceRecordSets(new ChangeResourceRecordSetsRequest() { ChangeBatch = batch, HostedZoneId = hostedZone });
            }
            catch (Exception ex)
            {
                Logger.Error(ex,string.Format("A failure occurred while attempting to create a Route53 A Record Entry for host='{0}' on ip='{1}'",host,ipAddress));
            }

        }
        public void CreateCName(string host, string toAddress, string accessKey, string secretAccessKey, string hostedZone)
        {
            var client = new AmazonRoute53Client(accessKey, secretAccessKey, Amazon.RegionEndpoint.USEast1);

            var batch = new ChangeBatch()
            {
                Comment = "adding a cname record for a client application",
                Changes =
                {
                     new Change()
                     {
                         Action = ChangeAction.CREATE,
                         ResourceRecordSet = new ResourceRecordSet()
                         {
                             Name = host,
                             Type = "CNAME",
                             TTL = 60,
                             ResourceRecords =
                             {
                                 new ResourceRecord() {Value = toAddress}
                             }
                         }
                     }
                }
            };
            try
            {
                client.ChangeResourceRecordSets(new ChangeResourceRecordSetsRequest() { ChangeBatch = batch, HostedZoneId = hostedZone });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("A failure occurred while attempting to create a Route53 CNAME Entry for host='{0}' on toaddress='{1}'", host, toAddress));
            }

        }
    }
}