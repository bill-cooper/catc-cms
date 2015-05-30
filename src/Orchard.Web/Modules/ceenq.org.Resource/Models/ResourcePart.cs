using System;
using Orchard.ContentManagement;
using ceenq.com.Core.ContentManagement;
using Orchard.ContentManagement.Records;

namespace ceenq.org.Resource.Models
{
    public sealed class ResourcePart : ContentPart<ResourcePartRecord>
    {
        public DateTime? DeliveredUtc
        {
            get { return Record.DeliveredUtc; }
            set { Record.DeliveredUtc = value; }
        }
        public string CorrespondingTexts
        {
            get { return Record.CorrespondingTexts; }
            set { Record.CorrespondingTexts = value; }
        }
    }


    public class ResourcePartAlterPartDefinition
    {
        [TaxonomyFieldBuilder(name: "Book", displayName: "Book", allowCustomTerms: false)]
        public WithField Book;

        [TaxonomyFieldBuilder(name: "Series", displayName: "Series",selectionMode: TaxonomyFieldBuilderAttribute.SelectionMode.SingleSelection)]
        public WithField Series;

        [TaxonomyFieldBuilder(name: "Speaker", displayName: "Speaker", selectionMode: TaxonomyFieldBuilderAttribute.SelectionMode.SingleSelection, hint:"Please provide the speaker/teacher.")]
        public WithField Speaker;
    }

    public class ResourcePartRecord : ContentPartRecord
    {
        public virtual DateTime? DeliveredUtc { get; set; }
        public virtual string CorrespondingTexts { get; set; }
    }
}