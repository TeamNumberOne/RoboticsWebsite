using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using RoboticsWebsite.Models;
using System.Data;
using RoboticsWebsite.Enums;
using RoboticsWebsite.Utilities;

namespace RoboticsWebsite.Data
{
    public class CalendarData
    {
        public SQLiteConnection dbConn;
        public static List<EventModel> Events { get; set; }
        // Used for Admins because they can see all events
        public CalendarData()
        {
            dbConn = new SQLiteConnection(ConnectionManager.GetConnectionString(), true);
            Events = new List<EventModel>();
        }

        //public List<EventModel> TestGetEvents(List<EventModel> eventList)
        //{
        //    EventModel event1 = new EventModel();
        //    event1.Type = EventType.Class;
        //    event1.Title = "Robotics";
        //    event1.Description = "Robotics Class";
        //    event1.StartTime = new DateTime(2015, 10, 6, 7, 0, 0);
        //    event1.EndTime = new DateTime(2015, 10, 6, 8, 0, 0);
        //    event1.StartTimeString = "7:00";
        //    event1.EndTimeString = "8:00";
        //    event1.Day = 6;
        //    eventList.Add(event1);

        //    EventModel event2 = new EventModel();
        //    event2.Type = EventType.Competition;
        //    event2.Title = "Robotics2";
        //    event2.Description = "Robotics Competition";
        //    event2.StartTime = new DateTime(2015, 10, 7, 9, 0, 0);
        //    event2.EndTime = new DateTime(2015, 10, 7, 10, 0, 0);
        //    event2.StartTimeString = "9:00";
        //    event2.EndTimeString = "10:00";
        //    event2.Day = 7;
        //    eventList.Add(event2);

        //    EventModel event3 = new EventModel();
        //    event3.Type = EventType.Competition;
        //    event3.Title = "Robotics3";
        //    event3.Description = "Robotics Competition";
        //    event3.StartTime = new DateTime(2015, 10, 8, 9, 0, 0);
        //    event3.EndTime = new DateTime(2015, 10, 8, 10, 0, 0);
        //    event3.StartTimeString = "9:00";
        //    event3.EndTimeString = "10:00";
        //    event3.Day = 8;
        //    eventList.Add(event3);

        //    Events.AddRange(eventList);
        //    return eventList;
        //}

        public List<EventModel> GetEvents()
        {
            //SQLiteConnection.CreateFile("../../../Users/Paul/Documents/Visual Studio 2015/Projects/RoboticsWebsite/RoboticsWebsite/Data/RoboticsDb.sqlite");

            string query;
            SQLiteCommand cmd;
            EventModel eventModel;

            try
            {
                dbConn.Open();

                query = "select * from events";
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);
                        
                        // While there are rows in the returned data create EventModels and add them to the EventModel list
                        for (int i = 0; i < dt.Rows.Count; i ++)
                        {
                            eventModel = new EventModel(dt.Rows[i]);
                            /*if (eventModel.Title.Equals("whatever"))
                            {
                                eventModel.Status = EventStatus.Cancelled;
                            }*/
                            Events.Add(eventModel);
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

            return Events;
        }

        public string[] AddEvent(EventModel calendarEvent)
        {
            string query = "select max(event_id) from events";
            DataTable dt = new DataTable();
            SQLiteCommand cmd;
            DataRow nameRow;
            string[] nameString = new string[2];

            try
            {
                dbConn.Open();

                // Get the next event id
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                        // Calculate new event_id
                        if (dt.Rows[0] != null) //EDIT: CHANGED THIS TO > 1 BECAUSE I THINK ROWS MIGHT START AT 1?  SO WAS GIVING INVALID CAST EXCEPTION FROM DBNull TO OTHER TYPES
                            calendarEvent.EventId = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()) + 1;
                        else
                            calendarEvent.EventId = 1;
                    }
                }

                // Insert the new row into the events table
                query = "insert into events values (" + calendarEvent.EventId + ", '" + calendarEvent.Type.ToString() + "', '" + calendarEvent.Title + 
                    "', '" + calendarEvent.Description + "', " + calendarEvent.Month + ", " + calendarEvent.Day + ", " + calendarEvent.Year + ", " + calendarEvent.StartHour + 
                    ", " + calendarEvent.StartMin + ", " + calendarEvent.EndHour + ", " + calendarEvent.EndMin + ", " + calendarEvent.CreatedById + ", '" + EventStatus.Current.ToString() + "')";

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                UserData ud = new UserData();
                string errorMessage = ud.AddUserToEvent(calendarEvent.CreatedById, calendarEvent.EventId);
                if (errorMessage.Equals("You are already enrolled in an event during this time period"))
                {
                    string query2 = "delete from events where event_id = " + calendarEvent.EventId;
                    cmd = new SQLiteCommand(query2, dbConn);
                    cmd.ExecuteNonQuery();

                    return new string[1];
                }

                // Get the first and last names of the user to be used when adding the comment for this new event
                query = "select first_name, last_name from users where user_id = " + calendarEvent.CreatedById;
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                        nameRow = dt.Rows[0];

                        // Populate the nameString variable with the first and last names
                        nameString[0] = nameRow[0].ToString();
                        nameString[1] = nameRow[1].ToString();
                    }
                }

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                dbConn.Close();
            }

            return nameString;
        }

        /*
         * TODO
         * GetClasses
         * GetCompetitions
         * ...
         */
    }
}