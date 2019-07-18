using FAIS.Models;
using FAIS.Models.Authorize;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace FAIS.Controllers
{
    [ViewAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            SessionVar.Clear();
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        
        public PartialViewResult Reporting()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [Route("img/{bo}/{field}/{id}")]
        public ActionResult Img(string bo, string field, string id)
        {
            
            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelectFields(bo, new List<string> { field }, " where c.BO_ID=" + id));
            ImageBase64 img = System.Web.Helpers.Json.Decode<ImageBase64>(dt.Rows[0][field].ToString());

            return base.File(Convert.FromBase64String(img.Base64.Split(new string[] { "base64," }, StringSplitOptions.None)[1]), "image/jpeg");
        }
    }
}
