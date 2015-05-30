using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;

namespace ceenq.com.LinuxCommands.Commands
{
    public class MountCommand :Component, IMountCommand
    {
        private string _shareName;
        private string _serverDirectory;
        private string _connectionUsername;
        private string _connectionPassword;
        public void Initialize(string[] parameters)
        {
            if (parameters.Length != 4)
                throw new OrchardException(T("MountCommand failed"), new ArgumentException("MountCommand expected exactly 4 arguments", "parameters"));

            _shareName = parameters[0];
            _serverDirectory = parameters[1];
            _connectionUsername = parameters[2];
            _connectionPassword = parameters[3];
        }
        public string CommandText {
            get
            {
                return string.Format("sudo mount -t cifs //{0}.file.core.windows.net/{2} {3} -o vers=2.1,username={0},password={1},dir_mode=0777,file_mode=0777"
                    , _connectionUsername
                    , _connectionPassword
                    , _shareName
                    ,_serverDirectory);
            }
        }
    }
}