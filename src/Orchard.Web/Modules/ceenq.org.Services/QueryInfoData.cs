using System;
using System.Collections.Generic;

namespace ceenq.org.Services
{
    public class QueryInfoData
    {
        public QueryInfoData()
        {
            Warnings = new List<Warning>();
        }

        /// <summary>
        ///     Some sort of fatal error, usually indicating an invalid key or no query. The content of this tag is a human-readable error message.
        /// </summary>
        public String Error { get; set; }

        /// <summary>
        ///     The text of your original query.
        /// </summary>
        public String Query { get; set; }

        /// <summary>
        ///     Either passage or word-search. Tells you how we understood your query.
        /// </summary>
        public QueryType QueryType { get; set; }

        /// <summary>
        ///     Appears if the query-type is passage. Gives a normalized passage reference (full book names and chapter references) that any Bible search engine should be able to understand.
        /// </summary>
        public String Readable { get; set; }

        /// <summary>
        ///     Appears if the query-type is passage. Numerically identifies the verses returned. Each unit component is 8 characters long: the first two characters identify the book (01-66), the next three characters identify the chapter (001-150), and the final three characters identify the verse (001-176). Hyphens (-) indicate a range of verses; commas (,) indicate separate passages.
        /// </summary>
        public String Unit { get; set; }

        /// <summary>
        ///     Appears if the query-type is word-count. Tells how many times the word appears in the Esv text (up to 1,000).
        /// </summary>
        public Int32 ResultCount { get; set; }

        /// <summary>
        ///     Appears if the query-type is passage. Has a value of 1 if the passage references refer only to complete chapters. Has a value of 0 if any of the passage references refer to a partial chapter.
        /// </summary>
        public Boolean IsCompleteChapter { get; set; }

        /// <summary>
        ///     If a passage reference could also be a word (as above, or, e.g., when someone searches for mark), these two tags appear set to word-search and the number of times the word appears in the Esv, respectively.
        /// </summary>
        public QueryType AlternateQueryType { get; set; }

        /// <summary>
        ///     If a passage reference could also be a word (as above, or, e.g., when someone searches for mark), these two tags appear set to word-search and the number of times the word appears in the Esv, respectively.
        /// </summary>
        public Int32 AlternateResultCount { get; set; }

        /// <summary>
        ///     Container tag. Doesn't appear in most queries. Can contain multiple <warning> tags. You can safely ignore these.
        /// </summary>
        public List<Warning> Warnings { get; set; }

        /// <summary>
        ///     Returns true is the QueryInfo object contains warnings and false if not.
        /// </summary>
        public Boolean HasWarnings
        {
            get { return Warnings.Count > 0; }
        }
    }
}