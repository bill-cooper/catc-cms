using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.Helpers;

namespace ceenq.com.Layouts.Elements
{
    public class Asset : Element
    {
        public override string Category
        {
            get { return "Content"; }
        }

        public override bool HasEditor
        {
            get { return true; }
        }

        public string Path
        {
            get { return this.Retrieve(x => x.Path); }
            set { this.Store(x => x.Path, value); }
        }

        public string Content
        {
            get;
            set;
        }
    }
}