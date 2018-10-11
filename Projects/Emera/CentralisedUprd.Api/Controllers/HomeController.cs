using CentralisedUprd.Api.Services;
using CentralisedUprd.Api.SQLDependencyHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CentralisedUprd.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var entityoacy = NotifierEntityService.GetNotifierEntityOfOACY();
            //Action<String> dispatcher = (t) => {  };
            //PushSqlDependency.Instance(NotifierEntity.FromJson(entityoacy), dispatcher, false);

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
