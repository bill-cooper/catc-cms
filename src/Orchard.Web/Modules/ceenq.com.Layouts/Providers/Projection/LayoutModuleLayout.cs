using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Models;
using Orchard.Layouts.Services;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Layout;
using Orchard.Projections.Services;

namespace ceenq.com.Layouts.Providers.Projection
{
    public class LayoutModuleLayout : ILayoutProvider
    {
        private readonly IContentManager _contentManager;
        private readonly ILayoutManager _layoutManager;
        private readonly IElementDisplay _elementDisplay;
        protected dynamic Shape { get; set; }

        public LayoutModuleLayout(IShapeFactory shapeFactory, IContentManager contentManager, ILayoutManager layoutManager, IElementDisplay elementDisplay)
        {
            _contentManager = contentManager;
            _layoutManager = layoutManager;
            _elementDisplay = elementDisplay;
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeLayoutContext describe)
        {
            describe.For("layoutmodulelayout", T("Layout"), T("Projection Layout Provider"))
                .Element("layout", T("Projection Layout Provider"), T("Applies a layout from the Orchard.Layouts module."),
                    DisplayLayout,
                    RenderLayout,
                    "layoutmodulelayout"
                );
        }

        public LocalizedString DisplayLayout(LayoutContext context)
        {

            return T("layoutmodulelayout");

        }

        public dynamic RenderLayout(LayoutContext context, IEnumerable<LayoutComponentResult> layoutComponentResults)
        {
            var template = _layoutManager.GetTemplates().FirstOrDefault(t => t.As<TitlePart>().Title.ToLower() == "page-item");

            string listId = context.State.ListId;

            var shapes = layoutComponentResults.Select(x =>
            {
                var part = x.ContentItem.As<LayoutPart>();
                var slate = template.LayoutData;
                //replace fields with that of the current types matching fields
                slate = slate.Replace("\"typeName\": \"Layout.", string.Format("\"typeName\": \"{0}.", x.ContentItem.ContentType));
                part.LayoutData = slate;

                var elements = _layoutManager.LoadElements(part);
                var layoutRoot = _elementDisplay.DisplayElements(elements, part, context.LayoutRecord.DisplayType);
                return layoutRoot;
            });

            return Shape.List(Id: listId, Items: shapes, Tag: "-", Classes: new string[] { }, ItemClasses: new string[] { });
        }
    }
}