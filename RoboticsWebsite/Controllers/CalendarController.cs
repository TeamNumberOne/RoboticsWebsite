using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Models;

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
            
            return View(calViewModel);
        }

        [HttpPost]
        public ActionResult Month(CalendarViewModel calViewModel)
        {
            calViewModel.GetEvents();
            calViewModel.AddTestEvent();
            
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
            return View();
        }
    }
}