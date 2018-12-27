using FAIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Profile")]
    public class ProfileManagerController : ApiController
    {
        FAISEntities db = new FAISEntities();
        // GET: api/ProfileManager
        [HttpGet]
        [Route("Menu")]
        public HttpResponseMessage Menu()
        {
            var dt = db.META_BO.Where(x => x.META_BO_ID != 1 && x.TYPE == "form").ToList();
            List<MenuFields> menu_ = new List<MenuFields>();

            // menu_.Add(new MenuFields { icon = "dashboard", text = "Meta Bo", href = "#router.metabo" });

            foreach (var item in dt)
            {
                menu_.Add(new MenuFields
                {
                    icon = "dashboard",
                    text = item.BO_NAME,
                    href = "bo.index." + item.BO_DB_NAME.Replace("_BO_", ""),
                    User = null,
                    parent = null,
                    childs = null
                });
            }



            var menu = new
            {
                Admin = new
                {
                    icon = "build",
                    text = "Administration",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new[] {
                        new MenuFields { icon = "dashboard", text = "Meta Bo", href = "router.metabo" },
                        new MenuFields { icon = "dashboard", text = "Workflow", href = "workflow.home" },
                        new MenuFields { icon = "pie_chart", text = "Reporting", href = "home.reporting" },
                    },
                    open= false,
                },
                Bo = new
                {
                    icon = "extension",
                    text = "Objects",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = menu_.ToArray(),
                    open = false,
                },
            };
            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                menu);
        }

        [HttpGet]
        [ResponseType(typeof(List<string>))]
        [Route("Validators")]
        public async Task<IHttpActionResult> GetValidators()
        {
            List<string> validators = await db.Database.SqlQuery<string>("SELECT [Email] FROM [AspNetUsers] order by Email").ToListAsync();            
            return Ok(validators);
        }

    }
}
