using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the readingPlanInfo function, providing a random verse from a selected list, or specified verse. See http://www.gnpcb.org/esv/share/rss2.0/?show-verses=true for verse list.
    /// </summary>
    public class VerseParameter : Parameter, IDefaultOptimization
    {
        /// <summary>
        ///     Get a specific passage or group of passages (e.g., Matthew 5:1,3,5). (default: A random passage)
        /// </summary>
        public String Passage { get; set; }

        /// <summary>
        ///     You can send your own seed to the random number generator if you want to get a specific result consistently. (For example, dailyVerse simply calls this function using the current date as the seed.) (default: A random passage)
        /// </summary>
        public String Seed { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(Passage)) ArgumentListManager.AddArgument(d, "passage", Passage);
            if (!String.IsNullOrEmpty(Seed)) ArgumentListManager.AddArgument(d, "seed", Seed);
            return d;
        }
    }
}