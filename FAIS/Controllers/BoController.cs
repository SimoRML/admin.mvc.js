using FAIS.Models;
using FAIS.Models.Authorize;
using FAIS.Models.Repository;
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
            id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView(meta);
        }

        [Route("SubForm/{id}")]
        public PartialViewResult SubForm(string id)
        {
            ViewBag.isSubForm = true;
            id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView("Index",meta);
        }

        // GET: Bo
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
        public PartialViewResult Formulaire(string id)
        {
            if(! id.Contains("_BO_")) id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView(meta);
        }
        [Route("Table/{id}")]
        public PartialViewResult Table(string id)
        {
            if (!id.Contains("_BO_")) id += "_BO_";
            var meta = new MetaBoRepo().GetMETA(id);

            return PartialView(meta);
        }
    }
}