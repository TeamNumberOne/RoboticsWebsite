using RoboticsWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class ModifyEventViewModel
    {
        public List<EventModel> Events { get; set; }

        public void GetEventsCreatedBy(int userId)
        {
            //UserData ud = new UserData();
            CalendarData ud = new CalendarData();
            //List<EventModel> evs = ud.GetEventsByUserId(userId);
            List<EventModel> evs = ud.GetEvents();

            Events = new List<EventModel>();

            foreach(var ev in evs)
            {
                if(ev.CreatedById == userId)
                {
                    Events.Add(ev);
                }
            }
        }
    }
}