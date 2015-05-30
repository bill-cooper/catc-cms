using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Vitus.GoogleCalendar.Models;

namespace Vitus.GoogleCalendar.Handlers
{
    public class GoogleCalendarPartHandler : ContentHandler
    {
        public GoogleCalendarPartHandler(
            IRepository<GoogleCalendarPartRecord> googleCalendarPartRepository)
        {
            Filters.Add(StorageFilter.For(googleCalendarPartRepository));
        }
    }
}
