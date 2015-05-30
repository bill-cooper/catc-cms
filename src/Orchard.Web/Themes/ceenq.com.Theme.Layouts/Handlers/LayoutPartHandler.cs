using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Title.Models;
using Orchard.Layouts.Models;
using Orchard.Layouts.Services;

namespace ceenq.com.Theme.Layouts.Handlers
{
    //public class LayoutActivatingFilter : ActivatingFilter<LayoutPart>, IContentActivatingFilter
    //{
    //    private readonly IContentDefinitionManager _contentDefinitionManager;
    //    public LayoutActivatingFilter(IContentDefinitionManager contentDefinitionManager)
    //    {
    //        _contentDefinitionManager = contentDefinitionManager;
    //    }

    //    public void Activating(ActivatingContentContext context)
    //    {
    //        var list = _contentDefinitionManager.ListTypeDefinitions();
    //        if(context.ContentType == "Page")
    //            context.Builder.Weld<LayoutPart>();
    //        else if (context.ContentType == "BlogPost")
    //            context.Builder.Weld<LayoutPart>();
    //    }
    //}

    public class LayoutPartHandler : ContentHandler
    {
        private readonly ILayoutManager _layoutManager;
        private readonly IOrchardServices _orchardServices;

        public LayoutPartHandler(ILayoutManager layoutManager, IOrchardServices orchardServices, IContentDefinitionManager contentDefinitionManager)
        {
            _layoutManager = layoutManager;
            _orchardServices = orchardServices;
            OnLoaded<LayoutPart>(AdjustSlate);
            //Filters.Add(new LayoutActivatingFilter(contentDefinitionManager));
        }

        private void AdjustSlate(LoadContentContext context, LayoutPart part)
        {
            
            var content = part.As<ContentItem>();

            if (content.ContentType != "Layout" && !_orchardServices.WorkContext.GetState<bool>("slated"))
            {
                var layout = _orchardServices.WorkContext.HttpContext.Request.Headers["layout"];
                if (layout != null)
                {
                    _orchardServices.WorkContext.SetState("slated", true);
                    var template = _layoutManager.GetTemplates().FirstOrDefault(t => t.As<TitlePart>().Title.ToLower().Trim().Replace(" ", "_") == layout.ToLower().Trim());
                    if (template != null)
                    {
                        var slate = template.LayoutData;
                        //replace fields with that of the current types matching fields
                        slate = slate.Replace("\"typeName\":\"Layout.", string.Format("\"typeName\":\"{0}.",content.ContentType));
                        part.LayoutData = slate;
                    }
                }

            }
        }




    }
}