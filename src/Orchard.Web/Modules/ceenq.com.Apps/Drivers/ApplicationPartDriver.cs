using System.Collections.Generic;
using ceenq.com.Apps.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.Apps.Drivers
{
    public class ApplicationPartDriver : ContentPartDriver<ApplicationPart>
    {
        protected override string Prefix
        {
            get { return "ApplicationPart"; }
        }

        protected override DriverResult Editor(ApplicationPart part, dynamic shapeHelper)
        {
            var results = new List<DriverResult> {
                ContentShape("Parts_Application_ApplicationPart",
                             () => shapeHelper.EditorTemplate(TemplateName: "Parts.Application.ApplicationPart", Model: part, Prefix: Prefix))
            };


            if (part.Id > 0)
                results.Add(ContentShape("Application_DeleteButton",
                    deleteButton => deleteButton));

            return Combined(results.ToArray());
        }

        protected override DriverResult Editor(ApplicationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

    }
}