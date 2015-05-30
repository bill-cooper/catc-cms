using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace ceenq.com.Layouts.TinyMCE {
    [OrchardSuppressDependency("TinyMce.ResourceManifest")]
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("TinyMce").SetUrl("~/modules/tinymce/scripts/tinymce.min.js").SetVersion("4.1.5").SetDependencies("jQuery");
            manifest.DefineScript("OrchardTinyMce").SetUrl("orchard-tinymce.js").SetDependencies("TinyMce");
        }
    }
}
