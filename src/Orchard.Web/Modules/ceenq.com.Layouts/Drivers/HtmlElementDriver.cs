using System.Linq;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;
using CsQuery;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Elements;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.ViewModels;
using Orchard.Localization;

namespace ceenq.com.Layouts.Drivers {

    [OrchardSuppressDependency("Orchard.Layouts.Drivers.HtmlElementDriver")]
    public class HtmlElementDriver : ElementDriver<Html> {

        private readonly IAssetManager _assetManager;
        private readonly IAccountContext _accountContext;

        public HtmlElementDriver(IAssetManager assetManager, IAccountContext accountContext)
        {
            _assetManager = assetManager;
            _accountContext = accountContext;
        }

        protected override EditorResult OnBuildEditor(Html element, ElementEditorContext context) {
            var viewModel = new HtmlEditorViewModel {
                Text = element.Content
            };
            var editor = context.ShapeFactory.EditorTemplate(TemplateName: "Elements.Html", Model: viewModel);

            if (context.Updater != null) {
                context.Updater.TryUpdateModel(viewModel, context.Prefix, null, null);
                element.Content = viewModel.Text;
            }
            
            return Editor(context, editor);
        }

        protected override void OnDisplaying(Html element, ElementDisplayContext context)
        {
            if (context.DisplayType == "Design" && !string.IsNullOrWhiteSpace(context.ElementShape.Element.Content))
            {
                CQ dom = context.ElementShape.Element.Content;
                if(dom == null) return;
                var assetImages = dom["img[asset-ref]"];
                if(!assetImages.Any()) return;
                foreach (var image in assetImages)
                {
                    var assetReference = image.Attributes["asset-ref"];
                    image.Attributes["src"] = string.Format("/api/files/{0}",assetReference);
                }
                context.ElementShape.Element.Content = dom.Render();
            }
        }

        protected override void OnLayoutSaving(Html element, ElementSavingContext context)
        {
            CQ dom = element.Content;
            if (dom == null) return;
            CQ assetImages = dom["img[asset-ref]"];
            if (!assetImages.Any()) return;
            foreach (var image in assetImages)
            {
                var assetReference = image.Attributes["asset-ref"];
                var asset = _assetManager.GetFileById(assetReference);
                var path = asset.Path.Substring(_accountContext.AssetPath.Length - 1);
                image.Attributes["src"] = path;
            }
            element.Content = dom.Render();
        }

    }
}