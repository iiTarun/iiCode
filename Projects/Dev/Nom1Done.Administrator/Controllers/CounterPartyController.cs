using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Services.Interface;


namespace Nom1Done.Admin.Controllers
{
    //[Authorize]
    public class CounterPartyController : Controller
    {
        // GET: CounterParty
        public CounterPartyController()
        {

        }

        private IUPRDCounterPartyService locService;
        public CounterPartyController(IUPRDCounterPartyService locRepository)
        {
            this.locService = locRepository;
        }


        // GET: CounterParties
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostData()
        {
            var detail = locService.GetCounterParties();
            return Json(new { data = detail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCounterParties()
        {
            var data = locService.GetCounterParties();
            return View(data);
        }


        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {

            if (id == 0)
            {
                return View(new CounterPartyDTO());
            }
            else
            {
                return View(locService.GetCounterPartyByid(id));
            }



        }
        [HttpPost]
        public ActionResult AddorEdit(CounterPartyDTO counter)
        {
            if (ModelState.IsValid)
            {

                string Message = locService.UpdateCounterPartyByID(counter);
                locService.Save();

                return Json(new { success = true, message = Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("AddorEdit");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            locService.DeleteCounterPartiesByID(id);
            return Json(new { success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Activate(int id)
        {
            locService.ActivateCounterParty(id);
            return Json(new { success = true, message = "Activate Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SyncData()
        {
            locService.ClientEnvironmentsetting();
            return Json(new { success = true, message = "Sync Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}