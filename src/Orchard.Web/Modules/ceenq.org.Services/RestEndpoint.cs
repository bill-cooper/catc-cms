using System;

namespace ceenq.org.Services
{
    public static class RestEndpoint
    {
        private const String Base = "http://www.esvapi.org/v2/rest/";

        public const String DailyVerse = Base + "dailyVerse";

        public const String PassageQuery = Base + "passageQuery";

        public const String Query = Base + "query";

        public const String QueryInfo = Base + "queryInfo";

        public const String ReadingPlanInfo = Base + "readingPlanInfo";

        public const String ReadingPlanQuery = Base + "readingPlanQuery";

        public const String Verse = Base + "verse";
    }
}