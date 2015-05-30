using ceenq.com.Core.Assets;
using ceenq.com.Core.Infrastructure.Compute;

namespace ceenq.com.LinuxCommands
{
    public class MountHelper : IMountHelper
    {
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly IAssetStorageCredentialsProvider _assetStorageCredentials;
        public MountHelper(IServerCommandProvider serverCommandProvider, IAssetStorageCredentialsProvider assetStorageCredentials)
        {
            _serverCommandProvider = serverCommandProvider;
            _assetStorageCredentials = assetStorageCredentials;
        }

        public void EnsureMount(IServerCommandClient commandClient, string shareName, string mountDirectory)
        {
            commandClient.ExecuteCommand(_serverCommandProvider.New<ICreateDirectoryCommand>(mountDirectory));//create a directory with the account name to mount to

            commandClient.ExecuteCommand(_serverCommandProvider.New<IMountCommand>(
                shareName,
                mountDirectory,
                _assetStorageCredentials.Username,
                _assetStorageCredentials.Password
            ));

            //ensure auto mount on server restart
            var result = commandClient.ExecuteCommand(_serverCommandProvider.New<IGetFileCommand>("/etc/fstab"));

            var mountDirective = MountDirective(shareName, mountDirectory);
            if (!result.Message.Contains(mountDirective))
            {
                string fileText = result.Message + "\n" + mountDirective;
                commandClient.ExecuteCommand(_serverCommandProvider.New<IWriteFileCommand>("/etc/fstab", fileText));
            }
        }
        private string MountDirective(string shareName, string mountDirectory)
        {
            return string.Format("//{0}.file.core.windows.net/{2} {3} cifs vers=2.1,dir_mode=0777,file_mode=0777,username={0},password={1}",
                _assetStorageCredentials.Username,
                _assetStorageCredentials.Password
                , shareName
                , mountDirectory
                );
        }
    }
}