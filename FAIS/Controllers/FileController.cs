using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using System.Drawing;

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
        [AllowAnonymous]
        [Route("ls/{dirName}")]
        public IHttpActionResult Ls(string dirName)
        {
            var d = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + dirName));
            var hasThumb = d.GetDirectories().Where(x => x.Name == "thumb").Count() > 0;
            return Ok(d.GetFiles().Select(
                (x, index) =>
                new
                {
                    name = x.Name,
                    index,
                    image = "Content/files/" + dirName + "/" + x.Name,
                    thumb = hasThumb ? "Content/files/" + dirName + "/thumb/" + x.Name : "",
                }
                )
            );
        }



        [HttpGet]
        [Route("name/{type}")]
        public IHttpActionResult Name(string type)
        {
            var fileName = "img-" + DateTime.Now.ToString("yyyyMMddhhmmss"); // DateTime.Now.ToString("yyyyMMdd.hhmmss.") + type + ".json";
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

        [HttpPost]
        [Route("saveimg")]
        public IHttpActionResult SaveImg(MultiFileModel model)
        {
            try
            {
                // var fileName = DateTime.Now.ToString("yyyyMMdd.hhmmss.") + type + ".json";
                // File.Create(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + fileName + ".tmp"));
                var dirName = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + model.FileName);
                Directory.CreateDirectory(dirName);
                Directory.CreateDirectory(Path.Combine(dirName, "thumb"));
                int thumbWidth = 120;

                foreach (var fileB64 in model.Files)
                {
                    if (fileB64.base64 == null) continue;
                    var fileName = Path.Combine(dirName, fileB64.name);
                    var thumbName = Path.Combine(dirName, "thumb", fileB64.name);
                    byte[] fileBytes = Convert.FromBase64String(fileB64.base64String);
                    using (MemoryStream uplfileStream = new MemoryStream(fileBytes, 0, fileBytes.Length))
                    {
                        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                        {
                            uplfileStream.CopyTo(fs);
                            fs.Flush();
                        }

                        Image image = Image.FromStream(uplfileStream);
                        Image thumb = image.GetThumbnailImage(thumbWidth, (int)(((float)image.Height / (float)image.Width) * (float)thumbWidth), () => false, IntPtr.Zero);
                        thumb.Save(thumbName);
                    }
                }
                return Ok(new { success = true, dir = dirName });
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
        public string base64String
        {
            get
            {
                return this.base64.Contains("base64,") ? this.base64.Split(new string[] { "base64," }, StringSplitOptions.None)[1] : this.base64;
            }
        }
        public int index { get; set; }
    }
}
