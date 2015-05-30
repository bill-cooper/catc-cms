using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ceenq.com.RoutingServer.Configuration
{
    public class ServerBlock
    {
        public ServerBlock()
        {
            DnsNames = new List<string>();
            LocationBlocks = new List<LocationBlock>();
            Port = new List<string>();
            Header = new List<string>();
            SslProtocols = new List<string>();
        }

        [DataMember(Name = "listen")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> Port { get; set; }

        [DataMember(Name = "server_name")]
        public List<string> DnsNames { get; set; }

        [DataMember(Name = "ssl_certificate")]
        public string SslCertLocation { get; set; }

        [DataMember(Name = "ssl_certificate_key")]
        public string SslCertKeyLocation { get; set; }

        [DataMember(Name = "ssl_prefer_server_ciphers")]
        public string SslPreferServerCiphers { get; set; }

        [DataMember(Name = "add_header")]
        [SerializeAs(SerializationFormat.DirectiveArray)]
        public List<string> Header { get; set; }

        [DataMember(Name = "ssl_protocols")]
        public List<string> SslProtocols { get; set; }

        [DataMember(Name = "ssl_ciphers")]
        public string SslCiphers { get; set; }

        [DataMember(Name = "return")]
        public string Return { get; set; }

        [DataMember(Name = "location")]
        [SerializeAs(SerializationFormat.SectionArray)]
        public List<LocationBlock> LocationBlocks { get; set; }


    }
}