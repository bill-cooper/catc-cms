using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ceenq.com.FeaturedItems
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("FeaturedItem", builder => builder
                .WithField("FeaturedContentItem", field => field.OfType("ContentPickerField")
                        .WithSetting("DisplayName", "Featured Content Item")
                        .WithSetting("ContentPickerFieldSettings.Required", "True")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "False")
                        .WithSetting("ContentPickerFieldSettings.ShowContentTab", "True")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "Series, Sermon, Page, BlogPost"))
                .WithField("FeaturedSliderImage", field => field.OfType("MediaLibraryPickerField")
                        .WithSetting("DisplayName", "Featured Slider Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", "False")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "False")));

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("FeaturedItem", builder => builder
                .WithPart("CommonPart", part => part
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False")
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                .WithPart("BodyPart", part => part
                            .WithSetting("BodyTypePartSettings.Flavor", "html"))
                .WithPart("TitlePart")
                .WithPart("FeaturedItem")
                .WithPart("ContainablePart")
                .Creatable()
                .Draftable()
                .DisplayedAs("Featured Item"));

            ContentDefinitionManager.AlterTypeDefinition("FeaturedItemList", builder => builder
                .WithPart("CommonPart", part => part
                    .WithSetting("DateEditorSettings.ShowDateEditor", "False")
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                .WithPart("ContainerPart", part => part
                    .WithSetting("ContainerTypePartSettings.PageSizeDefault", "100")
                    .WithSetting("ContainerTypePartSettings.PaginatedDefault", "False"))
                .DisplayedAs("Featured Item List"));

            return 2;
        }

    }
}