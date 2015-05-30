using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the queryInfo function, providing parsing and display information about your query, including identify whether it is a passage reference or a word search.
    /// </summary>
    public class QueryInfoParameter : Parameter, IDefaultOptimization
    {
        private String q;

        /// <summary>
        ///     The words or passage you want to parse.
        /// </summary>
        public String Q
        {
            get
            {
                if (String.IsNullOrEmpty(q))
                {
                    throw new EsvBibleException("Q (the words or passage you want to parse) is required");
                }
                return q;
            }
            set { q = value; }
        }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (String.IsNullOrEmpty(Q)) ArgumentListManager.AddArgument(d, "q", Q);
            return d;
        }
    }
}