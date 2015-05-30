using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the passageQuery function, proving access to passages.
    /// </summary>
    public class PassageQueryParameter : Parameter, IDefaultOptimization
    {
        private String passage;

        /// <summary>
        ///     The passage you want to display. It accepts a wide variety of input formats (e.g., Isaiah 1, Isa 1, and Is 1 all go to Isaiah 1).
        /// </summary>
        public String Passage
        {
            get
            {
                if (String.IsNullOrEmpty(passage))
                {
                    throw new EsvBibleException("Passage (the passage you want to display) is required");
                }
                return passage;
            }
            set { passage = value; }
        }

        /// <summary>
        ///     XPath query used for finding values in returned XML document.
        /// </summary>
        public String XPath { get; set; }

        /// <summary>
        ///     XPath query set used for finding values in returned XML document.  This prompts PassageQueryValueViaXPathMulti to return an array of Strings.
        /// </summary>
        public String[] XPathSet { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(Passage)) ArgumentListManager.AddArgument(d, "passage", Passage);
            return d;
        }
    }
}