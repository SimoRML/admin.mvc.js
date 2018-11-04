using System.Web;
using System.Web.Optimization;

namespace FAIS
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l'outil de génération à l'adresse https://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.            

            bundles.Add(new ScriptBundle("~/bundles/_front-before").Include(
                      "~/_front/util.js"));

            bundles.Add(new ScriptBundle("~/bundles/_front").Include(
                      "~/_front/models/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/_front-after").Include(
                      "~/_front/main.js"));
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
