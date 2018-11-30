using FAIS.Models;
using FAIS.Models.Authorize;
using System.Linq;
using System.Web.Mvc;

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
            var meta = db.META_BO.Where(o => o.BO_NAME == id).FirstOrDefault();


            return PartialView(meta);
        }
    }
}