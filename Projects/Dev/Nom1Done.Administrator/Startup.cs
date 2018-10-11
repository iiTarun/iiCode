using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Nom1Done.Admin.hubs;
using Owin;


//[assembly: OwinStartupAttribute(typeof(Nom1Done.Admin.App_Start.Startup))]
//[assembly: OwinStartup(typeof(Nom1Done.Admin.Startup))]

namespace Nom1Done.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           ConfigureAuth(app);
            app.MapSignalR();
             
        }
    }
}
