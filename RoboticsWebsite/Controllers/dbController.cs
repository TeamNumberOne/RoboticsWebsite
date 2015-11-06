using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SQLite;
using RoboticsWebsite.Utilities;

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
            //SQLiteConnection dbConn = new SQLiteConnection("Data Source=C:\\Users\\Paul\\Documents\\Visual Studio 2015\\Projects\\RoboticsWebsite\\RoboticsWebsite\\Data\\RoboticsDb.sqlite;Version=3;", true);
            SQLiteConnection dbConn = new SQLiteConnection(ConnectionManager.GetConnectionString(), true);

            try
            {
                dbConn.Open();

                // Keep these for reference
                // Delete table
                //query = "drop table events";

                // Create table
                //query = "create table events (event_id integer(1), type varchar(20), title varchar(50), description varchar(500), start_time datetime, end_time datetime, day integer(1), primary key (event_id))";

                // Insert values
                //query = "insert into events values (" + newId + ", 'Competition', 'Robotics Comp', 'A comp for robotics', '2015-01-01 02:30:00', '2015-01-01 03:30:00', 6)";

                // Add column
                //query = "alter table events add status varchar(20)";

                // Update
                query = "update events set status = 'Approved' where status is null";

                // Use this query to add your info to the users table
                //query = "insert into users values(2, 'Admin', 'luca.denk@uwlax.edu', '" + Cryptography.Encrypt("password") + "', 'Approved')";

                //query = "";

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                string dataSource = dbConn.ConnectionString;
                dbConn.Close();
                return "failed<br />" + ex.ToString() + "<br /> <br />" + dataSource + "lk";
            }

            return "success";
        }
    }
}