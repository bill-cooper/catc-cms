using System;
using System.Collections.Generic;
using Orchard.Taxonomies.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions.Models;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Utility;

namespace ceenq.org.Resource.Initialization
{
    public class SeriesListDataInitializer : TaxonomyDataInitializer
    {
        public SeriesListDataInitializer(ITaxonomyService taxonomyService, IContentManager contentManager)
            : base(taxonomyService, contentManager) { }

        protected override ExtensionDescriptor ContainerExtension
        {
            get { return ModuleUtility.ContainerExtentionFor<SeriesListDataInitializer>(); }
        }

        protected override string TaxonomyName
        {
            get { return "Series"; }
        }

        protected override Lazy<List<string>> Terms
        {
            get
            {
                return new Lazy<List<string>>(() => new List<string>());
            }
        }


    }
}