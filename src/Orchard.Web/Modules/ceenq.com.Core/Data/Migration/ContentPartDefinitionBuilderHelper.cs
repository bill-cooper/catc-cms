using System.Collections.Generic;
using Orchard.ContentManagement.MetaData.Builders;

namespace ceenq.com.Core.Data.Migration
{
    public static class ContentPartDefinitionBuilderHelper
    {
        public static ContentPartFieldDefinitionBuilder WithSettings(this ContentPartFieldDefinitionBuilder cpfdb, Dictionary<string, string> settings)
        {
            foreach (var setting in settings)
                cpfdb.WithSetting(setting.Key, setting.Value);
            return cpfdb;
        }
    }
}
