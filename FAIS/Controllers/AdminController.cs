using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult MetaField()
        {

            return PartialView();
        }

        public PartialViewResult MetaFields()
        {

            return PartialView();
        }
        
        public PartialViewResult Page(string id)
        {
            ViewBag.layout = id;

            return PartialView();
        }
        public PartialViewResult PageList()
        {
            return PartialView();
        }
        public PartialViewResult PageLayout(string id)
        {
            return PartialView("layouts/" + id);
        }

        public PartialViewResult Widgets(string id)
        {
            return PartialView("widgets/" + id);
        }

        public PartialViewResult Roles()
        {
            return PartialView();
        }

        public PartialViewResult RolesAdd()
        {
            return PartialView();
        }

        public PartialViewResult UserAdd()
        {
            return PartialView();
        }

        public PartialViewResult boRoles()
        {
            return PartialView();
        }

        public PartialViewResult RolesUsers()
        {
            return PartialView();
        }
    }
}