using Orchard.Environment.Extensions;
using Orchard.Layouts.Elements;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;

namespace ceenq.com.Layouts.Drivers {

    [OrchardSuppressDependency("Orchard.Layouts.Drivers.ContentFieldElementDriver")]
    public class ContentFieldElementDriver : ElementDriver<ContentField> {
        protected override void OnDisplaying(ContentField element, ElementDisplayContext context)
        {
            base.OnDisplaying(element, context);
        }
    }
}