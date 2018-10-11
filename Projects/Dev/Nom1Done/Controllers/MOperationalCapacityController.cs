using Jitbit.Utils;
using Newtonsoft.Json;
using Nom1Done.CustomSerialization;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Service.Interface;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class MOperationalCapacityController : BaseController
    {
       
      
       // IPipelineService piplineservice;
        ICycleIndicator ICycleIndicator;

        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient client = new RestClient(apiBaseUrl + "/api/OACY/");

        public MOperationalCapacityController(IPipelineService piplineservice,  ICycleIndicator ICycleIndicator) : base(piplineservice)
        {
           
            //this.piplineservice = piplineservice;
            this.ICycleIndicator = ICycleIndicator;           
            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }

        public ActionResult Index(SearchCriteriaOACY criteria)
        {
        

            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
          
            var model = new SearchCriteriaOACY();
            //model.PipelineID = criteria.PipelineID;           
            model.EffectiveStartDate=criteria.EffectiveStartDate; 
            model.keyword = criteria.keyword;
            model.WatchListId = criteria.WatchListId;
            model.Cycle = criteria.Cycle;
            model.postTime = criteria.postTime == null ? DateTime.Now.Date + new TimeSpan(DateTime.Now.AddHours(-4).TimeOfDay.Hours, 0, 0) : criteria.postTime;
            var pipelineDuns = criteria.PipelineDuns; // piplineservice.GetDunsByPipelineID(criteria.PipelineID.Value);
            model.PipelineDuns = criteria.PipelineDuns;

            if (criteria.flagDefault)
            {                
                var request = new RestRequest(string.Format("GetRecentOacyPostDate"), Method.GET);
                request.AddQueryParameter("PipelineDuns", pipelineDuns);
                var response = client.Execute<DateTime?>(request);            
                model.postStartDate = response.Data !=null ? response.Data : DateTime.Now.Date; 
            }
            else {
                model.postStartDate = criteria.postStartDate;
            }
            //model.postEndDate = DateTime.Now.AddHours(-4);
            var CycleIndicator = ICycleIndicator.GetCycles();
            ViewBag.Cycles = new SelectList(CycleIndicator.AsEnumerable(), "Code", "Name", 1);
            var result = GetAllOacyDataAsync(model);
            model.OacyPerTransactionViewModel = result.Result.OacyPerTransactionViewModel;           
            return this.View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Index(SearchCriteriaOACY criteria, string download)
        {  

            var CycleIndicator = ICycleIndicator.GetCycles();
            ViewBag.Cycles = new SelectList(CycleIndicator.AsEnumerable(), "Code", "Name", 1);         
           
            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
            criteria.flagDefault = false;
            if (download != null)
            {
                var request = new RestRequest(string.Format("GetTotalCountOacy"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(criteria);
                var response = client.Execute<int>(request);
                int totalRecords = response.Data;
                criteria.size = totalRecords;
                criteria.page = 1;
                var modelDownload = await GetAllOacyDataAsync(criteria);
                //DownloadCSV(modelDownload);
                return RedirectToAction("Index", modelDownload);
            }       
            return  RedirectToAction("Index",criteria);
 
        }
        
        private async Task<SearchCriteriaOACY> GetAllOacyDataAsync(SearchCriteriaOACY criteria)
        {
          
            List<OACYPerTransactionDTO> list = new List<OACYPerTransactionDTO>();
           
            var request = new RestRequest(string.Format("GetOACYByCriteria"), Method.POST) { RequestFormat = DataFormat.Json };           
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                          
            request.AddJsonBody(criteria);
            var response  = client.Execute<OacyResultDTO>(request);
            if (response.Data != null) {
                list = response.Data.oacyPerTransactionDTO;
                criteria.RecordCount = response.Data.RecordCount;
            }
            else {
                list = new List<OACYPerTransactionDTO>(); 
                criteria.RecordCount = 0;
            }

            criteria.OacyPerTransactionViewModel = list;
           

            return criteria;
        }

        [HttpPost]
        public async Task<ActionResult> GetData(OacyDataFilter oacyDataFilter)
        {
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
              

                // Loading.
                SearchCriteriaOACY criteria = new SearchCriteriaOACY();
                criteria.page = (startRec / pageSize)+1;
                criteria.size = pageSize;
                criteria.sort = GetColumnNameToSort(order);
                criteria.SortDirection = orderDir;
                //criteria.PipelineID = oacyDataFilter.PipelineID;
                criteria.keyword = oacyDataFilter.keyword;
                criteria.postStartDate = oacyDataFilter.postStartDate;
                criteria.EffectiveStartDate = oacyDataFilter.EffectiveGasDate;
                criteria.WatchListId = oacyDataFilter.WatchListId;
                criteria.flagDefault = oacyDataFilter.flagDefault;
                criteria.Cycle = oacyDataFilter.Cycle;
                criteria.PipelineDuns = oacyDataFilter.PipelineDuns;
                criteria = await GetAllOacyDataAsync(criteria);


                List<OACYPerTransactionDTO> OacyPerTransactionViewlist = criteria.OacyPerTransactionViewModel.ToList();
               
                // Total record count.
                               
                int totalRecords = criteria.RecordCount; 
                // Sorting.
              
                // Filter record count.
                int recFilter = criteria.RecordCount;

                // Apply pagination.
               
                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = OacyPerTransactionViewlist }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetColumnNameToSort(string order)
        {
            string result = string.Empty;
            switch (order)
            {
                case "0":
                    // Setting.
                    result = "Loc";
                    break;

                case "1":
                    // Setting.
                    result = "LocName";
                    break;

                case "2":
                    // Setting.
                    result = "CycleIndicator";
                    break;

                case "3":
                    // Setting.
                    result = "DesignCapacity";
                    break;
                case "4":
                    // Setting.
                    result = "OperatingCapacity";
                    break;

                case "5":
                    // Setting.
                    result = "TotalScheduleQty";
                    break;
                case "6":
                    // Setting.
                    result = "OperationallyAvailableQty";
                    break;

                case "7":
                    // Setting.
                    result = "AvailablePercentage";
                    break;

                case "8":
                    // Setting.   
                    result = "EffectiveGasDay";
                    break;
                case "9":
                    // Setting.
                    result = "PostingDate";
                    break;

                case "10":
                    // Setting.
                    result = "FlowIndicator";
                    break;
                case "11":
                    // Setting.
                    result = "LocQTIDesc";
                    break;

                case "12":
                    // Setting.
                    result = "MeasurementBasis";
                    break;
                case "13":
                    // Setting.
                    result = "ITIndicator";
                    break;
                case "14":
                    // Setting.
                    result = "AllQtyAvailableIndicator";
                    break;

            }
            return result;
        }


        public ActionResult DownloadCSV(string PipelineDuns, DateTime postStartDate, DateTime? EffectiveGasDate, string keyword, string Cycle)
        {
            try
            {
                SearchCriteriaOACY criteria = new SearchCriteriaOACY();
                criteria.PipelineDuns = PipelineDuns;
                criteria.postStartDate = postStartDate;
                criteria.EffectiveStartDate = EffectiveGasDate;
                criteria.keyword = keyword;
                criteria.Cycle = Cycle;
                var data = GetAllOacyDataAsync(criteria);
                criteria.OacyPerTransactionViewModel = data.Result.OacyPerTransactionViewModel;
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                var headerStyle = workbook.CreateCellStyle();
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);

                ISheet sheet1 = workbook.CreateSheet("Oacy");

                var oacyList = criteria.OacyPerTransactionViewModel.ToList();
                var typeList = oacyList.GetType();
                var type = oacyList.FirstOrDefault().GetType();
                var props = type.GetProperties();
                IRow row1 = sheet1.CreateRow(0);
                int headCell = 0;
                for (int i = 0; i < props.Count(); i++)
                {
                    String columnName = props[i].Name.ToString();
                    if (columnName == "Loc"
                        || columnName == "LocName"
                        || columnName == "CycleIndicator"
                        || columnName == "LocQTIDesc"
                        || columnName == "DesignCapacity"
                        || columnName == "OperatingCapacity"
                        || columnName == "MeasBasisDesc"
                        || columnName == "TotalScheduleQty"
                        || columnName == "OperationallyAvailableQty"
                        || columnName == "FlowIndicator"
                        || columnName == "ITIndicator"
                        || columnName == "AllQtyAvailableIndicator"
                        || columnName == "AvailablePercentage")
                    {
                        ICell cell = row1.CreateCell(headCell);
                        cell.SetCellValue(columnName);
                        cell.CellStyle = headerStyle;
                        headCell++;
                    }
                }
                // var result=unscList.Contains()
                for (int j = 0; j < oacyList.Count; j++)
                {
                    int rowCell = 0;
                    IRow row = sheet1.CreateRow(j + 1);
                    for (int i = 0; i < props.Count(); i++)
                    {
                        var columnName = props[i].Name;
                        if (columnName == "Loc"
                        || columnName == "LocName"
                        || columnName == "CycleIndicator"
                        || columnName == "LocQTIDesc"
                        || columnName == "DesignCapacity"
                        || columnName == "OperatingCapacity"
                        || columnName == "MeasBasisDesc"
                        || columnName == "TotalScheduleQty"
                        || columnName == "OperationallyAvailableQty"
                        || columnName == "FlowIndicator"
                        || columnName == "ITIndicator"
                        || columnName == "AllQtyAvailableIndicator"
                        || columnName == "AvailablePercentage")
                        {
                            var res = oacyList[j].GetType().GetProperty(columnName).GetValue(oacyList[j]);
                            string result = res != null ? res.ToString() : string.Empty;
                            ICell cell = row.CreateCell(rowCell);
                            cell.SetCellValue(result);
                            rowCell++;
                        }
                    }
                }
                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    return File(exportData.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "OACY.xls");
                }
            }
            catch (Exception ex)
            {
                return File(new byte[0],
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "OACY.xls");
            }
        }

    }
}