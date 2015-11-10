using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoboticsWebsite.Data;
using System.Web.Mvc;
using RoboticsWebsite.Enums;

namespace RoboticsWebsite.Models
{
    public class CalendarViewModel
    {
        public SelectList EventTypeSelectList { get; set; }
        public CalendarData cd;
        public List<EventModel> Events { get; set; }
        public List<EventModel> CurrentMonthEvents
        {
            get
            {
                return Events.Where(x => x.Month == CurrentMonthNum)
                             .OrderBy(x => x.Day).ThenBy(x => x.StartHour).ToList();
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

            // We shouldn't need this call anymore
            //Events = cd.TestGetEvents(Events);
        }

        public void GetEvents()
        {
            //CalendarData cd = new CalendarData();
            Events = cd.getEvents();
            //Events = cd.TestGetEvents(Events);
            Events = Events.OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day).ToList();
        }

        public void PopulateEventTypes()
        {
            IEnumerable<EventType> eventTypeIEnumerable;
            List<SelectListItem> eventTypeItems = new List<SelectListItem>();
            // Get the event types into the SelectList
            eventTypeIEnumerable = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            // For each event type create a new select list item and add it to the List of SelectListItems
            foreach(EventType eventType in eventTypeIEnumerable)
            {
                eventTypeItems.Add(new SelectListItem() { Value = eventType.ToString(), Text = eventType.ToString() });
            }

            EventTypeSelectList = new SelectList(eventTypeItems, "Value", "Text");
        }
    }
}