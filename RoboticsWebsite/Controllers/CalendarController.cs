using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Models;
using RoboticsWebsite.Enums;
using RoboticsWebsite.Data;

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
            calViewModel.PopulateEventTypes((string)Session["UserType"]);
            calViewModel.CurrentMonthNum = DateTime.Now.Month;
            calViewModel.CurrentYear = DateTime.Now.Year;

            return View(calViewModel);
        }

        [HttpPost]
        public ActionResult Month(CalendarViewModel calViewModel)
        {
            if (calViewModel.IsNewEvent)
            {
                calViewModel.NewEvent.CreatedById = (int)Session["UserId"];
                calViewModel.NewEvent.AddEvent();
            }
            else
            {
                UserData ud = new UserData();
                ViewData["ErrorMessge"] = ud.AddUserToEvent((int)Session["UserId"], calViewModel.EventIdToAddForUser);
            }

            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes((string)Session["UserType"]);

            // Reset the values of NewEvent so they aren't used to populate the new event form elements
            calViewModel.NewEvent.Type = EventType.Initial;
            calViewModel.NewEvent.Title = "";
            calViewModel.NewEvent.Description = "";
            calViewModel.NewEvent.Month = 0;
            calViewModel.NewEvent.Day = 0;
            calViewModel.NewEvent.Year = 0;
            calViewModel.NewEvent.StartHour = 0;
            calViewModel.NewEvent.StartMin = 0;
            calViewModel.NewEvent.EndHour = 0;
            calViewModel.NewEvent.EndMin = 0;

            // We shouldn't need this call anymore
            //calViewModel.AddTestEvent();
            
            return View(calViewModel);
        }

        [HttpPost]
        public ActionResult PrevMonth(CalendarViewModel calViewModel)
        {
            if (calViewModel.CurrentMonthNum == 1)
            {
                calViewModel.CurrentYear --;
                calViewModel.CurrentMonthNum = 12;
            }
            else
            {
                calViewModel.CurrentMonthNum--;
            }
            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes((string)Session["UserType"]);

            return View("Month", calViewModel);
        }
        [HttpPost]
        public ActionResult NextMonth(CalendarViewModel calViewModel)
        {
            if (calViewModel.CurrentMonthNum == 12)
            {
                calViewModel.CurrentYear ++;
                calViewModel.CurrentMonthNum = 1;
            }
            else
            {
                calViewModel.CurrentMonthNum++;
            }
            calViewModel.GetEvents();
            calViewModel.PopulateEventTypes((string)Session["UserType"]);

            return View("Month", calViewModel);
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