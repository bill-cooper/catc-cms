using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using ceenq.com.Common.Models;
using ceenq.com.Common.ViewModels;

namespace ceenq.com.Common.Drivers
{
    public class UserProfilePartDriver : ContentPartDriver<UserProfilePart> {
        private readonly IRepository<UserProfilePart> _userProfilePartRepository;
        public UserProfilePartDriver(IRepository<UserProfilePart> userProfilePartRepository) {
            _userProfilePartRepository = userProfilePartRepository;
        }

        protected override string Prefix
        {
            get
            {
                return "UserProfile";
            }
        }
        protected override DriverResult Editor(UserProfilePart part, dynamic shapeHelper) {
            var viewModel = new UserProfileViewModel {Name = part.Name};
            return ContentShape("Part_UserProfile_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/UserProfile", Model: viewModel, Prefix: Prefix));
        }
        protected override DriverResult Editor(UserProfilePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}