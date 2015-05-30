using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Core.Contents.Extensions;

namespace ceenq.com.LiveTiles
{
    public class Migration : DataMigrationImpl
    {
        public int Create()
        {

            SchemaBuilder.CreateTable("DashboardSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("Menu_id")
                );

            return 1;
        }
    }
}