using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using OpenChat.Models;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Web.Http;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(OpenChat.Startup))]

namespace OpenChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            var serializer = JsonSerializer.Create(serializerSettings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            app.MapSignalR(hubConfiguration);
        }
    }
}