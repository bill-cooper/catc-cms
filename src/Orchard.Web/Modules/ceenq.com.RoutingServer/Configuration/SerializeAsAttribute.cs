using System;

namespace ceenq.com.RoutingServer.Configuration {
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializeAsAttribute : Attribute
    {
        private readonly SerializationFormat _format;

        public SerializeAsAttribute(SerializationFormat format)
        {
            _format = format;
        }

        public SerializationFormat Format
        {
            get { return _format; }
        }
    }
}