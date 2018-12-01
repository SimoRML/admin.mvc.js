using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FAIS.Models
{
    public class Helper
    {
        public static string GetSQL(string fileName)
        {

            return File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/SQL/" + fileName))
                .Replace("\r\n", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                ;
        }
    }
}