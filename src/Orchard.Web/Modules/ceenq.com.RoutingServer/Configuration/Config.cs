using System.Collections.Generic;
using System.Runtime.Serialization;
using ceenq.com.Core.Routing;

namespace ceenq.com.RoutingServer.Configuration {
    public class Config: IRoutingConfigFile
    {
        public Config() {
            ServerBlock = new List<ServerBlock>();
        }

        [DataMember(Name = "server")]
        [SerializeAs(SerializationFormat.SectionArray)]
        public List<ServerBlock> ServerBlock { get; set; }
    }
}