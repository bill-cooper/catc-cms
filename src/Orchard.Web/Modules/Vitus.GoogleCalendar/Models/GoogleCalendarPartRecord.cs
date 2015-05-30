using Orchard.ContentManagement.Records;
using System;

namespace Vitus.GoogleCalendar.Models
{
    public class GoogleCalendarPartRecord : ContentPartRecord
    {
        public GoogleCalendarPartRecord()
        {
            // Init defaults
            this.HeaderLeft = "title";
            this.HeaderCenter = "";
            this.HeaderRight = "today prev,next";

            this.Weekends = true;

            this.AllDaySlot = true;
            this.SlotMinutes = 30;
            this.DefaultEventMinutes = 120;
            this.FirstHour = 6;
            this.MinTime = 0;
            this.MaxTime = 24;
        }

        public virtual string GoogleCalendarUrls { get; set; }

        public virtual string GoogleCalendarClasses { get; set; }

        public virtual bool Theme { get; set; }

        public virtual FullCalendarDefaultView DefaultView { get; set; }

        public virtual string HeaderLeft { get; set; }

        public virtual string HeaderCenter { get; set; }

        public virtual string HeaderRight { get; set; }

        public virtual FullCalendarWeekMode WeekMode { get; set; }

        public virtual bool Weekends { get; set; }

        public virtual bool WeekNumbers { get; set; }

        public virtual bool AllDaySlot { get; set; }

        public virtual int SlotMinutes { get; set; }

        public virtual int DefaultEventMinutes { get; set; }

        public virtual byte FirstHour { get; set; }

        public virtual byte MinTime { get; set; }

        public virtual byte MaxTime { get; set; }
    }
}