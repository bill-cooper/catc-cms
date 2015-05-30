using Orchard.Data.Migration;

namespace ceenq.com.Theme.Admin
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(
                "AdminThemeSettingsPartRecord",
                table => table
                             .ContentPartRecord()
                             .Column<string>("LogoUrl")
                             .Column<string>("Brand")
                             .Column<string>("Footer")
                             .Column<string>("ProductCssUrl")
                );

            return 1;
        }
    }
}