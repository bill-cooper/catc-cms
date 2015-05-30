using System;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Extensions;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;

namespace ceenq.com.RoutingServer
{
    public class RoutingServerCreationEventHandler: IRoutingServerCreationEventHandler
    {
        private readonly IServerManagement _serverManagement; 
        private readonly IAccountContext _accountContext;
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly IRoutingServerManager _routingServerManager;
        private readonly IMountHelper _mountHelper;
        public RoutingServerCreationEventHandler(IServerManagement serverManagement, IAccountContext accountContext, IServerCommandProvider serverCommandProvider,  IRoutingServerManager routingServerManager, IMountHelper mountHelper)
        {
            _serverManagement = serverManagement;
            _accountContext = accountContext;
            _serverCommandProvider = serverCommandProvider;
            _routingServerManager = routingServerManager;
            _mountHelper = mountHelper;
        }

        public void Creating(RoutingServerCreationEventContext context)
        {
            //TODO: Generate this name in a better place
            //TODO: The name is currently being truncated to the first 7 characters.  This could lead to name collisions.
            // Validation should be added to prevent the name collisions or the name should be generated in a different way
            var unique = Guid.NewGuid().ToString().Substring(0, 5);
            context.RoutingServer.Name = string.Format("rs-{0}{1}", _accountContext.Account.Truncate(7), unique);

            var serverInfo = _serverManagement.Create(new ServerOperationParameters()
            {
                Name = context.RoutingServer.Name
            });

            context.RoutingServer.DnsName = serverInfo.Host;
            context.RoutingServer.ConnectionPort = 22;
            context.RoutingServer.IpAddress = serverInfo.IpAddress;
            context.RoutingServer.Username = serverInfo.AdminUsername;
            context.RoutingServer.Password = serverInfo.AdminPassword;
        }

        public void Created(RoutingServerCreationEventContext context)
        {
            var commandClient = _routingServerManager.GetCommandClient(context.RoutingServer);

            commandClient.ExecuteCommand(_serverCommandProvider.New<IUpdateAptGetCommand>());
            try
            {
                //TODO: remove this try catch hack.  This is because the following command periodically fails due to a timing issue
                commandClient.ExecuteCommand(_serverCommandProvider.New<IInstallFileSystemUtilities>());
            }
            catch (ServerCommandException)
            {
                System.Threading.Thread.Sleep(10000);//sleep 10 seconds
                commandClient.ExecuteCommand(_serverCommandProvider.New<IUpdateAptGetCommand>());
                System.Threading.Thread.Sleep(5000);//sleep 5 seconds
                //try command again
                commandClient.ExecuteCommand(_serverCommandProvider.New<IInstallFileSystemUtilities>());
            }
            commandClient.ExecuteCommand(_serverCommandProvider.New<IInstallNginxCommand>());
            commandClient.ExecuteCommand(_serverCommandProvider.New<IInstallZipCommand>());

            _mountHelper.EnsureMount(commandClient, _accountContext.Account, _accountContext.ServerAbsoluteBasePath);

            //create the base asset path directory
            commandClient.ExecuteCommand(_serverCommandProvider.New<ICreateDirectoryCommand>(_accountContext.ServerAbsoluteAssetPath));

            //TODO: Clean up the following
            commandClient.ExecuteCommand(_serverCommandProvider.New<IWriteFileCommand>(
                "/etc/nginx/nginx.conf",
                @"
        worker_processes 1;
        events {
            worker_connections  4096;  ## Default: 1024
        }
        
        http{
            proxy_cache_path /home/" + context.RoutingServer.Username + @"/ngcache levels=1:2 keys_zone=cache:10m inactive=60m max_size=10g;
            proxy_cache_key ""$scheme$request_method$host$request_uri"";
            proxy_cache_use_stale   error timeout invalid_header updating http_500 http_502 http_503 http_504;
            
            
            # transfers the `Host` header to the backend
            proxy_set_header        Host $host;
            
            
            # cache 200 60 minutes, 404 1 minute, others status codes not cached
            proxy_cache_valid 200 60m;
            proxy_cache_valid 404 1m;
            
            proxy_http_version 1.1;
            
            # transfers real client IP to your ghost app,
            # otherwise you would see your server ip
            proxy_set_header        X-Real-IP        $remote_addr;
            proxy_set_header        X-Forwarded-For  $proxy_add_x_forwarded_for;
            
            client_max_body_size  120m;
            client_body_buffer_size    128k;
            
            # gzip every proxied responses
            gzip_proxied any;
            
            # gzip only if user asks it
            gzip_vary on;
            
            # gzip only theses mime types
            gzip_types text/plain text/css application/x-javascript text/xml application/xml application/xml+rss text/javascript application/json application/javascript;
            gzip_static on;
            
            # add a cache HIT/MISS header
            add_header X-Cache $upstream_cache_status;
            
            
            # do not show incoming headers from proxy
            proxy_hide_header X-AspNet-Version;
            proxy_hide_header X-AspNetMvc-Version;
            proxy_hide_header X-Powered-By;
            proxy_hide_header ETag;
            
            
            
            include /home/" + context.RoutingServer.Username + @"/*.conf;
            include /etc/nginx/mime.types;
            default_type application/octet-stream;
            
            #access_log /var/log/nginx/access.log;
            access_log off;
            error_log /var/log/nginx/error.log debug;
            #error_log /var/log/nginx/error.log;
    }



                            "
                ));

            commandClient.ExecuteCommand(_serverCommandProvider.New<IWriteFileCommand>(
                "404.conf",
                @"
                    server{
		                    listen	80;
		                    server_name	*." + _accountContext.AccountDomain + @";

		                    location / {
                                return 404;
		                    }
                    }
                "
                ));

            commandClient.ExecuteCommand(_serverCommandProvider.New<INginxReloadCommand>());
        }

        public void PostCreated(RoutingServerCreationEventContext context)
        {
        }
    }
}