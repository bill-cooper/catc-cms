using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using ceenq.org.Resource.Models;
using ceenq.org.Resource.ViewModels;
using ceenq.org.Services;

namespace ceenq.org.Resource.Drivers
{
    [UsedImplicitly]
    public class ResourcePartDriver : ContentPartDriver<ResourcePart>
    {
        private readonly IRepository<ResourcePart> _resourcePartRepository;
        private readonly IEsvBibleService _esvBibleService;
        public ResourcePartDriver(
            IRepository<ResourcePart> resourcePartRepository,
            IEsvBibleService esvBibleService
            )
        {
            _resourcePartRepository = resourcePartRepository;
            _esvBibleService = esvBibleService;
        }

        protected override string Prefix
        {
            get
            {
                return "ResourcePart";
            }
        }
        protected override DriverResult Display(ResourcePart part, string displayType, dynamic shapeHelper) {
            string passage = "";
            if (displayType.StartsWith("Detail"))
                passage = _esvBibleService.PassageQuery(part.CorrespondingTexts);
            return ContentShape("Parts_Resource", () => shapeHelper.Parts_Resource(Model: part, Passage: passage));
        }
        protected override DriverResult Editor(ResourcePart part, dynamic shapeHelper)
        {
            var viewModel = new ResourcePartViewModel { DeliveredUtc = part.DeliveredUtc, CorrespondingTexts = part.CorrespondingTexts };
            return ContentShape("Part_ResourcePart_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/ResourcePart", Model: viewModel, Prefix: Prefix));
        }
        protected override DriverResult Editor(ResourcePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}