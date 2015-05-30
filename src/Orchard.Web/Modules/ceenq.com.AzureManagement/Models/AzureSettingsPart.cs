using Orchard.ContentManagement;

namespace ceenq.com.AzureManagement.Models
{
    public class AzureSettingsPart : ContentPart<AzureSettingsRecord>
    {
        public virtual string SqlServerDomain
        {
            get { return Record.SqlServerDomain; }
            set { Record.SqlServerDomain = value; }
        }
        public virtual string SqlServerPort
        {
            get { return Record.SqlServerPort; }
            set { Record.SqlServerPort = value; }
        }
        public virtual string Base64EncodedCertificate
        {
            get { return Record.Base64EncodedCertificate; }
            set { Record.Base64EncodedCertificate = value; }
        }
        public virtual string SubscriptionId
        {
            get { return Record.SubscriptionId; }
            set { Record.SubscriptionId = value; }
        }
        public virtual string SqlServerDatabaseName
        {
            get { return Record.SqlServerDatabaseName; }
            set { Record.SqlServerDatabaseName = value; }
        }
        public virtual string SqlServerDatabaseUsername
        {
            get { return Record.SqlServerDatabaseUsername; }
            set { Record.SqlServerDatabaseUsername = value; }
        }
        public virtual string SqlServerDatabasePassword
        {
            get { return Record.SqlServerDatabasePassword; }
            set { Record.SqlServerDatabasePassword = value; }
        }
        public virtual string SqlServerDatabaseCollation
        {
            get { return Record.SqlServerDatabaseCollation; }
            set { Record.SqlServerDatabaseCollation = value; }
        }
        public virtual string SqlServerDatabaseEdition
        {
            get { return Record.SqlServerDatabaseEdition; }
            set { Record.SqlServerDatabaseEdition = value; }
        }
        public virtual int SqlServerDatabaseMaximumSizeInGb
        {
            get { return Record.SqlServerDatabaseMaximumSizeInGb; }
            set { Record.SqlServerDatabaseMaximumSizeInGb = value; }
        }
        public virtual int SqlServerDatabaseConnectionTimeout
        {
            get { return Record.SqlServerDatabaseConnectionTimeout; }
            set { Record.SqlServerDatabaseConnectionTimeout = value; }
        }
        public virtual string DataCenterRegion
        {
            get { return Record.DataCenterRegion; }
            set { Record.DataCenterRegion = value; }
        }
        public virtual string RoutingServerImageName
        {
            get { return Record.RoutingServerImageName; }
            set { Record.RoutingServerImageName = value; }
        }
        public virtual string RoutingServerAdminUserName
        {
            get { return Record.RoutingServerAdminUserName; }
            set { Record.RoutingServerAdminUserName = value; }
        }
        public virtual string RoutingServerAdminPassword
        {
            get { return Record.RoutingServerAdminPassword; }
            set { Record.RoutingServerAdminPassword = value; }
        }
    }
}