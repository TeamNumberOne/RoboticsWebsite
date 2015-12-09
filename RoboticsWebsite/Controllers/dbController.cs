using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SQLite;
using RoboticsWebsite.Utilities;
using System.Data;
using RoboticsWebsite.Models;

namespace RoboticsWebsite.Controllers
{
    public class dbController : Controller
    {
        // Use this to perform queries (inserts, updates, deletes, create tables)
        // All you need to do is edit the query string, run the application, then type the URL localhost:60866/db/Index
        public string Index()
        {
            string query;
            string result = "";
            string countBefore, countAfter;
            SQLiteCommand cmd;
            //SQLiteConnection dbConn = new SQLiteConnection("Data Source=C:\\Users\\Paul\\Documents\\Visual Studio 2015\\Projects\\RoboticsWebsite\\RoboticsWebsite\\Data\\RoboticsDb.sqlite;Version=3;", true);
            SQLiteConnection dbConn = new SQLiteConnection(ConnectionManager.GetConnectionString(), true);

            try
            {
                dbConn.Open();

                UserModel userModel;
                query = "select * from users";
                //query = "SELECT sql FROM (SELECT * FROM sqlite_master UNION ALL SELECT * FROM sqlite_temp_master) WHERE type!= 'meta' ORDER BY tbl_name, type DESC, name";
                //DataTable dt = new DataTable();
                //using (cmd = new SQLiteCommand(query, dbConn))
                //{
                //    using (SQLiteDataReader dr = cmd.ExecuteReader())
                //    {
                //        // Load the reader data into the DataTable
                //        dt.Load(dr);

                //        //for (int i=0; i < dt.Rows.Count; i++)
                //        //{
                //        //    result = result + "<br />" + dt.Rows[i][0];
                //        //}
                //        // While there are rows in the returned data create EventModels and add them to the EventModel list
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            userModel = new UserModel(dt.Rows[i]);
                //            result = result + "<br />" + userModel.UserId + " " + userModel.FirstName + " " + userModel.LastName + " " + userModel.Email + " " + userModel.Password + " " + userModel.Type.ToString() + " " + userModel.Status.ToString();
                //        }
                //    }
                //}

                //dbConn.Close();

                /****** non queries */

                //query = "delete from events where event_id = 1";
                // Keep these for reference
                // Delete table
                //query = "drop table events";
                //query = "drop table users";
                //query = "drop table user_event";

                //cmd = new SQLiteCommand(query, dbConn);
                //cmd.ExecuteNonQuery();

                // Create table
                query = "create table events (event_id integer(1), type varchar(20), title varchar(50), description varchar(500), month integer(1), day integer(1), year integer(1), start_hour integer(1), start_min integer(1), end_hour integer(1), end_min integer(1), created_by_id integer(1), status varchar(20), primary key (event_id))";
                //query = "create table users (user_id integer(1), type varchar(20), email varchar(50), password varchar(150), status varchar(20), first_name varchar(50), last_name varchar(50), primary key (user_id))";
                //query = "create table user_event (user_id integer(1), event_id integer(1), status varchar(20))";
                //query = "create table news_feed (user_id integer(1), first_name varchar(50), last_name varchar(50), comment varchar(500), month integer(1), day integer(1), year integer(1), hour integer(1), minute integer(1))";
                //query = "create table pledges (user_id integer(1), event_id integer(1), amount integer(1))";

                // Insert values
                //query = "insert into events values (" + newId + ", 'Competition', 'Robotics Comp', 'A comp for robotics', '2015-01-01 02:30:00', '2015-01-01 03:30:00', 6)";

                // Add column
                //query = "alter table user_event add status varchar(20)";
                //query = "alter table users add last_name varchar(50)";

                // Update
                //query = "update events set status = 'Approved' where status is null";

                // Use this query to add your info to the users table
                //query = "insert into users values(1, 'Admin', 'denk.luca@uwlax.edu', '" + Cryptography.Encrypt("password") + "', 'Approved', 'Lucas', 'Denk')";
                query = "insert into events values (1, 'Class', 'whatever', 'whatever', 1, 1, 1, 1, 1, 2, 2, 1, 'Current')";

                //query = "drop table events";

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

            return "success " + result;
        }
    }
}