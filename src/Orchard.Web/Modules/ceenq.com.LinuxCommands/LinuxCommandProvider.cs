using System.Linq;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard.Environment.Extensions;
using Autofac;

namespace ceenq.com.LinuxCommands
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Compute.DefaultServerCommandProvider")]
    public class LinuxCommandProvider : IServerCommandProvider
    {
        private readonly IComponentContext _componentContext;
        public LinuxCommandProvider(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public T New<T>(params string[] parameters) where T : class, IServerCommand
        {
            var command = _componentContext.Resolve<T>();
            command.Initialize(parameters);
            return command;
        }
    }
}