using System.Collections.Generic;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using Orchard;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Services
{
    public class ApplicationDnsNamesService : IApplicationDnsNamesService
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ApplicationDnsNamesService(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public List<string> DnsNames(IApplication application)
        {
            var names = new List<string>();
            if (!application.SuppressDefaultEndpoint)
            {
                names.Add(DefaultDnsName(application));
            }

            if (!string.IsNullOrWhiteSpace(application.Domain))
            {
                names.Add(application.Domain);
            }
            return names;
        }

        public string PrimaryDnsName(IApplication application)
        {
            if (string.IsNullOrWhiteSpace(application.Domain))
            {
                return DefaultDnsName(application);
            }
 
            return application.Domain;
        }

        public string DefaultDnsName(IApplication application)
        {
            var applicationSettings = _workContextAccessor.GetContext().CurrentSite.As<CoreSettingsPart>();
            var defaultHost = applicationSettings.AccountDomain.ToLower().Trim(new[] { '/' });

            return string.Format("{0}.{1}", application.Name.ToLower(), defaultHost);
        }
    }
}