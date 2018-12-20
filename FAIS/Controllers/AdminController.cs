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
    }
}