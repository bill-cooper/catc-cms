using System;
using System.IO;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Routing;
using ceenq.com.Core.Utility;
using ceenq.com.RoutingServer.Configuration;
using Orchard.Localization;

namespace ceenq.com.AppRoutingServer.ConfigEventHandlers
{
    public class LocationBlockPassToAdjustHandler : ILocationBlockAdjustHandler
    {
        private readonly IRouteService _routeService;
        public Localizer T { get; set; }
        public LocationBlockPassToAdjustHandler(IRouteService routeService)
        {
            _routeService = routeService;
            T = NullLocalizer.Instance;
        }

        public void AdjustLocationBlock(LocationBlockContext context)
        {
            if (context == null)
                throw new ConfigGenerationException(T("Could not configure location block.  The config context was not supplied."), new ArgumentException("Could not configure location block.  The config context was not supplied.", "context"));

            //if proxy path has aleady been provided, then do not process
            if (!string.IsNullOrWhiteSpace(context.LocationBlock.ProxyPass)) return;
            //if root has aleady been provided, then do not process
            if (!string.IsNullOrWhiteSpace(context.LocationBlock.Root)) return;

            if (_routeService.RoutesToLocalResource(context.Route))
            {
                if (context.Route.RequireAuthentication)
                    context.LocationBlock.ProxyPass =
                        context.AccountContext.ToInternalAbsoluteAssetPath(context.Route.PassTo);
                else
                {
                    //this rewrite provides a cleaner looking mapping from request patter to passto path.
                    //but if request pattern is base directory, there is no need to add the rewrite
                    if (context.Route.RequestPattern != "/")
                        context.LocationBlock.Rewrite = string.Format("(?i)^{0}/(.*)$ /$1 break",
                            context.Route.RequestPattern);
                    context.LocationBlock.Root =
                        context.AccountContext.ToServerAssetPath(context.Route.PassTo).TrimEnd(new[] { '/' });
                }

            }
            else
            {
                if (_routeService.RoutesToModuleResource(context.Route))
                {
                    context.LocationBlock.ProxyPass = context.AccountContext.ToAbsoluteModulePath(context.Route.PassTo);
                }
                else
                {
                    context.LocationBlock.ProxyPass = context.Route.PassTo;
                }


                if (context.LocationBlock.MatchPattern.EndsWith("(.*)$"))
                {
                    context.LocationBlock.ProxyPass = context.LocationBlock.ProxyPass + "$1$is_args$args";
                }
                else
                {
                    //if this is a direct reference to a file, then do not append the request uri
                    if (!UrlHelper.IsFilePath(context.LocationBlock.ProxyPass))
                        context.LocationBlock.ProxyPass = context.LocationBlock.ProxyPass.TrimEnd(new[] { '/' }) + "$request_uri";
                }
            }


        }
    }
}