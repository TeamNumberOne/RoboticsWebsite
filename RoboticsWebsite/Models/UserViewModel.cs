﻿using RoboticsWebsite.Data;
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

        public void GetEventsByUserId(int userId)
        {
            UserData ud = new UserData();
            Events = ud.GetEventsByUserId(userId);
        }
    }
}