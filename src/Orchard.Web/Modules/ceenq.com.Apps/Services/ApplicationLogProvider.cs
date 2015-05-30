using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Services
{
    public interface IApplicationLogProvider : IDependency
    {
        string LogFor(IApplication application);
    }

    public class ApplicationLogProvider : IApplicationLogProvider
    {
        private readonly IRoutingServerManager _routingServerManager;
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly IApplicationDnsNamesService _applicationDnsNamesService;
        public ApplicationLogProvider(IRoutingServerManager routingServerManager, IServerCommandProvider serverCommandProvider, IApplicationDnsNamesService applicationDnsNamesService)
        {
            _routingServerManager = routingServerManager;
            _serverCommandProvider = serverCommandProvider;
            _applicationDnsNamesService = applicationDnsNamesService;
        }

        public string LogFor(IApplication application)
        {

            var routingServer = _routingServerManager.Get(application.As<IApplicationRoutingServer>().IpAddress);
            var client = _routingServerManager.GetCommandClient(routingServer);
            var result = client.ExecuteCommand(_serverCommandProvider.New<IReadFileCommand>("/var/log/nginx/error.log","1000"));

            return ParseServerLog(application, result.Message);
        }

        private string ParseServerLog(IApplication application, string serverLog)
        {
            var builder = new StringBuilder(string.Empty);
            var allLines = serverLog.Split('\n');
            var filteredLines = new List<string>();
            foreach (var line in allLines)
            {
                if (line.Contains("epoll")) continue;
                if (line.Contains("free:")) continue;
                if (line.Contains("event timer")) continue;
                if (line.Contains("reusable connection")) continue;
                if (line.Contains("posix_memalign")) continue;
                if (line.Contains("malloc")) continue;
                if (line.Contains("client closed connection while waiting for request")) continue;
                if (line.Contains("close http connection")) continue;
                if (line.Contains("accept on 0.0.0.0:80, ready: 0")) continue;
                if (line.Contains("http wait request handler")) continue;
                if (line.Contains("recv:")) continue;
                if (line.Contains("recv() not ready")) continue;
                if (line.Contains("writev")) continue;
                if (line.Contains("write new buf")) continue;
                if (line.Contains("write old buf")) continue;
                if (line.Contains("rewrite phase")) continue;
                if (line.Contains("generic phase")) continue;
                if (line.Contains("access phase")) continue;
                if (line.Contains("content phase")) continue;
                if (line.Contains("accept: 100.")) continue; //internal ip address so ignore
                if (line.Contains("http keepalive handler")) continue;
                if (line.Contains("http write filter")) continue;
                if (line.Contains("http output filter")) continue;
                if (line.Contains("http copy filter")) continue;
                if (line.Contains("image filter")) continue;
                if (line.Contains("xslt filter body")) continue;
                if (line.Contains("http postpone filter")) continue;
                if (line.Contains("set http keepalive handler")) continue;
                if (line.Contains("http log handler")) continue;
                if (line.Contains("hc busy")) continue;
                if (line.Contains("tcp_nodelay")) continue;
                if (line.Contains("post event")) continue;
                if (line.Contains("delete posted event")) continue;
                if (line.Contains("http process request header line")) continue;
                if (line.Contains("http header done")) continue;
                if (line.Contains("http cl:")) continue;
                if (line.Contains("xslt filter header")) continue;
                if (line.Contains("pipe read")) continue;
                if (line.Contains("pipe write")) continue;
                if (line.Contains("pipe buf")) continue;
                if (line.Contains("pipe recv")) continue;
                if (line.Contains("pipe preread")) continue;
                if (line.Contains("pipe length")) continue;
                if (line.Contains("readv")) continue;
                if (line.Contains("get rr peer")) continue;
                if (line.Contains("free rr peer")) continue;
                if (line.Contains("http script copy")) continue;
                if (line.Contains("chain writer")) continue;
                if (line.Contains("input buf")) continue;
                if (line.Contains("http proxy filter init")) continue;
                if (line.Contains("http upstream request")) continue;
                if (line.Contains("http upstream temp")) continue;
                if (line.Contains("http upstream process upstream")) continue;
                if (line.Contains("http upstream dummy handle")) continue;
                if (line.Contains("http upstream exit")) continue;
                if (line.Contains("http proxy header done")) continue;
                if (line.Contains("http cleanup add")) continue;
                if (line.Contains("run cleanup")) continue;
                if (line.Contains("file cleanup")) continue;
                if (line.Contains("http empty handler")) continue;
                if (line.Contains("xxxxxxx")) continue;
                if (line.Contains("xxxxxxx")) continue;
                if (line.Contains("xxxxxxx")) continue;

                filteredLines.Add(line);
            }
            var appRequestKeys = new List<string>();
            filteredLines.ForEach((line) =>
            {
                if(!line.Contains(_applicationDnsNamesService.PrimaryDnsName(application)))
                    return;
                var lineParts = line.Split(' ');
                if (lineParts.Length >= 5)
                {
                    appRequestKeys.Add(lineParts[4]);
                }
            });
            filteredLines.ForEach((line) => 
            {
                //continue if the line does not contain any of the keys
                if (!appRequestKeys.Any(line.Contains)) return;
                builder.AppendLine(line);
                if (line.Contains("http close request"))
                {
                    builder.AppendLine("");
                    builder.AppendLine("---------------------------------------------------------------------------------------------------------------------");
                    builder.AppendLine("");
                }
            });
            
                
            return builder.ToString();
        }
    }
}