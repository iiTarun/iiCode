using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.ReceiveUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        IManageIncomingRequestService manageIncomingReq;
        public HomeController(IManageIncomingRequestService manageIncomingReq)
        {
            this.manageIncomingReq = manageIncomingReq;
        }
        public ActionResult Index()
        {
            bool isTestServer = Convert.ToBoolean(ConfigurationManager.AppSettings["isTestServer"]);
            bool separateFiles = Convert.ToBoolean(ConfigurationManager.AppSettings["separateFiles"]);
            string Gisb = manageIncomingReq.ProcessRequest(Request, isTestServer, separateFiles);
            if (Gisb != "false")
            {
                char[] res = Gisb.ToString().ToCharArray();
                Response.Write(res, 0, res.Length);
            }
            else
            {
                char[] res = "Invalid File.".ToCharArray();
                Response.Write(res, 0, res.Length);
            }
            return View();
        }
    }
}