using FAIS.Models;
using FAIS.Models.Authorize;
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
            var meta = db.META_BO.Where(o => o.BO_DB_NAME == id)
                .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
                .FirstOrDefault();
            
            return PartialView(meta);
        }
    }
}