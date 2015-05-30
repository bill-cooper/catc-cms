using System;
using ceenq.com.AzureManagement.Models;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;

namespace ceenq.com.AzureManagement.Commands
{
    public class AwsSettingsCommands : DefaultOrchardCommandHandler {
        private readonly IOrchardServices _orchardServices;

        public AwsSettingsCommands(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        [OrchardSwitch]
        public string SqlServerDomain { get; set; }
        [OrchardSwitch]
        public string SqlServerPort { get; set; }
        [OrchardSwitch]
        public string Base64EncodedCertificate { get; set; }
        [OrchardSwitch]
        public string SubscriptionId { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseName { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseUsername { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabasePassword { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseCollation { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseEdition { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseMaximumSizeInGb { get; set; }
        [OrchardSwitch]
        public string SqlServerDatabaseConnectionTimeout { get; set; }
        [OrchardSwitch]
        public string DataCenterRegion { get; set; }
        [OrchardSwitch]
        public string RoutingServerImageName { get; set; }
        [OrchardSwitch]
        public string RoutingServerAdminUserName { get; set; }
        [OrchardSwitch]
        public string RoutingServerAdminPassword { get; set; }


        [CommandName("azuresettings")]
        [CommandHelp("azuresettings [/SqlServerDomain:sqlServerDomain] [/SqlServerPort:sqlServerPort] [/Base64EncodedCertificate:base64EncodedCertificate] [/SubscriptionId:subscriptionId] [/SqlServerDatabaseName:sqlServerDatabaseName] [/SqlServerDatabaseUsername:sqlServerDatabaseUsername] [/SqlServerDatabasePassword:sqlServerDatabasePassword] [/SqlServerDatabaseCollation:sqlServerDatabaseCollation] [/SqlServerDatabaseEdition:sqlServerDatabaseEdition] [/SqlServerDatabaseMaximumSizeInGb:sqlServerDatabaseMaximumSizeInGb] [/SqlServerDatabaseConnectionTimeout:sqlServerDatabaseConnectionTimeout] [/DataCenterRegion:dataCenterRegion] [/RoutingServerImageName:routingServerImageName] [/RoutingServerAdminUserName:routingServerAdminUserName] [/RoutingServerAdminPassword:routingServerAdminPassword] \r\n\tPopulate Azure settings.")]
        [OrchardSwitches(",SqlServerDomain,SqlServerPort,Base64EncodedCertificate,SubscriptionId,SqlServerDatabaseName,SqlServerDatabaseUsername,SqlServerDatabasePassword,SqlServerDatabaseCollation,SqlServerDatabaseEdition,SqlServerDatabaseMaximumSizeInGb,SqlServerDatabaseConnectionTimeout,DataCenterRegion,RoutingServerImageName,RoutingServerAdminUserName,RoutingServerAdminPassword")]
        public void AwsSettings()
        {
            
            var settings = _orchardServices.WorkContext.CurrentSite.As<AzureDefaultSettingsPart>();
            if(!string.IsNullOrWhiteSpace(SqlServerDomain))settings.SqlServerDomain = SqlServerDomain;
            if(!string.IsNullOrWhiteSpace(SqlServerPort))settings.SqlServerPort = SqlServerPort;
            if(!string.IsNullOrWhiteSpace(Base64EncodedCertificate))settings.Base64EncodedCertificate = Base64EncodedCertificate;
            if(!string.IsNullOrWhiteSpace(SubscriptionId))settings.SubscriptionId = SubscriptionId;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseName))settings.SqlServerDatabaseName = SqlServerDatabaseName;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseUsername))settings.SqlServerDatabaseUsername = SqlServerDatabaseUsername;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabasePassword))settings.SqlServerDatabasePassword = SqlServerDatabasePassword;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseCollation))settings.SqlServerDatabaseCollation = SqlServerDatabaseCollation;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseEdition))settings.SqlServerDatabaseEdition = SqlServerDatabaseEdition;
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseMaximumSizeInGb))settings.SqlServerDatabaseMaximumSizeInGb = Convert.ToInt32(SqlServerDatabaseMaximumSizeInGb);
            if(!string.IsNullOrWhiteSpace(SqlServerDatabaseConnectionTimeout))settings.SqlServerDatabaseConnectionTimeout = Convert.ToInt32(SqlServerDatabaseConnectionTimeout);
            if(!string.IsNullOrWhiteSpace(DataCenterRegion))settings.DataCenterRegion = DataCenterRegion;
            if(!string.IsNullOrWhiteSpace(RoutingServerImageName))settings.RoutingServerImageName = RoutingServerImageName;
            if(!string.IsNullOrWhiteSpace(RoutingServerAdminUserName))settings.RoutingServerAdminUserName = RoutingServerAdminUserName;
            if(!string.IsNullOrWhiteSpace(RoutingServerAdminPassword))settings.RoutingServerAdminPassword = RoutingServerAdminPassword;

            Context.Output.WriteLine(T("AWS settings have been populated."));
        }
    }
}