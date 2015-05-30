using Orchard.UI.Resources;

namespace ceenq.com.Media {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("jPlayerStyle").SetUrl("premium-pixels/premium-pixels.min.css");
            manifest.DefineStyle("CeenqMedia").SetUrl("ceenq-media.min.css");
            manifest.DefineScript("jPlayer").SetDependencies("jquery").SetUrl("jquery.jplayer.min.js");
        }
    }
}
