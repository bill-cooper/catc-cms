using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.AwsManagement
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("AwsSettingsRecord",
                 table => table
                    .ContentPartRecord()
                    .Column<string>("HostedZoneId")
                    .Column<string>("AccessKey")
                    .Column<string>("SecretAccessKey")
                 );



            ContentDefinitionManager.AlterPartDefinition("AwsSettingsPart", part => part
                .WithDescription("This part provides Aws specific setting for an account"));

            ContentDefinitionManager.AlterTypeDefinition("Account", type => type
                .WithPart("AwsSettingsPart"));

            return 1;
        }

    }
}