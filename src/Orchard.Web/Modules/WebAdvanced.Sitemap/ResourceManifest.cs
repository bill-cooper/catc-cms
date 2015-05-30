using Orchard.UI.Resources;

namespace WebAdvanced.Sitemap
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineScript("Sitemap_Displaysettings").SetUrl("admin.displaysettings.js").SetDependencies("jQuery");

        }
    }
}
