using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands.Commands
{
    public class NginxReloadCommand : INginxReloadCommand
    {
        public void Initialize(params string[] parameters)
        {
        }

        public string CommandText {
            get { return "sudo nginx -s reload"; }
        }
    }
}