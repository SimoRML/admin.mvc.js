using FAIS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FAIS.Controllers
{
    public class RssController : ApiController
    {

        // GET api/values
        public IHttpActionResult Get()
        {
            SGBD s = new SGBD();
            DataTable types = s.Cmd("select * from [dbo].[Types_BO_]");

            DataTable t = s.Cmd(@"
select 
	BO_ID, 
	Type, 
	[Transaction], 
	Emplacement, 
	Pieces, 
	Salles_de_bains, 
	quipements_et_Caracteristiques, 
	Prix,
	Description, 
	Premium, 
	Titre,
	Superficie, 
	Refrence,
    Image_2
from akkorimm_admin_user.article_BO_
 Where Active = 1  AND Refrence is not null");

            var biens = new XElement("biens");
            foreach (DataRow row in t.Rows)
            {
                var bien = new XElement("bien");
                bien.SetAttributeValue("REFERENCE", row["Refrence"].ToString());
                bien.Add(new XElement("TITLE", row["Titre"].ToString()));
                bien.Add(new XElement("TYPE", types.Select("BO_ID = " + row["Type"].ToString())[0]["Label"].ToString()));
                bien.Add(new XElement("TRANSACTION", types.Select("BO_ID = " + row["Transaction"].ToString())[0]["Label"].ToString()));
                bien.Add(new XElement("DESCRIPTION", row["Description"].ToString()));

                bien.Add(new XElement("CITY", types.Select("BO_ID = " + row["Emplacement"].ToString())[0]["Label"].ToString()));
                bien.Add(new XElement("Currency", "MAD"));
                bien.Add(new XElement("PRICE", row["Prix"].ToString()));
                bien.Add(new XElement("SURFACE", row["Superficie"].ToString()));
                bien.Add(new XElement("NBRE_PIECES", row["Pieces"].ToString()));
                bien.Add(new XElement("NBRE_BATHS", row["Salles_de_bains"].ToString()));
                bien.Add(new XElement("FULLKITCHEN", row["quipements_et_Caracteristiques"].ToString().Contains("\"37\"") ? "yes" : "no"));
                bien.Add(new XElement("TERRACE", row["quipements_et_Caracteristiques"].ToString().Contains("\"29\"") ? "yes" : "no"));
                bien.Add(new XElement("POOL", row["quipements_et_Caracteristiques"].ToString().Contains("\"17\"") ? "yes" : "no"));
                bien.Add(new XElement("GARDEN", row["quipements_et_Caracteristiques"].ToString().Contains("\"27\"") ? "yes" : "no"));
                bien.Add(new XElement("ELEVATOR", row["quipements_et_Caracteristiques"].ToString().Contains("\"28\"") ? "yes" : "no"));
                bien.Add(new XElement("HEATING", row["quipements_et_Caracteristiques"].ToString().Contains("\"53\"") ? "yes" : "no"));
                bien.Add(new XElement("FIREPLACE", row["quipements_et_Caracteristiques"].ToString().Contains("\"67\"") ? "yes" : "no"));
                bien.Add(new XElement("TV", row["quipements_et_Caracteristiques"].ToString().Contains("\"18\"") ? "yes" : "no"));

                /*
                    MICROWAVE = "no",
                    FRIDGE = "no",
                    OVEN = "no",
                    WASHER = "no",
                GARAGE = "no",
                    SECURITY = "no",
                    SEAVIEWS = "no",
                    MOUNTAINSVIEWS = "no",
                    STORAGEROOM = "no",
                    DOUBLEGLAZING = "no",
                    REINFORCEDDOOR = "no",
                    SATELLITE = "no",
                    INTERNET = "no",
                    */


                try
                {
                    // GET IMAGES
                    var d = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/" + row["Image_2"].ToString()));

                    //bien.PHOTO1 = "http://net.akkor-immobilier.com/Content/files/"+ row["Image_2"].ToString() + "/" + d.GetFiles().FirstOrDefault().Name;
                    int idx = 1;
                    foreach (var x in d.GetFiles())
                    {
                        bien.Add(new XElement("PHOTO" + idx, "http://net.akkor-immobilier.com/Content/files/" + row["Image_2"].ToString() + "/" + x.Name));
                        idx++;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                }

                biens.Add(bien);
                
            }

            //return biens;
            return Content(HttpStatusCode.OK, biens, Configuration.Formatters.XmlFormatter);
        }

      
    }
    
}