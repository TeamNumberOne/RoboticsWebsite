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
        public ActionResult NewsFeed()
        {
            NewsFeedViewModel nfViewModel = new NewsFeedViewModel();
            nfViewModel.PopulateNewsFeed();

            return View(nfViewModel);
        }

        [HttpPost]
        public ActionResult NewsFeed(NewsFeedViewModel nfViewModel)
        {
            string status = nfViewModel.CommentToAdd.AddComment();
            ViewData["StatusMessage"] = status;
            nfViewModel.PopulateNewsFeed();

            return View(nfViewModel);
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

            return View("UserEvents", uvModel);
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

            return View("UserEvents", uvModel);
        }

        [HttpPost]
        public ActionResult AddPledge(UserViewModel uvModel)
        {
            UserData ud = new UserData();
            string errorMessage = ud.AddPledge((int)Session["UserId"], uvModel.EventIdForPledge, uvModel.AmountToPledge);

            uvModel.GetEventsByUserId((int)Session["UserId"]);

            ViewData["ErrorMessage"] = errorMessage;

            return View("UserEvents", uvModel);
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

        [HttpGet]
        public ActionResult ModifyUser()
        {
            UsersModel usersModel = new UsersModel();
            usersModel.GetUsers();
            return View("ModifyUsers", usersModel);
        }

        [HttpPost]
        public ActionResult ModifyUser(UsersModel usersModel)
        {
            
            //usersModel.GetUsers();
            usersModel.UpdateUser();
            usersModel.GetUsers();
            return View("ModifyUsers", usersModel);
        }
    }
}