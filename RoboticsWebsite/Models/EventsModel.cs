using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Models
{
    public class EventsModel
    {
        public List<EventModel> AllEvents { get; set; }

        public void GetAllEvents()
        {
            //UserData ud = new UserData();
            CalendarData cd = new CalendarData();
            UsersModel allUsers = new UsersModel();
            List<EventModel> events = new List<EventModel>();
            AllEvents = new List<EventModel>();
            //cd.GetEvents();

            /*foreach (var e in events)
            {
                if (e.CreatedById == )
                {
                    AllEvents.Add(e);
                }
            }*/
            allUsers.GetUsers();
            foreach(var user in allUsers.Users)
            {
                //events = ud.GetEventsByUserId(user.UserId);

                foreach(var e in cd.GetEvents())
                {
                    if (e.CreatedById == user.UserId)
                    {
                        AllEvents.Add(e);
                    }
                }
            }
        }
    }
}