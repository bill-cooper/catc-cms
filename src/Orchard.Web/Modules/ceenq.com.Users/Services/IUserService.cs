using System.Collections.Generic;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Models;
using ceenq.com.Users.Models;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Users.Services {
    public interface IUserService : IDependency
    {
        IUser CreateUser(UserModel userModel);
        void UpdateUser(UserModel userModel);

        void DeleteUser(UserModel userModel);
        UserModel Get(int id);
        IEnumerable<UserModel> GetUsers();
    }
}