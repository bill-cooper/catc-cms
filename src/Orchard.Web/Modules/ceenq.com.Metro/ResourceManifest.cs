using Orchard.UI.Resources;

namespace ceenq.com.Metro{
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("MetroJs").SetUrl("metrojs.min.js", "metrojs.js").SetDependencies("jQuery");
            manifest.DefineStyle("MetroStyle").SetUrl("metro-bootstrap.css");
        }
    }
}
