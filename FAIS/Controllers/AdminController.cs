using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
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
    }
}