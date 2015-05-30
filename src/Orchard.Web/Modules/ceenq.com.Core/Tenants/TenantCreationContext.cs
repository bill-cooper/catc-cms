using System.Collections.Generic;
using Orchard.Recipes.Models;

namespace ceenq.com.Core.Tenants {
    public class TenantCreationContext {
        public string SiteName { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string DatabaseProvider { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string DatabaseTablePrefix { get; set; }
        public IEnumerable<string> EnabledFeatures { get; set; }
        public Recipe Recipe { get; set; }
        public string RequestUrlHost { get; set; }
    }
}