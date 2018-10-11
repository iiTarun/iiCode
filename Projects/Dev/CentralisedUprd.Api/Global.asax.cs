using CentralisedUprd.Api.JobSchedular;
using CentralisedUprd.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;   

namespace CentralisedUprd.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected String SqlConnectionString { get; set; }

        protected void Application_Start()
        {

            using (var context =  new UprdDbEntities1())
                SqlConnectionString = context.Database.Connection.ConnectionString;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Start(SqlConnectionString);

            JobSchedularForAlerts.Start();

            //register your filter with Web API pipeline
            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionFilterAttribute());

        }

        protected void Application_End()
        {
            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Stop(SqlConnectionString);
        }

        protected void Application_Error()
        {
            var objError = Server.GetLastError();           
            ErrorLogService.LogError(objError);
            Server.ClearError();
        }

        

        //Create filter
        public class LogExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {

                var objError = context.Exception;               
                ErrorLogService.LogError(objError);
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        //common service to be used for logging errors
        public static class ErrorLogService
        {
            public static void LogError(Exception objError)
            {
                //Email to developers
                StringBuilder lasterror = new StringBuilder();
                if (objError != null)
                {
                    if (objError.Message != null)
                    {
                        lasterror.AppendLine("Message:");
                        lasterror.AppendLine(objError.Message);
                        lasterror.AppendLine();
                    }
                    if (objError.InnerException != null)
                    {
                        lasterror.AppendLine("InnerException:");
                        lasterror.AppendLine(objError.InnerException.ToString());
                        lasterror.AppendLine();
                    }

                    if (objError.Source != null)
                    {
                        lasterror.AppendLine("Source:");
                        lasterror.AppendLine(objError.Source);
                        lasterror.AppendLine();
                    }

                    if (objError.StackTrace != null)
                    {
                        lasterror.AppendLine("StackTrace:");
                        lasterror.AppendLine(objError.StackTrace);
                        lasterror.AppendLine();
                    }

                }

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("EmailIdForAlert"), ConfigurationManager.AppSettings.Get("EmailPasswordForAlert"))
                };


                string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                string subject = "Error Alert : CentralisedUPRD.API (NatGasHub.Com) ";
                string[] recipients;
                 var environment = ConfigurationManager.AppSettings.Get("Environment");
                //if ( environment == "local")
                //{
                //    recipients = new string[] { "vijay.kumar@fifthnote.co" };
                //}
                //else
                //{
                //    var developerIds = ConfigurationManager.AppSettings.Get("EmailIdsToGetErrors");
                //    recipients = new string[] { developerIds };
                //}

                var developerIds = ConfigurationManager.AppSettings.Get("EmailIdsToGetErrors");
                if (string.IsNullOrEmpty(developerIds)) { developerIds = "vijay.kumar@fifthnote.co"; }
                recipients = new string[] { developerIds };

                subject += "("+ environment + ")";
                using (var msg = new System.Net.Mail.MailMessage(from, recipients[0], subject, lasterror.ToString()))
                {
                    for (int i = 1; i < recipients.Length; i++)
                        msg.To.Add(recipients[i]);

                    //msg.CC.Add("lakhwinder.singh@invertedi.com");
                    msg.IsBodyHtml = false;
                    try
                    {
                        smtp.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("CentralisedUPRD.API Error-mailing is not working. ", ex);
                    }
                }
            }
        }


    }
}
