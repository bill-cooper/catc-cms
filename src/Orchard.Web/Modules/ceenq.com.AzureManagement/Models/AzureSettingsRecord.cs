using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ceenq.com.AzureManagement.Models
{
    public class AzureSettingsRecord : ContentPartRecord
    {
        public virtual string SqlServerDomain { get; set; }
        public virtual string SqlServerPort { get; set; }
        public virtual string Base64EncodedCertificate { get; set; }
        public virtual string SubscriptionId { get; set; }
        public virtual string SqlServerDatabaseName { get; set; }
        public virtual string SqlServerDatabaseUsername { get; set; }
        public virtual string SqlServerDatabasePassword { get; set; }
        public virtual string SqlServerDatabaseCollation { get; set; }
        public virtual string SqlServerDatabaseEdition { get; set; }
        public virtual int SqlServerDatabaseMaximumSizeInGb { get; set; }
        public virtual int SqlServerDatabaseConnectionTimeout { get; set; }
        public virtual string DataCenterRegion { get; set; }
        public virtual string RoutingServerImageName { get; set; }
        public virtual string RoutingServerAdminUserName { get; set; }
        public virtual string RoutingServerAdminPassword { get; set; }

    }
}