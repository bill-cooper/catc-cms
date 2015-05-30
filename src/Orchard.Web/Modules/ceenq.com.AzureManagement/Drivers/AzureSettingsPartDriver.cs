using ceenq.com.AzureManagement.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AzureManagement.Drivers
{
    public class AzureSettingsPartDriver : ContentPartDriver<AzureSettingsPart>
    {
        private readonly IOrchardServices _orchardServices;
        public AzureSettingsPartDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        protected override string Prefix
        {
            get { return "AzureSettingsPart"; }
        }

        protected override DriverResult Editor(AzureSettingsPart part, dynamic shapeHelper)
        {            
            //Only populate defaults if this is the creation of the content item and not an edit
            // this is identified as an Add because version = 0
            if (part.ContentItem.Version == 0)
            {
                var settings = _orchardServices.WorkContext.CurrentSite.As<AzureDefaultSettingsPart>();
                if (settings != null)
                {
                    part.SqlServerDomain = settings.SqlServerDomain;
                    part.SqlServerPort = settings.SqlServerPort;
                    part.Base64EncodedCertificate = settings.Base64EncodedCertificate;
                    part.SubscriptionId = settings.SubscriptionId;
                    part.SqlServerDatabaseName = settings.SqlServerDatabaseName;
                    part.SqlServerDatabaseUsername = settings.SqlServerDatabaseUsername;
                    part.SqlServerDatabasePassword = settings.SqlServerDatabasePassword;
                    part.SqlServerDatabaseCollation = settings.SqlServerDatabaseCollation;
                    part.SqlServerDatabaseEdition = settings.SqlServerDatabaseEdition;
                    part.SqlServerDatabaseMaximumSizeInGb = settings.SqlServerDatabaseMaximumSizeInGb;
                    part.SqlServerDatabaseConnectionTimeout = settings.SqlServerDatabaseConnectionTimeout;
                    part.DataCenterRegion = settings.DataCenterRegion;
                    part.RoutingServerImageName = settings.RoutingServerImageName;
                    part.RoutingServerAdminUserName = settings.RoutingServerAdminUserName;
                    part.RoutingServerAdminPassword = settings.RoutingServerAdminPassword;
                }
            }
            return ContentShape("Parts_Account_AzureSettingsPart",
                () =>
                    shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AzureSettingsPart",
                        Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(AzureSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return ContentShape("Parts_Account_AzureSettingsPart",
            () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts.Account.AzureSettingsPart",
                    Model: part, Prefix: Prefix));
        }
    }
}