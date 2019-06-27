using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get(string fileName)
        {
            return Ok(File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + fileName)));
        }

        [HttpGet]
        [Route("name/{type}")]
        public IHttpActionResult Name(string type)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd.hhmmss.") + type + ".json";
            // File.Create(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + fileName + ".tmp"));

            return Ok(fileName);
        }

        [HttpPost]
        [Route("save")]
        public IHttpActionResult Save(MultiFileModel model)
        {
            // var fileName = DateTime.Now.ToString("yyyyMMdd.hhmmss.") + type + ".json";
            // File.Create(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + fileName + ".tmp"));
            try
            {
                var serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

                File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + model.FileName), serializer.Serialize(model.Files), System.Text.Encoding.UTF8);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, msg = ex.Message });
            }
            
        }

    }

    public class MultiFileModel
    {
        public string FileName { get; set; }
        public List<FileBase64> Files { get; set; }
    }

    public class FileBase64
    {
        public string name { get; set; }
        public string fileType { get; set; }
        public string base64 { get; set; }
        public int index { get; set; }
    }
}
