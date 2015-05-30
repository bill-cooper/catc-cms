using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.EventHandlers
{
    public class NginxRestartCommandEventHandler : Component, IServerCommandEventHandler
    {
        public void CommandExecuted(ServerCommandContext context)
        {
            if (context.Command is INginxRestartCommand)
            {
                if (context.Message.Contains("fail"))
                    throw new ServerCommandException(T("Routing Server restart failed"), context.Command.CommandText, 0, context.Message,context.Message);

            }
        }
    }
}