using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Model;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
    [Authorize]
    public class LocationController : BaseController
    {
        // GET: Location
        IUprdLocationService ILocationService;
        IUprdPipelineService _pipelineService;
        //static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        //private RestClient clientLocation = new RestClient(apiBaseUrl + "/api/Location/");
        //public LocationController(ILocationService ILocationService, IPipelineService pipelineService) : base(pipelineService)
        //{
        //    this.ILocationService = ILocationService;

        //}

        public LocationController(IUprdLocationService ILocationService, IUprdPipelineService pipelineService) : base(pipelineService,null,null)
        {
            this.ILocationService = ILocationService;
            this._pipelineService = pipelineService;

        }


        public ActionResult Index(string pipelineDuns)
        {
            //string pipelineDuns = "007912959";
            LocationsDTO model = new LocationsDTO();
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();

            PipelineDTO pipe = new PipelineDTO();
            if (Request["pipelineDuns"] == null || string.IsNullOrEmpty(pipelineDuns))
            {
                var pipes = _pipelineService.GetAllActivePipeline();
                pipelineDuns = pipes.ToList().Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
            }
            else
            {
                pipelineDuns = Request["pipelineDuns"] != null ? Request["pipelineDuns"].ToString() : pipelineDuns;
            }
            model.PipelineDuns = pipelineDuns;
            return View(model);

        }



        [HttpPost]
        public ActionResult GetData(string pipelineDuns)
        {

            var LocationByPipeLine = ILocationService.GetLocationByPipeline(pipelineDuns);
            return Json(new { data = LocationByPipeLine }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AddLocation(FormCollection fc, LocationListDTO locList)
        {

            LocationsDTO locationsDTO = new LocationsDTO();
            List<LocationsDTO> loc = new List<LocationsDTO>();
            string pipelineDuns = fc["PipelineDuns"];
            loc = locList.LocationsList.ToList();
            locList.DunsNo = pipelineDuns;
            ILocationService.AddLocationByPipeline(loc, pipelineDuns);
            return RedirectToAction("Index", new { pipelineDuns = pipelineDuns });
        }

        public PartialViewResult AddLocationRow(string pipelineDuns, string RowCount)
        {

            LocationListDTO model = new LocationListDTO();
            model.CurrentLocationRow = Convert.ToInt32(RowCount);
            model.DunsNo = pipelineDuns;
            model.LocationsList = new List<LocationsDTO>();

            var item = new LocationsDTO();
            item.CreatedDate = DateTime.Now;
            model.LocationsList.Add(item);

            return PartialView("_AddLocationRow", model);
        }



        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            var source = ILocationService.GetLocationById(id);
            //LocationsDTO lstLocationsDTO = new LocationsDTO();

            //if (id == 0)
            //    return View(new LocationsDTO());
            //else
            //{
            //    //var request = new RestRequest(string.Format("EditLocation"), Method.POST) { RequestFormat = DataFormat.Json };
            //    //request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            //    //request.AddJsonBody(id);
            //    //var response = clientLocation.Execute<LocationsDTO>(request);
            //    var source = ILocationService.GetLocationById(id);

            //    int totalRecords = 0;
            //    if (source != null)
            //    {
            //        lstLocationsDTO = source;

            //    }
            //    else
            //    {
            //        lstLocationsDTO = new LocationsDTO();

            //        // Filter record count.
            //        //int recFilter = totalRecords;
            //    }
            //  return Json(new { data = lstLocationsDTO }, JsonRequestBehavior.AllowGet);
            return View("AddorEdit", source);
        }
        [HttpPost]
        public ActionResult AddOrEdit(LocationsDTO loc)
        {
            string msg = null;
            if (loc.ID == 0)
            {
                bool Data = ILocationService.UpdateLocationByID(loc);
                msg = (Data ? "Saved Successfully" : "Something went Wrong!!");
            }
            else
            {
                bool Data = ILocationService.UpdateLocationByID(loc);
                msg = (Data ? "Updated Successfully" : "Something went Wrong!!");

            }
            return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public ActionResult Delete(int id)
        {

            string msg = null;
            if (id == 0)
                msg = "Something went Wrong!!";
            else
            {

                var result = ILocationService.DeleteLocationByID(id);


                msg = "Deleted Successfully";
            }

            return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLocNotIntblLocation(string PipelineDuns)
        {
            // This method gets locations that are present in Oacy and Unsc but are not present in location
            LocationListDTO model = new LocationListDTO();

            if (!string.IsNullOrEmpty(PipelineDuns))
            {
                var result = ILocationService.AddLocsNotInTblLoc(PipelineDuns);
                int totalRecord = 0;
                if (result != null)
                {
                    model.LocationsList = result.locationsDTO;
                    totalRecord = result.RecordCount;
                }
                else
                {
                    model.LocationsList = new List<LocationsDTO>();
                }
            }
            return Json(new { data = model.LocationsList }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult EditLocNotInLocationTbl(List<LocationsDTO> loc)
        //{
        //    return View("AddorEdit", loc);
        //}
        [HttpPost]
        public ActionResult Activate(int id)
        {
            ILocationService.ActivateLocation(id);
            return Json(new { success = true, message = "Activate Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SyncData()
        {
            ILocationService.ClientEnvironmentsetting();
            return Json(new { success = true, message = "Sync Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}