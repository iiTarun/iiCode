using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    public class NomSettingsController : Controller
    {
        // GET: NomSettings
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            return View();
        }
    }
}