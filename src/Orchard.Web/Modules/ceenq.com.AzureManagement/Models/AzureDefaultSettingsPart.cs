using Orchard.ContentManagement;

namespace ceenq.com.AzureManagement.Models
{
    public class AzureDefaultSettingsPart : ContentPart
    {
        public  string SqlServerDomain
        {
            get { return this.Retrieve(x => x.SqlServerDomain); }
            set { this.Store(x => x.SqlServerDomain, value); }
        }
        public  string SqlServerPort
        {
            get { return this.Retrieve(x => x.SqlServerPort); }
            set { this.Store(x => x.SqlServerPort, value); }
        }
        public  string Base64EncodedCertificate
        {
            get { return this.Retrieve(x => x.Base64EncodedCertificate); }
            set { this.Store(x => x.Base64EncodedCertificate, value); }
        }
        public  string SubscriptionId
        {
            get { return this.Retrieve(x => x.SubscriptionId); }
            set { this.Store(x => x.SubscriptionId, value); }
        }
        public  string SqlServerDatabaseName
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseName); }
            set { this.Store(x => x.SqlServerDatabaseName, value); }
        }
        public  string SqlServerDatabaseUsername
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseUsername); }
            set { this.Store(x => x.SqlServerDatabaseUsername, value); }
        }
        public  string SqlServerDatabasePassword
        {
            get { return this.Retrieve(x => x.SqlServerDatabasePassword); }
            set { this.Store(x => x.SqlServerDatabasePassword, value); }
        }
        public  string SqlServerDatabaseCollation
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseCollation); }
            set { this.Store(x => x.SqlServerDatabaseCollation, value); }
        }
        public  string SqlServerDatabaseEdition
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseEdition); }
            set { this.Store(x => x.SqlServerDatabaseEdition, value); }
        }
        public  int SqlServerDatabaseMaximumSizeInGb
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseMaximumSizeInGb); }
            set { this.Store(x => x.SqlServerDatabaseMaximumSizeInGb, value); }
        }
        public  int SqlServerDatabaseConnectionTimeout
        {
            get { return this.Retrieve(x => x.SqlServerDatabaseConnectionTimeout); }
            set { this.Store(x => x.SqlServerDatabaseConnectionTimeout, value); }
        }
        public  string DataCenterRegion
        {
            get { return this.Retrieve(x => x.DataCenterRegion); }
            set { this.Store(x => x.DataCenterRegion, value); }
        }
        public  string RoutingServerImageName
        {
            get { return this.Retrieve(x => x.RoutingServerImageName); }
            set { this.Store(x => x.RoutingServerImageName, value); }
        }
        public  string RoutingServerAdminUserName
        {
            get { return this.Retrieve(x => x.RoutingServerAdminUserName); }
            set { this.Store(x => x.RoutingServerAdminUserName, value); }
        }
        public  string RoutingServerAdminPassword
        {
            get { return this.Retrieve(x => x.RoutingServerAdminPassword); }
            set { this.Store(x => x.RoutingServerAdminPassword, value); }
        }
    }
}