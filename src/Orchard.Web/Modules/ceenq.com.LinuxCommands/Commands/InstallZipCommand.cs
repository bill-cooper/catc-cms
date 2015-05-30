using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands.Commands
{
    public class InstallZipCommand : IInstallZipCommand
    {
        public void Initialize(params string[] parameters)
        {
        }

        public string CommandText {
            get { return "sudo apt-get -y install zip"; }
        }
    }
}