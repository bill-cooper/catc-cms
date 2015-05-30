using System;

namespace ceenq.com.Core.Utility
{
    public static class TypeHelper
    {
        public static string Name<T>() {
            return typeof(T).Name;
        }

        public static Type GetAlterPartDefinitionType(this Type partType)
        {
            return
                Type.GetType(partType.AssemblyQualifiedName.Replace(partType.FullName,
                                                                    partType.FullName + "AlterPartDefinition"));
        }
    }
}
