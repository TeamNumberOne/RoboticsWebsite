using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class EventModel
    {
        // Type could be "Class", "Competition", etc.
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }

        public EventModel()
        {
            Type = "";
            Title = "";
            Description = "";
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
        }
    }
}