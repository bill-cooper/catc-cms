using Orchard.UI.Resources;

namespace ceenq.com.LiveTiles
{
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("LiveTiles").SetUrl("livetiles.js").SetDependencies("MetroJs");
            manifest.DefineStyle("LiveTilesStyle").SetUrl("livetiles.css");
        }
    }
}
