using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Enums;
using RoboticsWebsite.Utilities;
using RoboticsWebsite.Models;

namespace RoboticsWebsite.Data
{
    public class UserData
    {
        public SQLiteConnection dbConn;
        public static List<UserModel> Users{ get; set; }
        // Used for Admins because they can see all events
        public UserData()
        {
            dbConn = new SQLiteConnection(ConnectionManager.GetConnectionString(), true);
            Users = new List<UserModel>();
        }

        public List<UserModel> GetUsers()
        {
            string query;
            SQLiteCommand cmd;
            UserModel userModel;
            List<UserModel> Users = new List<UserModel>();

            try
            {
                dbConn.Open();

                query = "select * from users";
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
                            userModel = new UserModel(dt.Rows[i]);
                            Users.Add(userModel);
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

            return Users;
        }

        public string AddUser(UserModel user)
        {
            string status = "";
            string query = "select max(user_id) from users";
            DataTable dt1 = new DataTable();
            SQLiteCommand cmd;

            try
            {
                dbConn.Open();

                if (IsNewUser(user.Email))
                {
                    using (cmd = new SQLiteCommand(query, dbConn))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            dt1.Load(dr);
                            // Calculate new user_id                            
                            user.UserId = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString()) + 1;
                        }
                    }
                
                    query = "insert into users values (" + user.UserId + ", '" + user.Type.ToString() +
                        "', '" + user.Email + "', '" + user.Password + "', '" + user.Status.ToString() + "', '" + user.FirstName + "', '" + user.LastName + "')";

                    cmd = new SQLiteCommand(query, dbConn);
                    cmd.ExecuteNonQuery();

                    status = "User information has been sent to administrators for approval";
                }
                else
                {
                    status = "The given email address is already in use by another user";
                }

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

        public Boolean IsNewUser(string email)
        {
            string query = "select * from users where email = '" + email + "'";
            DataTable dt1 = new DataTable();
            SQLiteCommand cmd;

            using (cmd = new SQLiteCommand(query, dbConn))
            {
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    dt1.Load(dr);
                    
                    return dt1.Rows.Count == 0;
                }
            }
        }

