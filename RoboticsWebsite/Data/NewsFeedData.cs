using RoboticsWebsite.Models;
using RoboticsWebsite.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Data
{
    public class NewsFeedData
    {
        public SQLiteConnection dbConn;

        public NewsFeedData()
        {
            dbConn = new SQLiteConnection(ConnectionManager.GetConnectionString(), true);
        }

        public string AddComment(NewsFeedModel comment)
        {
            string status = "";
            string query;
            DataTable dt1 = new DataTable();
            SQLiteCommand cmd;

            try
            {
                dbConn.Open();

                query = "insert into news_feed values (" + comment.UserId + ", '" + comment.FirstName + "', '" + comment.LastName + "', '" + comment.Comment + "', "+ comment.Month + 
                        ", " + comment.Day + ", " + comment.Year + ", " + comment.Hour + ", " + comment.Minute + ")";

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                status = "Comment Added";

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                status = ex.ToString();
                dbConn.Close();
            }

            return status;
        }

        public List<NewsFeedModel> GetNewsFeed()
        {
            string query;
            SQLiteCommand cmd;
            NewsFeedModel nfModel;
            List<NewsFeedModel> newsFeed = new List<NewsFeedModel>();

            try
            {
                dbConn.Open();

                query = "select * from news_feed";
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);

                        // While there are rows in the returned data create EventModels and add them to the EventModel list
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            nfModel = new NewsFeedModel(dt.Rows[i]);
                            newsFeed.Add(nfModel);
                        }
                    }
                }

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                dbConn.Close();
            }

            return newsFeed.Take(20).ToList();
        }
    }
}