using FAIS.Models;
using FAIS.Models.Authorize;
using FAIS.Models.Repository;
using System;
using System.Linq;
using System.Web.Mvc;
using Z.EntityFramework.Plus;

namespace FAIS.Controllers
{
    [ViewAuthorize]
    [RoutePrefix("Bo")]
    public class BoController : Controller
    {
        private FAISEntities db = new FAISEntities();
        // GET: Bo
        [Route("Index/{id}")]
        public PartialViewResult Index(string id)
        {            
            if(id!= "workflow") id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            try
            {
                UserRoleManager.Instance.VerifyAccess(meta.BO_DB_NAME);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                ViewBag.Message = ex.Message;
                return PartialView("Unauthorized");
            }

            return PartialView(meta);
        }

        [Route("SubForm/{id}")]
        public PartialViewResult SubForm(string id)
        {
            ViewBag.isSubForm = true;
            id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            try
            {
                UserRoleManager.Instance.VerifyAccess(meta.BO_DB_NAME);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                ViewBag.Message = ex.Message;
                return PartialView("Unauthorized");
            }

            return PartialView("Index",meta);
        }

        // GET: Bo
        [Authorize(Roles = "admin")]
        [Route("admin/{id}")]
        public PartialViewResult Admin(string id)
        {
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView("Index", meta);
        }

        [Route("test")]
        public PartialViewResult Test()
        {
            return PartialView();
        }

        [Route("Formulaire/{id}")]
        public PartialViewResult Formulaire(string id, string compKey)
        {
            if(! id.Contains("_BO_")) id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);
            ViewBag.compKey = compKey;
            return PartialView(meta);
        }
        [Route("Table/{id}")]
        public PartialViewResult Table(string id)
        {
            if (!id.Contains("_BO_")) id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView(meta);
        }
        [Route("tableeditable/{id}")]
        public PartialViewResult TableEditable(string id)
        {
            if (!id.Contains("_BO_")) id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView(meta);
        }

        // GET: Bo
        [Route("page/{id}")]
        public PartialViewResult Page(int id)
        {
            ViewBag.pageId = id;
            ViewBag.PageStatus = "PUBLIC";

            var data = db.PAGE.Where(p => p.BO_ID == id && p.STATUS == "public").FirstOrDefault();

            ViewBag.dataLayout = data.LAYOUT == null ? "" : data.LAYOUT;
            return PartialView("~/Views/Admin/Page.cshtml");
        }
    }
}