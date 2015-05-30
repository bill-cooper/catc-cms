using ceenq.com.Theme.Admin.Models;
using Orchard;
using Orchard.ContentManagement;

namespace ceenq.com.Theme.Admin.Services
{
    public interface IAdminThemeSettingsService : IDependency
    {
        AdminThemeSettingsPart GetServiceSettings();
    }
    public class AdminThemeSettingsService : IAdminThemeSettingsService
    {
        private readonly IWorkContextAccessor _wca;
        public AdminThemeSettingsService(IWorkContextAccessor wca)
        {
            _wca = wca;
        }
        public AdminThemeSettingsPart GetServiceSettings()
        {
            return _wca.GetContext().CurrentSite.ContentItem.As<AdminThemeSettingsPart>();
        }
    }
}