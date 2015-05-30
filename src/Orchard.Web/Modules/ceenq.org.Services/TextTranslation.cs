using System;

namespace ceenq.org.Services
{
    internal static class TextTranslation
    {
        internal static String GetReadingPlanText(ReadingPlan readingPlan)
        {
            switch (readingPlan)
            {
                case ReadingPlan.OneYearTract:
                    return "one-year-tract";
                case ReadingPlan.ThroughTheBible:
                    return "through-the-bible";
                case ReadingPlan.EveryDayInTheWord:
                    return "every-day-in-the-word";
                case ReadingPlan.Bcp:
                    return "bcp";
                default:
                    throw new EsvBibleException("Invalid ReadingPlan set.  Options are OneYearTract (0), ThroughTheBible (1), EveryDayInTheWord (2), and Bcp (3)");
            }
        }
    }
}