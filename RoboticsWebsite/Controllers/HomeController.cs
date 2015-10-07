using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoboticsWebsite.Models;

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