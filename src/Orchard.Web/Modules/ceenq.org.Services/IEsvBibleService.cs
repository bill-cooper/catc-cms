using System;
using System.Xml;
using Orchard;
using ceenq.org.Services.Parameter;

namespace ceenq.org.Services {
    public interface IEsvBibleService : IDependency
    {
        /// <summary>
        ///     Provides a new verse daily.
        /// </summary>
        /// <returns>Passage data</returns>
        String DailyVerse();

        /// <summary>
        ///     Provides a new verse daily.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String DailyVerse(DailyVerseParameter parameters);

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <returns>Passage data</returns>
        String PassageQuery(String passage);

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String PassageQuery(PassageQueryParameter parameters);

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">aAssage to query</param>
        /// <returns>Passage data</returns>
        XmlDocument PassageQueryAsXmlDocument(String passage);

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        XmlDocument PassageQueryAsXmlDocument(PassageQueryParameter parameters);

        /// <summary>
        ///     Provides the result of an XPath query on passage query data.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Result from XPath query</returns>
        String PassageQueryValueViaXPath(PassageQueryParameter parameters);

        /// <summary>
        ///     Provides multiple results of an XPath query on passage query data.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Results from XPath query</returns>
        String[] PassageQueryValueViaXPathMulti(PassageQueryParameter parameters);

        /// <summary>
        ///     Looks up a passage or shows word-search results, depending on the query.
        /// </summary>
        /// <returns>Keywords with which to query</returns>
        String Query(String query);

        /// <summary>
        ///     Looks up a passage or shows word-search results, depending on the query.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String Query(QueryParameter parameters);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="q">Text for which to query</param>
        /// <returns>Passage data</returns>
        String QueryInfo(String q);

        /// <summary>
        ///     Accesses passages from the Esv devotional section.
        /// </summary>
        /// <returns>Passage data</returns>
        String ReadingPlanQuery();

        /// <summary>
        ///     Accesses passages from the Esv devotional section.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String ReadingPlanQuery(ReadingPlanQueryParameter parameters);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String QueryInfo(QueryInfoParameter parameters);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Text for which to query</param>
        /// <returns>Passage data in an XmlDocument object</returns>
        XmlDocument QueryInfoAsXmlDocument(String q);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data in an XmlDocument object</returns>
        XmlDocument QueryInfoAsXmlDocument(QueryInfoParameter parameters);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Text for which to query</param>
        /// <returns>Query information in a QueryInfoData object</returns>
        QueryInfoData QueryInfoAsObject(String q);

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Query information in a QueryInfoData object</returns>
        QueryInfoData QueryInfoAsObject(QueryInfoParameter parameters);

        /// <summary>
        ///     Provides information about the readings for a given day.
        /// </summary>
        /// <returns>Passage data</returns>
        String ReadingPlanInfo();

        /// <summary>
        ///     Provides information about the readings for a given day.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String ReadingPlanInfo(ReadingPlanInfoParameter parameters);

        /// <summary>
        ///     Provides a random verse from a selected list, or specified verse. See http://www.gnpcb.org/esv/share/rss2.0/?show-verses=true for verse list.
        /// </summary>
        /// <returns>Passage data</returns>
        String Verse();

        /// <summary>
        ///     Provides a random verse from a selected list, or specified verse. See http://www.gnpcb.org/esv/share/rss2.0/?show-verses=true for verse list.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        String Verse(VerseParameter parameters);
    }
}