using Nom1Done.Service.Interface;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Nom1Done.Receive.Controllers
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


    //public class HomeController : ApiController
    //{
    //    IManageIncomingRequestService manageIncomingReq;
    //    public HomeController(IManageIncomingRequestService manageIncomingReq)
    //    {
    //        this.manageIncomingReq = manageIncomingReq;
    //    }


    //    [Route("")]
    //    public IEnumerable<string> Get()
    //    {           
    //        return new string[] { "Browser Test Passed" };
    //    }


    //    public string Get(int id)
    //    {            
    //        return "value";
    //    }


    //    [Route("")]
    //    public IHttpActionResult Post()
    //    {
    //        var rf = Request;
    //        bool isTestServer = Convert.ToBoolean(ConfigurationManager.AppSettings["isTestServer"]);
    //        bool separateFiles = Convert.ToBoolean(ConfigurationManager.AppSettings["separateFiles"]);
    //        var Req = new HttpRequestWrapper(HttpContext.Current.Request);
    //        string Gisb = manageIncomingReq.ProcessRequest(Req, isTestServer, separateFiles);
    //        if (Gisb != "false")
    //        {
    //            return Json(Gisb);

    //        }
    //        else
    //        {
    //            return Json("Invalid File.");

    //        }

    //    }


    //    public void Put(int id, [FromBody]string value)
    //    {
    //    }


    //    public void Delete(int id)
    //    {
    //    }
    //}
}
