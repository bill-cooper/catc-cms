using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Title.Models;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Localization;
using Vitus.GoogleCalendar.Models;

namespace Vitus.GoogleCalendar
{
    public class Migrations : DataMigrationImpl
    {
        public Migrations()
        {
        }

        public int Create()
        {
            SchemaBuilder.CreateTable("GoogleCalendarPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("GoogleCalendarUrls", col => col.WithLength(600))
                    .Column<string>("GoogleCalendarClasses", col => col.WithLength(255))
                    .Column<bool>("Theme")
                    .Column<string>("DefaultView", c => c.WithLength(100))
                    .Column<string>("HeaderLeft", c => c.WithLength(100))
                    .Column<string>("HeaderCenter", c => c.WithLength(100))
                    .Column<string>("HeaderRight", c => c.WithLength(100))
                    .Column<string>("WeekMode", c => c.WithLength(100))
                    .Column<bool>("Weekends")
                    .Column<bool>("WeekNumbers")
                    .Column<bool>("AllDaySlot")
                    .Column<int>("SlotMinutes")
                    .Column<int>("DefaultEventMinutes")
                    .Column<byte>("FirstHour")
                    .Column<byte>("MinTime")
                    .Column<byte>("MaxTime")
                );

            ContentDefinitionManager.AlterPartDefinition("GoogleCalendarPart", builder => builder
                .Attachable()
                .WithDescription("Displays a read-only Google calendar using jQuery Full Calendar plugin."));

            ContentDefinitionManager.AlterTypeDefinition("GoogleCalendarWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .WithPart("GoogleCalendarPart")
                    .WithSetting("Stereotype", "Widget")
                );
            
            return 1;
        }
    }
}