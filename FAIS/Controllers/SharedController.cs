using FAIS.Models;
using FAIS.Models.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/shared")]
    public class SharedController : ApiController
    {

        private FAISEntities db = new FAISEntities();

        [HttpGet]
        [Route("lang/{lang}")]
        public IHttpActionResult Langue(string lang)
        {
            ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("lang", "./Resources", typeof(Dictionary<string, string>));
            ResourceSet resourceSet  = rm.GetResourceSet(new System.Globalization.CultureInfo(lang), true, false);
            
            return Ok(Assembly.GetExecutingAssembly().GetManifestResourceNames());
        }

        [HttpGet]
        [Route("page/{id}")]
        public IHttpActionResult Page(long id)
        {
            var data = db.PAGE.Where(p => p.BO_ID == id).FirstOrDefault();

            return Ok(data);
        }

        [HttpPost]
        [Route("query")]
        public IHttpActionResult Query(UserQuery model)
        {
            if (!model.validate()) return BadRequest("Only select querys are granted !");
            try
            {
                return Ok(model.Execute());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }
    }
}
