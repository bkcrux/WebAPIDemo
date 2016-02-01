using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace ExpenseTracker.WebClient.Helpers
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {

            switch (context.Resource.First().Value)
            {
                case "ExpenseGroup":
                    return AuthorizationExpenseGroup(context);

                default:
                    return Nok();
            }
        }


        private Task<bool> AuthorizationExpenseGroup(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case "Read":
                    return Eval(context.Principal.HasClaim("role", "WebReaderUser"));
                case "Write":
                    return Eval(context.Principal.HasClaim("role", "WebWriterUser"));
                default:
                    return Nok();
            }
        }

    }
}