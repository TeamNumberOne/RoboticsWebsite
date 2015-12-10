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
        public ActionResult Index()//LoginViewModel model)
        {
            NewsFeedViewModel nfViewModel = new NewsFeedViewModel();

            if (Session["Valid"] != null && (bool)Session["Valid"])
            {
                //Session["NewsFeedPopulated"] = true;
                nfViewModel.PopulateNewsFeed();

                return View("Index", nfViewModel);
            }

            else
                return Redirect("~/Account/Login");
        }

        [HttpGet]
        public ActionResult NewsFeed()
        {
            NewsFeedViewModel nfViewModel = new NewsFeedViewModel();
            nfViewModel.PopulateNewsFeed();

            return View("Index", nfViewModel);
        }

        [HttpPost]
        public ActionResult NewsFeed(NewsFeedViewModel nfViewModel)
        {
            string status = nfViewModel.CommentToAdd.AddComment();
            ViewData["StatusMessage"] = status;
            nfViewModel.PopulateNewsFeed();

            return View("Index", nfViewModel);
        }

        /*[HttpGet]
        public ActionResult Enter()
        {
            return View();
        }*/

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
            string errorMessage = ud.RemoveEventFromUser((int)Session["UserId"], uvModel.EventIdToRemove, (string)Session["UserType"]);

            uvModel.GetEventsByUserId((int)Session["UserId"]);

            // Insert this ViewData object onto the page somewhere so the user knows if there is an error
            // Otherwise the event will disappear from the list
            ViewData["ErrorMessage"] = errorMessage;

            return View("UserEvents", uvModel);
        }

        [HttpGet]
        public ActionResult SearchEvents()
        {
            if (Session["UserId"] == null || (int)Session["UserId"] == 0)
            {
                ViewData["ErrorMessage"] = "You must be signed in to view this data";
                return Redirect("~/Account/Login");
            }

            SearchEventsViewModel model = new SearchEventsViewModel();
            //uvModel.GetEventsByUserId((int)Session["UserId"]);

            return View("SearchEvents", model);// "UserEvents", uvModel);
        }

        [HttpPost]
        public ActionResult SearchEvents(SearchEventsViewModel searchEventsVM, string searchString)
        {
            UserData ud = new UserData();
            string[] names = searchString.Split(null);
            string firstName;
            string lastName;
            string errorMessage = null;
            int userId;

            if(names.Length == 2)
            {
                firstName = names[0];
                lastName  = names[1];
            }
            else
            {
                errorMessage = "Search string in an unexpected format.  Expected: FirstName LastName";
                return View();
            }

            userId = searchEventsVM.GetUserIdByName(firstName, lastName);
            if(userId == -1)
            {
                errorMessage = "The searched for was not found";
                return View();
            }

            searchEventsVM.SearchString = searchString;
            searchEventsVM.GetEventsByUserId(userId);
            ViewData["ErrorMessage"] = errorMessage;

            return View("SearchEvents", searchEventsVM);
        }

        [HttpGet]
        public ActionResult TotalDonations()
        {
            UserData ud = new UserData();
            ViewBag.Donations = ud.GetTotalDonations();

            return View();
        }

        [HttpGet]
        public ActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Donate(int Amount)
        {
            //Increment total donation amount
            UserData ud = new UserData();
            ud.AddPledge((int)Session["UserId"], Amount);
            //ud.AddPledge((int)Session["UserId"])
            return View();
        }

        [HttpPost]
        public ActionResult AddPledge(UserViewModel uvModel)
        {
            UserData ud = new UserData();
            string errorMessage = ud.AddPledge((int)Session["UserId"], uvModel.AmountToPledge);

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

        [HttpGet]
        public ActionResult ModifyEvents()
        {
            if (Session["UserId"] == null || (int)Session["UserId"] == 0)
            {
                ViewData["ErrorMessage"] = "You must be signed in to view this data";
                return Redirect("~/Account/Login");
            }

            ModifyEventViewModel model = new ModifyEventViewModel();
            model.GetEventsCreatedBy((int)Session["UserId"]);

            return View("ModifyEvents", model);
        }

        [HttpPost]
        public ActionResult ModifyEvents(EventModel model, string StartTime, string EndTime, DateTime NewDate)
        {
            ModifyEventViewModel model2 = new ModifyEventViewModel();
            int startMin;
            int startHour;
            int endMin;
            int endHour;
            
            string[] split = StartTime.Split(new char[] { ':', ' ' });
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

            string[] split2 = EndTime.Split(new char[] { ':', ' ' });
            endMin = Int32.Parse(split2[1]);

            if (split2[2].Equals("PM") & !split2[2].Equals("12"))
            {
                endHour = Int32.Parse(split2[0]) + 12;
            }
            else if (split2[2].Equals("AM") && split2[2].Equals("12"))
            {
                endHour = 24;
            }
            else
            {
                endHour = Int32.Parse(split2[0]);
            }
            model.StartMin = startMin;
            model.StartHour = startHour;
            model.EndMin = endMin;
            model.EndHour = endHour;
            model.Day = NewDate.Day;
            model.Month = NewDate.Month;
            model.Year = NewDate.Year;

            //uvModel.GetEventsCreatedBy((int)Session["UserId"]);
            model.ChangeEventDetails(model);
            model2.GetEventsCreatedBy((int)Session["UserId"]);

            return View("ModifyEvents", model2);
        }

        [HttpGet]
        public ActionResult AllEvents()
        {
            EventsModel events = new EventsModel();
            events.GetAllEvents();

            return View("AllEvents", events);
        }
    }
}