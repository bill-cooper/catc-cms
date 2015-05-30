using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Setting
{
    public class PlainTextSetting : IOutputSetting, IDefaultOptimization
    {
        public PlainTextSetting()
        {
            IncludePassageReferences = true;
            IncludeFirstVerseNumbers = true;
            IncludeVerseNumbers = true;
            IncludeFootnotes = true;
            IncludeShortCopyright = true;
            IncludeCopyright = false;
            IncludePassageHorizontalLines = true;
            IncludeHeadingHorizontalLines = true;
            IncludeHeadings = true;
            IncludeSubheadings = true;
            IncludeSelahs = true;
            IncludeContentType = true;
            LineLength = 74;

            IncludePassageReferencesDefault = true;
            IncludeFirstVerseNumbersDefault = true;
            IncludeVerseNumbersDefault = true;
            IncludeFootnotesDefault = true;
            IncludeShortCopyrightDefault = true;
            IncludeCopyrightDefault = false;
            IncludePassageHorizontalLinesDefault = true;
            IncludeHeadingHorizontalLinesDefault = true;
            IncludeHeadingsDefault = true;
            IncludeSubheadingsDefault = true;
            IncludeSelahsDefault = true;
            IncludeContentTypeDefault = true;
            LineLengthDefault = 74;
        }

        /// <summary>
        ///     Adds a Content-Type: text/plain HTTP header. Set to false if you want the default content type. (For example, in an RSS feed you want the content type to remain text/xml). (default: true)
        /// </summary>
        public Boolean IncludeContentType { get; set; }

        /// <summary>
        ///     Show a copyright notice at the bottom of the text. Any page that shows the Esv text from this service needs to include a copyright notice, but you do not need to include it with each passage. Best practice is probably to include the copyright manually on your page, rather than download it every time. This option is mutually exclusive with include-short-copyright, which overrides include-copyright. (default: false)
        /// </summary>
        public Boolean IncludeCopyright { get; set; }

        /// <summary>
        ///     Show the verse number at the beginnings of chapters. In the example below, [23:1]. (default: true)
        /// </summary>
        public Boolean IncludeFirstVerseNumbers { get; set; }

        /// <summary>
        ///     Show references to footnotes in the text in parentheses, and include the text of the footnotes at the bottom of the text. (default: true)
        /// </summary>
        public Boolean IncludeFootnotes { get; set; }

        /// <summary>
        ///     Each passage from the Esv needs to include the letters "Esv" at the end of the passage. To turn off this behavior, set this option to false. (default: true)
        /// </summary>
        public Boolean IncludeShortCopyright { get; set; }

        /// <summary>
        ///     Include a line of underscores (___) above each section heading. (default: true)
        /// </summary>
        public Boolean IncludeHeadingHorizontalLines { get; set; }

        /// <summary>
        ///     Include section headings. In the below example, Seven Woes to the Scribes and Pharisees. (default: true)
        /// </summary>
        public Boolean IncludeHeadings { get; set; }

        /// <summary>
        ///     Include a line of equals signs (===) above the beginning of each passage. (default: true)
        /// </summary>
        public Boolean IncludePassageHorizontalLines { get; set; }

        /// <summary>
        ///     Include a line saying which passage is being displayed. In the example below, Matthew 22:43 - 23:1. (default: true)
        /// </summary>
        public Boolean IncludePassageReferences { get; set; }

        /// <summary>
        ///     Include subheadings. Subheadings are the titles of psalms (e.g., Psalm 73's A Maskil of Asaph), the acrostic divisions in Psalm 119, the speakers in Song of Solomon, and the textual notes that appear in John 7 and Mark 16. (default: true)
        /// </summary>
        public Boolean IncludeSubheadings { get; set; }

        /// <summary>
        ///     Include Selahs, which are found mostly in the Psalms (e.g., Psalm 46:3). (default: true)
        /// </summary>
        public Boolean IncludeSelahs { get; set; }

        /// <summary>
        ///     Show verse numbers in brackets. (default: true)
        /// </summary>
        public Boolean IncludeVerseNumbers { get; set; }

        /// <summary>
        ///     How long to make each line. Can be any integer 36 and over. Set to 0 for unlimited length--each paragraph or line of poetry will be on a separate line. (default: true)
        /// </summary>
        public Int32 LineLength { get; set; }

        private Boolean IncludePassageReferencesDefault { get; set; }

        private Boolean IncludeFirstVerseNumbersDefault { get; set; }

        private Boolean IncludeVerseNumbersDefault { get; set; }

        private Boolean IncludeFootnotesDefault { get; set; }

        private Boolean IncludeShortCopyrightDefault { get; set; }

        private Boolean IncludeCopyrightDefault { get; set; }

        private Boolean IncludePassageHorizontalLinesDefault { get; set; }

        private Boolean IncludeHeadingHorizontalLinesDefault { get; set; }

        private Boolean IncludeHeadingsDefault { get; set; }

        private Boolean IncludeSubheadingsDefault { get; set; }

        private Boolean IncludeSelahsDefault { get; set; }

        private Boolean IncludeContentTypeDefault { get; set; }

        private Int32 LineLengthDefault { get; set; }

        /// <summary>
        ///     Time in seconds to wait before failing the service call
        /// </summary>
        public Int32 Timeout { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            ArgumentListManager.AddArgument(d, "output-format", "plain-text");
            if (IncludePassageReferences != IncludePassageReferencesDefault) ArgumentListManager.AddArgument(d, "include-passage-references", IncludePassageReferences);
            if (IncludeFirstVerseNumbers != IncludeFirstVerseNumbersDefault) ArgumentListManager.AddArgument(d, "include-first-verse-numbers", IncludeFirstVerseNumbers);
            if (IncludeVerseNumbers != IncludeVerseNumbersDefault) ArgumentListManager.AddArgument(d, "include-verse-numbers", IncludeVerseNumbers);
            if (IncludeFootnotes != IncludeFootnotesDefault) ArgumentListManager.AddArgument(d, "include-footnotes", IncludeFootnotes);
            if (IncludeShortCopyright != IncludeShortCopyrightDefault) ArgumentListManager.AddArgument(d, "include-short-copyright", IncludeShortCopyright);
            if (IncludeCopyright != IncludeCopyrightDefault) ArgumentListManager.AddArgument(d, "include-copyright", IncludeCopyright);
            if (IncludePassageHorizontalLines != IncludePassageHorizontalLinesDefault) ArgumentListManager.AddArgument(d, "include-passage-horizontal-lines", IncludePassageHorizontalLines);
            if (IncludeHeadingHorizontalLines != IncludeHeadingHorizontalLinesDefault) ArgumentListManager.AddArgument(d, "include-heading-horizontal-lines", IncludeHeadingHorizontalLines);
            if (IncludeHeadings != IncludeHeadingsDefault) ArgumentListManager.AddArgument(d, "include-headings", IncludeHeadings);
            if (IncludeSubheadings != IncludeSubheadingsDefault) ArgumentListManager.AddArgument(d, "include-subheadings", IncludeSubheadings);
            if (IncludeSelahs != IncludeSelahsDefault) ArgumentListManager.AddArgument(d, "include-selahs", IncludeSelahs);
            if (IncludeContentType != IncludeContentTypeDefault) ArgumentListManager.AddArgument(d, "include-content-type", IncludeContentType);
            if (LineLength != LineLengthDefault) ArgumentListManager.AddArgument(d, "line-length", LineLength);
            return d;
        }
    }
}