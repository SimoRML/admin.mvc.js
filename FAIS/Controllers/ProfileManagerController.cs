using FAIS.Models;
using FAIS.Models.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public async Task<IHttpActionResult> Menu()
        {
            var metas = await new MetaBoRepo().GetMetaBoExAsync(" WHERE CREATED_BY <> 'admin' AND TYPE='form'");

            Dictionary<string, object> menu = new Dictionary<string, object>();
            menu.Add("Admin", new
            {
                icon = "build",
                text = "Administration",
                href = "home",
                User = User.Identity.Name,
                parent = true,
                childs = new[] {
                        new MenuFields { icon = "dashboard", text = "Meta Bo", href = "router.metabo" },
                        new MenuFields { icon = "dashboard", text = "Workflow", href = "workflow.home" },
                        new MenuFields { icon = "web", text = "Page", href = "admin.page" },
                        new MenuFields { icon = "pie_chart", text = "Reporting", href = "home.reporting" },
                    },
                open = false,
            });

            foreach (var meta in metas)
            {
                if (meta.GROUPE == null) meta.GROUPE = "Objects";

                if (!menu.ContainsKey(meta.GROUPE))
                {
                    menu.Add(meta.GROUPE, new Dictionary<string, object>()
                        {
                            { "icon", "layers" },
                            { "text", meta.GROUPE },
                            { "href", "home" },
                            { "User", User.Identity.Name },
                            { "parent", true },
                            { "childs", new List<object>() },
                            { "open", false }
                        });
                }
                ((List<object>)((Dictionary<string, object>)menu[meta.GROUPE])["childs"]).Add(new
                {
                    icon = "",
                    text = meta.BO_NAME,
                    href = "bo.index." + meta.BO_DB_NAME.Replace("_BO_", ""),
                    User = "",
                    parent = false,
                    childs = ""
                });
            }
            return Ok(menu);
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
