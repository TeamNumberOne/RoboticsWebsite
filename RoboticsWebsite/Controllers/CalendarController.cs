using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Models;
using RoboticsWebsite.Enums;

namespace RoboticsWebsite.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        [HttpGet]
        public ActionResult Month()
        {
            CalendarViewModel calViewModel = new CalendarViewModel();
            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes();
            calViewModel.CurrentMonthNum = DateTime.Now.Month;

            return View(calViewModel);
        }

        [HttpPost]
        public ActionResult PrevMonth(CalendarViewModel calViewModel)
        {
            if (calViewModel.CurrentMonthNum == 1)
            {
                calViewModel.CurrentMonthNum = 12;
            }
            else
            {
                calViewModel.CurrentMonthNum --;
            }
            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes();

            return View("Month", calViewModel);
        }
        [HttpPost]
        public ActionResult NextMonth(CalendarViewModel calViewModel)
        {
            if (calViewModel.CurrentMonthNum == 12)
            {
                calViewModel.CurrentMonthNum = 1;
            }
            else
            {
                calViewModel.CurrentMonthNum ++;
            }
            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes();

            return View("Month", calViewModel);
        }
        [HttpPost]
        public ActionResult Month(CalendarViewModel calViewModel)
        {
            //calViewModel.NewEvent.AddEvent();

            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes();

            // Reset the values of NewEvent so they aren't used to populate the new event form elements
            calViewModel.NewEvent.Type = EventType.Initial;
            calViewModel.NewEvent.Title = "";
            calViewModel.NewEvent.Description = "";
            //calViewModel.NewEvent.StartTime = DateTime.MinValue;
            //calViewModel.NewEvent.EndTime = DateTime.MaxValue;
            calViewModel.NewEvent.Day = 0;

            // We shouldn't need this call anymore
            //calViewModel.AddTestEvent();
            
            return View(calViewModel);
        }

        [HttpPost]
        public ActionResult AddEvent()
        {

            return View();
        }

        [HttpGet]
        public ActionResult NewEventDialog()
        {
            /// PUT EVENTS FOR DAY SELECTED INTO CALVIEW MODEL
            return View();
        }
    }
}