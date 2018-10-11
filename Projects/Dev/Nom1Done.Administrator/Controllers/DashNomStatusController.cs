using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Model;
using UPRD.Services;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
   // [Authorize]
    public class DashNomStatusController : Controller
    {
        IDashNominationStatusService dashNominationStatusService;
        INotifierEntityService _notifierEntityService;
        static string timeElapsed = ConfigurationManager.AppSettings.Get("TimeElapsed");
        public DashNomStatusController(IDashNominationStatusService DashNominationStatusService, INotifierEntityService notifierEntityService)
        {
            this.dashNominationStatusService = DashNominationStatusService;
            this._notifierEntityService = notifierEntityService;

           /// 
        }

        // GET: DashNomStatus
        public ActionResult Index(string shipperDuns)
        {
            Double ExpiredTime = Convert.ToDouble(timeElapsed);
            DashNominationStatusListDTO dns = new DashNominationStatusListDTO();       
            dns.dashNominationStatusDTO = dashNominationStatusService.GetDashNominationStatus(shipperDuns).ToList();
            var lstStatus = dns.dashNominationStatusDTO.Where(x => x.StatusId == 5 || x.StatusId == 2);
            foreach (var item in lstStatus)
            {
               DateTime subDate =  item.SubmittedDate;
               DateTime currentDate = DateTime.Now;
               TimeSpan timedifference = currentDate - subDate;
                if (timedifference.TotalSeconds < ExpiredTime)
                {
                    double timeElapsed = ExpiredTime - timedifference.TotalSeconds;
                    item.TimeElapsed = timeElapsed;
                }
                else
                {
                    item.TimeElapsed = 0;
                }
            }
            var notifier = _notifierEntityService.GetNotifierEntityForNomStatus();
        
            ViewBag.NotifierEntity = notifier;
            return View(dns);
        }
        [HttpPost]
        public ActionResult UpdateAlertTrigger(String ShipperDuns, String pipeDuns,string StatusID)
        {
            var result =dashNominationStatusService.UpdateNomStatusIsTriggered(ShipperDuns, pipeDuns,Convert.ToInt32(StatusID));

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult SwitchEngineStatus(String ShipperDuns,bool EngineStatus)
        {
           var resultMsg = dashNominationStatusService.SwitchEngineStatus(ShipperDuns,EngineStatus);
            return Json(new { data = resultMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}