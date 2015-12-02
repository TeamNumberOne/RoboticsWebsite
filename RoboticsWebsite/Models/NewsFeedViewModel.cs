using RoboticsWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Models
{
    public class NewsFeedViewModel
    {
        public List<NewsFeedModel> NewsFeed { get; set; }
        public NewsFeedModel CommentToAdd { get; set; }

        public void PopulateNewsFeed()
        {
            NewsFeedData nfd = new NewsFeedData();

            NewsFeed = nfd.GetNewsFeed();
        }
    }
}