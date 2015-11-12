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

        public List<EventModel> getEvents()
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

        public void addEvent(EventModel calendarEvent)
        {
            string query = "select max(event_id) from events";
            DataTable dt1 = new DataTable();
            SQLiteCommand cmd;

            try
            {
                dbConn.Open();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dt1.Load(dr);
                        // Calculate new event_id
                        if (dt1.Rows.Count > 0) //EDIT: CHANGED THIS TO > 1 BECAUSE I THINK ROWS MIGHT START AT 1?  SO WAS GIVING INVALID CAST EXCEPTION FROM DBNull TO OTHER TYPES
                            calendarEvent.EventId = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString()) + 1;
                        else
                            calendarEvent.EventId = 1;
                    }
                }
                
                query = "insert into events values (" + calendarEvent.EventId + ", '" + calendarEvent.Type.ToString() +
                    "', '" + calendarEvent.Title + "', '" + calendarEvent.Description + "', " + calendarEvent.Month + ", " + calendarEvent.Day + ", " + calendarEvent.Year +
                    ", " + calendarEvent.StartHour + ", " + calendarEvent.StartMin + ", " + calendarEvent.EndHour + ", " + calendarEvent.EndMin + ", " + calendarEvent.CreatedById + ")";

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                dbConn.Close();
            }
        }

        /*
         * TODO
         * GetClasses
         * GetCompetitions
         * ...
         */
    }
}