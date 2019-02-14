using System;
using System.Collections;
using System.Collections.Generic;
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
        [HttpGet]
        [Route("lang/{lang}")]
        public IHttpActionResult Langue(string lang)
        {
            ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("lang", "./Resources", typeof(Dictionary<string, string>));
            ResourceSet resourceSet  = rm.GetResourceSet(new System.Globalization.CultureInfo(lang), true, false);
            
            return Ok(Assembly.GetExecutingAssembly().GetManifestResourceNames());
        }
    }
}
