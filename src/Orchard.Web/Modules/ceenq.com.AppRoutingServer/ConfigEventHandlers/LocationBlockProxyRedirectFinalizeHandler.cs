using System;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockProxyRedirectFinalizeHandler : ILocationBlockFinalizeHandler
    {
        public Localizer T { get; set; }

        public LocationBlockProxyRedirectFinalizeHandler()
        {
            T = NullLocalizer.Instance;
        }
        public void FinalizeLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not generate location block.  The config context was not supplied."), new ArgumentException("Could not generate location block.  The config context was not supplied.", "context"));
            
            //we only care about adding proxy_redirect to the locations that have proxy pass set
            if(string.IsNullOrWhiteSpace(context.LocationBlock.ProxyPass)) return;

            var proxyPass = context.LocationBlock.ProxyPass.Replace("$1$is_args$args", "").Replace("$request_uri", "");

            Uri proxyPassUri;
            if (!Uri.TryCreate(proxyPass, UriKind.Absolute, out proxyPassUri)) return;
            
            context.LocationBlock.ProxyRedirect = string.Format("{0} {1}", proxyPassUri.AbsoluteUri, proxyPassUri.AbsoluteUri.Replace(proxyPassUri.Host, "$host"));

        }
    }
}