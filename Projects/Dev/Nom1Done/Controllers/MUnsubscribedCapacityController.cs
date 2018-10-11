using Jitbit.Utils;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class MUnsubscribedCapacityController : BaseController
    {
        
       // IPipelineService piplineservice;
        //private readonly RestClient _client;
        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient client = new RestClient(apiBaseUrl + "/api/Unsc/");

        public MUnsubscribedCapacityController(IPipelineService piplineservice):base(piplineservice)
        {           
           // this.piplineservice = piplineservice;
            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);           
        }

        [HttpGet]
        public ActionResult Index(SearchCriteriaUNSC criteria)
        {

            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
            var model = new SearchCriteriaUNSC();
            //model.PipelineID = criteria.PipelineID;
            var pipelineDuns = criteria.PipelineDuns;
            model.PipelineDuns = pipelineDuns;
            model.postTime = criteria.postTime == null ? DateTime.Now.Date + new TimeSpan(DateTime.Now.AddHours(-4).TimeOfDay.Hours, 0, 0) : criteria.postTime;
            model.EffectiveStartDate = criteria.EffectiveStartDate;
            model.EffectiveEndDate = criteria.EffectiveEndDate;
            if (criteria.flagDefault)
            {
                var request = new RestRequest(string.Format("GetRecentUnscPostDate"), Method.GET);
                request.AddQueryParameter("PipelineDuns", pipelineDuns);
                var response = client.Execute<DateTime?>(request);
                model.postStartDate = response.Data != null ? response.Data : DateTime.Now.Date; // _IUNSCService.GetRecentUnscPostDate(pipelineDuns); 

            }
            else
            {
                model.postStartDate = criteria.postStartDate;
            }
            //criteria.postStartDate = model.postStartDate;

            model.keyword = criteria.keyword;
            //model.UnscPerTransactionViewModel = (list != null && list.Result.UnscPerTransactionViewModel.Count() > 0) ? list.Result.UnscPerTransactionViewModel;
            //var modelDownload = await GetAllUnscDataAsync(criteria);
            var data =  GetAllUnscDataAsync(model);
            model.UnscPerTransactionViewModel = data.Result.UnscPerTransactionViewModel;
            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(SearchCriteriaUNSC criteria, string download)
        {


            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
            criteria.flagDefault = false;
          
            if (download != null)
            {
                var request = new RestRequest(string.Format("GetTotalRecordsCountUNSC"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(criteria);
                var response = client.Execute<int>(request);

                int totalRecords = response.Data; // unscPerTransactionViewlist.Count;
                criteria.size = totalRecords;
                criteria.page = 1;
                var modelDownload = await GetAllUnscDataAsync(criteria);
                //DownloadCSV(modelDownload);
                return RedirectToAction("Index", modelDownload);
            }
            return RedirectToAction("Index", criteria);
        }
        public async System.Threading.Tasks.Task<SearchCriteriaUNSC> GetAllUnscDataAsync(SearchCriteriaUNSC criteria)
        {
            try
            {
                List<UnscPerTransactionDTO> list = new List<UnscPerTransactionDTO>();
                UnscResultDTO unscResult = new UnscResultDTO();
                //_client.BaseUrl = new Uri("http://localhost:56676");
                var request = new RestRequest(string.Format("GetUnscByCriteria"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(criteria);
                var response = client.Execute<UnscResultDTO>(request);
                if (response.Data != null) {
                    list = response.Data.unscPerTransactionDTO;
                    criteria.RecordCount = response.Data.RecordCount;
                } else {
                    list = new List<UnscPerTransactionDTO>(); 
                    criteria.RecordCount = 0;
                }              
                criteria.UnscPerTransactionViewModel = list;
                return criteria;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> GetData(UnscDataFilter unscDataFilter)
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
                SearchCriteriaUNSC criteria = new SearchCriteriaUNSC();
                criteria.page = (startRec / pageSize) + 1;
                criteria.size = pageSize;
                criteria.sort = GetColumnNameToSort(order);
                criteria.SortDirection = orderDir;
                //criteria.PipelineID = unscDataFilter.PipelineID;
                criteria.keyword = unscDataFilter.keyword;
                criteria.postStartDate = unscDataFilter.postStartDate;
                criteria.EffectiveStartDate = unscDataFilter.StartEffectiveGasDate;
                criteria.EffectiveEndDate = unscDataFilter.EndEffectiveGasDate;
                criteria.WatchlistId = unscDataFilter.WatchListId;
                criteria.flagDefault = unscDataFilter.flagDefault;
                criteria.PipelineDuns = unscDataFilter.PipelineDuns;             
                criteria = await GetAllUnscDataAsync(criteria);
                List<UnscPerTransactionDTO> unscPerTransactionViewlist = criteria.UnscPerTransactionViewModel.ToList();

                //unscPerTransactionViewlist = ApplyGroupBy(unscPerTransactionViewlist);

                // Total record count.

                int totalRecords = criteria.RecordCount; // unscPerTransactionViewlist.Count;
               
                // Sorting.
               // unscPerTransactionViewlist = this.SortByColumnWithOrder(order, orderDir, unscPerTransactionViewlist);

                // Filter record count.
                int recFilter = criteria.RecordCount;   //unscPerTransactionViewlist.Count;

                // Apply pagination.
                // unscPerTransactionViewlist = unscPerTransactionViewlist.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = unscPerTransactionViewlist }, JsonRequestBehavior.AllowGet);

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
                    result = "Loc";
                    break;

                case "1":
                    result = "LocName";
                    break;

                case "2":
                    result = "LocQTIDesc";
                    break;

                case "3":

                    result = "PostingDate";
                    break;

                case "4":
                    result = "EffectiveGasDay";
                    break;

                case "5":
                    result = "EndingEffectiveDay";
                    break;

                case "6":
                    result = "MeasBasisDesc";
                    break;

                case "7":
                    result = "LocZn";
                    break;

                case "8":
                    result = "UnsubscribeCapacity";
                    break;

                case "9":
                    result = "ChangePercentage";
                    break;
                 }
            return result;
            }


            #region Sort by column with order method

            /// <summary>
            /// Sort by column with order method.
            /// </summary>
            /// <param name="order">Order parameter</param>
            /// <param name="orderDir">Order direction parameter</param>
            /// <param name="data">Data parameter</param>
            /// <returns>Returns - Data</returns>
        //    private List<UnscPerTransactionDTO> SortByColumnWithOrder(string order, string orderDir, List<UnscPerTransactionDTO> data)
        //{
        //    // Initialization.
        //    List<UnscPerTransactionDTO> lst = new List<UnscPerTransactionDTO>();

        //    try
        //    {
        //        // Sorting
        //        switch (order)
        //        {
        //            case "0":
                        
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Loc).ToList()
        //                                                                                         : data.OrderBy(p => p.Loc).ToList();
        //                break;

        //            case "1":
                       
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LocName).ToList()
        //                                                                                         : data.OrderBy(p => p.LocName).ToList();
        //                break;

        //            case "2":
                       
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LocQTIDesc).ToList()
        //                                                                                         : data.OrderBy(p => p.LocQTIDesc).ToList();
        //                break;

        //            case "3":
                       
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => Convert.ToDateTime(p.PostingDate)).ToList()
        //                                                                                         : data.OrderBy(p => Convert.ToDateTime(p.PostingDate)).ToList();
        //                break;

        //            case "4":
                        
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => Convert.ToDateTime(p.EffectiveGasDay)).ToList()
        //                                                                                         : data.OrderBy(p => Convert.ToDateTime(p.EffectiveGasDay)).ToList();
        //                break;

        //            case "5":
                       
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => Convert.ToDateTime(p.EndingEffectiveDay)).ToList()
        //                                                                                         : data.OrderBy(p => Convert.ToDateTime(p.EndingEffectiveDay)).ToList();
        //                break;

        //            case "6":
                        
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MeasBasisDesc).ToList()
        //                                                                                         : data.OrderBy(p => p.MeasBasisDesc).ToList();
        //                break;

        //            case "7":
                       
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LocZn).ToList()
        //                                                                                           : data.OrderBy(p => p.LocZn).ToList();
        //                break;

        //            case "8":
                      
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => int.Parse((p.UnsubscribeCapacity ?? "0"), NumberStyles.AllowThousands)).ToList()
        //                                                                                         : data.OrderBy(p => int.Parse((p.UnsubscribeCapacity ?? "0"), NumberStyles.AllowThousands)).ToList();

        //               break;

        //            case "9":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChangePercentage).ToList()
        //                                                                   : data.OrderBy(p => p.ChangePercentage).ToList();
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // info.
        //        Console.Write(ex);
        //    }

        //    // info.
        //    return lst;
        //}

        #endregion

        public ActionResult DownloadCSV(string PipelineDuns, DateTime postStartDate,DateTime? StartEffectiveGasDate,DateTime? EndEffectiveGasDate, string keyword)
        {
            //var csvExport = new CsvExport();
            try
            {
                SearchCriteriaUNSC criteria = new SearchCriteriaUNSC();
                criteria.PipelineDuns = PipelineDuns;
                criteria.postStartDate = postStartDate;
                criteria.EffectiveStartDate = StartEffectiveGasDate;
                criteria.EffectiveEndDate = EndEffectiveGasDate;
                criteria.keyword = keyword;
                var data = GetAllUnscDataAsync(criteria);
                criteria.UnscPerTransactionViewModel = data.Result.UnscPerTransactionViewModel;
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                var headerStyle = workbook.CreateCellStyle();
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);
                //headerStyle.FillBackgroundColor = IndexedColors.Green.Index;


                ISheet sheet1 = workbook.CreateSheet("Unsc");

                var unscList = criteria.UnscPerTransactionViewModel.ToList();
                var typeList = unscList.GetType();
                var type = unscList.FirstOrDefault().GetType();
                var props = type.GetProperties();
                IRow row1 = sheet1.CreateRow(0);
                int headCell = 0;
                for (int i = 0; i < props.Count(); i++)
                {

                    String columnName = props[i].Name.ToString();
                    if(columnName== "Loc" 
                        || columnName == "LocName"
                        || columnName == "LocZn"
                        || columnName == "LocQTIDesc"
                        || columnName == "PostingDate"
                        || columnName == "EffectiveGasDay"
                        || columnName == "EndingEffectiveDay"
                        || columnName == "MeasBasisDesc"
                        || columnName == "UnsubscribeCapacity")
                    {
                        ICell cell = row1.CreateCell(headCell);
                        cell.SetCellValue(columnName);
                        cell.CellStyle = headerStyle;
                        headCell++;
                    }
                        
                }
                // var result=unscList.Contains()
                for(int j= 0;j < unscList.Count; j++)
                {
                    int rowCell = 0;
                    IRow row = sheet1.CreateRow(j + 1);
                    for (int i = 0; i < props.Count(); i++)
                    {
                        var columnName = props[i].Name;
                        if (columnName == "Loc"
                        || columnName == "LocName"
                        || columnName == "LocZn"
                        || columnName == "LocQTIDesc"
                        || columnName == "PostingDate"
                        || columnName == "EffectiveGasDay"
                        || columnName == "EndingEffectiveDay"
                        || columnName == "MeasBasisDesc"
                        || columnName == "UnsubscribeCapacity")
                        {
                            var res = unscList[j].GetType().GetProperty(columnName).GetValue(unscList[j]);
                            string result = res != null ? res.ToString() : string.Empty;
                            ICell cell = row.CreateCell(rowCell);
                            cell.SetCellValue(result);
                            rowCell++;
                        }
                    }
                }

                //foreach (var Item in unscList)
                //{
                //    csvExport.AddRow();
                //    csvExport["Loc/LocPropCode"] = Item.Loc;
                //    csvExport["LocName"] = Item.LocName;
                //    csvExport["LocQTIDesc"] = Item.LocQTIDesc;
                //    csvExport["Post Start Date"] = Item.PostingDate.ToString("MM/dd/yyyy");
                //    csvExport["Start Eff Gas Day"] = Item.EffectiveGasDay.ToString("MM/dd/yyyy");
                //    csvExport["End Eff Gas Day"] = Item.EndingEffectiveDay.ToString("MM/dd/yyyy");
                //    csvExport["MeasBasis"] = Item.MeasBasisDesc;
                //    csvExport["LocZn"] = Item.LocZn;                   
                //    csvExport["UnsubscribeCapacity"] = Item.UnsubscribeCapacity;
                //   // csvExport["Available%"] = Item.AvailablePercentage;
                //}

                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    return File(exportData.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "UNSC.xls");
                }
                //return true;
            }
            catch (Exception ex)
            {
                return File(new byte[0],
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "UNSC.xls");
            }
        }


        public List<UnscPerTransactionDTO> ApplyGroupBy(List<UnscPerTransactionDTO> unscData) {
            if (unscData.Count > 1) {
                var unscNewData = (from a in unscData
                                   group a by new {
                                       a.Loc,                                     
                                      post=a.PostingDate.Date,
                                      eff=a.EffectiveGasDay.Date, 
                                       a.LocQTIDesc
                                   } into s
                                   select s.OrderByDescending(a=>a.PostingDate).FirstOrDefault()).ToList();
                return unscNewData;
            } else {
                return unscData;
            }

        } 

    }
}