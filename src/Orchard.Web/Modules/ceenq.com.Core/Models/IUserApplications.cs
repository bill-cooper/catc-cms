using System.Collections.Generic;
using Orchard.ContentManagement;

namespace ceenq.com.Core.Models {
    public interface IUserApplications : IContent {
        IEnumerable<string> ApplicationNames { get; }
    }
}