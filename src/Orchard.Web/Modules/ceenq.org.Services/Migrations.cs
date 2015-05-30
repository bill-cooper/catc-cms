using Orchard.Data.Migration;

namespace ceenq.org.Services {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("ESVBibleServiceSettingsPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("EsvBibleServiceKey")
                );

            return 1;
        }
    }
}