using Orchard.Environment;
using Orchard.Environment.Extensions.Models;

namespace ceenq.com.Core.Environment
{
    public abstract class  DataInitializer : IFeatureEventHandler
    {
        protected abstract ExtensionDescriptor ContainerExtension { get; }

        public virtual void Enabled(Feature feature) { }
        public virtual void Installing(Feature feature) { }
        public virtual void Installed(Feature feature) { }
        public virtual void Enabling(Feature feature) { }
        public virtual void Disabling(Feature feature) { }
        public virtual void Disabled(Feature feature) { }
        public virtual void Uninstalling(Feature feature) { }
        public virtual void Uninstalled(Feature feature) { }
    }
}
