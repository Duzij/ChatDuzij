using System;
using System.Collections.Generic;
using System.Linq;
using ChatDuzijCore.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatDuzijCore.Startup))]

namespace ChatDuzijCore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ApplicationDbContext context = new ApplicationDbContext();
        }
    }
}