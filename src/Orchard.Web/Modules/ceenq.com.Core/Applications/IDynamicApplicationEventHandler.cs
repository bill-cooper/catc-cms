using System.Collections.Generic;
using Orchard.Events;

namespace ceenq.com.Core.Applications
{
    public interface IDynamicApplicationEventHandler : IEventHandler
    {
        void FindDynamicApplications(DynamicApplicationContext context);
    }

    public class DynamicApplicationContext
    {
        public DynamicApplicationContext()
        {
            Applications = new List<IApplication>();
        }
        public IList<IApplication> Applications { get; private set; }
    }
}