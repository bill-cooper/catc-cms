using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Setting
{
    public class CrosswayXmlVersion10Setting : IOutputSetting, IDefaultOptimization
    {
        public CrosswayXmlVersion10Setting()
        {
            IncludeXmlDeclaration = false;
            IncludeDocType = true;
            IncludeQuoteEntities = true;
            IncludeSimpleEntities = true;
            IncludeCrossReferences = false;
            IncludeLineBreaks = true;
            IncludeWordIds = false;
            IncludeVirtualAttributes = false;

            IncludeXmlDeclarationDefault = false;
            IncludeDocTypeDefault = true;
            IncludeQuoteEntitiesDefault = true;
            IncludeSimpleEntitiesDefault = false;
            IncludeCrossReferencesDefault = false;
            IncludeLineBreaksDefault = true;
            IncludeWordIdsDefault = false;
            IncludeVirtualAttributesDefault = false;
        }

        /// <summary>
        ///     Set to paragraph to transform the output from verse-unit-based markup to paragraph-based markup. In doing so, you lose a little bit of information--you can no longer tell whether the end of a passage indicates the true end of a paragraph or block indent in the text, or whether it was put there to allow the proper closing of tags. That is to say, there is no longer 'end-(paragraph|block-indent) class="virtual"/'. (default: verse-unit)
        /// </summary>
        public String BaseElement { get; set; }

        /// <summary>
        ///     Currently, the cross-references don't point to anything, so there's little reason to include them. (default: false)
        /// </summary>
        public Boolean IncludeCrossReferences { get; set; }

        /// <summary>
        ///     Include a DOCTYPE declaration. Note that it links to a DTD that only explains the character entities, not the elements. (default: true)
        /// </summary>
        public Boolean IncludeDocType { get; set; }

        /// <summary>
        ///     Include line breaks in the XML output. It doesn't make any functional difference; it's just easier to read. (default: false)
        /// </summary>
        public Boolean IncludeLineBreaks { get; set; }

        /// <summary>
        ///     The original XML files contain an elaborate system of marking where quotations begin and end. This information is unnecessary for most purposes. Leaving this option set to true transforms the quotations from 'q' elements to the entities referenced in the DTD (e.g., 'ldblquot' for an opening double quotation mark). Setting this option to false leaves in the 'q' tags, as described in the XML Schema. (default: true)
        /// </summary>
        public Boolean IncludeQuoteEntities { get; set; }

        /// <summary>
        ///     Translates character entities to native XML entities (e.g., 'ldblquot' becomes 'quot') for non-DTD-aware parsers. You can see the specific transformations we do in our PHP function translate_entities_to_simple_xml(). If you also disable the option include-quote-entities, the quote entities are translated to 'q' tags before applying this transformation. (default: false)
        /// </summary>
        public Boolean IncludeSimpleEntities { get; set; }

        /// <summary>
        ///     Include word ids ('w' tags) around each word in the text. The wid attribute consists of an eight-digit verse identifier, followed by a period (.) and a two-digit word identifier. The wids are unique for each word in the Bible; however, if you call the same passage more than once, the wids will repeat. In other words, two instances of Matthew 1:2 will have 'w' tags with identical wid values. (default: false)
        /// </summary>
        public Boolean IncludeWordIds { get; set; }

        /// <summary>
        ///     All begin- and end- tags must match--for example, a 'begin-paragraph' tag always has a corresponding 'end-paragraph' tag. If a tag had to be added programmatically (for example, if the requested passage ends in the middle of a paragraph), a virtual="virtual" attribute is added to the tag (for example, 'end-paragraph virtual="virtual"'). If you want to know whether a given tag was added programmatically and doesn't really exist in the text, set this option to true. (default: false)
        /// </summary>
        public Boolean IncludeVirtualAttributes { get; set; }

        /// <summary>
        ///     Include an XML declaration at the top of the document. (default: false)
        /// </summary>
        public Boolean IncludeXmlDeclaration { get; set; }

        private String BaseElementDefault { get; set; }

        private Boolean IncludeXmlDeclarationDefault { get; set; }

        private Boolean IncludeDocTypeDefault { get; set; }

        private Boolean IncludeQuoteEntitiesDefault { get; set; }

        private Boolean IncludeSimpleEntitiesDefault { get; set; }

        private Boolean IncludeCrossReferencesDefault { get; set; }

        private Boolean IncludeLineBreaksDefault { get; set; }

        private Boolean IncludeWordIdsDefault { get; set; }

        private Boolean IncludeVirtualAttributesDefault { get; set; }

        /// <summary>
        ///     Time in seconds to wait before failing the service call
        /// </summary>
        public Int32 Timeout { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            ArgumentListManager.AddArgument(d, "output-format", "crossway-xml-1.0");
            if (IncludeXmlDeclaration != IncludeXmlDeclarationDefault) ArgumentListManager.AddArgument(d, "include-xml-declaration", IncludeXmlDeclaration);
            if (IncludeDocType != IncludeDocTypeDefault) ArgumentListManager.AddArgument(d, "include-doctype", IncludeDocType);
            if (IncludeQuoteEntities != IncludeQuoteEntitiesDefault) ArgumentListManager.AddArgument(d, "include-quote-entities", IncludeQuoteEntities);
            if (IncludeSimpleEntities != IncludeSimpleEntitiesDefault) ArgumentListManager.AddArgument(d, "include-simple-entities", IncludeSimpleEntities);
            if (IncludeCrossReferences != IncludeCrossReferencesDefault) ArgumentListManager.AddArgument(d, "include-cross-references", IncludeCrossReferences);
            if (IncludeLineBreaks != IncludeLineBreaksDefault) ArgumentListManager.AddArgument(d, "include-line-breaks", IncludeLineBreaks);
            if (IncludeWordIds != IncludeWordIdsDefault) ArgumentListManager.AddArgument(d, "include-word-ids", IncludeWordIds);
            if (IncludeVirtualAttributes != IncludeVirtualAttributesDefault) ArgumentListManager.AddArgument(d, "include-virtual-attributes", IncludeVirtualAttributes);
            if (!String.IsNullOrEmpty(BaseElement)) ArgumentListManager.AddArgument(d, "base-element", BaseElement);
            return d;
        }
    }
}