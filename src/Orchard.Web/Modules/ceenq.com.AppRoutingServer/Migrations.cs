using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.AppRoutingServer
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("ApplicationRoutingServerRecord",
                 table => table
                     .ContentPartRecord()
                     .Column<string>("IpAddress")
                 );



            ContentDefinitionManager.AlterPartDefinition("ApplicationRoutingServerPart", part => part
                .WithDescription("This part provides a routing server association on an application"));

            ContentDefinitionManager.AlterTypeDefinition("Application", type => type
                .WithPart("ApplicationRoutingServerPart"));

            return 1;
        }

    }
}