﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Core.Models;

namespace ExpenseTracker.IdSrv.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    DisplayName = "Roles",
                    Description = "The roles the user belongs to",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }

                },
            };

            return scopes;
        }
    }
}