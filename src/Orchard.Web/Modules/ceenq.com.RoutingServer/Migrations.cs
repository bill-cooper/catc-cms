using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.RoutingServer
{
    public class DataMigration : DataMigrationImpl
    {

        public int Create()
        {

            SchemaBuilder.CreateTable("RoutingServerRecord", table => table
                .ContentPartRecord()
                .Column<string>("Name")
                .Column<string>("DnsName")
                .Column<int>("ConnectionPort")
                .Column<string>("Username")
                .Column<string>("Password")
                .Column<string>("IpAddress")
            );


            ContentDefinitionManager.AlterPartDefinition("RoutingServerPart", part => part
                .WithDescription("Routing Server Part."));

            ContentDefinitionManager.AlterTypeDefinition("RoutingServer", type => type
                .WithPart("CommonPart", p => p
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                    .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                .WithPart("RoutingServerPart")
                .DisplayedAs("RoutingServer"));

            return 1;
        }

    }
}