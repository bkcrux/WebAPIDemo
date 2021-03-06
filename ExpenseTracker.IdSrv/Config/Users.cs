﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace ExpenseTracker.IdSrv.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>() {
                new InMemoryUser
                {
                    Username = "Kevin",
                    Password = "secret",
                    Subject = "1",

                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Kevin"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Doxxx"),
                        new Claim(Constants.ClaimTypes.Role, "WebReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "WebWriterUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileWriterUser"),

                    }
                },
                new InMemoryUser
                {
                    Username = "Sven",
                    Password = "secret",
                    Subject = "2",

                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Sven"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Jones"),
                        new Claim(Constants.ClaimTypes.Role, "WebReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileWriterUser"),
                    }
                },
                new InMemoryUser
                {
                    Username = "Fred",
                    Password = "secret",
                    Subject = "3",

                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Fred"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Role, "MobileReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileWriterUser"),
                    }
                },

                new InMemoryUser
                {
                    Username = "Jimmy",
                    Password = "secret",
                    Subject = "4",

                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Jimmy"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Lawson"),
                        new Claim(Constants.ClaimTypes.Role, "MobileReaderUser"),
                        new Claim(Constants.ClaimTypes.Role, "MobileWriterUser"),
                    }
                },


            };
        }
    }
}