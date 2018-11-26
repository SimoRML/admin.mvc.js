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

            var menu = new
            {
                Admin = new
                {
                    icon = "add_shopping_cart",
                    text = "Administration",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        da = new { icon = "dashboard", text = "Meta Bo", href = "#router.metabo" },
                    }
                }
                /*
                Dashboard = new { icon = "dashboard", text = "Tableau de bord", href = "home", User = User.Identity.Name, parent = false },

                Achat = new
                {
                    icon = "add_shopping_cart",
                    text = "Achat",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        da = new { icon = "dashboard", text = "Demande d'achat", href = "home" },
                        bc = new { icon = "dashboard", text = "Bon de commande", href = "home" },
                        br = new { icon = "dashboard", text = "Bon de reception", href = "home" },
                        ff = new { icon = "dashboard", text = "Facture fournisseur", href = "home" }
                    }
                },
                Stock = new
                {
                    icon = "sd_card",
                    text = "Stock",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        br = new { icon = "dashboard", text = "Bon de reception", href = "home" },
                        bs = new { icon = "dashboard", text = "Bon de sortie", href = "home" },
                        inventaire = new { icon = "dashboard", text = "Inventaire", href = "home" },
                        Valorisation = new { icon = "dashboard", text = "Valorisation", href = "home" },
                        cc = new { icon = "dashboard", text = "Calcul des coûts", href = "home" },
                        etatmonth = new { icon = "dashboard", text = "Etat mensuel du stock", href = "home" }
                    }
                },
                MainOeuvre = new
                {
                    icon = "group_add",
                    text = "Main d'oeuvre",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        mot_Journalier = new { icon = "dashboard", text = "Main d'oeuvre Travaux Journaliers", href = "home" },
                        mot_total = new { icon = "dashboard", text = "Main d'oeuvre Travaux Total ", href = "home" },
                        heureSup = new { icon = "dashboard", text = "Main d'oeuvre Heure SUP", href = "home" },
                        absence = new { icon = "dashboard", text = "Déclaration d'absences", href = "home" }
                    }
                },
                Immobilisation = new
                {
                    icon = "time_to_leave",
                    text = "Immobilisation",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        atelier = new { icon = "dashboard", text = "Atelier", href = "home" },
                        materiel = new { icon = "dashboard", text = "Matériel", href = "home" }
                    }
                },
                Budget = new
                {
                    icon = "attach_money",
                    text = "Budget",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        Assolement = new { icon = "dashboard", text = "Assolement", href = "home" }

                    }
                },
                Vente = new
                {
                    icon = "monetization_on",
                    text = "Vente",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        Vente = new { icon = "dashboard", text = "Vente", href = "home" },
                        Facture = new { icon = "dashboard", text = "Facture", href = "home" }
                    }
                },
                Dematerialisation = new { icon = "commute", text = "Dématérialisation", href = "home", User = User.Identity.Name, parent = false },
                Reporting = new
                {
                    icon = "print",
                    text = "Reporting",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        Vente = new { icon = "dashboard", text = "Etat stock", href = "home" },
                        stock_par_date = new { icon = "dashboard", text = "Stock par date", href = "home" },
                        stock_par_mois = new { icon = "dashboard", text = "Stock par mois", href = "home" },
                        ventilation_materiel = new { icon = "dashboard", text = "Ventilation Matériel", href = "home" },
                        mo_tj = new { icon = "dashboard", text = "MO - Travaux Journaliers", href = "home" },
                        mo_h_sup = new { icon = "dashboard", text = "MO - Heures SUP", href = "home" },
                        mo_absence = new { icon = "dashboard", text = "MO - Absences", href = "home" }
                    }
                },
                Edition = new
                {
                    icon = "find_replace",
                    text = "Edition",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        edition_da = new { icon = "dashboard", text = "Demande d'achat", href = "home" },
                        edition_bc = new { icon = "dashboard", text = "bon de commande", href = "home" },
                        edition_br = new { icon = "dashboard", text = "Bon de réception", href = "home" },
                        edition_bs = new { icon = "dashboard", text = "Bon de sortie", href = "home" },
                        edition_vs = new { icon = "dashboard", text = "Ventilation sorties", href = "home" }
                    }
                },
                Analytics = new
                {
                    icon = "pie_chart",
                    text = "Analitics",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new
                    {
                        edition_da = new { icon = "dashboard", text = "Consommations valorisées / Article", href = "home" },
                        edition_bc = new { icon = "dashboard", text = "Consommations valorisées / Catégorie", href = "home" }
                    }
                },*/
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
