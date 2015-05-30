using System;
using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using EMS.PageTitleOverride.Models;

namespace EMS.PageTitleOverride.Services {
    public interface IPageTitleOverrideService : IDependency {
        bool GetIsPageTitleSiteNameLast();
        bool GetIsPageTitleHideSiteName();
        string GetPageTitleOverride();
    }
}