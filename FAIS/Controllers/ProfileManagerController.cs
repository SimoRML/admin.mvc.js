using FAIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Profile")]
    public class ProfileManagerController : ApiController
    {
        // GET: api/ProfileManager
        [HttpGet]
        [Route("Menu")]
        public HttpResponseMessage Menu()
        {
            FAISEntities db = new FAISEntities();
            var dt = db.META_BO.Where(x => x.META_BO_ID != 1).ToList();
            List<MenuFields> menu_ = new List<MenuFields>();

            menu_.Add(new MenuFields { icon = "dashboard", text = "Meta Bo", href = "#router.metabo" });

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
                    icon = "add_shopping_cart",
                    text = "Administration",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = menu_.ToArray()
                },

            };
            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                menu);
        }

        // GET: api/ProfileManager/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProfileManager
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ProfileManager/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ProfileManager/5
        public void Delete(int id)
        {
        }

    }
}
