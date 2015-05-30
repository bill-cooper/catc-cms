using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.Apps {
    public class AccountsDataMigration : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("ApplicationRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Name")
                    .Column<string>("AuthenticationRedirect")
                    .Column<string>("AccountVerification")
                    .Column<string>("ResetPassword")
                    .Column<string>("Domain")
                    .Column<bool>("SuppressDefaultEndpoint")
                    .Column<bool>("TransportSecurity")
                );

            SchemaBuilder.CreateTable("RouteRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("RequestPattern")
                    .Column<string>("PassTo")
                    .Column<string>("Rules")
                    .Column<int>("RouteOrder")
                    .Column<bool>("RequireAuthentication")
                    .Column<bool>("CachingEnabled")
                    .Column<int>("ApplicationId")
                );

            SchemaBuilder.CreateTable("UserApplicationsPartRecord",
                table => table.ContentPartRecord()
                );

            SchemaBuilder.CreateTable("UserApplicationJoinRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("UserApplicationsPartRecord_Id")
                    .Column<int>("ApplicationRecord_Id")
                );

            ContentDefinitionManager.AlterTypeDefinition("Route", type => type
                .WithPart("RoutePart")
                .WithPart("CommonPart", p => p
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                    .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                .Listable(false)
                .Creatable(false)
                );

            ContentDefinitionManager.AlterTypeDefinition("Application", type => type
                .WithPart("ApplicationPart")
                .WithPart("CommonPart", p => p
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                    .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                .Listable(false)
                .Creatable(false)
                );


            return 1;
        }
    }
}