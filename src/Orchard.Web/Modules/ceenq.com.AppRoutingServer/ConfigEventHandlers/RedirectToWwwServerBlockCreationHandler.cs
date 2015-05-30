using System;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class RedirectToWwwServerBlockCreationHandler : IServerBlockCreationHandler
    {
        public Localizer T { get; set; }
        public RedirectToWwwServerBlockCreationHandler()
        {
            T = NullLocalizer.Instance;
        }

        public void AddServerBlock(ConfigContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate 'redirect to www' server block.  The config context was not supplied."), new ArgumentException("Could not generate 'redirect to www' server block.  The config context was not supplied.", "context"));

            if (string.IsNullOrWhiteSpace(context.Application.Domain)) return;
            if (context.Application.TransportSecurity) return;

            var domain = context.Application.Domain.ToLower();
            var domainParts = context.Application.Domain.ToLower().Split('.');

            //If we are not dealing with a custom domain of the form www.somesite.com then
            // we can return because this situation is not applicable to this handler
            if (domainParts.Length != 3 || domainParts[0] != "www") return;

            var serverBlock = new ServerBlock();

            //add non-www version of the domain 
            serverBlock.DnsNames.Add(domain.Replace("www.", ""));
            //and return a 301 redirect to the www subdomain
            serverBlock.Return = string.Format("301 http://{0}$request_uri", domain);

            context.Config.ServerBlock.Add(serverBlock);
        }
    }
}