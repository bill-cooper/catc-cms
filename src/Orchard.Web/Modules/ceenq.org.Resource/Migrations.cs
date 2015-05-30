using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.MediaLibrary.Fields;
using ceenq.com.Core.Data.Migration;
using ceenq.org.Resource.Models;

namespace ceenq.org.Resource
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition<ResourcePart>();

            ContentDefinitionManager.AlterTypeDefinition("Sermon", builder =>
                builder
                    .WithPart("TitlePart")
                    .WithPart("CommonPart", partbuilder =>
                        partbuilder
                            .WithSetting("DateEditorSettings.ShowDateEditor", "true")
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                            )
                    .WithPart("AutoRoutePart", partbuilder =>
                        partbuilder
                            .WithSetting("AutorouteSettings.PerItemConfiguration", "false")
                            .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                            .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                            .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'sermon route', Pattern: 'sermons/{Content.Slug}', Description: 'sermons/sermon-title'}]")
                            .WithSetting("AutorouteSettings.DefaultPatternIndex", "0")
                    )
                    .WithPart("CommentsPart")
                    .WithPart("ResourcePart")
                    .WithPart("SermonPart")
                    .Creatable()
                );

            ContentDefinitionManager.AlterPartDefinition(typeof(SermonPart).Name, builder =>
                builder.WithField("Sermon Audio File", f => f.OfType(typeof(MediaLibraryPickerField).Name))
                );
  
            SchemaBuilder.CreateFor<ResourcePartRecord>();

            return 1;
        }


    }
}