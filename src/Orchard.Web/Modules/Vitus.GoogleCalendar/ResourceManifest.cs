using Orchard.UI.Resources;

namespace Vitus.GoogleCalendar
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("FullCalendar").SetUrl("fullcalendar-1.6.3.min.js", "fullcalendar-1.6.3.js").SetVersion("1.6.3")
                .SetDependencies("jQuery");

            manifest.DefineScript("FullCalendar_GoogleCalendar").SetUrl("gcal.js").SetVersion("1.6.3")
                .SetDependencies("FullCalendar");

            manifest.DefineStyle("FullCalendar").SetUrl("fullcalendar-1.6.3.css").SetVersion("1.6.3");
        }
    }
}
