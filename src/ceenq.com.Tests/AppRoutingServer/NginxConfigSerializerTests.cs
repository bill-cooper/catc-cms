using System.Collections.Generic;
using System.Text.RegularExpressions;
using ceenq.com.AppRoutingServer.Services;
using ceenq.com.RoutingServer.Configuration;
using NUnit.Framework;

namespace ceenq.com.Tests.AppRoutingServer
{
    [TestFixture]
    public class NginxConfigSerializerTests
    {
        [Test]
        public void ShouldSerializeConfig()
        {
            var config = new Config
            {
                ServerBlock = new List<ServerBlock>
                {
                    new ServerBlock()
                }
            };
            var serializer = new NginxConfigSerializer(new NginxConfigPrettyFormatter());
            var serializedConfig = serializer.Serialize(config);

            Assert.That(Regex.Matches(serializedConfig, "server").Count == 1,"This test expected only one server block to have been created");
        }
    }
}
