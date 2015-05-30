using System;
using System.Text.RegularExpressions;
using Orchard.Autoroute.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Utility.Extensions;

namespace ceenq.com.AppRoutingServer.Services {
    [OrchardSuppressDependency("Orchard.Autoroute.Services.DefaultSlugService")]
    public class SlugService : ISlugService {

        private readonly ISlugEventHandler _slugEventHandler;

        public SlugService(
            ISlugEventHandler slugEventHander
            ) {
                _slugEventHandler = slugEventHander;
        }

        public string Slugify(IContent content) {
            var metadata = content.ContentItem.ContentManager.GetItemMetadata(content);
            if (metadata == null || String.IsNullOrEmpty(metadata.DisplayText)) return null;
            var title = metadata.DisplayText.Trim();
            return Slugify(new FillSlugContext(content,title));
        }

        private string Slugify(FillSlugContext slugContext) {
            _slugEventHandler.FillingSlugFromTitle(slugContext);

            if (!slugContext.Adjusted) {

                var disallowed = new Regex(@"[:?#\[\]@!$&'()*+,;=\s\""\<\>\\\|%]+");

                var cleanedSlug = disallowed.Replace(slugContext.Title, "-").Trim('-','.');

                slugContext.Slug = Regex.Replace(cleanedSlug, @"\-{2,}", "-");

                if (slugContext.Slug.Length > 1000) {
                    slugContext.Slug = slugContext.Slug.Substring(0, 1000).Trim('-', '.');
				}

                slugContext.Slug = slugContext.Slug.ToLower().RemoveDiacritics();
            }
            
            _slugEventHandler.FilledSlugFromTitle(slugContext);

            return slugContext.Slug;
        }

        public string Slugify(string text) {
            return Slugify(new FillSlugContext(null, text));
        }
       
    }
}
