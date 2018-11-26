using FAIS.Models.Authorize;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [ViewAuthorize]
    [RoutePrefix("Bo")]
    public class BoController : Controller
    {
        // GET: Bo
        [Route("Index/{id}")]
        public PartialViewResult Index(string id)
        {
            ViewBag.name = id;
            return PartialView();
        }
    }
}