using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Models;
using RoboticsWebsite.Data;

namespace RoboticsWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(LoginViewModel model)
        {
            if (Session["Valid"] != null && (bool)Session["Valid"])
                return View("Index");
            else
                return Redirect("~/Account/Login");
        }

        [HttpGet]
        public ActionResult UserEvents()
        {
            if (Session["UserId"] == null || (int) Session["UserId"] == 0)
            {
                ViewData["ErrorMessage"] = "You must be signed in to view this data";
                return Redirect("~/Account/Login");
            }

            UserViewModel uvModel = new UserViewModel();
            uvModel.GetEventsByUserId((int)Session["UserId"]);

            return View(uvModel);
        }

        [HttpPost]
        public ActionResult UserEvents(UserViewModel uvModel)
        {
            UserData ud = new UserData();
            string errorMessage = ud.RemoveEventFromUser((int)Session["UserId"], uvModel.EventIdToRemove);

            uvModel.GetEventsByUserId((int)Session["UserId"]);

            // Insert this ViewData object onto the page somewhere so the user knows if there is an error
            // Otherwise the event will disappear from the list
            ViewData["ErrorMessage"] = errorMessage;

            return View(uvModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            if (Session["Valid"] != null && (bool)Session["Valid"])
                return View("About");
            else
                return Redirect("~/Account/Login");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            if (Session["Valid"] != null && (bool)Session["Valid"])
                return View("Contact");
            else
                return Redirect("~/Account/Login");
        }
    }
}