using FAIS.Models;
using FAIS.Models.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [ViewAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            SessionVar.Clear();
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        
        public PartialViewResult Reporting()
        {
            return PartialView();
        }
    }
}
