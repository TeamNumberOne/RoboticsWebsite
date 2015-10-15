using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Enums;
using System.Data;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Models
{
    public class EventModel
    {
        // Type could be "Class", "Competition", etc.
        public int EventId { get; set; }
        public EventType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartDayString { get; set; }
        public string StartTimeString { get; set; }
        public string EndDayString { get; set; }
        public string EndTimeString { get; set; }
        public int Day { get; set; }

        public EventModel()
        {
            Type = EventType.Initial;
            Title = "";
            Description = "";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
        }

        public EventModel(DataRow dataRow)
        {
            EventId = Convert.ToInt32(dataRow[(int)EventIndices.EventId].ToString());
            Type = (EventType) Enum.Parse(typeof(EventType), dataRow[1].ToString());
            Title = dataRow[(int)EventIndices.Title].ToString();
            Description = dataRow[(int)EventIndices.Description].ToString();
            StartTime = Convert.ToDateTime(dataRow[(int)EventIndices.StartTime].ToString());
            EndTime = Convert.ToDateTime(dataRow[(int)EventIndices.EndTime].ToString());
            Day = Convert.ToInt32(dataRow[(int)EventIndices.Day].ToString());
        }

        public void AddEvent()
        {
            CalendarData cd = new CalendarData();
            cd.addEvent(this);
        }
    }
}