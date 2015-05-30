using System.Linq;
using Autofac;
using Orchard;
using Orchard.Environment;
using Orchard.Environment.Configuration;
using Orchard.Logging;

namespace ceenq.com.Core.Tenants
{
    public interface ITenantContextProvider : IDependency
    {
        IWorkContextScope ContextFor(string tenant);
    }

    public class TenantContextProvider :Component, ITenantContextProvider
    {

        private readonly IShellSettingsManager _shellSettingsManager;
        private readonly IOrchardHost _orchardHost;
        public TenantContextProvider(IShellSettingsManager shellSettingsManager, IOrchardHost orchardHost)
        {
            _shellSettingsManager = shellSettingsManager;
            _orchardHost = orchardHost;
        }

        public IWorkContextScope ContextFor(string tenant)
        {
            var tenantShellSettings = _shellSettingsManager.LoadSettings().FirstOrDefault(settings => settings.Name == tenant);
            if (tenantShellSettings == null)
            {
                Logger.Error(string.Format("An attempt was made to create a tenant context for tenant named '{0}', but the ShellSettingsManager does not have settings loaded for this tenant.",tenant));
                throw new OrchardException(T("Tenant with the name '{0}' could not be found", tenant));
            }
            var shellContext = _orchardHost.GetShellContext(tenantShellSettings);
            return shellContext.LifetimeScope.Resolve<IWorkContextAccessor>().CreateWorkContextScope();
        }
    }
}