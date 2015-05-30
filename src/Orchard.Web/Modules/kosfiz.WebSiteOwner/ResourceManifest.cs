using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace kosfiz.WebSiteOwner
{
    public class ResourceManifest: IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add().DefineStyle("WebSiteOwnerStyle").SetUrl("WebSiteOwner.css");
        }
    }
}