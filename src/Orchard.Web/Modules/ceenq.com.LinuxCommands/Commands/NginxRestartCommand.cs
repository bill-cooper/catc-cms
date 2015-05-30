using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands.Commands
{
    public class NginxRestartCommand : INginxRestartCommand
    {
        public void Initialize(params string[] parameters)
        {
        }

        public string CommandText {
            get { return "sudo service nginx restart"; }
        }
    }
}