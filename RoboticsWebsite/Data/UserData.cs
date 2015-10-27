using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
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

        public string AddUser(UserModel user)
        {
            string status = "";
            string query = "select max(user_id) from events";
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
                        "', '" + user.Email + "', '" + user.Password + "', '" + user.Status.ToString() + "')";

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

        public UserType VerifyUser(string email, string password)
        {
            UserType userType = UserType.Unknown;
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
                            userType = UserType.Unknown;
                        }
                        // User account is in a pending status
                        else if (dt.Rows[0][(int)UserIndices.Status].Equals(UserType.Pending.ToString()))
                        {
                            userType = UserType.Pending;
                        }
                        // User was found, get the type of the user
                        else
                        {
                            userType = (UserType) Enum.Parse(typeof(UserType), dt.Rows[0][(int)UserIndices.Type].ToString());
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

            return userType;
        }
    }
}