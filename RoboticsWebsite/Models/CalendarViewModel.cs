using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Models
{
    public class CalendarViewModel
    {
        public List<EventModel> Events { get; set; }
        public EventModel NewEvent { get; set; }
        
        public int CurrentMonthNum { get; set; }
        public string CurrentMonth
        {
            get { return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentMonthNum); }
        }
        public int NumDays
        {
            get { return DateTime.DaysInMonth(DateTime.Now.Year, CurrentMonthNum); }
        }
        /*{
            set { NumDays = DateTime.ParseExact(CurrentMonth, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month; }
        }*/

        public CalendarViewModel()
        {
            Events = new List<EventModel>();
            NewEvent = new EventModel();
        }

        public void GetEvents()
        {
            CalendarData cd = new CalendarData();
            Events = cd.TestGetEvents(Events);
        }

        public void AddTestEvent()
        {
            Events.Add(NewEvent);
            NewEvent = new EventModel();
        }
    }
}