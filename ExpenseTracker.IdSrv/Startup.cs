using ExpenseTracker.IdSrv.Config;
using Microsoft.Owin;
using Owin;
using System;
using System.Security.Cryptography.X509Certificates;
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
                    Factory = InMemoryFactory.Create(
                        users: Users.Get(),
                        clients: Clients.Get(),
                        scopes: Scopes.Get()),
                    SigningCertificate = LoadCertificate()
                });
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
