using System.Web.Http;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ceenq.com.Core.Api
{
    public class JsonFormattingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}