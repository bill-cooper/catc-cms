using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ceenq.com.Core.Extensions;
using ceenq.com.RoutingServer.Configuration;
using Orchard;

namespace ceenq.com.AppRoutingServer.Services {
    public interface INginxConfigSerializer : IDependency
    {
        string Serialize(Config configuration);
        void Serialize(Stream stream, object data);
        void Serialize(StreamWriter sw, object data);
    }

    public class NginxConfigSerializer : IDisposable, INginxConfigSerializer {

        private readonly INginxConfigPrettyFormatter _nginxConfigPrettyFormatter;
        private StreamWriter _sw;

        public NginxConfigSerializer(INginxConfigPrettyFormatter nginxConfigPrettyFormatter)
        {
            _nginxConfigPrettyFormatter = nginxConfigPrettyFormatter;
        }

        public string Serialize(Config configuration)
        {
            string output;
            using (var memoryStream = new MemoryStream())
            {
                Serialize(memoryStream, configuration);
                memoryStream.Flush();
                memoryStream.Position = 0;
                var streamReader = new StreamReader(memoryStream);

                output = streamReader.ReadToEnd();
                output = _nginxConfigPrettyFormatter.Format(output);
            }
            return output;
        }
        public void Serialize(Stream stream, object data)
        {
            _sw = new StreamWriter(stream);
            Serialize(_sw, data);
            _sw.Flush();
        }

        public void Serialize(StreamWriter sw, object data)
        {
            var type = data.GetType();
            var propertyDescriptors = type.GetPublicProperties().Where(p => p.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                .Select((p) =>
                {
                    var name = p.GetCustomAttribute<DataMemberAttribute>() == null ? p.Name.ToLower() : p.GetCustomAttribute<DataMemberAttribute>().Name;
                    var format = p.GetCustomAttribute<SerializeAsAttribute>() == null ? SerializationFormat.Directive : p.GetCustomAttribute<SerializeAsAttribute>().Format;
                    var value = p.GetValue(data);

                    var newValue = string.Empty;
                    if (p.IsIEnumerable() && format == SerializationFormat.Directive)
                    {
                        newValue = ((IEnumerable)value).Cast<object>().Aggregate(newValue, (current, item) => current + (item.ToString() + " "));
                        value = string.IsNullOrWhiteSpace(newValue) ? null : newValue.Trim();
                    }
                    return new
                    {
                        PropertyInfo = p,
                        Name = name,
                        Value = value,
                        Format = format,
                    };
                });

            foreach (var propertyDescriptor in propertyDescriptors)
            {
                //do not render properties that had a value of null.
                if (propertyDescriptor.Value == null) continue;
                switch (propertyDescriptor.Format)
                {
                    case SerializationFormat.Directive:
                        if (propertyDescriptor.PropertyInfo.GetCustomAttribute<PreSectionModifierAttribute>() != null)
                            break;
                        sw.Write(propertyDescriptor.Name);
                        sw.Write(' ');
                        sw.Write(propertyDescriptor.Value);
                        sw.Write(';');
                        sw.Write(Environment.NewLine);
                        sw.Flush();
                        break;
                    case SerializationFormat.DirectiveArray:
                        foreach (var item in (IEnumerable)propertyDescriptor.Value)
                        {
                            sw.Write(propertyDescriptor.Name);
                            sw.Write(' ');
                            sw.Write(item);
                            sw.Write(';');
                            sw.Write(Environment.NewLine);
                            sw.Flush();
                        }
                        break;
                    case SerializationFormat.SectionArray:
                        foreach (var item in (IEnumerable)propertyDescriptor.Value)
                        {
                            string sectionBegin = " {";
                            var presectionModifier = item.GetType().GetPublicProperties().FirstOrDefault(p => p.GetCustomAttribute<PreSectionModifierAttribute>() != null);
                            if (presectionModifier != null)
                            {
                                sectionBegin = " " + presectionModifier.GetValue(item) + sectionBegin;
                            }

                            sw.Write(propertyDescriptor.Name);
                            sw.Write(sectionBegin);
                            sw.Flush();
                            new NginxConfigSerializer(_nginxConfigPrettyFormatter).Serialize(sw, item);
                            sw.Write('}');
                            sw.Write(Environment.NewLine);
                            sw.Flush();
                        }
                        break;
                }
            }
        }

        public void Dispose()
        {
            if (_sw != null)
            {
                _sw.Close();
                _sw.Dispose();
            }
        }
    }
}