using FAIS.Models;
using FAIS.Models.Authorize;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [ViewAuthorize]
    public class WorkflowController : Controller
    {
        private FAISEntities db = new FAISEntities();
        public PartialViewResult Home(long id)
        {
            ViewBag.id = id;
            //META_BO mETA_BO = await db.META_BO.FindAsync(id);
            WORKFLOW workflow = db.WORKFLOW.Find(id);

            return PartialView(workflow);
        }

        public PartialViewResult Validation(long id)
        {
            ViewBag.id = id;
            return PartialView();
        }
        [Route("Workflow/Mapping/{id}")]
        public PartialViewResult Mapping(long id, long id2)
        {
            ViewBag.id_parent = id;
            ViewBag.id_child = id2;
            return PartialView();
        }

        [Route("api/workflow/submit/{id}")]
        public PartialViewResult Submit(long id)
        {
            return PartialView();
        }
    }
}