        public UserStatus VerifyUser(string email, string password, ref UserType userType, ref int userId, ref string firstName, ref string lastName)
        {
            UserStatus userStatus = UserStatus.Unknown;
            string query;
            SQLiteCommand cmd;

            password = Cryptography.Encrypt(password);

            try
            {
                dbConn.Open();

                query = "select * from users where email = '" + email + "' and password = '" + password + "'";
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);

                        // No user with email and password combo
                        if (dt.Rows.Count == 0)
                        {
                            userStatus = UserStatus.Unknown;
                        }
                        // User account is in a pending status
                        else if (dt.Rows[0][(int)UserIndices.Status].Equals(UserStatus.Pending.ToString()))
                        {
                            userStatus = UserStatus.Pending;
                        }
                        // User was found, get the type of the user
                        else
                        {
                            userId = Convert.ToInt32(dt.Rows[0][(int)UserIndices.UserId].ToString());
                            userType = (UserType) Enum.Parse(typeof(UserType), dt.Rows[0][(int)UserIndices.Type].ToString());
                            firstName = dt.Rows[0][(int)UserIndices.FirstName].ToString();
                            lastName = dt.Rows[0][(int)UserIndices.LastName].ToString();
                            userStatus = UserStatus.Approved;
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

            return userStatus;
        }

        public List<EventModel> GetEventsByUserId(int userId)
        {
            string query, query2;
            SQLiteCommand cmd;
            EventModel eventModel;
            List<EventModel> Events = new List<EventModel>();

            try
            {
                dbConn.Open();
                // Get the event info and the donation amount if it isn't null
                // This will set the donation amount to 0 if there is no donation amount associated with the user/event
                query = "select E.* from events E, user_event UE " +
                        "where UE.user_id = '" + userId + "' and UE.event_id = E.event_id";
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

        public string RemoveEventFromUser(int userId, int eventIdToRemove, string userType)
        {
            string query, errorMessage = "";
            SQLiteCommand cmd, cmd2;
            int created_by_user_id = 0;

            try
            {
                dbConn.Open();

                query = "select created_by_id from events where event_id = " + eventIdToRemove;
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);

                        // This user created the event, so we need to set all references to the event to Cancelled
                        if (Convert.ToInt32(dt.Rows[0][0].ToString()) == userId || userType.Equals("Admin"))
                        {
                            query = "update user_event set status = '" + EventStatus.Cancelled.ToString() + "' where event_id = " + eventIdToRemove;
                            cmd2 = new SQLiteCommand(query, dbConn);
                            cmd2.ExecuteNonQuery();

                            query = "update events set status = '" + EventStatus.Cancelled.ToString() + "' where event_id = " + eventIdToRemove;
                            cmd2 = new SQLiteCommand(query, dbConn);
                            cmd2.ExecuteNonQuery();
                        }
                        // User didn't create the event so set the entry's status to Removed in the user_event table
                        else
                        {
                            query = "update user_event set status = '" + EventStatus.Cancelled.ToString() + "' where event_id = " + eventIdToRemove + " and user_id = " + userId;
                            cmd2 = new SQLiteCommand(query, dbConn);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                //Console.Write(ex.ToString());
                errorMessage = ex.ToString();
                dbConn.Close();
            }

            return errorMessage;
        }

        public string AddUserToEvent(int userId, int eventId)
        {
            string query, errorMessage = "";
            SQLiteCommand cmd, cmd2;
            EventModel eventModel;
            List<EventModel> Events = new List<EventModel>();

            try
            {
                dbConn.Open();

                //query = "with recursive event(month, day, year, start_hour, start_min, end_hour, end_min) as (select month, day, year, start_hour, start_min, end_hour, end_min from events where event_id = " + eventId + ") "
                //        + "select count(*) from events where event_id != " + eventId + " and month = event.month and day = event.day and year = event.year "
                //        + "and ((start_hour < event.start_hour and end_hour > event.end_hour) or "
                //        + "(start_hour < event.start_hour and end_hour = event.end_hour and end_min > event.end_min) or "
                //        + "(start_hour = event.start_hour and end_hour > event.end_hour and start_min < event.start_min) or"
                //        + "(start_hour = event.start_hour and end_hour = event.end_hour and (start_min between event.start_min and event.end_min or end_min between event.start_min and event.end_min)) or "
                //        + "(start_hour = event.start_hour and end_hour = event.end_hour and start_min < event.start_min and end_min > event.end_min))";
                query = "select E.* from events E, user_event UE where UE.user_id = " + userId + " and UE.event_id = E.event_id";
                   // insert into user_event values (" + userId + ", " + eventId;
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    //doesn't execute
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);


                        // While there are rows in the returned data create EventModels and add them to the EventModel list
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            eventModel = new EventModel(dt.Rows[i]);
                            Events.Add(eventModel);
                        }

                        // Check if there is a conflict with the events this user is signed up for
                        query = "select * from events where event_id = " + eventId + " and status not in ('Cancelled', 'Removed')";
                        DataTable dt2 = new DataTable();
                        using (cmd2 = new SQLiteCommand(query, dbConn))
                        {
                            using (SQLiteDataReader dr2 = cmd2.ExecuteReader())
                            {
                                dt2.Load(dr2);
                                eventModel = new EventModel(dt2.Rows[0]);

                                Events = Events.Where(x => x.Month == eventModel.Month && x.Day == eventModel.Day && x.Year == eventModel.Year && 
                                                           ((x.StartHour < eventModel.StartHour && x.EndHour > eventModel.EndHour) ||
                                                           (x.StartHour < eventModel.StartHour && x.EndHour == eventModel.EndHour && x.EndMin > eventModel.EndMin) ||
                                                           (x.StartHour == eventModel.StartHour && x.EndHour > eventModel.EndHour && x.StartMin < eventModel.StartMin) ||
                                                           (x.StartHour == eventModel.StartHour && x.EndHour == eventModel.EndHour && (x.StartMin >= eventModel.StartMin && x.StartMin <= eventModel.EndMin) || (x.EndMin >= eventModel.StartMin && x.EndMin <= eventModel.EndMin)) ||
                                                           (x.StartHour == eventModel.StartHour && x.EndHour == eventModel.EndHour && x.StartMin < eventModel.StartMin && x.EndMin > eventModel.EndMin))).ToList();
                                // There is no event overlap for the user
                                //if (Events.Count == 0)
                                //{
                                    bool current = false;
                                    foreach(var e in Events)
                                    {
                                        if(e.Status == EventStatus.Current)
                                        {
                                            current = true;
                                        }
                                    }
                                    if (!current)
                                    {
                                        query = "insert into user_event values (" + userId + ", " + eventId + ", '" + EventStatus.Current.ToString() + "')";
                                        cmd2 = new SQLiteCommand(query, dbConn);
                                        cmd2.ExecuteNonQuery();
                                    }
                                //}
                                // There is event overlap for the user
                                else
                                {
                                    errorMessage = "You are already enrolled in an event during this time period";
                                }
                            }
                        }

                    }
                }

                dbConn.Close();
            }
            catch (SQLiteException ex)
            {
                //Console.Write(ex.ToString());
                errorMessage = ex.ToString();
                dbConn.Close();
            }

