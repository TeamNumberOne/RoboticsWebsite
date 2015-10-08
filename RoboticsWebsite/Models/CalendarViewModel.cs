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
        public List<EventModel> CurrentMonthEvents
        {
            get
            {
                return Events.Where(x => (x.StartTime >= new DateTime(2015, CurrentMonthNum, 1, 0, 0, 0)) && 
                                         (x.EndTime <= new DateTime(2015, CurrentMonthNum, DateTime.DaysInMonth(2015, CurrentMonthNum), 0, 0, 0)))
                             .OrderBy(x => x.StartTime).ToList();
            }

            set { }
        }
        public EventModel NewEvent { get; set; }
        
        public int CurrentMonthNum { get; set; }
        public int CurrentDay { get; set; }

        public DayOfWeek StartDay
        {
            get { return new DateTime(2015, CurrentMonthNum, 1).DayOfWeek; }
        }

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
            Events = Events.OrderBy(x => x.StartTime).ToList();
        }

        public void AddTestEvent()
        {
            Events.Add(NewEvent);
            Events = Events.OrderBy(x => x.StartTime).ToList();
            NewEvent = new EventModel();
        }
    }
}