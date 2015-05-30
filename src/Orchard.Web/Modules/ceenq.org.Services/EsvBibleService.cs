using System;
using System.Collections.Generic;
using System.Xml;
using Nalarium.Xml;
using Orchard;
using Orchard.ContentManagement;
using ceenq.org.Services.Models;
using ceenq.org.Services.Parameter;
using ceenq.org.Services.Setting;

namespace ceenq.org.Services
{
    /// <summary>
    ///     Proxy for Esv Bible Web Service API (Version 2)
    /// </summary>
    public class EsvBibleService : IEsvBibleService
    {
        private readonly IOrchardServices _orchardServices;
        private OutputFormat _outputFormat = OutputFormat.Html;
        private IOutputSetting _settings;
        private string ApiKey
        {
            get
            {
                var settings = _orchardServices.WorkContext.CurrentSite.As<ESVBibleServiceSettingsPart>();
                if (settings == null || string.IsNullOrEmpty(settings.EsvBibleServiceKey))
                    throw new Exception("The ESV Bible API cannot be used until the API setting is configured.");
                return settings.EsvBibleServiceKey;
            }
        }

        /// <summary>
        ///     Proxy for Esv Bible Web Service API (Version 2)
        /// </summary>
        public EsvBibleService(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        /// <summary>
        ///     Proxy for Esv Bible Web Service API (Version 2)
        /// </summary>
        /// <param name="outputFormat">Enumerating stating the output format to use.  Setting this without stating setting a settings object will leave the output settings at their defaults for this output format.</param>
        public EsvBibleService(OutputFormat outputFormat)
        {
            OutputFormat = outputFormat;
        }

        /// <summary>
        ///     Proxy for Esv Bible Web Service API (Version 2)
        /// </summary>
        /// <param name="settings">Settings regarding the format of the output message.</param>
        public EsvBibleService(IOutputSetting settings)
        {
            Settings = settings;
        }

        private IOutputSetting TempSettings { get; set; }

        /// <summary>
        ///     Read-only view of the output format of the proxy.  Set either OutputFormat or Settings; changing one alters the other.
        /// </summary>
        public OutputFormat OutputFormat
        {
            get { return _outputFormat; }

            set
            {
                _outputFormat = value;
                _settings = CreateSettings(_outputFormat);
            }
        }

        /// <summary>
        ///     Settings regarding the format of the output message.  Set either OutputFormat or Settings; changing one alters the other.
        /// </summary>
        public IOutputSetting Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = CreateSettings(OutputFormat);
                }

