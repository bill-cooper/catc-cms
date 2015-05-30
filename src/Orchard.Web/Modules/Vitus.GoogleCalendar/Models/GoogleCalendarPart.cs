using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vitus.GoogleCalendar.Models
{
    public class GoogleCalendarPart : ContentPart<GoogleCalendarPartRecord>
    {
        [Required, DataType(DataType.Url)]
        public string GoogleCalendarUrls
        {
            get { return this.Record.GoogleCalendarUrls; }
            set { this.Record.GoogleCalendarUrls = value; }
        }

        public string GoogleCalendarClasses
        {
            get { return this.Record.GoogleCalendarClasses; }
            set { this.Record.GoogleCalendarClasses = value; }
        }

        public bool Theme
        {
            get { return this.Record.Theme; }
            set { this.Record.Theme = value; }
        }
        
        public FullCalendarDefaultView DefaultView
        {
            get { return this.Record.DefaultView; }
            set { this.Record.DefaultView = value; }
        }

        public string HeaderLeft
        {
            get { return this.Record.HeaderLeft; }
            set { this.Record.HeaderLeft = value; }
        }

        public string HeaderCenter
        {
            get { return this.Record.HeaderCenter; }
            set { this.Record.HeaderCenter = value; }
        }

        public string HeaderRight
        {
            get { return this.Record.HeaderRight; }
            set { this.Record.HeaderRight = value; }
        }
        
        public FullCalendarWeekMode WeekMode
        {
            get { return this.Record.WeekMode; }
            set { this.Record.WeekMode = value; }
        }

        public bool Weekends
        {
            get { return this.Record.Weekends; }
            set { this.Record.Weekends = value; }
        }

        public bool WeekNumbers
        {
            get { return this.Record.WeekNumbers; }
            set { this.Record.WeekNumbers = value; }
        }

        public bool AllDaySlot
        {
            get { return this.Record.AllDaySlot; }
            set { this.Record.AllDaySlot = value; }
        }

        [Range(0,1440)]
        public int SlotMinutes
        {
            get { return this.Record.SlotMinutes; }
            set { this.Record.SlotMinutes = value; }
        }

        [Range(0,1440)]
        public int DefaultEventMinutes
        {
            get { return this.Record.DefaultEventMinutes; }
            set { this.Record.DefaultEventMinutes = value; }
        }

        [Range(typeof(byte), "0", "23")]
        public byte FirstHour
        {
            get { return this.Record.FirstHour; }
            set { this.Record.FirstHour = value; }
        }

        [Range(typeof(byte), "0", "23")]
        public byte MinTime
        {
            get { return this.Record.MinTime; }
            set { this.Record.MinTime = value; }
        }

        [Range(typeof(byte), "1", "24")]
        public byte MaxTime
        {
            get { return this.Record.MaxTime; }
            set { this.Record.MaxTime = value; }
        }
    }
}