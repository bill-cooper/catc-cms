using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ceenq.com.Theme.Admin.Models
{
    public class AdminThemeSettingsPart : ContentPart<AdminThemeSettingsPartRecord>
    {
        public string LogoUrl
        {
            get { return Record.LogoUrl; }
            set { Record.LogoUrl = value; }
        }

        public string Brand
        {
            get { return Record.Brand; }
            set { Record.Brand = value; }
        }

        public string Footer
        {
            get { return Record.Footer; }
            set { Record.Footer = value; }
        }

        public string ProductCssUrl
        {
            get { return Record.ProductCssUrl; }
            set { Record.ProductCssUrl = value; }
        }
    }

    public class AdminThemeSettingsPartRecord : ContentPartRecord
    {
        public virtual string LogoUrl { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Footer { get; set; }
        public virtual string ProductCssUrl { get; set; }
    }
}