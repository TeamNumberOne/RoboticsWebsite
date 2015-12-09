using RoboticsWebsite.Data;
using RoboticsWebsite.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class NewsFeedModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        public NewsFeedModel(DataRow dataRow)
        {
            UserId = Convert.ToInt32(dataRow[(int)NewsFeedIndices.UserId].ToString());
            FirstName = dataRow[(int)NewsFeedIndices.FirstName].ToString();
            LastName = dataRow[(int)NewsFeedIndices.LastName].ToString();
            Comment = dataRow[(int)NewsFeedIndices.Comment].ToString();
            Month = Convert.ToInt32(dataRow[(int)NewsFeedIndices.Month].ToString());
            Day = Convert.ToInt32(dataRow[(int)NewsFeedIndices.Day].ToString());
            Year = Convert.ToInt32(dataRow[(int)NewsFeedIndices.Year].ToString());
            Hour = Convert.ToInt32(dataRow[(int)NewsFeedIndices.Hour].ToString());
            Minute = Convert.ToInt32(dataRow[(int)NewsFeedIndices.Minute].ToString());
        }

        public NewsFeedModel(int userId, string firstName, string lastName, string comment)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Comment = comment;
            Month = DateTime.Now.Month;
            Day = DateTime.Now.Day;
            Year = DateTime.Now.Year;
            Hour = DateTime.Now.Hour;
            Minute = DateTime.Now.Minute;
        }

        public string AddComment()
        {
            NewsFeedData nfd = new NewsFeedData();
            return nfd.AddComment(this);
        }
    }
}