                return _settings;
            }
            set
            {
                _settings = value;
                _outputFormat = GetOutputFormat(_settings);
            }
        }

        private OutputFormat GetOutputFormat(IOutputSetting settings)
        {
            if (settings is CrosswayXmlVersion10Setting)
            {
                return OutputFormat.CrosswayXmlVersion10;
            }
            else if (settings is HtmlOutputSetting)
            {
                return OutputFormat.Html;
            }
            else if (settings is PlainTextSetting)
            {
                return OutputFormat.PlainText;
            }
            else
            {
                throw new EsvBibleException("Unknown settings type");
            }
        }

        private IOutputSetting CreateSettings(OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.CrosswayXmlVersion10:
                    return new CrosswayXmlVersion10Setting();

                case OutputFormat.Html:
                    return new HtmlOutputSetting();

                case OutputFormat.PlainText:
                    return new PlainTextSetting();

                default:
                    throw new EsvBibleException("Unknown output format");
            }
        }

        /// <summary>
        ///     Provides a new verse daily.
        /// </summary>
        /// <returns>Passage data</returns>
        public String DailyVerse()
        {
            return DailyVerse(new DailyVerseParameter { Key = ApiKey });
        }

        /// <summary>
        ///     Provides a new verse daily.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String DailyVerse(DailyVerseParameter parameters)
        {
            return new RestServiceCaller().CallDailyVerseMethod(parameters, Settings);
        }

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <returns>Passage data</returns>
        public String PassageQuery(String passage)
        {
            try
            {
                return PassageQuery(new PassageQueryParameter { Key = ApiKey, Passage = passage });
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String PassageQuery(PassageQueryParameter parameters)
        {
            try
            {
                return new RestServiceCaller().CallPassageQueryMethod(parameters, Settings);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">aAssage to query</param>
        /// <returns>Passage data</returns>
        public XmlDocument PassageQueryAsXmlDocument(String passage)
        {
            return PassageQueryAsXmlDocument(new PassageQueryParameter { Passage = passage });
        }

        /// <summary>
        ///     Looks up a passage.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public XmlDocument PassageQueryAsXmlDocument(PassageQueryParameter parameters)
        {
            if (!(Settings is CrosswayXmlVersion10Setting))
            {
                TempSettings = Settings;
                Settings = new CrosswayXmlVersion10Setting();
            }

            String data = String.Empty;
            try
            {
                data = new RestServiceCaller().CallPassageQueryMethod(parameters, Settings);
            }
            catch (EsvBibleException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EsvBibleException("Error in service call or message format.  See InnerException for more information.", ex);
            }

            if (TempSettings != null && !(TempSettings is CrosswayXmlVersion10Setting))
            {
                Settings = TempSettings;
            }

            var doc = new XmlDocument();
            doc.LoadXml(data);

            if (data.Contains("<error>"))
            {
                String error = doc.DocumentElement.SelectSingleNode("//crossway-bible/error").InnerXml;
                throw new EsvBibleException(error);
            }

            return doc;
        }

        /// <summary>
        ///     Provides the result of an XPath query on passage query data.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Result from XPath query</returns>
        public String PassageQueryValueViaXPath(PassageQueryParameter parameters)
        {
            if (!String.IsNullOrEmpty(parameters.XPath))
            {
                XmlDocument doc = PassageQueryAsXmlDocument(parameters);
                if (doc != null && doc.DocumentElement != null)
                {
                    XmlNode node = doc.DocumentElement.SelectSingleNode(parameters.XPath);
                    if (node != null)
                    {
                        return node.InnerXml;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    throw new EsvBibleException("Error retreiving XML document.");
                }
            }
            else
            {
                throw new EsvBibleException("XPath (used for finding values in returned XML document )is required when calling QueryInfoValueViaXml.");
            }
        }

        /// <summary>
        ///     Provides multiple results of an XPath query on passage query data.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Results from XPath query</returns>
        public String[] PassageQueryValueViaXPathMulti(PassageQueryParameter parameters)
        {
            var results = new List<String>();
            if (parameters.XPathSet != null && parameters.XPathSet.Length > 0)
            {
                XmlDocument doc = PassageQueryAsXmlDocument(parameters);
                if (doc != null && doc.DocumentElement != null)
                {
                    foreach (String xpath in parameters.XPathSet)
                    {
                        XmlNode node = doc.DocumentElement.SelectSingleNode(xpath);
                        if (node != null)
                        {
                            results.Add(node.InnerXml);
                        }
                    }
                    return results.ToArray();
                }
                else
                {
                    throw new EsvBibleException("Error retreiving XML document.");
                }
            }
            else
            {
                throw new EsvBibleException("XPathSet (used for finding values in returned XML document )is required when calling QueryInfoValueViaXml.");
            }
        }

        /// <summary>
        ///     Looks up a passage or shows word-search results, depending on the query.
        /// </summary>
        /// <returns>Keywords with which to query</returns>
        public String Query(String query)
        {
            return Query(new QueryParameter { Key = ApiKey, Q = query });
        }

        /// <summary>
        ///     Looks up a passage or shows word-search results, depending on the query.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String Query(QueryParameter parameters)
        {
            return new RestServiceCaller().CallQueryMethod(parameters, Settings);
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="q">Text for which to query</param>
        /// <returns>Passage data</returns>
        public String QueryInfo(String q)
        {
            return QueryInfo(new QueryInfoParameter { Key = ApiKey, Q = q });
        }

        /// <summary>
        ///     Accesses passages from the Esv devotional section.
        /// </summary>
        /// <returns>Passage data</returns>
        public String ReadingPlanQuery()
        {
            return ReadingPlanQuery(new ReadingPlanQueryParameter { Key = ApiKey });
        }

        /// <summary>
        ///     Accesses passages from the Esv devotional section.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String ReadingPlanQuery(ReadingPlanQueryParameter parameters)
        {
            return new RestServiceCaller().CallReadingPlanQueryMethod(parameters, Settings);
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String QueryInfo(QueryInfoParameter parameters)
        {
            return new RestServiceCaller().CallQueryInfoMethod(parameters, Settings);
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Text for which to query</param>
        /// <returns>Passage data in an XmlDocument object</returns>
        public XmlDocument QueryInfoAsXmlDocument(String q)
        {
            return QueryInfoAsXmlDocument(new QueryInfoParameter { Key = ApiKey, Q = q });
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data in an XmlDocument object</returns>
        public XmlDocument QueryInfoAsXmlDocument(QueryInfoParameter parameters)
        {
            String data = new RestServiceCaller().CallQueryInfoMethod(parameters, Settings);

            var doc = new XmlDocument();
            doc.LoadXml(data);

            if (data.Contains("<error>"))
            {
                String error = doc.DocumentElement.SelectSingleNode("//crossway-bible/error").InnerXml;
                throw new EsvBibleException(error);
            }

            return doc;
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Text for which to query</param>
        /// <returns>Query information in a QueryInfoData object</returns>
        public QueryInfoData QueryInfoAsObject(String q)
        {
            return QueryInfoAsObject(new QueryInfoParameter { Key = ApiKey, Q = q });
        }

        /// <summary>
        ///     Provides parsing and display information about your query, including identify whether it is a passage reference or a word search.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Query information in a QueryInfoData object</returns>
        public QueryInfoData QueryInfoAsObject(QueryInfoParameter parameters)
        {
            XmlDocument doc = QueryInfoAsXmlDocument(parameters);
            var qi = new QueryInfoData();

            qi.Error = XPathProcessor.GetString(doc, "//crossway-bible/error");
            qi.Query = XPathProcessor.GetString(doc, "//crossway-bible/query");
            qi.QueryType = XPathProcessor.GetString(doc, "//crossway-bible/query-type") == "passage" ? QueryType.Passage : QueryType.WordSearch;

            switch (qi.QueryType)
            {
                case QueryType.Passage:
                    qi.Readable = XPathProcessor.GetString(doc, "//crossway-bible/readable");
                    qi.Unit = XPathProcessor.GetString(doc, "//crossway-bible/unit");
                    qi.Readable = XPathProcessor.GetString(doc, "//crossway-bible/readable");
                    qi.IsCompleteChapter = Int32.Parse(XPathProcessor.GetString(doc, "//crossway-bible/is-complete-chapter")) == 1 ? true : false;
                    qi.AlternateQueryType = XPathProcessor.GetString(doc, "//crossway-bible/alternate-query-type") == "passage" ? QueryType.Passage : QueryType.WordSearch;

                    Int32 count = 0;
                    if (Int32.TryParse(XPathProcessor.GetString(doc, "//crossway-bible/alternate-result-count"), out count))
                    {
                        qi.AlternateResultCount = count;
                    }

                    break;
                case QueryType.WordSearch:
                    count = 0;
                    if (Int32.TryParse(XPathProcessor.GetString(doc, "//crossway-bible/result-count"), out count))
                    {
                        qi.ResultCount = count;
                    }
                    break;
            }

            XmlNode node = XPathProcessor.GetNode(doc, "//crossway-bible/warnings");
            if (node != null && node.HasChildNodes)
            {
                qi.Warnings = new List<Warning>();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    qi.Warnings.Add(new Warning
                        {
                            Code = childNode.ChildNodes[0].InnerText,
                            Readable = childNode.ChildNodes[1].InnerText
                        });
                }
            }

            return qi;
        }

        /// <summary>
        ///     Provides information about the readings for a given day.
        /// </summary>
        /// <returns>Passage data</returns>
        public String ReadingPlanInfo()
        {
            return ReadingPlanInfo(new ReadingPlanInfoParameter { Key = ApiKey });
        }

        /// <summary>
        ///     Provides information about the readings for a given day.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String ReadingPlanInfo(ReadingPlanInfoParameter parameters)
        {
            return new RestServiceCaller().CallReadingPlanInfoMethod(parameters, Settings);
        }

        /// <summary>
        ///     Provides a random verse from a selected list, or specified verse. See http://www.gnpcb.org/esv/share/rss2.0/?show-verses=true for verse list.
        /// </summary>
        /// <returns>Passage data</returns>
        public String Verse()
        {
            return Verse(new VerseParameter { Key = ApiKey });
        }

        /// <summary>
        ///     Provides a random verse from a selected list, or specified verse. See http://www.gnpcb.org/esv/share/rss2.0/?show-verses=true for verse list.
        /// </summary>
        /// <param name="parameters">Parameters for request</param>
        /// <returns>Passage data</returns>
        public String Verse(VerseParameter parameters)
        {
            return new RestServiceCaller().CallVerseMethod(parameters, Settings);
        }
    }
}