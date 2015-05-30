using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace ceenq.com.Common
{
    public class Migrations: DataMigrationImpl
    {
        public int Create() {

            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg
                    .WithPart("UserProfilePart")
                );


            SchemaBuilder.CreateTable("UserProfilePartRecord", table =>
                table
                    .ContentPartRecord()
                    .Column<string>("Name")
                );

            return 1;
        }
    }
}