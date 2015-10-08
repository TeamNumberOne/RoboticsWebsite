using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Models;

namespace RoboticsWebsite.Data
{
    public class CalendarData
    {
        // Used for Admins because they can see all events
        public List<EventModel> TestGetEvents(List<EventModel> eventList)
        {
            EventModel event1 = new EventModel();
            event1.Type = "Class";
            event1.Title = "Robotics";
            event1.Description = "Robotics Class";
            event1.StartTime = new DateTime(2015, 10, 6, 7, 0, 0);
            event1.EndTime = new DateTime(2015, 10, 6, 8, 0, 0);
            event1.StartTimeString = "7:00";
            event1.EndTimeString = "8:00";
            eventList.Add(event1);

            EventModel event2 = new EventModel();
            event2.Type = "Competition";
            event2.Title = "Robotics2";
            event2.Description = "Robotics Competition";
            event2.StartTime = new DateTime(2015, 10, 7, 9, 0, 0);
            event2.EndTime = new DateTime(2015, 10, 7, 10, 0, 0);
            event2.StartTimeString = "9:00";
            event2.EndTimeString = "10:00";
            eventList.Add(event2);

            EventModel event3 = new EventModel();
            event3.Type = "Competition";
            event3.Title = "Robotics3";
            event3.Description = "Robotics Competition";
            event3.StartTime = new DateTime(2015, 10, 8, 9, 0, 0);
            event3.EndTime = new DateTime(2015, 10, 8, 10, 0, 0);
            event3.StartTimeString = "9:00";
            event3.EndTimeString = "10:00";
            eventList.Add(event3);

            return eventList;
        }

        // TODO GetEvents - real; works with database

        /*
         * TODO
         * GetClasses
         * GetCompetitions
         * ...
         */
    }
}