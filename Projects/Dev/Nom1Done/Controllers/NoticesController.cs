using Jitbit.Utils;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Models;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Net;
using System.Configuration;
using Nom1Done.CustomSerialization;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class NoticesController : BaseController
    {


       // IPipelineService IPipelineService;

        // private readonly RestClient _client;
        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient client = new RestClient(apiBaseUrl + "/api/Swnt/");


        public NoticesController(IPipelineService IPipelineService) : base(IPipelineService)
        {
            //this.IPipelineService = IPipelineService;
            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }


        public ActionResult Index(string pipelineDuns, BONoticeSearchCriteria criteria)
        {

            // criteria.IsCritical = true;

            DateTime minDate = DateTime.MinValue;

            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
            int companyId = String.IsNullOrEmpty(currentIdentity.CompanyId) ? 0 : int.Parse(currentIdentity.CompanyId);

            criteria.RecipientCompanyID = companyId;

            ViewBag.pipelineDuns = pipelineDuns;
           // criteria.pipelineId = pipelineId;
           
            criteria.PipelineDuns = pipelineDuns;
            criteria.postStartDate = null;
            criteria.postEndDate = null;
            criteria.EffectiveStartDate = null;
            criteria.EffectiveEndDate = null;
            //criteria.pipelineId = pipelineId;
            return this.View(criteria);
        }
        [HttpPost]
        public ActionResult Index(BONoticeSearchCriteria criteria, string download)
        {

            ShipperReturnByIdentity currentIdentity = new ShipperReturnByIdentity();
            currentIdentity = GetValueFromIdentity();
            int companyId = String.IsNullOrEmpty(currentIdentity.CompanyId) ? 0 : int.Parse(currentIdentity.CompanyId);

             string pipelineDuns = criteria.PipelineDuns;
             ViewBag.pipelineDuns = pipelineDuns;

            if (download != null)
            {
                var request = new RestRequest(string.Format("GetSwntTotalRecords"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(criteria);
                var response = client.Execute<int>(request);
                int totalRecords = response.Data;
                criteria.PageSize = totalRecords;
                criteria.PageNo = 1;
                criteria.NoticeList = GetNoticesBySearch(criteria);
                DownloadCSV(criteria);
            }
            return this.View(criteria);
        }

        public ActionResult Detail(int ID, string pipelineDuns)
        {
            var request = new RestRequest(string.Format("GetNoticeOnId/" + ID), Method.GET);
            var response = client.Execute<BONotice>(request);           
            BONotice obj = new BONotice();
            if (response.Data != null)
            {
                obj = response.Data;
                var pipes = GetPipelines();
                var pipe = pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault();
                ViewBag.pipelinedetail = pipe.Name;
                obj.Message = StringToParagraphs(obj.Message, 100);
                obj.TSP = pipe.Name; //pipe.NameWithoutDuns + "/" + pipe.DUNSNo;
            }            
            return this.View(obj);
        }

        public string StringToParagraphs(string input, int rowLength = 250)
        {

            StringBuilder result = new StringBuilder();
            StringBuilder line = new StringBuilder();
            Queue<string> stack = new Queue<string>(input.Split(' '));
            while (stack.Count > 0)
            {

                var word = stack.Dequeue();
                if (word.Length > rowLength && word.EndsWith("."))
                {

                    string head = word.Substring(0, rowLength);
                    string tail = word.Substring(rowLength);
                    word = head;
                    stack.Enqueue(tail);
                }
                if ((line.Length + word.Length) >= rowLength && word.EndsWith("."))
                {
                    line.Append(word + " ");
                    result.AppendLine(line.ToString() + "<br><br>");
                    line.Clear();
                }
                else
                {
                    line.Append(word + " ");
                }

            }
            result.Append(line);
            return result.ToString();
        }


        private BONotice MergeNotices(BONoticeList list)
        {
            BONotice obj = new BONotice();
            obj.CreatedDate = list.FirstOrDefault().CreatedDate;

            obj.StatusID = list.FirstOrDefault().StatusID;
            obj.PipelineDuns = list.FirstOrDefault().PipelineDuns;
            obj.RecipientCompanyID = list.FirstOrDefault().RecipientCompanyID;

            obj.NoticeEffectiveDate = DateTime.ParseExact(list.Where(a => a.NoticeEffectiveDate != "").FirstOrDefault().NoticeEffectiveDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            obj.NoticeEndDate = DateTime.ParseExact(list.Where(a => a.NoticeEndDate != "").FirstOrDefault().NoticeEndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            obj.Message = list.FirstOrDefault().Message;
            obj.MessageID = list.FirstOrDefault().MessageID;
            obj.IsCritical = list.FirstOrDefault().IsCritical;
            obj.InboxID = list.FirstOrDefault().InboxID;
            obj.ID = list.FirstOrDefault().ID;
            obj.RecipientCompanyID = list.FirstOrDefault().RecipientCompanyID;

            return obj;
        }

        public bool DownloadCSV(BONoticeSearchCriteria criteria)
        {
            var csvExport = new CsvExport();
            try
            {
                var noticeList = criteria.NoticeList;
               // noticeList = ReduceDuplicates(noticeList);

                foreach (var Item in noticeList)
                {
                    csvExport.AddRow();
                    csvExport["Subject"] = Item.Subject;
                    csvExport["Posted Date"] = Item.PostingDateTime;
                }

                Response.Clear();
                MemoryStream Ms = new MemoryStream(csvExport.ExportToBytes());
                Response.ContentType = "application/CSV";
                Response.AddHeader("content-disposition", "attachment;filename=" + "NoticesDataTable.csv");
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Ms.WriteTo(Response.OutputStream);
                Response.End();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<SwntPerTransactionDTO> GetNoticesBySearch(BONoticeSearchCriteria criteria)
        {
            List<SwntPerTransactionDTO> finalResult = new List<SwntPerTransactionDTO>();
            // If All Search Date-Fields are Empty or Null

            // _client.BaseUrl = new Uri("http://localhost:56676");
            var request = new RestRequest(string.Format("GetSwntByCriteria"), Method.POST) { RequestFormat = DataFormat.Json };
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(criteria);
            var response = client.Execute<SwntResultDTO>(request);
            if (response.Data != null) {
                finalResult = response.Data.swntPerTransactionDTO;
                criteria.RecordCount = response.Data.RecordCount;
            } else {
                finalResult =new List<SwntPerTransactionDTO>();
                criteria.RecordCount = 0;
            }
           
          

            return finalResult;

        }
        [HttpPost]
        public async Task<ActionResult> GetData(string pipelineDuns, SwntDateFilter filters)
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

                //Loading.
                BONoticeSearchCriteria criteria = new BONoticeSearchCriteria();

                criteria.PageNo = (startRec / pageSize) + 1;
                criteria.PageSize = pageSize;
                criteria.SortField = GetColumnNameToSort(order);
                criteria.SortDirection = orderDir;
                criteria.PipelineDuns = pipelineDuns;
                criteria.postStartDate = filters.PostStartDate;
                criteria.postEndDate = filters.PostEndDate;
                criteria.EffectiveStartDate = filters.EffectiveStartDate;
                criteria.EffectiveEndDate = filters.EffectiveEndDate;
                criteria.Keyword = filters.Keyword;
                criteria.IsCritical = filters.IsCritical == "true" ? true : false;
                criteria.WatchListId = filters.WatchListId;

                List<SwntPerTransactionDTO> swntPerTransactionlist = new List<SwntPerTransactionDTO>();
                swntPerTransactionlist = GetNoticesBySearch(criteria);                

                //  swntPerTransactionlist = ReduceDuplicates(swntPerTransactionlist);

                // Total record count.
               

                int totalRecords = criteria.RecordCount;  // swntPerTransactionlist.Count;

                // Verification.             

                // Sorting.
                //  swntPerTransactionlist = this.SortByColumnWithOrder(order, orderDir, swntPerTransactionlist);

                // Filter record count.
                int recFilter = criteria.RecordCount;  // swntPerTransactionlist.Count;

                // Apply pagination.
                // swntPerTransactionlist = swntPerTransactionlist.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = swntPerTransactionlist }, JsonRequestBehavior.AllowGet);

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
                    result = "NoticeTypeDesc1";
                    break;

                case "1":
                    // Setting.
                    result = "NoticeId";
                    break;

                case "2":
                    // Setting.
                    result = "PostingDateTime";
                    break;

                case "3":
                    // Setting.
                    result = "NoticeEffectiveDateTime";
                    break;

                case "4":
                    // Setting.
                    result = "NoticeEndDateTime";
                    break;

                case "5":
                    // Setting.
                    result = "Subject";
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
        private List<SwntPerTransactionDTO> SortByColumnWithOrder(string order, string orderDir, List<SwntPerTransactionDTO> data)
        {
            // Initialization.
            List<SwntPerTransactionDTO> lst = new List<SwntPerTransactionDTO>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NoticeTypeDesc1).ToList()
                                                                                                 : data.OrderBy(p => p.NoticeTypeDesc1).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p =>p.NoticeId).ToList()
                                                                                                 : data.OrderBy(p => p.NoticeId).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p =>p.PostingDateTime).ToList()
                                                                                                 : data.OrderBy(p => p.PostingDateTime).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p =>p.NoticeEffectiveDateTime).ToList()
                                                                                                 : data.OrderBy(p => p.NoticeEffectiveDateTime).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p =>p.NoticeEndDateTime).ToList()
                                                                                                 : data.OrderBy(p => p.NoticeEndDateTime).ToList();
                        break;

                    case "5":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList()
                                                                                                   : data.OrderBy(p => p.Subject).ToList();
                        break;                   

                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        #endregion


        public List<SwntPerTransactionDTO> ReduceDuplicates(List<SwntPerTransactionDTO> swntData) {
            if (swntData.Count > 1) {
                var swntNewData = (from a in swntData
                                   group a by new {
                                      a.NoticeId,
                                      a.PostingDateTime.Value.Date,                                      
                                      a.TransportationserviceProvider
                                   } into s
                                   select s.OrderByDescending(a=>a.PostingDateTime).FirstOrDefault()).ToList();
                return swntNewData;
            } else {
                return swntData;
            }
        }


    }
}