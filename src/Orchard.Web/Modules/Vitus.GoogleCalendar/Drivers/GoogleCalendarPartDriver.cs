using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using System;
using Vitus.GoogleCalendar.Models;

namespace Vitus.GoogleCalendar.Drivers
{
    public class GoogleCalendarPartDriver : ContentPartDriver<GoogleCalendarPart>
    {
        protected override string Prefix { get { return "GoogleCalendar"; } }

        protected override DriverResult Display(GoogleCalendarPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_GoogleCalendar",
                () => shapeHelper.Parts_GoogleCalendar(Calendar: part));
        }

        protected override DriverResult Editor(GoogleCalendarPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_GoogleCalendar_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/GoogleCalendar",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(GoogleCalendarPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }

        protected override void Importing(GoogleCalendarPart part, ImportContentContext context)
        {
            var googleCalendarUrls = context.Attribute(part.PartDefinition.Name, "GoogleCalendarUrls");
            var googleCalendarClasses = context.Attribute(part.PartDefinition.Name, "GoogleCalendarClasses");
            var theme = context.Attribute(part.PartDefinition.Name, "Theme");
            var defaultView = context.Attribute(part.PartDefinition.Name, "DefaultView");
            var headerLeft = context.Attribute(part.PartDefinition.Name, "HeaderLeft");
            var headerCenter = context.Attribute(part.PartDefinition.Name, "HeaderCenter");
            var headerRight = context.Attribute(part.PartDefinition.Name, "HeaderRight");
            var weekMode = context.Attribute(part.PartDefinition.Name, "WeekMode");
            var weekends = context.Attribute(part.PartDefinition.Name, "Weekends");
            var weekNumbers = context.Attribute(part.PartDefinition.Name, "WeekNumbers");
            var allDaySlot = context.Attribute(part.PartDefinition.Name, "AllDaySlot");
            var slotMinutes = context.Attribute(part.PartDefinition.Name, "SlotMinutes");
            var defaultEventMinutes = context.Attribute(part.PartDefinition.Name, "DefaultEventMinutes");
            var firstHour = context.Attribute(part.PartDefinition.Name, "FirstHour");
            var minTime = context.Attribute(part.PartDefinition.Name, "MinTime");
            var maxTime = context.Attribute(part.PartDefinition.Name, "MaxTime");

            if (googleCalendarUrls != null) part.GoogleCalendarUrls = googleCalendarUrls;
            if (googleCalendarClasses != null) part.GoogleCalendarClasses = googleCalendarClasses;
            if (theme != null) part.Theme = bool.Parse(theme);
            if (defaultView != null) part.DefaultView = (FullCalendarDefaultView)Enum.Parse(typeof(FullCalendarDefaultView), defaultView); ;
            if (headerLeft != null) part.HeaderLeft = headerLeft;
            if (headerCenter != null) part.HeaderCenter = headerCenter;
            if (headerRight != null) part.HeaderRight = headerRight;
            if (weekMode != null) part.WeekMode = (FullCalendarWeekMode)Enum.Parse(typeof(FullCalendarWeekMode), weekMode);
            if (weekends != null) part.Weekends = bool.Parse(weekends);
            if (weekNumbers != null) part.WeekNumbers = bool.Parse(weekNumbers);
            if (allDaySlot != null) part.AllDaySlot = bool.Parse(allDaySlot);
            if (slotMinutes != null) part.SlotMinutes = int.Parse(slotMinutes);
            if (defaultEventMinutes != null) part.DefaultEventMinutes = int.Parse(defaultEventMinutes);
            if (firstHour != null) part.FirstHour = byte.Parse(firstHour);
            if (minTime != null) part.MinTime = byte.Parse(minTime);
            if (maxTime != null) part.MaxTime = byte.Parse(maxTime);
        }

        protected override void Exporting(GoogleCalendarPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("GoogleCalendarUrls", part.Record.GoogleCalendarUrls);
            context.Element(part.PartDefinition.Name).SetAttributeValue("GoogleCalendarClasses", part.Record.GoogleCalendarClasses);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Theme", part.Record.Theme);
            context.Element(part.PartDefinition.Name).SetAttributeValue("DefaultView", part.Record.DefaultView.ToString("G"));
            context.Element(part.PartDefinition.Name).SetAttributeValue("HeaderLeft", part.Record.HeaderLeft);
            context.Element(part.PartDefinition.Name).SetAttributeValue("HeaderCenter", part.Record.HeaderCenter);
            context.Element(part.PartDefinition.Name).SetAttributeValue("HeaderRight", part.Record.HeaderRight);
            context.Element(part.PartDefinition.Name).SetAttributeValue("WeekMode", part.Record.WeekMode.ToString("G"));
            context.Element(part.PartDefinition.Name).SetAttributeValue("Weekends", part.Record.Weekends);
            context.Element(part.PartDefinition.Name).SetAttributeValue("WeekNumbers", part.Record.WeekNumbers);
            context.Element(part.PartDefinition.Name).SetAttributeValue("AllDaySlot", part.Record.AllDaySlot);
            context.Element(part.PartDefinition.Name).SetAttributeValue("SlotMinutes", part.Record.SlotMinutes);
            context.Element(part.PartDefinition.Name).SetAttributeValue("DefaultEventMinutes", part.Record.DefaultEventMinutes);
            context.Element(part.PartDefinition.Name).SetAttributeValue("FirstHour", part.Record.FirstHour);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MinTime", part.Record.MinTime);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MaxTime", part.Record.MaxTime);
        }
    }
}
