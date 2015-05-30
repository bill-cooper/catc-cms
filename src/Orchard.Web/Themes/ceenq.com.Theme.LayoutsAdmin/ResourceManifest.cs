using Orchard.UI.Resources;

namespace ceenq.com.Theme.LayoutsAdmin {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("CnqStyle").SetUrl("/dashboard/styles/main.css");

        }
    }
}
