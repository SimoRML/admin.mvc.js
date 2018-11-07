using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    public class RouterController : Controller
    {
        public PartialViewResult MetaBo()
        {
            return PartialView("_meta_bo");
        }
    }
}