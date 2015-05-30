using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands.Commands
{
    public class InstallFileSystemUtilities : IInstallFileSystemUtilities
    {
        public void Initialize(params string[] parameters)
        {
        }

        public string CommandText {
            get { return "sudo apt-get -y install cifs-utils"; }
        }
    }
}