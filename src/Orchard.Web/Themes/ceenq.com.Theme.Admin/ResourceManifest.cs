using Orchard.UI.Resources;

namespace ceenq.com.Theme.Admin {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("FontAwesome").SetUrl("//netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css");

            manifest.DefineScript("FontAwesome")
                .SetVersion("3.2.1")
                .SetUrl("font/font-awesome.min.js", "font/font-awesome.js")
                .SetCdn("//netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.min.css", "//netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css", true);
        }
    }
}
