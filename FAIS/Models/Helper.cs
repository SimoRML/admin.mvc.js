using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FAIS.Models
{
    public class Helper
    {
        private const string EmptyChar = "";

        public static string GetSQL(string fileName) => File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/SQL/" + fileName))
                .Replace("\r\n", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                ;


        public static string cleanDBName(string str)
        {
            str = str
                .Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                .Replace(' ', '_')
                .Replace("(", EmptyChar)
                .Replace(")", EmptyChar)
                .Replace("[", EmptyChar)
                .Replace("]", EmptyChar)
                .Replace("{", EmptyChar)
                .Replace("}", EmptyChar)
                .Replace('é', 'e')
                .Replace('è', 'e')
                .Replace('ë', 'e')
                .Replace('ê', 'e')
                .Replace('à', 'a')
                .Replace('â', 'a')
                .Replace('ç', 'c')
                ;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') /*|| c == '.'*/ || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}