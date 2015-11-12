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
            //calViewModel.StartTime = "00:00 AM";
            //calViewModel.EndTime = "00:00 AM";
           return View(calViewModel);
        }

        [HttpPost]
        public ActionResult Month(CalendarViewModel calViewModel)
        {
            int startMin;
            int startHour;
            int endMin;
            int endHour;

            if (calViewModel.IsNewEvent)
            {
                string[] split = calViewModel.StartTime.Split(new char[] { ':', ' ' });
                startMin = Int32.Parse(split[1]);

                if (split[2].Equals("PM") & !split[2].Equals("12"))
                {
                    startHour = Int32.Parse(split[0]) + 12;
                }
                else if (split[2].Equals("AM") && split[2].Equals("12"))
                {
                    startHour = 24;
                }
                else
                {
                    startHour = Int32.Parse(split[0]);
                }

                string[] split2 = calViewModel.EndTime.Split(new char[] { ':', ' ' });
                endMin = Int32.Parse(split2[1]);

                if (split2[2].Equals("PM") &! split2[2].Equals("12"))
                {
                    endHour = Int32.Parse(split2[0]) + 12;
                }
                else if(split2[2].Equals("AM") && split2[2].Equals("12"))
                {
                    endHour = 24;
                }
                else
                {
                    endHour = Int32.Parse(split2[0]);
                }
                calViewModel.NewEvent.StartMin  = startMin;
                calViewModel.NewEvent.StartHour = startHour;
                calViewModel.NewEvent.EndMin    = endMin;
                calViewModel.NewEvent.EndHour   = endHour;
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
            //calViewModel.NewEvent.StartTime = "00:00 AM";
            calViewModel.NewEvent.StartHour = 0;
            calViewModel.NewEvent.StartMin = 0;
            //calViewModel.NewEvent.EndTime = "00:00 AM";
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