            return errorMessage;
        }

        public string UpdateUser(int userId, UserStatus newStatus)
        {
            string query, errorMessage = "Update Successful";
            SQLiteCommand cmd;

            query = "update users set status = '" + newStatus.ToString() + "' where user_id = " + userId;
            try
            {
                dbConn.Open();
                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                dbConn.Close();
            }

            return errorMessage;
        }

        public string AddPledge(int userId, int amount)
        {
            string status = "";
            string query = "select * from pledges where user_id = " + userId;
            DataTable dt = new DataTable();
            SQLiteCommand cmd;

            try
            {
                dbConn.Open();

                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                            
                        // If the user id and event id already have an entry in the database
                        if (dt.Rows.Count == 1)
                        {
                            query = "update pledges set amount = " + (amount + Convert.ToInt32(dt.Rows[0][1])).ToString()/*(amount + Convert.ToInt32(dt.Rows[1/*(int)PledgeIndices.Amount*].ToString()))*/ + 
                                    " where user_id = " + userId;
                        }
                        // If this is a new pledge for the user id/event id combination
                        else
                        {
                            query = "insert into pledges values (" + userId + ", " + amount + ")";
                        }
                    }
                }

                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();

                status = "Update successful";

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

        public string ChangeEventDetails(EventModel eventModel)
        {
            string query, errorMessage = "Update Successful";
            SQLiteCommand cmd;

            query = "update events set month = " + eventModel.Month + ", day = " + eventModel.Day + ", year = " + eventModel.Year + ", start_hour = " + eventModel.StartHour +
                    ", start_min = " + eventModel.StartMin + ", end_hour = " + eventModel.EndHour + ", end_min = " + eventModel.EndMin + 
                    " where created_by_id = " + eventModel.CreatedById + " and event_id = " + eventModel.EventId;
            try
            {
                dbConn.Open();
                cmd = new SQLiteCommand(query, dbConn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                dbConn.Close();
            }

            return errorMessage;
        }

        public int GetTotalDonations()
        {
            string query;
            SQLiteCommand cmd;
            int totalDonations = 0;

            try
            {
                dbConn.Open();

                query = "select sum(amount) from pledges";
                DataTable dt = new DataTable();
                using (cmd = new SQLiteCommand(query, dbConn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        // Load the reader data into the DataTable
                        dt.Load(dr);

                        if (dt.Rows.Count != 0)
                        {
                            totalDonations = Convert.ToInt32(dt.Rows[0][0].ToString());
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

            return totalDonations;
        }

    }
}