using System;
using Orchard.ContentManagement.MetaData;
using ceenq.com.Core.ContentManagement;
using Orchard.Core.Contents.Extensions;
using ceenq.com.Core.Utility;

namespace ceenq.com.Core.Data.Migration
{
    public static class ContentDefinitionManagerHelper
    {
        public static void AlterPartDefinition<T>(this IContentDefinitionManager cdm)
        {
            var t = typeof(T);
            cdm.AlterPartDefinition(t.Name, p =>
                {
                    p.Attachable();
                    var alterPartDefinitionType = t.GetAlterPartDefinitionType();
                    if (alterPartDefinitionType == null)
                        throw new Exception(string.Format("No alter part diffinition was found for type {0}.  Are you mission the type {0}AlterPartDefinition ?",t.FullName));
                    
                    var fields = alterPartDefinitionType.GetFields();
                    foreach (var field in fields)
                    {
                        object[] attributes = field.GetCustomAttributes(false);
                        foreach (var attribute in attributes)
                        {
                            var fieldBuilderAttribute = attribute as FieldBuilderAttribute;
                            if (fieldBuilderAttribute != null)
                                fieldBuilderAttribute.AttachField(p);
                        }
                    }
                });

        }
    }
}
