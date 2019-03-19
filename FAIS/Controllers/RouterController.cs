using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [Authorize]
    public class RouterController : Controller
    {
        [Authorize(Roles = "admin")]
        public PartialViewResult MetaBo()
        {
            return PartialView("_meta_bo");
        }
        public PartialViewResult TestMenu()
        {
            return PartialView("testmenu");
        }

        public PartialViewResult VComponents()
        {
            var a = new List<string>();
            foreach (string file in Directory.EnumerateFiles(Server.MapPath("~/Views/v_components/"), "*.cshtml"))
            {
                a.Add(String.Format("~/Views/v_components/{0}", file.Split('\\').Last()));
            }
            ViewBag.Views = a;
            return PartialView("_v_vomponents");
        }
    }
}