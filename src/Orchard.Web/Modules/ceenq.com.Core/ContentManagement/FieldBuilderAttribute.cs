using System;
using System.Collections.Generic;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Localization;
using ceenq.com.Core.Data.Migration;

namespace ceenq.com.Core.ContentManagement
{
    public abstract class FieldBuilderAttribute : Attribute
    {
        protected readonly string Name;
        protected readonly string DisplayName;
        protected Dictionary<string, string> Settings;
        public Localizer T { get; set; }
        public string FieldName { get { return this.GetType().Name.Replace("BuilderAttribute", ""); } }

        protected FieldBuilderAttribute(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            Settings = new Dictionary<string, string>();
        }

        public virtual void AttachField(ContentPartDefinitionBuilder cpdb)
        {
            cpdb.WithField(Name, f => f.OfType(FieldName).WithSettings(Settings));
        }
    }
}