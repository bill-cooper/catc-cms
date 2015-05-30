using System.Collections.Generic;
using System.Linq;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.Framework.Harvesters;
using Orchard.Layouts.Services;

namespace ceenq.com.Layouts.Providers {

    [OrchardSuppressDependency("Orchard.Layouts.Providers.TypedElementHarvester")]
    public class TypedElementHarvester : ElementHarvester
    {
        private readonly Work<IElementManager> _elementManager;
        private readonly Work<IElementFactory> _factory;

        public TypedElementHarvester(Work<IElementManager> elementManager, Work<IElementFactory> factory)
        {
            _elementManager = elementManager;
            _factory = factory;
        }

        public IEnumerable<ElementDescriptor> HarvestElements(HarvestElementsContext context)
        {
            var drivers = _elementManager.Value.GetDrivers();
            var elementTypes = drivers
                .Select(x => x.GetType().BaseType.GenericTypeArguments[0])
                .Where(x => !x.IsAbstract && !x.IsInterface
                && !x.Name.Contains("Image")
                && !x.Name.Contains("Media")
                && !x.Name.Contains("Shape")
                && !x.Name.Contains("Paragraph")
                &&  x.Name != "Text"
                )
                .Distinct()
                .ToArray();

            return elementTypes.Select(elementType =>
            {
                var element = _factory.Value.Activate(elementType);
                return new ElementDescriptor(elementType, element.Type, element.DisplayText, element.Description, element.Category)
                {
                    GetDrivers = () => _elementManager.Value.GetDrivers(element),
                    IsSystemElement = element.IsSystemElement,
                    EnableEditorDialog = element.HasEditor,
                    ToolboxIcon = element.ToolboxIcon
                };
            });
        }
    }
}