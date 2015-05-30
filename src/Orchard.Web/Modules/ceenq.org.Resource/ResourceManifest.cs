using Orchard.UI.Resources;

namespace ceenq.org.Resource {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("ResourceStyle").SetUrl("esv-text.min.css");
        }
    }
}
