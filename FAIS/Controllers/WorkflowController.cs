using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    public class WorkflowController : Controller
    {
        public PartialViewResult Home()
        {
            return PartialView();
        }

        public PartialViewResult Validation(long id)
        {
            ViewBag.id = id;
            return PartialView();
        }
    }
}