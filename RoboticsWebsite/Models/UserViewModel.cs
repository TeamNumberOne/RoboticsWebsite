using RoboticsWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class UserViewModel
    {
        public List<EventModel> Events { get; set; }
        // UserId isn't being used right now
        public int UserId { get; set; }
        public int EventIdToRemove { get; set; }
        public int EventIdForPledge { get; set; }
        public int AmountToPledge { get; set; }

        public void GetEventsByUserId(int userId)
        {
            UserData ud = new UserData();
            //int userId = (int)Session["userId"];
            Events = ud.GetEventsByUserId(userId);
        }
    }
}