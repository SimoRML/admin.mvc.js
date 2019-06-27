using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using FAIS.Providers;
using FAIS.Models;
using System.Web.Cors;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;
using System.Configuration;

namespace FAIS
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // Pour plus d'informations sur la configuration de l'authentification, visitez https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configurer le contexte de base de données et le gestionnaire des utilisateurs pour utiliser une instance unique par demande
            
            if (! string.IsNullOrEmpty(ConfigurationManager.AppSettings["cors"]))
            {
                var policy = new CorsPolicy()
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true,
                    SupportsCredentials = true
                };
                string[] cors = ConfigurationManager.AppSettings["cors"].Split(';');
                foreach (var c in cors)
                {
                    policy.Origins.Add(c);
                }
                app.UseCors(new CorsOptions
                {
                    PolicyProvider = new CorsPolicyProvider
                    {
                        PolicyResolver = context => Task.FromResult(policy)
                    }
                });
            }
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Activer l'application pour utiliser un cookie afin de stocker les informations de l'utilisateur connecté
            // et utiliser un cookie pour stocker temporairement des informations sur un utilisateur qui se connecte avec un fournisseur de connexion tiers
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configurer l'application pour le flux basé sur OAuth
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // Jeu de modes en production AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Activer l'application pour utiliser les jetons du porteur afin d'authentifier les utilisateurs
            app.UseOAuthBearerTokens(OAuthOptions);

            // Décommenter les lignes suivantes pour activer la connexion avec des fournisseurs de connexion tiers
            app.UseMicrosoftAccountAuthentication(
                clientId: "7a32145e-7759-48d8-9146-c1d978e5fb0c",
                clientSecret: "Ig9Nr6qW85og7L24YjtHA5YRpypak+fOD/U5bS93ZdU=");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
