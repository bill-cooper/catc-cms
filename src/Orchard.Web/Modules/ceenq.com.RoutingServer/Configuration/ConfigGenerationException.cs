using System;
using System.Runtime.Serialization;
using Orchard;
using Orchard.Localization;

namespace ceenq.com.RoutingServer.Configuration
{
    public class ConfigGenerationException : OrchardException
    {
        public ConfigGenerationException(LocalizedString message) : base(message)
        {
        }

        public ConfigGenerationException(LocalizedString message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}