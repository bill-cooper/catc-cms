using Orchard.Environment.Extensions.Models;

namespace ceenq.com.Core.Utility
{
    public static class ModuleUtility
    {
        public static ExtensionDescriptor ContainerExtentionFor<T>()
        {
            return new ExtensionDescriptor() { Name = typeof(T).Assembly.GetName().Name };
        }
    }
}
