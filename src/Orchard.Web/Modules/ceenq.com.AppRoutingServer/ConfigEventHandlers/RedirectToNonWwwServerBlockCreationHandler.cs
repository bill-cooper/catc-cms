using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class RedirectToNonWwwServerBlockCreationHandler : IServerBlockCreationHandler
    {
        public Localizer T { get; set; }
        public RedirectToNonWwwServerBlockCreationHandler()
        {
            T = NullLocalizer.Instance;
        }

        public void AddServerBlock(ConfigContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate 'redirect to non-www' server block.  The config context was not supplied."), new ArgumentException("Could not generate 'redirect to non-www' server block.  The config context was not supplied.", "context"));

            if (string.IsNullOrWhiteSpace(context.Application.Domain)) return;
            if (context.Application.TransportSecurity) return;

            var domain = context.Application.Domain.ToLower();
            var domainParts = context.Application.Domain.ToLower().Split('.');

            //If we have more than just the domain name and top level domain (i.e. somesite.com) then
            // we can return because this situation is not applicable to this handler
            if (domainParts.Length != 2) return;

            var serverBlock = new ServerBlock();

            //add www subdomain
            serverBlock.DnsNames.Add(string.Format("www.{0}", domain));
            //and return redirect to non-www domain
            serverBlock.Return = string.Format("301 http://{0}$request_uri", domain);

            context.Config.ServerBlock.Add(serverBlock);
        }
    }
}