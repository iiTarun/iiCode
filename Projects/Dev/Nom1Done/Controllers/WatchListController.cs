using Microsoft.AspNet.Identity;
using Nom.ViewModel;
using Nom1Done.CustomSerialization;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class WatchListController : BaseController
    {
        
      
       // IPipelineService pipelineService;
      

        static string apiBaseUrl= ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient client = new RestClient(apiBaseUrl+"/api/WatchList/");

        public WatchListController(ILocationService locationService,IPipelineService pipelineService) : base(pipelineService)
        {
          
           // this.pipelineService = pipelineService;         
            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }


        public ActionResult Index(int watchListId = 0)
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Status = TempData["Msg"] + "";
            }
            WatchListDTO watchList = new WatchListDTO();           
            if (watchListId != 0)
            {
                ViewBag.EditFlag = watchListId + "";
                RestRequest request = new RestRequest("GetWatchListById/" + watchListId, Method.GET);               
                IRestResponse<WatchListDTO> response = client.Execute<WatchListDTO>(request);
                if (response.IsSuccessful)
                {
                    watchList.DatasetId = response.Data.DatasetId; 
                    
                }
                watchList.id = watchListId;
            }           
       
            watchList.RuleList = new List<WatchListRule>() {
                new WatchListRule() {
                }
            };
            return View(watchList);
        }

        public string Validation(WatchListDTO watchlist)
        {
            string msg = string.Empty;
            if (watchlist.DatasetId == 0) { return "Please, Select DataSet for watchlist."; }
            if (string.IsNullOrEmpty(watchlist.ListName) || string.IsNullOrWhiteSpace(watchlist.ListName)) { return "Please, Eneter watchLlist Name."; }
            if (watchlist.RuleList == null || watchlist.RuleList.Count == 0) { return "Please, Select at least one Rule for watchlist."; }
            if (watchlist.RuleList.Count > 0)
            {
                
                foreach (var item in watchlist.RuleList) {
                    if (item.PropertyId == 0 && watchlist.DatasetId == EnercrossDataSets.SWNT) { continue; }
                    if (item.PropertyId == 0) { return "Please, select Column for watchlist Rule."; }
                    if (watchlist.DatasetId != EnercrossDataSets.OACY)
                    {
                        if (item.ComparatorsId == 0) { return "Please, select condition for watchlist Rule"; }
                    }
                                
                    if (string.IsNullOrEmpty(item.value) || string.IsNullOrWhiteSpace(item.value)) { return "Please, enter value for rule of watchlist."; }
                    try {
                        RestRequest requestForProp = new RestRequest("GetPropertyById", Method.GET);
                        requestForProp.AddQueryParameter("propertyId", item.PropertyId.ToString());
                        IRestResponse<WatchListProperty> responseForProp = client.Execute<WatchListProperty>(requestForProp);
                        var prop = responseForProp.Data;
                       if (prop.Datatype == "decimal") { var dec = decimal.Parse(item.value); }
                        if (prop.Datatype == "long") { var lon = long.Parse(item.value); }
                        if (prop.Datatype == "DateTime") { var dat = DateTime.ParseExact(item.value, "MM/dd/yyyy", CultureInfo.InvariantCulture); }
                    } catch (Exception ex) {
                        return "Please, Enter Value in Correct Format. Don't use any special character in value."; }
                }
            }
            return msg;
        }


        public string BaseUrl()
        {
            // variables  
                string Authority = Request.Url.GetLeftPart(UriPartial.Authority).TrimStart('/').TrimEnd('/');
                string ApplicationPath = Request.ApplicationPath.TrimStart('/').TrimEnd('/');

                // add trailing slashes if necessary  
                if (Authority.Length > 0)
                {
                    Authority += "/";
                }

                if (ApplicationPath.Length > 0)
                {
                    ApplicationPath += "/";
                }

                // return  
                return string.Format("{0}{1}", Authority, ApplicationPath);            
        }

        [HttpPost]
        public ActionResult Index(WatchListDTO watchlist, string Save)
        {
            var msg=Validation(watchlist);
            if (string.IsNullOrEmpty(msg))
            {
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            watchlist.UserId = currentIdentity.UserId;
            var url =    BaseUrl();
            
            watchlist.MoreDetailURLinAlert = url; 
            if (Save == "Update")
            {
                    // Code for Update
                  
                    RestRequest request = new RestRequest("UpdateWatchList", Method.POST);
                    request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                    request.AddJsonBody(watchlist);
                    IRestResponse<bool> response = client.Execute<bool>(request);
                    return RedirectToAction("List");
                }
            else
            {
                    //Code for Save
                    RestRequest request = new RestRequest("SaveWatchList", Method.POST);
                    request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                    request.AddJsonBody(watchlist);
                    IRestResponse<bool> response = client.Execute<bool>(request);
                    bool save = response.Data; 
                if (save != false)
                    TempData["Msg"] = "Data saved successfully";
                else
                {
                    TempData["Msg"] = "Data saving failed";
                }
                return RedirectToAction("Index", new { watchListId = 0 });
              }
            }
            else {
                TempData["Msg"] ="Action failed."+ msg;
                return RedirectToAction("Index", new { watchListId = 0 });
            }
        }

        public ActionResult List()
        {
            WatchListCollection model = new WatchListCollection();           
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            RestRequest request = new RestRequest("GetWatchListByUserId", Method.POST);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(currentIdentity.UserId);
            IRestResponse<List<WatchListDTO>> response = client.Execute<List<WatchListDTO>>(request);
            if (response.IsSuccessful) { 
                   model.CollectionWatchList = response.Data; 
            }
            else {
                model.CollectionWatchList = new List<WatchListDTO>();
            }
            if (TempData["WatchListDeleted"] != null) { ViewBag.Status = TempData["WatchListDeleted"].ToString(); }
            return this.View(model);
        }

        public ActionResult UpdateWatchListSetting(int id)
        {
            RestRequest request = new RestRequest("GetWatchListById/" + id, Method.GET);
            IRestResponse<WatchListDTO> response = client.Execute<WatchListDTO>(request);
            if (response.IsSuccessful)
            {
            var watchListDto = response.Data; 
                RestRequest requestForProp = new RestRequest("GetPropertiesByDataSet", Method.GET);
                requestForProp.AddQueryParameter("dataset", watchListDto.DatasetId.ToString());
                IRestResponse<List<WatchListProperty>> responseForProp = client.Execute<List<WatchListProperty>>(requestForProp);
                List<WatchListProperty> properties = responseForProp.Data!=null ? responseForProp.Data : new List<WatchListProperty>() ; //IWatchlistService.GetPropertiesByDataSet(watchListDto.DatasetId);
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            var user = SignInManager.UserManager.FindById(currentIdentity.UserId);
            if (user != null)
            {
                watchListDto.UserEmail = user.Email;
            }
            ViewBag.Properties = properties;
            if (properties.Count > 0)
                ViewBag.Operators = properties.FirstOrDefault().Operators;
            else
            {
                ViewBag.Operators = new List<WatchListOperator>();
            }
            var pipelineList = GetPipelines();
            ViewBag.PipelineId = pipelineList;
                
            return PartialView("_updatewatchList", watchListDto);
            }
            else
            {
                return PartialView("_updatewatchList", null);
            }
        }

        public ActionResult Delete(int id)
        {
            
            bool isDeleted = false;
            RestRequest request = new RestRequest("DeleteWatchListById", Method.POST);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(id);
            IRestResponse<bool> response = client.Execute<bool>(request);
            if (response.IsSuccessful)
            {
                isDeleted = response.Data; 
            }
            if (isDeleted) { TempData["WatchListDeleted"] = "WatchList Deleted"; } else { TempData["WatchListDeleted"] = "Not Deleted."; }
          
            return RedirectToAction("List");
        }
        
        [HttpGet]
        public ActionResult GetWatchListSetting(int selectedItem)
        {

            WatchListDTO watchList = new WatchListDTO();
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            var user = SignInManager.UserManager.FindById(currentIdentity.UserId);
            if (user != null) {
                watchList.UserEmail = user.Email;
            }

            watchList.RuleList = new List<WatchListRule>()
            {
            };
            ViewBag.Pipeline = GetPipelines(); // pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId());
            switch (selectedItem)
            {
                case 1:
                    watchList.DatasetId = Enums.EnercrossDataSets.OACY;
                    break;
                case 2:
                    watchList.DatasetId = Enums.EnercrossDataSets.UNSC;
                    break;
                case 3:
                    watchList.DatasetId = Enums.EnercrossDataSets.SWNT;
                    break;
            }
            return PartialView(watchList);
        }

        public ActionResult AddRuleSettings(int datasetId)
        {
            WatchListDTO watchList = new WatchListDTO();
            switch (datasetId)
            {
                case 1:
                    watchList.DatasetId = Enums.EnercrossDataSets.OACY;
                    break;
                case 2:
                    watchList.DatasetId = Enums.EnercrossDataSets.UNSC;
                    break;
                case 3:
                    watchList.DatasetId = Enums.EnercrossDataSets.SWNT;
                    break;
            }
            RestRequest requestForProp = new RestRequest("GetPropertiesByDataSet", Method.GET);
            requestForProp.AddQueryParameter("dataset", watchList.DatasetId.ToString());
            requestForProp.JsonSerializer = NewtonsoftJsonSerializer.Default;
            IRestResponse<List<WatchListProperty>> responseForProp = client.Execute<List<WatchListProperty>>(requestForProp);
            List<WatchListProperty> properties = responseForProp.Data!=null ? responseForProp.Data : new List<WatchListProperty>(); //IWatchlistService.GetPropertiesByDataSet(watchList.DatasetId);
            ViewBag.Properties = properties;
           
            if (properties.Count > 0)
                ViewBag.Operators = properties.FirstOrDefault().Operators;
            else
            {
                ViewBag.Operators = new List<WatchListOperator>();
            }
            List<WatchListRule> newwatchRule = new List<WatchListRule>() {
                new WatchListRule() {
                }
            };           
            watchList.RuleList = newwatchRule;

           // var pipelineList = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId());
            ViewBag.PipelineId = GetPipelines();           
           
            ViewBag.LocationIdentifier = new List<LocationsDTO>(); 
            if (watchList.DatasetId == Enums.EnercrossDataSets.OACY)
            {
                return PartialView("OacyRuleSettings", watchList);
            }
            else
            {
                return PartialView(watchList);
            }
        }

        [HttpPost]
        public JsonResult GetOperator(int propertyId)
        {
            RestRequest requestForProp = new RestRequest("GetPropertyById", Method.GET);
            requestForProp.AddQueryParameter("propertyId", propertyId.ToString());
            IRestResponse<WatchListProperty> responseForProp = client.Execute<WatchListProperty>(requestForProp);
            var property = responseForProp.Data!=null ? responseForProp.Data : new WatchListProperty(); //IWatchlistService.GetPropertyById(propertyId);
            return Json(property);           
        }


        [HttpPost]
        public JsonResult GetLocations(string pipelineduns,string Keyword)
        {
            RestRequest requestForLoc = new RestRequest("GetLocationsFromUPRDs", Method.GET);
            requestForLoc.AddQueryParameter("PipelineDuns", pipelineduns);
            requestForLoc.AddQueryParameter("Keyword", Keyword);
            IRestResponse<List<LocationsDTO>> responseForLoc = client.Execute<List<LocationsDTO>>(requestForLoc);
            var locs = responseForLoc.Data!=null ? responseForLoc.Data : new List<LocationsDTO>(); //IWatchlistService.GetLocationsFromUPRDs(pipelineduns, datasetType); 
            return Json(locs);
        }

        
        public ActionResult ExecutedResult(int watchListId)
        {
            WatchListAlertExecutedDataDTO model = new WatchListAlertExecutedDataDTO();

            RestRequest request = new RestRequest("ExecutedResult/"+ watchListId, Method.GET);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            IRestResponse<WatchListAlertExecutedDataDTO> response = client.Execute<WatchListAlertExecutedDataDTO>(request);

            model = response.Data;

            return View(model);
        }
       

        public PartialViewResult PopUpPartials(EnercrossDataSets dataSet, string clickedRow, string pipelineDuns)
        {
            WatchListPopUpPartialDTO model = new WatchListPopUpPartialDTO();
            model.RowId = clickedRow;
            model.DataSet = dataSet;
            model.PipelineDuns = pipelineDuns; 
            model.Locations = new List<LocationsDTO>(); 
            return PartialView("~/Views/WatchList/_UprdLocations.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetLocationFromUPRD(EnercrossDataSets dataSet, string pipelineDuns)
        {
            JsonResult result = new JsonResult();
            try
            {
                List<LocationsDTO> dataList = new List<LocationsDTO>();
                int totalRecords = 0;
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                if (order == "2")
                {
                    order = "Loc";
                }
                else {
                    order = "LocName";
                }
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                int pageno = (startRec / pageSize) + 1;


               
                // Loading.
                 RestRequest requestForLoc = new RestRequest("GetLocationsFromUPRDsUsingPaging", Method.GET);
                requestForLoc.JsonSerializer = NewtonsoftJsonSerializer.Default;
                requestForLoc.AddQueryParameter("PipelineDuns", pipelineDuns);
                requestForLoc.AddQueryParameter("datasetType", dataSet.ToString());
                requestForLoc.AddQueryParameter("Keyword", search);
                requestForLoc.AddQueryParameter("PageNo", pageno.ToString());
                requestForLoc.AddQueryParameter("PageSize", pageSize.ToString());
                requestForLoc.AddQueryParameter("order", order);
                requestForLoc.AddQueryParameter("orderDir", orderDir);
                IRestResponse<LocationsResultDTO> responseForLoc = client.Execute<LocationsResultDTO>(requestForLoc);
                if (responseForLoc.Data != null)
                {
                    dataList = responseForLoc.Data.locationsDTO;
                    totalRecords = responseForLoc.Data.RecordCount;
                }
                else {
                    dataList = new List<LocationsDTO>();
                    totalRecords =0;
                }
               
             
               // Filter record count.
                int recFilter = totalRecords;

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = dataList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
               
            }
            return result;
        }
    }
}