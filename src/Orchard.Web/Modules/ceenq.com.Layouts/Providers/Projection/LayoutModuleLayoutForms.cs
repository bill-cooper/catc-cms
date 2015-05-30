using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Layouts.Services;
using Orchard.Localization;

namespace ceenq.com.Layouts.Providers.Projection {

    public class LayoutModuleLayoutForms : IFormProvider {
        protected dynamic Shape { get; set; }
        private readonly ILayoutManager _layoutManager;
        public Localizer T { get; set; }

        public LayoutModuleLayoutForms(
            IShapeFactory shapeFactory, ILayoutManager layoutManager) {
            Shape = shapeFactory;
            _layoutManager = layoutManager;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context) {
            Func<IShapeFactory, object> form =
                shape => {



                    var f = Shape.Form(
                        Id: "layoutmodulelayout",
                        _Parts: Shape.SelectList(
                            Id: "layout-name", Name: "LayoutName",
                            Title: T("Layout Name"),
                            Description: T("Select a layout name.")
                            )
                        );


                    foreach (var layout in _layoutManager.GetTemplates().OrderBy(t => t.As<TitlePart>().Title))
                    {
                        f._Parts.Add(new SelectListItem { Value = layout.As<TitlePart>().Title, Text = layout.As<TitlePart>().Title });
                    }

                    return f;


                };

            context.Form("layoutmodulelayout", form);

        }
    }
}