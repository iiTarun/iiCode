using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Nom1Done.Startup))]
namespace Nom1Done
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
