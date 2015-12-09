using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Models
{
    public class SearchEventsViewModel
    {
        public string SearchString { get; set; }
        public List<EventModel> Events { get; set; }

        public void GetEventsByUserId(int userId)
        {
            UserData ud = new UserData();
            //int userId = (int)Session["userId"];
            Events = ud.GetEventsByUserId(userId);
        }

        public int GetUserIdByName(string first, string last)
        {
            UserData ud = new UserData();

            List<UserModel> users = ud.GetUsers();
            
            foreach(var user in users)
            {
                if(user.FirstName.Equals(first) && user.LastName.Equals(last))
                {
                    return user.UserId;
                }
            }

            return -1;
        }
    }
}