using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the query function, looking up a passage or showing word-search results, depending on the query.
    /// </summary>
    public class QueryParameter : Parameter, IDefaultOptimization
    {
        private String q;

        public QueryParameter()
        {
            Page = 1;
            ResultsPerPage = 25;

            PageDefault = 1;
            ResultsPerPageDefault = 25;
        }

        /// <summary>
        ///     Keywords with which to query.
        /// </summary>
        public String Q
        {
            get
            {
                if (String.IsNullOrEmpty(q))
                {
                    throw new EsvBibleException("Q (the string to search for) is required");
                }
                return q;
            }
            set { q = value; }
        }

        /// <summary>
        ///     Specifies what link to use when linking from the search results page to another page. If the page that handles your requests is named bible.search.php, for example, set this option to bible.search.php. (default: /esv/search/)
        /// </summary>
        public String LinkUrl { get; set; }

        /// <summary>
        ///     Exclude verses containing certain words. Can only be used in conjunction with the words and phrase options.
        /// </summary>
        public String NotWords { get; set; }

        /// <summary>
        ///     [Empty] matches anywhere in words (the finds father). beginning matches beginnings of words (the finds there). exact matches exact words only (the only finds the). The latter two options are slower.
        /// </summary>
        public String Matches { get; set; }

        /// <summary>
        ///     Limit your search to certain sections of the Bible. Accepts the predefined inputs listed in the dropdown menu on the Advanced Search page at http://www.gnpcb.org/esv/advanced.search/. (default: [Complete Bible])
        /// </summary>
        public String Scope { get; set; }

        /// <summary>
        ///     'all' searches the complete text of the Bible, including headings and footnotes. Set to text to exclude headings and footnotes.( default: all)
        /// </summary>
        public String SearchText { get; set; }

        /// <summary>
        ///     If the results span more than one page (e.g., 27 results were found, but only 25 are shown per page), set the page number (starting at 1) to show a different page of results. (default: 1)
        /// </summary>
        public Int32 Page { get; set; }

        /// <summary>
        ///     Put a string in here to force a passage search. This behavior is otherwise identical to passageQuery.
        /// </summary>
        public String Passage { get; set; }

        /// <summary>
        ///     Search for an exact phrase. In general, you can also surround words in q with quotation marks to mimic this functionality.
        /// </summary>
        public String Phrase { get; set; }

        /// <summary>
        ///     How many search results to show per page. Must be between 2 and 100 (inclusive). (default: 24)
        /// </summary>
        public Int32 ResultsPerPage { get; set; }

        /// <summary>
        ///     Put a string in here to force a word search.
        /// </summary>
        public String Words { get; set; }

        private Int32 PageDefault { get; set; }

        private Int32 ResultsPerPageDefault { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(Q)) ArgumentListManager.AddArgument(d, "q", Q);
            if (!String.IsNullOrEmpty(Passage)) ArgumentListManager.AddArgument(d, "passage", Passage);
            if (!String.IsNullOrEmpty(Words)) ArgumentListManager.AddArgument(d, "words", Words);
            if (!String.IsNullOrEmpty(Phrase)) ArgumentListManager.AddArgument(d, "phrase", Phrase);
            if (!String.IsNullOrEmpty(NotWords)) ArgumentListManager.AddArgument(d, "not-words", NotWords);
            if (!String.IsNullOrEmpty(Scope)) ArgumentListManager.AddArgument(d, "scope", Scope);
            if (!String.IsNullOrEmpty(Matches)) ArgumentListManager.AddArgument(d, "matches", Matches);
            if (!String.IsNullOrEmpty(SearchText)) ArgumentListManager.AddArgument(d, "search-text", SearchText);
            if (Page != PageDefault) ArgumentListManager.AddArgument(d, "page", Page);
            if (!String.IsNullOrEmpty(LinkUrl)) ArgumentListManager.AddArgument(d, "link-url", LinkUrl);
            if (ResultsPerPage != ResultsPerPageDefault) ArgumentListManager.AddArgument(d, "results-per-page", ResultsPerPage);
            return d;
        }
    }
}