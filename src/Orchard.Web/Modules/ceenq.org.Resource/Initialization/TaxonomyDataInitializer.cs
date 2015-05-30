using System;
using System.Collections.Generic;
using ceenq.com.Core.Environment;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions.Models;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace ceenq.org.Resource.Initialization
{
    public abstract class  TaxonomyDataInitializer : DataInitializer
    {
        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;

        protected TaxonomyDataInitializer(ITaxonomyService taxonomyService, IContentManager contentManager)
        {
            _taxonomyService = taxonomyService;
            _contentManager = contentManager;
        }

        protected abstract string TaxonomyName { get; }
        protected abstract Lazy<List<string>> Terms { get; }

        public override void Enabled(Feature feature)
        {
            if (feature.Descriptor.Name != ContainerExtension.Name) return; // only want to execute this initialization for the proper feature

            if (_taxonomyService.GetTaxonomyByName(TaxonomyName) == null)
            {
                var taxonomy = _contentManager.New<TaxonomyPart>("Taxonomy");
                taxonomy.Name = TaxonomyName;
                _contentManager.Create(taxonomy, VersionOptions.Published);

                foreach (var termName in Terms.Value)
                {
                    var term = _taxonomyService.NewTerm(taxonomy);
                    term.Container = taxonomy.ContentItem;
                    term.Name = termName;
                    term.Path = "/";
                    _taxonomyService.ProcessPath(term);
                    _contentManager.Create(term, VersionOptions.Published);
                }
            }
        }
    }
}
