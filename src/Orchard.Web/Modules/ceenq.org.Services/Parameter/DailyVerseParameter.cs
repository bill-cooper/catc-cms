using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the dailyVerse function, providing a new verse daily.
    /// </summary>
    public class DailyVerseParameter : Parameter, IDefaultOptimization
    {
        public DailyVerseParameter()
        {
            IncludeHeadings = false;
            CorrectCapitalization = true;
            CorrectEndPunctuation = true;
            CorrectQuotes = true;

            IncludeHeadingsDefault = false;
            CorrectCapitalizationDefault = true;
            CorrectEndPunctuationDefault = true;
            CorrectQuotesDefault = true;
        }

        /// <summary>
        ///     If the first letter of the verse is lower-case, indicating that the verse begins in the middle of a sentence, you can prepend a string to the verse. For example, you might set this option to &ellipsis; or ... to begin such verses with an ellipsis. (default: An empty string)
        /// </summary>
        public String BeginCharacter { get; set; }

        /// <summary>
        ///     If the first letter of the verse is lower-case, setting this option to true will upper-case it. (default: true)
        /// </summary>
        public Boolean CorrectCapitalization { get; set; }

        /// <summary>
        ///     If the verse ends with anything besides a period, question mark, or exclamation mark (for example, a comma, semicolon, or dash), it replaces the ending punctuation with whatever you set in end-character. (default: true)
        /// </summary>
        public Boolean CorrectEndPunctuation { get; set; }

        /// <summary>
        ///     Balances quotes. If a quotation beginning in a previous verse ends in the current verse, an opening quotation mark is added to the beginning of the verse. Similarly, an ending quotation mark is added to the end of the verse if the quotation begins in the verse but extends beyond the end of the verse. So, all quotations have both beginning and ending marks. (default: true)
        /// </summary>
        public Boolean CorrectQuotes { get; set; }

        /// <summary>
        ///     If the verse ends with anything besides a period, question mark, or exclamation mark (for example, a comma, semicolon, or dash), the ending punctuation will be replaced with this string. (default: &ellipsis4;)
        /// </summary>
        public String EndCharacter { get; set; }

        /// <summary>
        ///     Whether to include the section headings (e.g., Jesus Feeds the Five Thousand in John 6). (default: false)
        /// </summary>
        public Boolean IncludeHeadings { get; set; }

        private Boolean IncludeHeadingsDefault { get; set; }

        private String BeginCharacterDefault { get; set; }

        private Boolean CorrectCapitalizationDefault { get; set; }

        private Boolean CorrectEndPunctuationDefault { get; set; }

        private Boolean CorrectQuotesDefault { get; set; }

        private String EndCharacterDefault { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (IncludeHeadings != IncludeHeadingsDefault) ArgumentListManager.AddArgument(d, "include-headings", IncludeHeadings);
            if (!String.IsNullOrEmpty(BeginCharacter)) ArgumentListManager.AddArgument(d, "begin-character", BeginCharacter);
            if (CorrectCapitalization != CorrectCapitalization) ArgumentListManager.AddArgument(d, "correct-capitalization", CorrectCapitalization);
            if (CorrectEndPunctuation != CorrectEndPunctuation) ArgumentListManager.AddArgument(d, "correct-end-punctuation", CorrectEndPunctuation);
            if (!String.IsNullOrEmpty(EndCharacter)) ArgumentListManager.AddArgument(d, "end-character", EndCharacter);
            if (CorrectQuotes != CorrectQuotes) ArgumentListManager.AddArgument(d, "correct-quotes", CorrectQuotes);
            return d;
        }
    }
}