using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Enums;
using System.Data;
using RoboticsWebsite.Data;
using RoboticsWebsite.Utilities;

namespace RoboticsWebsite.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public UserType Type { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }

        public UserModel()
        {
            Type = UserType.Unknown;
            Email = "";
            Password = "";
            Status = UserStatus.Pending;
        }

        public UserModel(string email, string password)
        {
            Type = UserType.Unknown;
            Email = email;
            Password = Cryptography.Encrypt(password);
            Status = UserStatus.Pending;
        }

        public UserModel(DataRow dataRow)
        {
            UserId = Convert.ToInt32(dataRow[(int)UserIndices.UserId].ToString());
            Type = (UserType) Enum.Parse(typeof(UserType), dataRow[(int)UserIndices.Type].ToString());
            Email = dataRow[(int)UserIndices.Email].ToString();
            Password = dataRow[(int)UserIndices.Password].ToString();
            Status = (UserStatus) Enum.Parse(typeof(UserStatus), dataRow[(int)UserIndices.Status].ToString());
        }

        public string AddUser()
        {
            UserData ud = new UserData();
            return ud.AddUser(this);
        }
    }
}