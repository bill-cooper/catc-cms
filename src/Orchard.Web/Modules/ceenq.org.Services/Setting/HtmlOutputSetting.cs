using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Setting
{
    public class HtmlOutputSetting : IOutputSetting, IDefaultOptimization
    {
        public HtmlOutputSetting()
        {
            IncludePassageReferences = true;
            IncludeFirstVerseNumbers = true;
            IncludeVerseNumbers = true;
            IncludeFootnotes = true;
            IncludeFootnoteLinks = true;
            IncludeHeadings = true;
            IncludeSubheadings = true;
            IncludeSurroundingChapters = false;
            IncludeWordIds = false;
            IncludeAudioLink = true;
            IncludeShortCopyright = true;
            IncludeCopyright = false;

            IncludePassageReferencesDefault = true;
            IncludeFirstVerseNumbersDefault = true;
            IncludeVerseNumbersDefault = true;
            IncludeFootnotesDefault = true;
            IncludeFootnoteLinksDefault = true;
            IncludeHeadingsDefault = true;
            IncludeSubheadingsDefault = true;
            IncludeSurroundingChaptersDefault = false;
            IncludeWordIdsDefault = false;
            IncludeAudioLinkDefault = true;
            IncludeShortCopyrightDefault = true;
            IncludeCopyrightDefault = false;
        }

        /// <summary>
        ///     Takes a value of flash, mp3, real, or wma to indicate the format of the audio. It defaults to flash, but the default could change; if you have a strong preference for one of these formats, we recommend that you specify it explicitly. We recommend flash as the most flexible; an embedded Flash player is included in the text. The mp3 option includes a link to an M3U file. However, this link expires after about 24 hours, making it unsuitable for extended caching. The other two options, real and wma, only have New Testament recordings by Marquis Laughlin available. (The real option refers to RealAudio, and wma refers to Windows Media.) (default: flash)
        /// </summary>
        public String AudioFormat { get; set; }

        /// <summary>
        ///     Which recording to use. The options are: mm (Max McLean, complete Bible), ml (Marquis Laughlin, New Testament only), and ml-mm (Max McLean for Old Testament, Marquis Laughlin for New Testament. Only affects the output if audio-format is flash or mp3. (Max McLean's version is only available in these two formats.) (default: mm)
        /// </summary>
        public String AudioVersion { get; set; }

        /// <summary>
        ///     Include 'h2's that indicate which passage is being displayed. For example: Isaiah 53:1-5. (default: true)
        /// </summary>
        public Boolean IncludePassageReferences { get; set; }

        /// <summary>
        ///     Show the verse number (e.g., 53:1) at the beginnings of chapters. (default: true)
        /// </summary>
        public Boolean IncludeFirstVerseNumbers { get; set; }

        /// <summary>
        ///     Show verse numbers in the text. (default: true)
        /// </summary>
        public Boolean IncludeVerseNumbers { get; set; }

        /// <summary>
        ///     Include footnotes and references to them in the text. (default: true)
        /// </summary>
        public Boolean IncludeFootnotes { get; set; }

        /// <summary>
        ///     If you have set Includefootnotes to true, set this option to false to turn off the links to the footnotes within the text. The footnotes will still appear at the bottom of the passage. If Includefootnotes is false, this parameter does not do anything. (default: true)
        /// </summary>
        public Boolean IncludeFootnoteLinks { get; set; }

        /// <summary>
        ///     Include section headings. For example, the section heading of Matthew 5 is The Sermon on the Mount. (default: true)
        /// </summary>
        public Boolean IncludeHeadings { get; set; }

        /// <summary>
        ///     Include subheadings. Subheadings are the titles of psalms (e.g., Psalm 73's A Maskil of Asaph), the acrostic divisions in Psalm 119, the speakers in Song of Solomon, and the textual notes that appear in John 7 and Mark 16. (default: true)
        /// </summary>
        public Boolean IncludeSubheadings { get; set; }

        /// <summary>
        ///     If you have set Includefootnotes to true, set this option to false to turn off the links to the footnotes within the text. The footnotes will still appear at the bottom of the passage. If Includefootnotes is false, this parameter does not do anything. (default: true)
        /// </summary>
        public Boolean IncludeSurroundingChapters { get; set; }

        /// <summary>
        ///     Include a 'span' tags surrounding each word with a unique id. The id has several parts; the id "w40001002.01-1" consists of: the letter w (needed for valid XHTML ids), an eight-digit verse identifier (40001002 indicates Matthew 1:2), a period (.), a two-digit word identifier (01), and a hyphen followed by a number (this number is incremented with each passage; it starts with 1). Footnotes do not have word ids. (default: false)
        /// </summary>
        public Boolean IncludeWordIds { get; set; }

        /// <summary>
        ///     Include a link to the audio version of the requested passage. The link appears in a 'small' tag in the passage's identifying 'h2'. (default: true)
        /// </summary>
        public Boolean IncludeAudioLink { get; set; }

        /// <summary>
        ///     Each passage from the Esv needs to include the letters "Esv" at the end of the passage. To turn off this behavior, set this option to false. (default: true)
        /// </summary>
        public Boolean IncludeShortCopyright { get; set; }

        /// <summary>
        ///     Show a copyright notice at the bottom of the text. Any page that shows the Esv text from this service needs to include a copyright notice, but you do not need to include it with each passage. Best practice is probably to include the copyright manually on your page, rather than download it every time. This option is mutually exclusive with Includeshort-copyright, which overrides Includecopyright. (default: false)
        /// </summary>
        public Boolean IncludeCopyright { get; set; }

        /// <summary>
        ///     Where embedded links to other passages should point. It currently applies only when Includesurrounding-chapters is set to true. (default: http://www.gnpcb.org/esv/search/)
        /// </summary>
        public String LinkUrl { get; set; }

        private Boolean IncludePassageReferencesDefault { get; set; }

        private Boolean IncludeFirstVerseNumbersDefault { get; set; }

        private Boolean IncludeVerseNumbersDefault { get; set; }

        private Boolean IncludeFootnotesDefault { get; set; }

        private Boolean IncludeFootnoteLinksDefault { get; set; }

        private Boolean IncludeHeadingsDefault { get; set; }

        private Boolean IncludeSubheadingsDefault { get; set; }

        private Boolean IncludeSurroundingChaptersDefault { get; set; }

        private Boolean IncludeWordIdsDefault { get; set; }

        private String LinkUrlDefault { get; set; }

        private Boolean IncludeAudioLinkDefault { get; set; }

        private String AudioFormatDefault { get; set; }

        private String AudioVersionDefault { get; set; }

        private Boolean IncludeShortCopyrightDefault { get; set; }

        private Boolean IncludeCopyrightDefault { get; set; }

        /// <summary>
        ///     Time in seconds to wait before failing the service call
        /// </summary>
        public Int32 Timeout { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (IncludePassageReferences != IncludePassageReferencesDefault) ArgumentListManager.AddArgument(d, "include-passage-references", IncludePassageReferences);
            if (IncludeFirstVerseNumbers != IncludeFirstVerseNumbersDefault) ArgumentListManager.AddArgument(d, "include-first-verse-numbers", IncludeFirstVerseNumbers);
            if (IncludeVerseNumbers != IncludeVerseNumbersDefault) ArgumentListManager.AddArgument(d, "include-verse-numbers", IncludeVerseNumbers);
            if (IncludeFootnotes != IncludeFootnotesDefault) ArgumentListManager.AddArgument(d, "include-footnotes", IncludeFootnotes);
            if (IncludeFootnoteLinks != IncludeFootnoteLinksDefault) ArgumentListManager.AddArgument(d, "include-footnote-links", IncludeFootnoteLinks);
            if (IncludeHeadings != IncludeHeadingsDefault) ArgumentListManager.AddArgument(d, "include-headings", IncludeHeadings);
            if (IncludeSubheadings != IncludeSubheadingsDefault) ArgumentListManager.AddArgument(d, "include-subheadings", IncludeSubheadings);
            if (IncludeSurroundingChapters != IncludeSurroundingChaptersDefault) ArgumentListManager.AddArgument(d, "include-surrounding-chapters", IncludeSurroundingChapters);
            if (IncludeWordIds != IncludeWordIdsDefault) ArgumentListManager.AddArgument(d, "include-word-ids", IncludeWordIds);
            if (IncludeAudioLink != IncludeAudioLinkDefault) ArgumentListManager.AddArgument(d, "include-audio-link", IncludeAudioLink);
            if (IncludeShortCopyright != IncludeShortCopyrightDefault) ArgumentListManager.AddArgument(d, "include-short-copyright", IncludeShortCopyright);
            if (IncludeCopyright != IncludeCopyrightDefault) ArgumentListManager.AddArgument(d, "include-copyright", IncludeCopyright);
            if (!String.IsNullOrEmpty(LinkUrl)) ArgumentListManager.AddArgument(d, "link-url", LinkUrl);
            if (!String.IsNullOrEmpty(AudioFormat)) ArgumentListManager.AddArgument(d, "audio-format", AudioFormat);
            if (!String.IsNullOrEmpty(AudioVersion)) ArgumentListManager.AddArgument(d, "audio-version", AudioVersion);
            return d;
        }
    }
}