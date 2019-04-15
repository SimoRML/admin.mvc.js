using FAIS.Models;
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
        private FAISEntities db = new FAISEntities();
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
        
        public PartialViewResult Page(int id)
        {
            ViewBag.pageId = id;

            var data = db.PAGE.Where(p => p.BO_ID == id).FirstOrDefault();

            ViewBag.STATUS = data.STATUS;
            ViewBag.Mode = "edit";
            ViewBag.dataLayout = data.LAYOUT == null ? "" : data.LAYOUT;
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