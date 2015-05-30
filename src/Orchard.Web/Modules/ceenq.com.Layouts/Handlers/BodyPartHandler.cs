using System.Linq;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;
using CsQuery;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;

namespace ceenq.com.Layouts.Handlers
{
    public class BodyPartHandler : ContentHandler
    {
        public BodyPartHandler(IAssetManager assetManager, IAccountContext accountContext)
        {
            OnUpdated<BodyPart>((context, part) =>
            {
                CQ dom = part.Text;
                if (dom == null) return;
                CQ assetImages = dom["img[asset-ref]"];
                if (!assetImages.Any()) return;
                foreach (var image in assetImages)
                {
                    var assetReference = image.Attributes["asset-ref"];
                    var asset = assetManager.GetFileById(assetReference);
                    var path = asset.Path.Substring(accountContext.AssetPath.Length - 1);
                    image.Attributes["src"] = path;
                }
                part.Text = dom.Render();
            });
        }
    }
}
