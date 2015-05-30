using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Models;
using ceenq.com.Users.Models;
using Orchard;
using Orchard.Data;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using Orchard.Users.Events;
using Orchard.Users.Models;
using Orchard.ContentManagement;

namespace ceenq.com.Users.Services
{
    public class UserService : IUserService
    {

        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IRepository<UserRolesPartRecord> _userRolesRepository;
        private readonly IRoleService _roleService;
        private readonly IUserEventHandler _userEventHandler;

        public UserService(IOrchardServices orchardServices, IMembershipService membershipService, IRepository<UserRolesPartRecord> userRolesRepository, IRoleService roleService, IUserEventHandler userEventHandler)
        {
            _orchardServices = orchardServices;
            _membershipService = membershipService;
            _userRolesRepository = userRolesRepository;
            _roleService = roleService;
            _userEventHandler = userEventHandler;
        }

        public IUser CreateUser(UserModel userModel)
        {

            var user = _membershipService.CreateUser(new CreateUserParams(userModel.UserName, userModel.Password, userModel.Email, null, null, false));
            if (user != null)
            {
                if (userModel.IsAccountAdmin)
                {
                    var accountAdminRole = _roleService.GetRoleByName(Roles.AccountAdmin);
                    _userRolesRepository.Create(new UserRolesPartRecord { UserId = user.Id, Role = accountAdminRole });
                }
                var applicationUserRole = _roleService.GetRoleByName(Roles.ApplicationUser);
                _userRolesRepository.Create(new UserRolesPartRecord { UserId = user.Id, Role = applicationUserRole });
            }
            return user;
        }

        public void UpdateUser(UserModel userModel)
        {
            var user = _orchardServices.ContentManager.Get(userModel.Id);
            user.VersionRecord.Published = false;

            user.As<UserPart>().UserName = userModel.UserName;
            user.As<UserPart>().Email = userModel.Email;

            var accountAdminRole = _roleService.GetRoleByName(Roles.AccountAdmin);
            var userRoles = user.As<IUserRoles>();
            if (userModel.IsAccountAdmin)
            {
                if (!userRoles.Roles.Contains(Roles.AccountAdmin))
                {
                    _userRolesRepository.Create(new UserRolesPartRecord { UserId = user.Id, Role = accountAdminRole });
                }
            }
            else
            {
                if (userRoles.Roles.Contains(Roles.AccountAdmin))
                {
                    var currentUserRoleRecords = _userRolesRepository.Fetch(x => x.UserId == user.Id);
                    _userRolesRepository.Delete(currentUserRoleRecords.FirstOrDefault(ur => ur.Role == accountAdminRole));
                }
            }

            _orchardServices.ContentManager.Publish(user);

            if (!string.IsNullOrWhiteSpace(userModel.Password))
            {
                var muser = _membershipService.GetUser(user.As<UserPart>().UserName);

                if (muser != null)
                {
                    _membershipService.SetPassword(muser, userModel.Password);
                    _userEventHandler.ChangedPassword(muser);
                }
            }
        }

        public UserModel Get(int id)
        {
            var user = _orchardServices.ContentManager.Get<UserPart>(id);
            if (user == null) return null;

            var userRoles = user.As<IUserRoles>();
            var usersApplications = user.As<IUserApplications>();
            var model = new UserModel() {
                Id = user.Id, 
                Email = user.Email, 
                UserName = user.UserName, 
                Applications = usersApplications.ApplicationNames.ToList(),
                IsAccountAdmin = userRoles.Roles.Contains(Roles.AccountAdmin)
            };

            return model;
        }


        public void DeleteUser(UserModel userModel)
        {
            var contentItem = _orchardServices.ContentManager.Get(userModel.Id);
            _orchardServices.ContentManager.Remove(contentItem);
        }

        public IEnumerable<UserModel> GetUsers()
        {
            var users = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().List().OrderBy(u => u.UserName);

            return users.Select(u => new UserModel() { Id = u.Id, Email = u.Email, UserName = u.UserName });
        }

    }
}