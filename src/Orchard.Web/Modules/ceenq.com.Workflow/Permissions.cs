using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace ceenq.com.Workflow {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageWorkflows = new Permission { Description = "Manage Workflows", Name = "ManageWorkflows" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageWorkflows
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageWorkflows}
                },
            };
        }

    }
}