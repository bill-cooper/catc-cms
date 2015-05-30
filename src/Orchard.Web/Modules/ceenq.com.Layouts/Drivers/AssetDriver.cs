using ceenq.com.Core.Assets;
using ceenq.com.Layouts.Elements;
using ceenq.com.Layouts.ViewModel;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;

namespace ceenq.com.Layouts.Drivers {
    public class AssetDriver : ElementDriver<Asset>
    {
        private readonly IAssetManager _assetManager;
        public AssetDriver(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        protected override EditorResult OnBuildEditor(Asset element, ElementEditorContext context)
        {
            var viewModel = new AssetEditorViewModel {
                Path = element.Path
            };
            var editor = context.ShapeFactory.EditorTemplate(TemplateName: "Elements.Asset", Model: viewModel);

            if (context.Updater != null) {
                context.Updater.TryUpdateModel(viewModel, context.Prefix, null, null);
                element.Path = viewModel.Path;
            }
            
            return Editor(context, editor);
        }

        protected override void OnDisplaying(Asset element, ElementDisplayContext context)
        {
            if (context.DisplayType == "Design")
            {
                element.Content = string.Format("[ Local Asset : {0} ]", element.Path);
            }
            else
            {
                var file = _assetManager.GetFile(element.Path);
                if (file != null)
                {
                    var encoding = new System.Text.UTF8Encoding(false);
                    var data = file.Read();
                    element.Content = encoding.GetString(data);
                }
            }
        }
    }
}