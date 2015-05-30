using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.VirtualPath;
using Orchard.Layouts.Services;

namespace ceenq.com.Layouts.Services
{
    [OrchardSuppressDependency("Orchard.Layouts.Services.ContentFieldDisplay")]
    public class ContentFieldDisplay : ContentDisplayBase, IContentFieldDisplay
    {
        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;

        public ContentFieldDisplay(
            IShapeFactory shapeFactory,
            Lazy<IShapeTableLocator> shapeTableLocator,
            RequestContext requestContext,
            IVirtualPathProvider virtualPathProvider,
            IWorkContextAccessor workContextAccessor,
            IEnumerable<IContentFieldDriver> contentFieldDrivers)
            : base(shapeFactory, shapeTableLocator, requestContext, virtualPathProvider, workContextAccessor)
        {

            _contentFieldDrivers = contentFieldDrivers;
        }

        public override string DefaultStereotype
        {
            get { return "ContentField"; }
        }

        public dynamic BuildDisplay(IContent content, ContentField field, string displayType, string groupId)
        {
            var context = BuildDisplayContext(content, displayType, groupId);
            var drivers = GetFieldDrivers(field.FieldDefinition.Name);

            drivers.Invoke(driver =>
            {
                var result = driver.BuildDisplayShape(context);
                if (result != null)
                    result.Apply(context);
            }, Logger);

            //This is a hack that has been added to override the default functionality.  By default, multiple fields
            // could possibly be placed in this shape.  I have added the following filtering to remove any but
            // the one with the matching name
            if (context.Shape != null && context.Shape.Content != null)
            {
                for(int i = context.Shape.Content.Items.Count - 1; i >= 0; i--)
                {
                    if (context.Shape.Content.Items[i].ContentField.Name != field.Name)
                        context.Shape.Content.Items.Remove(context.Shape.Content.Items[i]);
                }
            }

            return context.Shape;
        }

        public dynamic BuildEditor(IContent content, ContentField field, string groupId)
        {
            var context = BuildEditorContext(content, groupId);
            var drivers = GetFieldDrivers(field.FieldDefinition.Name);

            drivers.Invoke(driver =>
            {
                var result = driver.BuildEditorShape(context);
                if (result != null)
                    result.Apply(context);
            }, Logger);

            return context.Shape;
        }

        public dynamic UpdateEditor(IContent content, ContentField field, IUpdateModel updater, string groupInfoId)
        {
            var context = UpdateEditorContext(content, updater, groupInfoId);
            var drivers = GetFieldDrivers(field.FieldDefinition.Name);

            drivers.Invoke(driver =>
            {
                var result = driver.UpdateEditorShape(context);
                if (result != null)
                    result.Apply(context);
            }, Logger);

            return context.Shape;
        }

        private IEnumerable<IContentFieldDriver> GetFieldDrivers(string fieldName)
        {
            return _contentFieldDrivers.Where(x => x.GetType().BaseType.GenericTypeArguments[0].Name == fieldName);
        }
    }
}
