using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoboticsWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("~/Account/Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return Redirect("~/Account/Login");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return Redirect("~/Account/Login");
        }
    }
}