using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboticsWebsite.Utilities
{
    public class ConnectionManager
    {
        public static string GetConnectionString()
        {
            return "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Data\\RoboticsDb.sqlite;Version=3;";
        }
    }
}