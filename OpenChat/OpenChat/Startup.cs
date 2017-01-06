using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using OpenChat.Models;
using Owin;

[assembly: OwinStartup(typeof(OpenChat.Startup))]

namespace OpenChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ChatDbContext context = new ChatDbContext();
            context.Configuration.AutoDetectChangesEnabled = false;
        }
    }
}