using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Models
{
    public class CalendarViewModel
    {
        public CalendarData cd;
        public List<EventModel> Events { get; set; }
        public List<EventModel> CurrentMonthEvents
        {
            get
            {
                return Events.Where(x => (x.StartTime >= new DateTime(2015, CurrentMonthNum, 1, 0, 0, 0)) && 
                                         (x.EndTime <= new DateTime(2015, CurrentMonthNum, DateTime.DaysInMonth(2015, CurrentMonthNum), 23, 59, 59)))
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
            cd = new CalendarData();
            Events = new List<EventModel>();
            NewEvent = new EventModel();
            Events = cd.TestGetEvents(Events);
        }

        public void GetEvents()
        {
            //CalendarData cd = new CalendarData();
            Events = cd.getEvents();
            //Events = cd.TestGetEvents(Events);
            Events = Events.OrderBy(x => x.StartTime).ToList();
        }

        public void AddTestEvent()
        {
            DateTime start = new DateTime(2015, CurrentMonthNum, NewEvent.Day);
            //Events.Add(NewEvent);
            cd.addEvent(NewEvent);
            Events = cd.getEvents();
            Events = Events.OrderBy(x => x.StartTime).ToList();
            NewEvent = new EventModel();
        }
    }
}