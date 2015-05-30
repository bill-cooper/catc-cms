using System;
using System.Collections.Generic;

namespace ceenq.org.Services.Parameter
{
    /// <summary>
    ///     Parameters representing the readingPlanInfo function, providing information about the readings for a given day.
    /// </summary>
    public class ReadingPlanInfoParameter : Parameter, IDefaultOptimization
    {
        public ReadingPlanInfoParameter()
        {
            ReadingPlan = ReadingPlan.OneYearTract;
            ReadingPlanDefault = ReadingPlan.OneYearTract;
        }

        /// <summary>
        ///     Get the reading plan for a certain date. Should be in YYYY-MM-DD format (e.g., 2007-11-17 for today). The date changes at midnight US Central Time (GMT -5 or -6). (Today's date)
        /// </summary>
        public String Date { get; set; }

        /// <summary>
        ///     Specifies which reading plan to use. Possibilities are one-year-tract, through-the-bible, every-day-in-the-word, and bcp. For more information about these reading plans, please see our Devotions area at http://www.gnpcb.org/esv/devotions/. (default: one-year-tract)
        /// </summary>
        public ReadingPlan ReadingPlan { get; set; }

        /// <summary>
        ///     Specify a start date for the reading plan if you want to begin your readings at the beginning of the plan on dates besides January 1. Should be in YYYY-MM-DD format.
        /// </summary>
        public String StartDate { get; set; }

        private ReadingPlan ReadingPlanDefault { get; set; }


        Dictionary<String, String> IDefaultOptimization.GetSettingsList()
        {
            var d = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(Date)) ArgumentListManager.AddArgument(d, "date", Date);
            if (ReadingPlan != ReadingPlanDefault) ArgumentListManager.AddArgument(d, "reading-plan", TextTranslation.GetReadingPlanText(ReadingPlan));
            if (!String.IsNullOrEmpty(StartDate)) ArgumentListManager.AddArgument(d, "start-date", StartDate);
            return d;
        }
    }
}