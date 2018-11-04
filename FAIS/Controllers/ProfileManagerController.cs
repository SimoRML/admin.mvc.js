using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


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
            var menu = new
            {
                Accueil = new { text="Accueil", href = "home", User=User.Identity.Name }
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
