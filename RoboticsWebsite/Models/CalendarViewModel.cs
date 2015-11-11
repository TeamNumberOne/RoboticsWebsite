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
        public Boolean IsNewEvent { get; set; }
        public int EventIdToAddForUser { get; set; }
        public SelectList EventTypeSelectList { get; set; }
        public CalendarData cd;

        public List<EventModel> Events { get; set; }
        public EventModel NewEvent { get; set; }

        public List<EventModel> CurrentMonthEvents
        {
            get
            {
                return Events.Where(x => x.Month == CurrentMonthNum)
                             .OrderBy(x => x.Day).ThenBy(x => x.StartHour).ToList();
            }
            set { }
        }

        public string StartTime { get; set; }
        public int StartMin { get; set; }
        public int StartHour { get; set; }

        public string EndTime { get; set; }
        public int EndMin { get; set; }
        public int EndHour { get; set; }
               
        public int CurrentYear { get; set; }
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

        public void PopulateEventTypes(string userType)
        {
            IEnumerable<EventType> eventTypeIEnumerable;
            List<SelectListItem> eventTypeItems = new List<SelectListItem>();
            // Get the event types into the SelectList
            eventTypeIEnumerable = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            if (userType != null)
            {
                // For each event type create a new select list item and add it to the List of SelectListItems
                foreach (EventType eventType in eventTypeIEnumerable)
                {
                    // If statement to prevent sponsors from creating classes and volunteers from creating competitions
                    if (!(userType.Equals(UserType.Sponsor) && eventType == EventType.Class) && !(userType.Equals(UserType.Volunteer) && eventType == EventType.Competition))
                        eventTypeItems.Add(new SelectListItem() { Value = eventType.ToString(), Text = eventType.ToString() });
                }
            }

            EventTypeSelectList = new SelectList(eventTypeItems, "Value", "Text");
        }
    }
}