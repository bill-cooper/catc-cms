using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.Accounts
{
    public class AccountsDataMigration : DataMigrationImpl
    {

        public int Create()
        {

            SchemaBuilder.CreateTable("AccountRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Name")
                    .Column<string>("DisplayName")
                    .Column<string>("Domain")
                );

 
            return 1;
        }

        public int UpdateFrom1()
        {


            ContentDefinitionManager.AlterTypeDefinition("Account", type => type
                .WithPart("AccountPart")
                .WithPart("CommonPart", p => p
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                    .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                .Listable(false)
                .Creatable(false)
                );

            return 2;
        }


    }
}