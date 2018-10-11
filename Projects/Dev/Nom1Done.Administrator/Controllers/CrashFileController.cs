using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
    public class CrashFileController : Controller
    {
        ICrashFileService _CrashFileService;
        IClientEnvironmentSettingsService _ClientSettingsService;
        public CrashFileController(ICrashFileService CrashFileService,IClientEnvironmentSettingsService ClientSettingsService)
        {
            _CrashFileService = CrashFileService;
            _ClientSettingsService = ClientSettingsService;
        }

        // GET: CrashFile
        [HttpGet]
        public ActionResult Index()
        {
            //var files = _CrashFileService.GetFiles("Enercross");
            var ShipperCompanies = _ClientSettingsService.GetShipperComapnies();
            ViewBag.ShipperDropdown = new SelectList(ShipperCompanies, "ShipperDuns", "ShipperNameWithDuns");
            return PartialView("_CrashFilePartial",new List<CrashFileDTO>());
        }

        [HttpPost]
        public ActionResult GetCrashFileByShipperduns(string ShipperDuns)
        {
            var files = _CrashFileService.GetFiles(ShipperDuns);
            if (files != null)
                return PartialView("_CrashFileTblPartial", files);
            else
                return PartialView("_CrashFileTblPartial", new CrashFileDTO());
        }

        [HttpGet]
        public FileResult DownloadCrashFile(string fileName,string shipperDuns)
        {
            var data = _CrashFileService.GetFileData(fileName, shipperDuns);
            return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, fileName+".txt");
        }
    }
}