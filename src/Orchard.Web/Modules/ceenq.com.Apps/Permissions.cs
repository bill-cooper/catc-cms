using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace ceenq.com.Apps {
    public class Permissions : IPermissionProvider {
        public static readonly Permission AssignApplications = new Permission { Description = "Assign Applications", Name = "AssignApplications" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                AssignApplications
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {AssignApplications}
                },
            };
        }

    }
}