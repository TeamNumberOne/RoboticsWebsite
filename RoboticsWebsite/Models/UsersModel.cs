using RoboticsWebsite.Data;
using RoboticsWebsite.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class UsersModel
    {
        public List<UserModel> Users { get; set; }
        public int UserIdToModify { get; set; }
        public UserStatus NewUserStatus { get; set; }
        public Boolean Approved { get; set; }

        public UsersModel()
        {
            Users = new List<UserModel>();
            UserIdToModify = 0;
            NewUserStatus = UserStatus.Unknown;
        }

        public void GetUsers()
        {
            UserData ud = new UserData();
            Users = ud.GetUsers();
        }

        public string UpdateUser()
        {
            UserData ud = new UserData();
            if (Approved)
            {
                NewUserStatus = UserStatus.Approved;
            }
            else
            {
                NewUserStatus = UserStatus.Rejected;
            }
            string result = ud.UpdateUser(UserIdToModify, NewUserStatus);

            return result;
        }
    }
}