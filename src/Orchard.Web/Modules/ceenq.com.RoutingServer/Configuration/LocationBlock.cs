using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ceenq.com.RoutingServer.Configuration
{
    public class LocationBlock
    {
        public LocationBlock()
        {
            ErrorPage = new List<string>();
            ProxySetHeader = new List<string>();
            ProxyIgnoreHeaders = new List<string>();
            Header = new List<string>();
            ProxyHideHeader = new List<string>();
            ProxyCacheValid = new List<string>();
        }

        [IgnoreDataMember]
        public int Order { get; set; }

        [DataMember(Name = "rewrite")]
        public string Rewrite { get; set; }

        public string Resolver { get; set; }

        [PreSectionModifier]
        public string MatchPattern { get; set; }

        [DataMember(Name = "proxy_pass")]
        public string ProxyPass { get; set; }

        [DataMember(Name = "proxy_redirect")]
        public string ProxyRedirect { get; set; }

        [DataMember(Name = "proxy_set_header")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> ProxySetHeader { get; set; }

        [DataMember(Name = "proxy_ignore_headers")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> ProxyIgnoreHeaders { get; set; }

        [DataMember(Name = "add_header")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> Header { get; set; }

        [DataMember(Name = "proxy_hide_header")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> ProxyHideHeader { get; set; }

        [DataMember(Name = "proxy_cache_valid")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> ProxyCacheValid { get; set; }

        [DataMember(Name = "proxy_cache")]
        public string ProxyCache { get; set; }

        [DataMember(Name = "expires")]
        public string Expires { get; set; }

        [DataMember(Name = "proxy_pass_request_headers")]
        public string ProxyPassSetHeaders
        {
            get { return ProxySetHeader.Count > 0 ? "on" : null; }
        }

        [DataMember(Name = "proxy_intercept_errors")]
        public string ProxyInterceptErrors { get; set; }

        [DataMember(Name = "recursive_error_pages")]
        public string RecursiveErrorPages { get; set; }

        [DataMember(Name = "error_page")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> ErrorPage { get; set; }

        [DataMember(Name = "root")]
        public string Root { get; set; }

        [DataMember(Name = "client_max_body_size")]
        public string ClientMaxBodySize { get; set; }
    }
}