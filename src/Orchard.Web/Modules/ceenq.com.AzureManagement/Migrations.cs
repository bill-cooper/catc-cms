using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.AzureManagement
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("AzureSettingsRecord",
                 table => table
                     .ContentPartRecord()
                     .Column<string>("SqlServerDomain")
                     .Column<string>("SqlServerPort")
                     .Column<string>("Base64EncodedCertificate", column => column.Unlimited())
                     .Column<string>("SubscriptionId")
                     .Column<string>("SqlServerDatabaseName")
                     .Column<string>("SqlServerDatabaseUsername")
                     .Column<string>("SqlServerDatabasePassword")
                     .Column<string>("SqlServerDatabaseCollation")
                     .Column<string>("SqlServerDatabaseEdition")
                     .Column<int>("SqlServerDatabaseMaximumSizeInGb")
                     .Column<int>("SqlServerDatabaseConnectionTimeout")
                     .Column<string>("DataCenterRegion")
                     .Column<string>("RoutingServerImageName")
                     .Column<string>("RoutingServerAdminUserName")
                     .Column<string>("RoutingServerAdminPassword")
                     
                 );



            ContentDefinitionManager.AlterPartDefinition("AzureSettingsPart", part => part
                .WithDescription("This part provides Azure specific setting for an account")
                );

            ContentDefinitionManager.AlterTypeDefinition("Account", type => type
                .WithPart("AzureSettingsPart"));

            return 1;
        }

    }
}