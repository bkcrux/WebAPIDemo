using ExpenseTracker.IdSrv.Config;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json.Linq;
using Owin;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Configuration;

[assembly: OwinStartupAttribute(typeof(ExpenseTracker.IdSrv.Startup))]
namespace ExpenseTracker.IdSrv
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    IssuerUri = ExpenseTrackerConstants.IdSrvIssuedUri,
                    SigningCertificate = LoadCertificate(),

                    Factory = InMemoryFactory.Create(
                        users: Users.Get(),
                        clients: Clients.Get(),
                        scopes: Scopes.Get()),

                    AuthenticationOptions = new Thinktecture
                        .IdentityServer.Core.Configuration.AuthenticationOptions
                    {
                        IdentityProviders = ConfigureIdentityProvider
                    }
                });
            });
        }


        private void ConfigureIdentityProvider(IAppBuilder app, string signInAsType)
        {
            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {               
                AuthenticationType = "Facebook",                
                Caption = "Sign-in with Facebook",
                SignInAsAuthenticationType = signInAsType,
                AppId = "xxxx",
                AppSecret = "xxxx",

                Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        JToken lastName, firstName;
                        if (context.User.TryGetValue("last_name", out lastName))
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim(
                                Thinktecture.IdentityServer.Core.Constants.ClaimTypes.FamilyName,
                                lastName.ToString()));

                        }
                        else
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim(
                                Thinktecture.IdentityServer.Core.Constants.ClaimTypes.FamilyName,
                                context.User.ToString()));
                        }

                        if (context.User.TryGetValue("first_name", out firstName))
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim(
                                Thinktecture.IdentityServer.Core.Constants.ClaimTypes.GivenName,
                                firstName.ToString()));
                        }
                        else
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim(
                                Thinktecture.IdentityServer.Core.Constants.ClaimTypes.GivenName,
                                "John"));
                        }

                        context.Identity.AddClaim(new System.Security.Claims.Claim("role", "WebReaderUser"));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("role", "WebWriterUser"));
                        return Task.FromResult(0);

                    }
                },

            });
        }


        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}
