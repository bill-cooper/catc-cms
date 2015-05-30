using System;
using Orchard;

namespace ceenq.com.Core.Assets
{
    public interface IAssetStorageCredentialsProvider: IDependency
    {
        string Username { get; }
        string Password { get; }
    }

    public class DefaultAssetStorageCredentialsProvider : IAssetStorageCredentialsProvider
    {
        public string Username {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string Password
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
