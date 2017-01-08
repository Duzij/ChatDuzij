using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using OpenChat.Models;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(OpenChat.Startup))]
namespace OpenChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ChatDbContext context = new ChatDbContext();
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);
        }
    }
}