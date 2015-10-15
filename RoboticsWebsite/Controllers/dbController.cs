using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SQLite;

namespace RoboticsWebsite.Controllers
{
    public class dbController : Controller
    {
        // Use this to perform queries (inserts, updates, deletes, create tables)
        // All you need to do is edit the query string, run the application, then type the URL localhost:60866/db/Index
        public string Index()
        {
            string query;
            string countBefore, countAfter;
            SQLiteCommand cmd;
            SQLiteConnection dbConn = new SQLiteConnection("Data Source=C:\\Users\\Paul\\Documents\\Visual Studio 2015\\Projects\\RoboticsWebsite\\RoboticsWebsite\\Data\\RoboticsDb.sqlite;Version=3;", true);

            try
            {
                dbConn.Open();

                query = "delete from events where event_id = 3";

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                dbConn.Close();
                return "failed\n" + ex.ToString();
            }

            return "success";
        }
    }
}