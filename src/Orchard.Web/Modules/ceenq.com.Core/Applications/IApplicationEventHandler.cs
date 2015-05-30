using Orchard.Events;

namespace ceenq.com.Core.Applications
{
    public interface IApplicationEventHandler: IEventHandler
    {
        void ApplicationCreating(ApplicationEventContext context);
        void ApplicationCreated(ApplicationEventContext context);
        void ApplicationUpdating(ApplicationEventContext context);
        void ApplicationUpdated(ApplicationEventContext context);
        void ApplicationDeleting(ApplicationEventContext context);
        void ApplicationDeleted(ApplicationEventContext context);
    }

    public class ApplicationEventContext
    {
        public IApplication Application { get; set; }
    }
}
