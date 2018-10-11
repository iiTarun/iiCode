using Nom.ViewModel;
using Nom1Done.CustomSerialization;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Nom1Done.Model;
using Nom1Done.Data;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class LocationController : BaseController
    {
        ILocationService ILocationService;
        //IPipelineService _pipelineService;
        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient clientLocation = new RestClient(apiBaseUrl + "/api/Location/");
        public LocationController(ILocationService ILocationService ,IPipelineService pipelineService): base(pipelineService)
        {
            this.ILocationService = ILocationService;
            //  this._pipelineService = pipelineService;
            clientLocation.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }
        public ActionResult Index(string pipelineDuns)
        {
            LocationsDTO model = new LocationsDTO();
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();

            PipelineDTO pipe = new PipelineDTO();
            if (Request["pipelineDuns"] == null || string.IsNullOrEmpty(pipelineDuns))
            {
                var pipes = GetPipelines();
                pipelineDuns = pipes.Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;               
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
            LocationListDTO model = new LocationListDTO();
            //int pipeId = pipelineId.GetValueOrDefault();
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            //if (pipelineId == null)
            //    pipelineId = currentIdentityValues.FirstSelectedPipeIdByUser;
            model.DunsNo = pipelineDuns;

            if (!string.IsNullOrEmpty(pipelineDuns))
            {
                var request = new RestRequest(string.Format("GetPipelineLocation"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(pipelineDuns);
                var response = clientLocation.Execute<LocationsResultDTO>(request);

                int totalRecords = 0;
                if (response.Data != null)
                {
                    model.LocationsList = response.Data.locationsDTO;
                    totalRecords = response.Data.RecordCount;
                }
                else
                {
                    model.LocationsList = new List<LocationsDTO>();
                    totalRecords = 0;
                }

                // Filter record count.
                int recFilter = totalRecords;
            }
            return Json(new { data =model.LocationsList},JsonRequestBehavior.AllowGet);        
        }
        [HttpPost]
        public ActionResult AddLocation(FormCollection fc,LocationListDTO locList)
        {
            LocationsDTO locationsDTO = new LocationsDTO();
            List<LocationsDTO> loc = new List<LocationsDTO>();
            string PipelineDuns = fc["hdnPipelineDuns"]; 

            loc = locList.LocationsList.ToList();       
            var request = new RestRequest(string.Format("AddLocation"), Method.POST) { RequestFormat = DataFormat.Json };
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(loc);
            var response = clientLocation.Execute<bool>(request);
            string pipeline = loc.Select(x => x.PipelineDuns).ToString();
            return RedirectToAction("Index", new { pipelineDuns = pipeline });
        }

        public PartialViewResult AddLocationRow(string pipelineDuns,string RowCount)
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
           LocationsDTO lstLocationsDTO = new LocationsDTO();
   
            if (id == 0)
                return View(new LocationsDTO());
            else
            {
                    var request = new RestRequest(string.Format("EditLocation"), Method.POST) { RequestFormat = DataFormat.Json };
                    request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                    request.AddJsonBody(id);
                    var response = clientLocation.Execute<LocationsDTO>(request);

                    int totalRecords = 0;
                    if (response.Data != null)
                    {
                    lstLocationsDTO = response.Data;
                       
                    }
                    else
                    {
                    lstLocationsDTO = new LocationsDTO();
                       
                    // Filter record count.
                    int recFilter = totalRecords;
                    }
                //  return Json(new { data = lstLocationsDTO }, JsonRequestBehavior.AllowGet);
                return View("AddorEdit", lstLocationsDTO);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(LocationsDTO loc)
        {
            string msg = null;
            if (loc.ID == 0)
            {
                // Save location which are in Oacy/Unsc But not in location table 
                var req = new RestRequest(string.Format("UpdateLocation"), Method.POST) { RequestFormat = DataFormat.Json };
                req.JsonSerializer = NewtonsoftJsonSerializer.Default;
                req.AddJsonBody(loc);

                var response = clientLocation.Execute<bool>(req);
                msg = (response.Data ? "Saved Successfully" : "Something went Wrong!!");                       
            }
            else
            {
                // Update location in location table 
                var request = new RestRequest(string.Format("UpdateLocation"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(loc);
                var response = clientLocation.Execute<bool>(request);
                msg = (response.Data ? "Updated Successfully" : "Something went Wrong!!");
             
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
                var request = new RestRequest(string.Format("DeleteLocation"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(id);
                var response = clientLocation.Execute<Boolean>(request);

                if (response.Data)
                {
                    msg = "Deleted Successfully";
                }


            }

            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public ActionResult GetLocNotIntblLocation(string PipelineDuns)
        {
            // This method gets locations that are present in Oacy and Unsc but are not present in location
            LocationListDTO model = new LocationListDTO();
           // var Pipelineduns = _pipelineService.GetDunsByPipelineID(PipelineDuns);
            if (!string.IsNullOrEmpty(PipelineDuns))
            {
                var request = new RestRequest(string.Format("AddLocsNotInTblLoc"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(PipelineDuns);
                var response = clientLocation.Execute<LocationsResultDTO>(request);
                int totalRecord = 0;
                if (response.Data != null)
                {
                    model.LocationsList = response.Data.locationsDTO;
                    totalRecord = response.Data.RecordCount;
                }
                else
                {
                    model.LocationsList = new List<LocationsDTO>();                  
                }             
            }
            return Json(new {data = model.LocationsList}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditLocNotInLocationTbl(List<LocationsDTO> loc)
        {
                return View("AddorEdit", loc);
        }


    }
}