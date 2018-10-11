using Nom1Done.Data;
using Nom1Done.Schedular;
using System;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Nom1Done
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected String SqlConnectionString { get; set; }

        protected void Application_Start()
        {

            using (var context = new NomEntities())
                SqlConnectionString = context.Database.Connection.ConnectionString;

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JobScheduler.Start();

            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Start(SqlConnectionString);           
        }


        protected void Application_End()
        {
            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Stop(SqlConnectionString);
        }
    }
}
