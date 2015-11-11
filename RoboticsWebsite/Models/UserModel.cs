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
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserModel()
        {
            Type = UserType.Admin;
            Email = "";
            Password = "";
            Status = UserStatus.Pending;
            FirstName = "";
            LastName = "";
        }

        public UserModel(RegisterViewModel rvModel)
        {
            Type = rvModel.UserType;
            Email = rvModel.Email;
            Password = Cryptography.Encrypt(rvModel.Password);
            Status = UserStatus.Pending;
            FirstName = rvModel.FirstName;
            LastName = rvModel.LastName;
        }

        public UserModel(DataRow dataRow)
        {
            UserId = Convert.ToInt32(dataRow[(int)UserIndices.UserId].ToString());
            Type = (UserType) Enum.Parse(typeof(UserType), dataRow[(int)UserIndices.Type].ToString());
            Email = dataRow[(int)UserIndices.Email].ToString();
            Password = dataRow[(int)UserIndices.Password].ToString();
            Status = (UserStatus) Enum.Parse(typeof(UserStatus), dataRow[(int)UserIndices.Status].ToString());
            FirstName = dataRow[(int)UserIndices.FirstName].ToString();
            LastName = dataRow[(int)UserIndices.LastName].ToString();
        }

        public string AddUser()
        {
            UserData ud = new UserData();
            return ud.AddUser(this);
        }
    }
}