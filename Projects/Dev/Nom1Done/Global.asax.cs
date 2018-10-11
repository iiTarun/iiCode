using Nom1Done.Data;
using Nom1Done.Schedular;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
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


        protected void Application_Error()
        {
            var objError = Server.GetLastError();
            Console.Write(objError.Message);
            StringBuilder lasterror = new StringBuilder();
            if (objError != null) {              
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
            string subject = "Error Alert: NatGasHub.Com ";
            string[] recipients;
            var env = ConfigurationManager.AppSettings.Get("Environment");

            //if ( env == "Local")
            //{
            //   recipients = new string[] { "vijay.kumar@fifthnote.co" };               
            //}
            //else
            //{
            //    var developerIds = ConfigurationManager.AppSettings.Get("EmailIdsToGetErrors");
            //    recipients = new string[] { developerIds };               
            //}

            var developerIds = ConfigurationManager.AppSettings.Get("EmailIdsToGetErrors");
            if (string.IsNullOrEmpty(developerIds)) { developerIds = "vijay.kumar@fifthnote.co"; }
            recipients = new string[] { developerIds };       

            subject += "(" + env + ")";
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
                        throw new Exception("Error-mailing in Web is not working. ", ex);
                    }
                }
            Server.TransferRequest("Error/Error");
            Server.ClearError();

        }

           
        }

  }

