using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using kosfiz.WebSiteOwner.Models;
using Orchard;

namespace kosfiz.WebSiteOwner.Services
{
    public interface IWebSiteOwnerService: IDependency
    {
        WebSiteOwnerRecord Get(int Id);
        List<WebSiteOwnerRecord> Get();
        bool Set(int Id, string Title, string MetaName, string MetaContent);
        bool Delete(int Id);
        void Add(string Title, string MetaName, string MetaContent);
    }
}
