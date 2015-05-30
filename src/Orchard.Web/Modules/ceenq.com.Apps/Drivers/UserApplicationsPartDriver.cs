using System;
using System.Linq;
using ceenq.com.Apps.Models;
using ceenq.com.Apps.Services;
using ceenq.com.Apps.ViewModels;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Security;

namespace ceenq.com.Apps.Drivers
{
    [UsedImplicitly]
    public class UserApplicationsPartDriver : ContentPartDriver<UserApplicationsPart> {
        private readonly IApplicationManager _applicationManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;

        public UserApplicationsPartDriver( 
            IApplicationManager accountService, 
            IAuthenticationService authenticationService,
            IAuthorizationService authorizationService) {
            _applicationManager = accountService;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix {
            get {
                return "UserApplicationsPart";
            }
        }

        public Localizer T { get; set; }

        protected override DriverResult Editor(UserApplicationsPart userApplicationsPart, dynamic shapeHelper) {
            // don't show editor without apply accounts permission
            if (!_authorizationService.TryCheckAccess(Permissions.AssignApplications, _authenticationService.GetAuthenticatedUser(), userApplicationsPart))
                return null;

            return ContentShape("Parts_Application_UserApplicationsPart",
                    () => {
                            var applicationRecords = _applicationManager.GetApplications();
                            var applications = applicationRecords.Select(x => new UserApplicationEntry
                            {
                                                                              ApplicationId = x.Id,
                                                                              Name = x.Name,
                                                                              UserHasApplicationAccess = userApplicationsPart.ApplicationNames.Contains(x.Name)
                           });
                       
                       var model = new UserApplicationsViewModel {
                           User = userApplicationsPart.As<IUser>(),
                           UserApplications = userApplicationsPart,
                           Applications = applications.ToList(),
                       };
                       return shapeHelper.EditorTemplate(TemplateName: "Parts.Application.UserApplicationsPart", Model: model, Prefix: Prefix);
                    });
        }

        protected override DriverResult Editor(UserApplicationsPart userApplicationsPart, IUpdateModel updater, dynamic shapeHelper) {

            if (!_authorizationService.TryCheckAccess(StandardPermissions.SiteOwner, _authenticationService.GetAuthenticatedUser(), userApplicationsPart))
                return null;

            var model = BuildEditorViewModel(userApplicationsPart);
            if (updater.TryUpdateModel(model, Prefix, null, null)) {
                _applicationManager.UpdateUsersApplications(userApplicationsPart.As<IUser>(),model.Applications.Where(m => m.UserHasApplicationAccess).Select(m => m.Name).ToList());
            }
            return ContentShape("Parts_Application_UserApplicationsPart",
                                () => shapeHelper.EditorTemplate(TemplateName: "Parts.Application.UserApplicationsPart", Model: model, Prefix: Prefix));
        }

        private static UserApplicationsViewModel BuildEditorViewModel(UserApplicationsPart userApplicationsPart) {
            return new UserApplicationsViewModel { User = userApplicationsPart.As<IUser>(), UserApplications = userApplicationsPart };
        }

        protected override void Importing(UserApplicationsPart part, Orchard.ContentManagement.Handlers.ImportContentContext context) {
            var accounts = context.Attribute(part.PartDefinition.Name, "Applications");
            if(string.IsNullOrEmpty(accounts)) {
                return;
            }

            var userApplications = accounts.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            // create new accounts
            foreach (var account in userApplications) {
                _applicationManager.AddUserToApplication(part.As<IUser>(),account);
            }
        }

        protected override void Exporting(UserApplicationsPart part, Orchard.ContentManagement.Handlers.ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Applications", string.Join(",", part.Applications.Select(a => a.Name)));
        }
    }
}