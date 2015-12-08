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
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        //public string StartTime { get; set; }
        public int StartMin { get; set; }
        public int StartHour { get; set; }
        
        //public string EndTime { get; set; }
        public int EndMin { get; set; }        
        public int EndHour { get; set; }        
        public int CreatedById { get; set; }
        public EventStatus Status { get; set; }
        public int Pledge { get; set; }


        public EventModel()
        {
            Type = EventType.Initial;
            Title = "";
            Description = "";
            Day = 0;
            Month = 0;
            Year = 0000;
            //StartTime = "00:00 AM";
            StartHour = 0;
            StartMin = 0;
            //EndTime = "00:00 AM";
            EndHour = 0;
            EndMin = 0;
            Pledge = -1;
        }

        public EventModel(DataRow dataRow)
        {
            EventId = Convert.ToInt32(dataRow[(int)EventIndices.EventId].ToString());
            Type = (EventType) Enum.Parse(typeof(EventType), dataRow[(int)EventIndices.Type].ToString());
            Title = dataRow[(int)EventIndices.Title].ToString();
            Description = dataRow[(int)EventIndices.Description].ToString();
            Month = Convert.ToInt32(dataRow[(int)EventIndices.Month].ToString());
            Day = Convert.ToInt32(dataRow[(int)EventIndices.Day].ToString());
            Year = Convert.ToInt32(dataRow[(int)EventIndices.Year].ToString());
            StartHour = Convert.ToInt32(dataRow[(int)EventIndices.StartHour].ToString());
            StartMin = Convert.ToInt32(dataRow[(int)EventIndices.StartMin].ToString());
            EndHour = Convert.ToInt32(dataRow[(int)EventIndices.EndHour].ToString());
            EndMin = Convert.ToInt32(dataRow[(int)EventIndices.EndMin].ToString());
            CreatedById = Convert.ToInt32(dataRow[(int)EventIndices.CreatedById].ToString());
            Status = (EventStatus)Enum.Parse(typeof(EventStatus), dataRow[(int)EventIndices.Status].ToString());
            Pledge = Convert.ToInt32(dataRow[((int)EventIndices.Status) + 1].ToString());
        }

        

        public void AddEvent()
        {
            CalendarData cd = new CalendarData();
            DataRow name = cd.AddEvent(this);

            // Add a comment in the news feed for this event
            string firstName = name[0].ToString();
            string lastName = name[1].ToString();

            string comment = firstName + " " + lastName + " added " + Type.ToString() + " " + Title + "!";
            NewsFeedModel nm = new NewsFeedModel(CreatedById, name[0].ToString(), name[1].ToString(), comment);
            NewsFeedData nd = new NewsFeedData();
            nd.AddComment(nm);
        }
    }
}