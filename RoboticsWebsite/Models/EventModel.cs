using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class EventModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}