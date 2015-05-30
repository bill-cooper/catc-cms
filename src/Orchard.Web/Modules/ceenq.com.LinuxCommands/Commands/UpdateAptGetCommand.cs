using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands.Commands
{
    public class UpdateAptGetCommand : IUpdateAptGetCommand
    {
        public void Initialize(params string[] parameters)
        {
        }

        public string CommandText {
            get { return "sudo apt-get update"; }
        }
    }
}