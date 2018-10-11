using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class UploadNominationsController : BaseController
    {
        char specialChar = '_';
        private readonly IUploadNominationService uploadNominationService;
        //private readonly IPipelineService pipelineService;
        private readonly ICounterPartiesService counterPartyService;
        private readonly INonPathedService nonPathedService;
        public UploadNominationsController(IPipelineService pipelineService, IUploadNominationService uploadNominationService, ICounterPartiesService counterPartyService, INonPathedService nonPathedService):base(pipelineService)
        {
            this.uploadNominationService = uploadNominationService;
             //this.pipelineService = pipelineService;
            this.counterPartyService = counterPartyService;
            this.nonPathedService = nonPathedService;
        }
        public ActionResult Index(string pipelineDuns)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ViewBag.ss = pipelineDuns;
            UploadFilesListDTO model = new UploadFilesListDTO();
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            model.UploadedFilesList = uploadNominationService.GetUploadedFiles(UserId);
            return View(model);
        }
        public ActionResult UploadedNominationFiles()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            UploadFilesListDTO model = new UploadFilesListDTO();
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            model.UploadedFilesList = uploadNominationService.GetUploadedFiles(UserId);
            return PartialView("_uploadedNominationFiles", model);
        }

        public FileResult DownloadSampleFile()
        {
            string filepath = HostingEnvironment.MapPath("~/Assets/NomsUploadTemplate.xlsx");
            return File(filepath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(filepath));
                       
        }

        public void DownloadFile(int FileID, string pipelineDuns)
        {
            UploadedFile file;
            try
            {
                file = uploadNominationService.DownloadFile(FileID);
                if (file != null)
                {
                    //  Response.Clear();
                    // using (MemoryStream Ms = new MemoryStream())
                    //  {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + file.FileName);
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    // Ms.Write(file.FileBytes,0,file.FileBytes.Length);
                    //Ms.WriteByte(Response.OutputStream);
                    // Response.OutputStream.Write(file.FileBytes, 0, file.FileBytes.Length);
                    Response.BinaryWrite(file.FileBytes);
                    Response.Flush();
                    Response.Close();

                    //using (MemoryStream Ms = new MemoryStream(file.FileBytes))
                    //{
                    //    IWorkbook workbook = new XSSFWorkbook();
                    //    Response.Clear();
                    //    //MemoryStream mss = new MemoryStream(file.FileBytes);
                    //    workbook.Write(Ms);
                    //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "tpms_Dict.xlsx"));
                    //    Response.BinaryWrite(Ms.GetBuffer());
                    //    Response.End();
                    //}
                    //Response.Clear();
                    //MemoryStream mss = new MemoryStream(file.FileBytes);
                    //Response.ContentType = "application/docx";
                    //Response.AddHeader("content-disposition", "attachment;filename=" + file.FileName);
                    //Response.Buffer = true;
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //mss.WriteTo(Response.OutputStream);
                    //Response.End();



                    //  }
                    //Response.End();
                    // return RedirectToAction("Index", new { PipelineId = pipelineId });
                }
                //else
                // return null;
            }
            catch (Exception ex)
            {
                // return null;
            }

        }
        [HttpPost]
        public ActionResult GetFiles(string pipelineDuns)
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
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var UserId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
                IEnumerable<UploadFileDTO> uploadedFilesList = uploadNominationService.GetUploadedFiles(UserId);

                // Total record count.
                int totalRecords = uploadedFilesList.Count();
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    uploadedFilesList = uploadedFilesList.Where(p => p.FileName.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.CreatedDate.ToString().ToLower().Contains(search.ToLower())).ToList();
                }
                // Sorting.
                uploadedFilesList = this.SortByColumnWithOrder(order, orderDir, uploadedFilesList);
                // Filter record count.
                int recFilter = uploadedFilesList.Count();
                // Apply pagination.
                uploadedFilesList = uploadedFilesList.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = uploadedFilesList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        private List<UploadFileDTO> SortByColumnWithOrder(string order, string orderDir, IEnumerable<UploadFileDTO> data)
        {
            // Initialization.
            List<UploadFileDTO> lst = new List<UploadFileDTO>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList()
                                                                                                 : data.OrderBy(p => p.FileName).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedDate).ToList()
                                                                                                 : data.OrderBy(p => p.CreatedDate).ToList();
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
        public string GetFilteredString(string NumberString)    // string filtered only when first char is underscore(_).
        {
            if (!string.IsNullOrEmpty(NumberString))
            {
                char firstchr = NumberString[0];
                string newString = "";
                if (firstchr == specialChar)
                {
                    newString = NumberString.Substring(1);
                }
                else
                {
                    newString = NumberString;
                }
                return newString;
            }
            else return "";


        }

        public void DeleteFile(string fileLocation)
        {
            try
            { 
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
            }
            catch (Exception ex) {
               
            }
        }


        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, string pipelineDuns)
        {
            string fileLocation = "";
            OleDbConnection excelConnection1 = null;
            string successMsg = "All files have been successfully stored. ";
            string pathedMsg = "";
            bool pathedMsgFlag = false;
            string pntMsg = "";
            bool pntMsgFlag = false;
            string nonPathedMsg = "";
            bool nonPathedMsgFlag = false;
            try
            {
                System.Data.DataSet batchds = new System.Data.DataSet();
                System.Data.DataSet contractpathds = new System.Data.DataSet();
                System.Data.DataSet supplyds = new System.Data.DataSet();
                System.Data.DataSet marketds = new System.Data.DataSet();
                System.Data.DataSet pathedds = new System.Data.DataSet();
                System.Data.DataSet nonpahedds = new System.Data.DataSet();

                int cycleId = 0;
                var timely = "TIM";
                var evening = "EVE";
                var intraDay1 = "ID1";
                var intraDay2 = "ID2";
                var intraDay3 = "ID3";
                var SONAT = "006900518";  // Sonat Duns. for Route
                var ElPaso = "008001703";  // El Paso Duns. for Route

                var CurrentBusinessTTSell = "01";   // "Current Business (Sell)";  //01
                var CurrentBusinessTTBuy = "01";  //"Current Business (Buy)";    //01
                var OffsystemMarket = "117";      // "Off-system Market";  //117
                var offsystemSupply = "118";     // "Off-system Supply";   //118  

                var Loan = "28";
                var ParkWithdrawal = "27";
                var StorageWithdrawal = "07";

                var LoanPayBack = "29";
                var Park = "26";
                var StorageInjection = "06";
               
                //int PipelineId = Id;
                var currentInfo = GetValueFromIdentity();
                string UserID = currentInfo.UserId;
                string shipperDuns = currentInfo.ShipperDuns;
                string shipperName = currentInfo.ShipperName;

                string company = currentInfo.CompanyId;
                int companyID = String.IsNullOrEmpty(company) ? 0 : int.Parse(company);
                List<string> notAccessedPipeduns = new List<string>();
                List<string> accessedPipeduns = new List<string>();
                Random random = new Random((int)DateTime.Now.Ticks);                
                var PipelinesForUser = GetPipelines();
                PipelinesForUser = PipelinesForUser.Where(a => a.IsNoms==true).ToList();
                if (PipelinesForUser == null) { PipelinesForUser = new List<PipelineDTO>(); }
                //= pipelineService.GetAllPipelineList(GetCurrentCompanyID(), UserID);

                foreach (var file in files)
                {

                    // if (Request.Files["files"].ContentLength > 0)
                    if (file.ContentLength > 0)
                    {

                        #region file with Data

                        fileLocation = Server.MapPath("~/Content/") + file.FileName;
                        DeleteFile(fileLocation);
                        Request.Files["files"].SaveAs(fileLocation);

                        // string fileExtension = Path.GetExtension(Request.Files["files"].FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {

                            #region Upload file in db
                            MemoryStream mem = new MemoryStream();
                            mem.SetLength((int)file.ContentLength);
                            file.InputStream.Read(mem.GetBuffer(), 0, (int)file.ContentLength);
                            UploadedFile fileSave = new UploadedFile();
                            fileSave.AddedBy = UserID;
                            fileSave.CreatedDate = DateTime.Now;
                            fileSave.FileBytes = mem.GetBuffer();
                            fileSave.FileName = file.FileName;
                            fileSave.PipelineDuns = pipelineDuns;
                            uploadNominationService.SaveUploadedFile(fileSave);
                            #endregion

                            string excelConnectionString = string.Empty;  // fileLocation
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;ImportMixedTypes=Text;IMEX=1\"";
                            //connection String for xls file format.
                            if (fileExtension == ".xls")
                            {
                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;ImportMixedTypes=Text;IMEX=1\"";
                            }
                            //connection String for xlsx file format.
                            else if (fileExtension == ".xlsx")
                            {

                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;ImportMixedTypes=Text;IMEX=1\"";
                            }

                            excelConnection1 = new OleDbConnection(excelConnectionString);
                            excelConnection1.Open();

                            try { 
                            string query1 = string.Format("Select * from [{0}]", "Batch$A1:D");//"",Contract Paths$,Market$,NonPathed$,Pathed$,Supply$
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query1, excelConnection1))
                            {
                                dataAdapter.Fill(batchds);
                            }
                            }
                            catch (Exception ex) {
                                return Json("Something went wrong with Batch tab.");
                            }


                            try { 
                            string query2 = string.Format("Select * from [{0}]", "Contract Paths$A1:N");//,,NonPathed$,Pathed$,Supply$
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query2, excelConnection1))
                            {
                                dataAdapter.Fill(contractpathds);
                            }
                            }
                            catch (Exception ex) { return Json("Something went wrong with Contract Path tab."); }


                            try { 
                            string query3 = string.Format("Select * from [{0}]", "Market$A1:L");//"Batch$",Contract Paths$,Market$,NonPathed$,Pathed$,Supply$
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query3, excelConnection1))
                            {
                                dataAdapter.Fill(marketds);
                            }
                            }
                            catch (Exception ex) { return Json("Something went wrong with Market tab."); }



                            // string query4 = string.Format("Select * from [{0}]", "NonPathed$");  //"Batch$",Contract Paths$,Market$,NonPathed$,Pathed$,Supply$
                            // string query4 = String.Format("select * from [{0}{1}]", "NonPathed$", "A3:AB3");
                            string query4 = string.Format("Select * from [{0}]", "NonPathed$A2:AB");
                            try {                            
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query4, excelConnection1))
                            {
                                dataAdapter.Fill(nonpahedds);
                            }                         


                            string delNonPathedQuery = string.Format("Select * from [{0}]", "NonPathed$A2:N2");
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query4, excelConnection1))
                            {
                                dataAdapter.Fill(nonpahedds);
                            }
                            }
                            catch (Exception ex) { return Json("Something went wrong with Non-Pathed tab."); }


                            try { 
                            string query5 = string.Format("Select * from [{0}]", "Pathed$A1:V");//"Batch$",Contract Paths$,Market$,NonPathed$,Pathed$,Supply$
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query5, excelConnection1))
                            {
                                dataAdapter.Fill(pathedds);
                            }
                            }
                            catch (Exception ex) { return Json("Something went wrong with Pathed tab."); }

                            try
                            {
                                string query6 = string.Format("Select * from [{0}]", "Supply$A1:L");//"Batch$",Contract Paths$,Market$,NonPathed$,Pathed$,Supply$
                                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query6, excelConnection1))
                                {
                                    dataAdapter.Fill(supplyds);
                                }
                            }
                            catch (Exception ex) { return Json("Something went wrong with Supply tab."); }


                            try
                            {                           

                            #region DataTables validations

                            string errorMsg = string.Empty;
                            bool isBatchempty = false;
                            if (batchds.Tables.Count == 0 || batchds.Tables[0].Rows.Count == 0)
                            {
                                isBatchempty = true;
                                    errorMsg += " Batch tab is empty."; 
                            }
                            else
                            {
                                errorMsg += ValideBatchColumnHeadins(batchds);
                            }
                           

                            bool isSupplyEmpty = false;
                            if (supplyds.Tables.Count == 0 || supplyds.Tables[0].Rows.Count == 0)
                            {
                                isSupplyEmpty = true;
                                    errorMsg += " supply tab is empty";
                            }
                            else
                            {
                                errorMsg += ValidateSupplyColumnHeadings(supplyds);
                            }

                            bool isContractEmpty = false;
                            if (contractpathds.Tables.Count == 0 || contractpathds.Tables[0].Rows.Count == 0)
                            {
                                isContractEmpty = true;
                                    errorMsg += "contractpath tab is empty.";
                            }
                            else
                            {
                                errorMsg += ValidateContractPathColumnHeadings(contractpathds);
                            }

                            bool isMarketEmpty = false;
                            if (marketds.Tables.Count == 0 || marketds.Tables[0].Rows.Count == 0)
                            {
                                isMarketEmpty = true;
                                    errorMsg += " Market tab is empty";
                            }
                            else
                            {
                                errorMsg += ValidateMarketColumnHeadings(marketds);
                            }

                            bool isPathedEmpty = false;
                            if (pathedds.Tables.Count == 0 || pathedds.Tables[0].Rows.Count == 0)
                            { isPathedEmpty = true;
                                    errorMsg += " Pathed tab is empty.";
                                }
                            else
                            {
                                errorMsg += ValidatePathedColumnHeadings(pathedds);
                            }

                            bool isNonPathedmpty = false;
                            if (nonpahedds.Tables.Count == 0 || nonpahedds.Tables[0].Rows.Count == 0)
                            {
                                    isNonPathedmpty = true;
                                    errorMsg += " Non-Pathed tab is empty.";
                                }
                            else
                            {
                                errorMsg += ValidateNonPathedColumnHeadings(nonpahedds);
                            }


                            // Return Error message of validation.
                            if (!string.IsNullOrEmpty(errorMsg)) { return Json(errorMsg); }

                                #endregion

                            }
                            catch (Exception ex) {
                              return  Json("Excel format is not correct. Please, Check excelsheet column-headings.");
                            }

                            List<V4_Nomination> NomsList = new List<V4_Nomination>();
                            List<V4_Batch> batchList = new List<V4_Batch>();
                            PipelineDTO pipeline = new PipelineDTO();
                            Random ran = new Random((int)DateTime.Now.Ticks);

                            #region pathed dataset
                            string CurrentColumn = string.Empty;
                            try
                            {
                                string msg = "Something wrong!! Please, Check ";                               
                               // int rowskipCount = 0;
                               // int printCount = 0;

                               
                            foreach (DataRow row in pathedds.Tables[0].Rows)
                            {
                                V4_Batch batch = new V4_Batch();
                                batch.Nominations = new List<V4_Nomination>();
                                if (DBNull.Value.Equals(row["Beg Date"]) && DBNull.Value.Equals(row["End Date"]) && DBNull.Value.Equals(row[" Cycle"]) && DBNull.Value.Equals(row["DunsNo"]) && DBNull.Value.Equals(row["Svc Req K"]) && DBNull.Value.Equals(row["Fuel Percentage"]) && DBNull.Value.Equals(row["Nom Sub Cycle"]) && DBNull.Value.Equals(row["TT"])) {
                                     continue;
                                }  // if row is empty, go to next row.
                                var PipeDuns = Convert.ToString(row["DunsNo"]);
                                if(!PipelinesForUser.Any(a=>a.DUNSNo==PipeDuns))
                                {
                                    //successMsg += Environment.NewLine+ " Contact to admin to have access to the pipeline " + PipeDuns;
                                    notAccessedPipeduns.Add(PipeDuns);
                                    continue;
                                }
                                    accessedPipeduns.Add(PipeDuns);
                                CurrentColumn = "Beg Date ";   
                                var bDate = Convert.ToDateTime(row["Beg Date"]);

                                CurrentColumn = "End Date ";
                                var eDate = Convert.ToDateTime(row["End Date"]);
                                batch.CreatedBy = UserID;
                                batch.CreatedDate = DateTime.Now;
                                batch.IsActive = true;

                                    CurrentColumn = "Cycle ";
                                    string cycleCode = Convert.ToString(row[" Cycle"]);
                                cycleId = 0;
                                if (cycleCode == timely)
                                {
                                   // bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 1;
                                }
                                else if (cycleCode == evening)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 2;
                                }
                                else if (cycleCode == intraDay1)
                                {
                                   // bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 14, 0, 0);
                                  // eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 3;
                                }
                                else if (cycleCode == intraDay2)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 18, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 4;
                                }
                                else if (cycleCode == intraDay3)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 22, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 5;
                                }
                                batch.FlowEndDate = eDate;
                                batch.FlowStartDate = bDate;
                                batch.CycleId = cycleId;
                                batch.Description = "Bulk Upload - Pathed";
                                    CurrentColumn = "Nom Sub Cycle ";
                                batch.NomSubCycle = Convert.ToString(row["Nom Sub Cycle"]);
                                batch.StatusID = (int)statusBatch.Draft;
                                batch.SubmitDate = DateTime.MaxValue;
                                batch.ScheduleDate = DateTime.MaxValue;
                                    CurrentColumn = "DunsNo. ";
                                
                                //if (!string.IsNullOrEmpty(PipeDuns))
                                //{
                                //    pipeline = pipelineService.GetPipelineByDunsNo(PipeDuns);
                                //}
                                batch.PipelineDuns = PipeDuns;
                                batch.ServiceRequester = shipperDuns;
                                batch.ShowZeroCheck = false;
                                batch.ShowZeroDn = false;
                                batch.ShowZeroUp = false;
                                batch.RankingCheck = false;
                                batch.PakageCheck = false;
                                batch.UpDnContractCheck = false;
                                batch.UpDnPkgCheck = false;                               
                                batch.ReferenceNumber = ran.Next(999999999).ToString();
                                string path = Path.GetRandomFileName();
                                path = path.Replace(".", "");
                                batch.TransactionSetControlNumber = path;
                                batch.NomTypeID = (int)NomType.Pathed;



                                V4_Nomination nom = new V4_Nomination();
                                    CurrentColumn = "Svc Req K ";
                                nom.ContractNumber = Convert.ToString(row["Svc Req K"]);
                                int? recQty = null;
                                int? delQty = null;
                                decimal? fuelPercentage = null;  
                               
                                nom.BidTransportationRate = "";
                                    CurrentColumn = "Fuel Percentage ";
                                if (DBNull.Value.Equals(row["Fuel Percentage"]))
                                {
                                    fuelPercentage = null;
                                }
                                else
                                {
                                    fuelPercentage = Convert.ToDecimal(row["Fuel Percentage"]);
                                }
                                nom.FuelPercentage = fuelPercentage;


                                    CurrentColumn = "Rec Qty";
                                if (DBNull.Value.Equals(row["Rec Qty"]))
                                {
                                    recQty = null;
                                }
                                else
                                {
                                    recQty = Convert.ToInt32(row["Rec Qty"]);
                                }

                                    CurrentColumn = "Del Qty";
                                if (DBNull.Value.Equals(row["Del Qty"]))
                                {
                                    delQty = null;
                                } else {
                                    delQty = Convert.ToInt32(row["Del Qty"]);
                                }                                
                                 nom.Quantity =ApplyFormulasOnRecQty(recQty,delQty,fuelPercentage);
                                 nom.DelQuantity = ApplyFormulasOnDelQty(recQty,delQty,fuelPercentage);
                                    CurrentColumn = "Nom Sub Cycle";
                                nom.NominationSubCycleIndicator =Convert.ToString(row["Nom Sub Cycle"]);
                                    // nom.AssociatedContract = (string)row["Act Cd"];
                                    CurrentColumn = "TT";
                                nom.TransactionType = Convert.ToString(row["TT"]);
                                nom.TransactionTypeDesc = string.Empty;
                                    CurrentColumn = "Rec Loc";
                                nom.ReceiptLocationIdentifier = Convert.ToString(row["Rec Loc"]);
                                nom.receiptLocationPropCode = Convert.ToString(row["Rec Loc"]);
                                    CurrentColumn = "Up Id Prop";
                                if (DBNull.Value.Equals(row["Up ID Prop"]))
                                {
                                    nom.UpstreamPropCode = Convert.ToString(row["Up ID"]);
                                }
                                else {
                                    nom.UpstreamPropCode = Convert.ToString(row["Up ID Prop"]);
                                }
                                    CurrentColumn = "Up ID";
                                    nom.UpstreamIdentifier = Convert.ToString(row["Up ID"]);
                                    CurrentColumn = "Up K";
                                    nom.UpstreamContractIdentifier = Convert.ToString(row["Up K"]);
                                    CurrentColumn = "Rec Rank";
                                    nom.ReceiptRank = Convert.ToString(row["Rec Rank"]).ToString();
                                    CurrentColumn = "Del Loc";
                                    nom.DeliveryLocationIdentifer = Convert.ToString(row["Del Loc"]);
                                nom.DeliveryLocationPropCode = Convert.ToString(row["Del Loc"]);
                                    CurrentColumn = "Dn Id Prop";
                                    if (DBNull.Value.Equals(row["Dn ID Prop"])) {
                                    nom.DownstreamPropCode = Convert.ToString(row["Dn ID"]);
                                } else {
                                    nom.DownstreamPropCode = Convert.ToString(row["Dn ID Prop"]);
                                }
                                    CurrentColumn = "Dn Id";
                                    nom.DownstreamIdentifier = Convert.ToString(row["Dn ID"]);
                                    CurrentColumn = "Dn K";
                                    nom.DownstreamContractIdentifier = Convert.ToString(row["Dn K"]);

                                    CurrentColumn = "Del Rank";
                                    nom.DeliveryRank = Convert.ToString(row["Del Rank"]);
                                    CurrentColumn = "Pkg ID";
                                nom.PackageId = Convert.ToString(row["Pkg ID"]);
                                nom.NominatorTrackingId = NomTrackingID(9, random);
                                nom.PathType = "P";
                                nom.QuantityTypeIndicator = "R";
                                nom.UnitOfMeasure = "BZ";
                                batch.Nominations.Add(nom);
                                batchList.Add(batch);
                                   
                            }                                
                              
                               
                                
                            }
                            catch (Exception)
                            {
                                return Json("Something wrong!! Please, Check "+CurrentColumn+" in Pathed tab.");
                            }

                            #endregion


                            #region PNTBatch dataset

                           

                            foreach (DataRow row in batchds.Tables[0].Rows)
                            {
                                V4_Batch batch = new V4_Batch();
                                batch.Nominations = new List<V4_Nomination>();
                                if (DBNull.Value.Equals(row["Start Date"])) { continue; }  // if row is empty, go to next row.


                                var PipeDunsPNT = Convert.ToString(row["DunsNo"]);
                                if (!PipelinesForUser.Any(a => a.DUNSNo == PipeDunsPNT))
                                {
                                    //successMsg += Environment.NewLine + " Contact to admin to have access to the pipeline " + PipeDunsPNT;
                                    notAccessedPipeduns.Add(PipeDunsPNT);
                                    continue;
                                }
                                accessedPipeduns.Add(PipeDunsPNT);
                                var bDate = Convert.ToDateTime(row["Start Date"]);
                                var eDate = Convert.ToDateTime(row["End Date"]);
                                batch.CreatedBy = UserID;
                                batch.CreatedDate = DateTime.Now;
                                batch.IsActive = true;
                                string cycleCode =Convert.ToString(row["CycleCode"]);
                                cycleId = 0;
                                if (cycleCode == timely)
                                {
                                   // bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
                                   // eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 1;
                                }
                                else if (cycleCode == evening)
                                {
                                   // bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 2;
                                }
                                else if (cycleCode == intraDay1)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 14, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 3;
                                }
                                else if (cycleCode == intraDay2)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 18, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 4;
                                }
                                else if (cycleCode == intraDay3)
                                {
                                  //  bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 22, 0, 0);
                                  //  eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
                                    cycleId = 5;
                                }
                                batch.FlowEndDate = eDate;
                                batch.FlowStartDate = bDate;
                                batch.CycleId = cycleId;
                                batch.Description = "Bulk Upload - PNT Batch";
                                batch.NomSubCycle = "";
                                batch.StatusID = (int)statusBatch.Draft;
                                batch.SubmitDate = DateTime.MaxValue;
                                batch.ScheduleDate = DateTime.MaxValue;
                                
                                //if (!string.IsNullOrEmpty(PipeDunsPNT))
                                //{
                                //    pipeline = pipelineService.GetPipelineByDunsNo(PipeDunsPNT);
                                //}
                                batch.PipelineDuns = PipeDunsPNT;
                                batch.ServiceRequester = shipperDuns;
                                batch.ShowZeroCheck = false;
                                batch.ShowZeroDn = false;
                                batch.ShowZeroUp = false;
                                batch.RankingCheck = false;
                                batch.PakageCheck = false;
                                batch.UpDnContractCheck = false;
                                batch.UpDnPkgCheck = false;
                                //Random ran = new Random((int)DateTime.Now.Ticks);
                                batch.ReferenceNumber = ran.Next(999999999).ToString();
                                string path = Path.GetRandomFileName();
                                path = path.Replace(".", "");
                                batch.TransactionSetControlNumber = path;
                                batch.NomTypeID = (int)NomType.PNT;

                                #region Contract dataset
                                string contractCurrentColumn = string.Empty;
                                try {
                                   
                                foreach (DataRow nomrow in contractpathds.Tables[0].Rows)
                                {
                                    int? recQty = null;
                                    int? delQty = null;
                                    decimal? fuelPercentage = null;
                                    V4_Nomination nomContract = new V4_Nomination();
                                    if (DBNull.Value.Equals(nomrow["Svc Req K"]) ||  Convert.ToString(nomrow["DunsNo"]) != PipeDunsPNT) { continue; }

                                        contractCurrentColumn = "Svc Req K";
                                        nomContract.ContractNumber = Convert.ToString(nomrow["Svc Req K"]);

                                        contractCurrentColumn = "Fuel Percentage";
                                        if (DBNull.Value.Equals(nomrow["Fuel Percentage"])) { fuelPercentage = null; } else { fuelPercentage = Convert.ToDecimal(nomrow["Fuel Percentage"]); }
                                    nomContract.FuelPercentage = fuelPercentage;

                                        contractCurrentColumn = "Rec Qty (Gross)";
                                    if (DBNull.Value.Equals(nomrow["Rec Qty (Gross)"])) { recQty = null; } else { recQty= Convert.ToInt32(nomrow["Rec Qty (Gross)"]); }

                                        contractCurrentColumn = "Del Qty (Net)";
                                        if (DBNull.Value.Equals(nomrow["Del Qty (Net)"])) { delQty = null; } else { delQty = Convert.ToInt32(nomrow["Del Qty (Net)"]); }
                                    nomContract.Quantity = ApplyFormulasOnRecQty(recQty,delQty,fuelPercentage);
                                    nomContract.DelQuantity = ApplyFormulasOnDelQty(recQty,delQty,fuelPercentage);
                                        contractCurrentColumn = "TT";
                                    nomContract.TransactionType = Convert.ToString(nomrow["TT"]);
                                        contractCurrentColumn = "Rec Loc";
                                    nomContract.ReceiptLocationIdentifier = Convert.ToString(nomrow["Rec Loc"]);
                                    nomContract.receiptLocationPropCode = Convert.ToString(nomrow["Rec Loc"]);

                                        contractCurrentColumn = "Dn Rank/Rec Rank";
                                    nomContract.ReceiptRank = Convert.ToString(nomrow["Dn Rank/Rec Rank"]);

                                        contractCurrentColumn = "Del Loc";                                 
                                    nomContract.DeliveryLocationIdentifer = Convert.ToString(nomrow["Del Loc"]);
                                    nomContract.DeliveryLocationPropCode = Convert.ToString(nomrow["Del Loc"]);

                                        contractCurrentColumn = "Up Rank/Del Rank";
                                    nomContract.DeliveryRank = Convert.ToString(nomrow["Up Rank/Del Rank"]);

                                        contractCurrentColumn = "Path Rank";
                                    nomContract.PathRank = Convert.ToString(nomrow["Path Rank"]);

                                        contractCurrentColumn = "Pkg ID";
                                    nomContract.PackageId = Convert.ToString(nomrow["Pkg ID"]);
                                    //nomContract.ImbalancePeriod = Convert.ToString(nomrow["Imbalance Month"]);
                                    if (PipeDunsPNT == SONAT || PipeDunsPNT == ElPaso)
                                    {
                                        nomContract.Route = Convert.ToString(nomrow.ItemArray[12]);
                                    }
                                    nomContract.NominatorTrackingId = NomTrackingID(9, random);
                                    nomContract.PathType = "T";
                                    nomContract.QuantityTypeIndicator = "R";
                                    nomContract.UnitOfMeasure = "BZ";

                                    batch.Nominations.Add(nomContract);
                                   }

                                }
                                catch (Exception)
                                {
                                    return Json("Something went wrong!! Please, Check "+ contractCurrentColumn + " in ContractPath Tab.");
                                }


                                #endregion

                                #region Supply dataset

                                var supplyCurrentColumn = string.Empty;
                                try { 

                                foreach (DataRow nomrow in supplyds.Tables[0].Rows)
                                {
                                    int? recQty = null;
                                    int? delQty = null;
                                    decimal? fuelPercentage = null;
                                    V4_Nomination nomSupply = new V4_Nomination();
                                    if (DBNull.Value.Equals(nomrow["Svc Req K"]) || Convert.ToString(nomrow["DunsNo"]) != PipeDunsPNT) { continue; }

                                    supplyCurrentColumn = "Rec Loc";
                                    nomSupply.ReceiptLocationIdentifier = Convert.ToString(nomrow["Rec Loc"]);
                                    nomSupply.receiptLocationPropCode = Convert.ToString(nomrow["Rec Loc"]);

                                        supplyCurrentColumn = "TT";
                                    nomSupply.TransactionType = Convert.ToString(nomrow["TT"]);
                                        supplyCurrentColumn = "Svc Req K";
                                    nomSupply.ContractNumber = Convert.ToString(nomrow["Svc Req K"]);

                                        supplyCurrentColumn = "Fuel Percentage";
                                    if (DBNull.Value.Equals(nomrow["Fuel Percentage"])) { fuelPercentage = null; } else { fuelPercentage = Convert.ToDecimal(nomrow["Fuel Percentage"]); }
                                    nomSupply.FuelPercentage = fuelPercentage;

                                    if (nomSupply.TransactionType == CurrentBusinessTTBuy || nomSupply.TransactionType == offsystemSupply)
                                    {                                            
                                        nomSupply.ContractNumber = shipperDuns;
                                        nomSupply.FuelPercentage = 0;
                                    }

                                        supplyCurrentColumn = "Rec Qty (Gross)";
                                    if (DBNull.Value.Equals(nomrow["Rec Qty (Gross)"])) { recQty = null; } else { recQty = Convert.ToInt32(nomrow["Rec Qty (Gross)"]); }
                                    nomSupply.Quantity = recQty;
                                    nomSupply.DelQuantity = ApplyFormulasOnDelQty(recQty,delQty,fuelPercentage);
                                        supplyCurrentColumn = "Up ID Prop ";
                                    if (DBNull.Value.Equals(nomrow["Up ID Prop "])) {
                                        nomSupply.UpstreamPropCode = Convert.ToString(nomrow["Up ID"]);
                                    } else {
                                        nomSupply.UpstreamPropCode = Convert.ToString(nomrow["Up ID Prop "]);
                                    }

                                        supplyCurrentColumn = "Up ID";
                                    nomSupply.UpstreamIdentifier = Convert.ToString(nomrow["Up ID"]);

                                        supplyCurrentColumn = "Up Rank/Del Rank";
                                    nomSupply.ReceiptRank = Convert.ToString(nomrow["Up Rank/Del Rank"]);
                                    nomSupply.UpstreamRank= Convert.ToString(nomrow["Up Rank/Del Rank"]);
                                        supplyCurrentColumn = "Pkg ID";
                                    nomSupply.PackageId = Convert.ToString(nomrow["Pkg ID"]);

                                        supplyCurrentColumn = "Up/Dn Contract";
                                    if (DBNull.Value.Equals(nomrow["Up/Dn Contract"]))
                                    {
                                        nomSupply.UpstreamContractIdentifier = string.Empty;
                                    }
                                    else {
                                      nomSupply.UpstreamContractIdentifier = Convert.ToString(nomrow["Up/Dn Contract"]);
                                    }
                                    // nomSupply.ImbalancePeriod = Convert.ToString(nomrow["Imbalance Month"]);                              
                                    nomSupply.NominatorTrackingId = NomTrackingID(9, random);
                                    nomSupply.PathType = "S";
                                    nomSupply.QuantityTypeIndicator = "R";
                                    nomSupply.UnitOfMeasure = "BZ";
                                   if (nomSupply.TransactionType == Loan || nomSupply.TransactionType == ParkWithdrawal || nomSupply.TransactionType == StorageWithdrawal)
                                   {                                           
                                            nomSupply.UpstreamIdentifier = shipperDuns;
                                            nomSupply.UpstreamPropCode = shipperDuns;                                            
                                   }

                                   batch.Nominations.Add(nomSupply);
                                }

                                }
                                catch (Exception ex)
                                { return Json("Something went wrong!! Please, Check "+supplyCurrentColumn+" in Supply tab."); }



                                #endregion

                                #region Market DataSet

                                string marketCurrentColumn = string.Empty;

                                try { 

                                foreach (DataRow nomrow in marketds.Tables[0].Rows)
                                {
                                    int? recQty = null;
                                    int? delQty = null;
                                    decimal? fuelPercentage = null;
                                    V4_Nomination nomMarket = new V4_Nomination();
                                    if (DBNull.Value.Equals(nomrow["Svc Req K"]) ||  Convert.ToString(nomrow["DunsNo"]) != PipeDunsPNT) { continue; }

                                        marketCurrentColumn = "Del Loc";
                                    nomMarket.DeliveryLocationIdentifer = Convert.ToString(nomrow["Del Loc"]);
                                    nomMarket.DeliveryLocationPropCode = Convert.ToString(nomrow["Del Loc"]);

                                        marketCurrentColumn = "TT";
                                    nomMarket.TransactionType = Convert.ToString(nomrow["TT"]);

                                        marketCurrentColumn = "Svc Req K";
                                    nomMarket.ContractNumber = Convert.ToString(nomrow["Svc Req K"]);

                                        marketCurrentColumn = "Fuel Percentage";
                                    if (!DBNull.Value.Equals(nomrow["Fuel Percentage"])) { fuelPercentage= Convert.ToDecimal(nomrow["Fuel Percentage"]); }
                                         nomMarket.FuelPercentage = fuelPercentage;
                                   if (nomMarket.TransactionType == CurrentBusinessTTSell || nomMarket.TransactionType == OffsystemMarket)
                                   {
                                            nomMarket.ContractNumber = shipperDuns;
                                            nomMarket.FuelPercentage = 0;                                            
                                   }

                                        marketCurrentColumn = "Rec Qty";
                                    if (!DBNull.Value.Equals(nomrow["Rec Qty"])) { recQty= Convert.ToInt32(nomrow["Rec Qty"]); }
                                    nomMarket.Quantity = recQty;
                                    nomMarket.DelQuantity = ApplyFormulasOnDelQty(recQty,delQty,fuelPercentage);
                                        marketCurrentColumn = "Dn Id Prop";
                                    if (DBNull.Value.Equals(nomrow["Dn Id Prop"])) {
                                        nomMarket.DownstreamPropCode = Convert.ToString(nomrow["Dn ID "]);
                                    } else {
                                        nomMarket.DownstreamPropCode = Convert.ToString(nomrow["Dn Id Prop"]);
                                    }
                                        marketCurrentColumn = "Dn ID";
                                    nomMarket.DownstreamIdentifier = Convert.ToString(nomrow["Dn ID "]);
                                        marketCurrentColumn = "Dn Rank/Rec Rank";
                                    nomMarket.DeliveryRank = Convert.ToString(nomrow["Dn Rank/Rec Rank"]);
                                    nomMarket.DownstreamRank = Convert.ToString(nomrow["Dn Rank/Rec Rank"]);
                                        marketCurrentColumn = "Pkg ID";
                                    nomMarket.PackageId = Convert.ToString(nomrow["Pkg ID"]);
                                        marketCurrentColumn = "Up/Dn Contract";
                                      if (DBNull.Value.Equals(nomrow["Up/Dn Contract"]))
                                        {
                                            nomMarket.DownstreamContractIdentifier = string.Empty;
                                        }
                                        else
                                        {
                                            nomMarket.DownstreamContractIdentifier = Convert.ToString(nomrow["Up/Dn Contract"]);
                                        }

                                        nomMarket.NominatorTrackingId = NomTrackingID(9, random);
                                    nomMarket.PathType = "M";
                                    nomMarket.QuantityTypeIndicator = "D";
                                    nomMarket.UnitOfMeasure = "BZ";

                                    if (nomMarket.TransactionType == LoanPayBack || nomMarket.TransactionType == StorageInjection || nomMarket.TransactionType == Park)
                                    {
                                            nomMarket.DownstreamIdentifier = shipperDuns;
                                            nomMarket.DownstreamPropCode = shipperDuns;
                                    }

                                    batch.Nominations.Add(nomMarket);
                                }

                                }
                                catch (Exception ex)
                                { return Json("Something went wrong! Please, Check "+ marketCurrentColumn+" in Market tab."); }

                                #endregion

                                batchList.Add(batch);

                            }
                            #endregion


                            #region NONPathed dataset
                            try
                            {                           

                            foreach (DataRow row in nonpahedds.Tables[0].Rows)
                            {
                                
                                V4_Batch batchRec = new V4_Batch();
                                V4_Batch batchDel = new V4_Batch();
                                batchDel.Nominations = new List<V4_Nomination>();
                                batchRec.Nominations = new List<V4_Nomination>();
                                if (DBNull.Value.Equals(row["Start Date"]) && DBNull.Value.Equals(row["Start Date1"])) { continue; }  // if row is empty, go to next row.

                                if (!DBNull.Value.Equals(row["Start Date"]))
                                {
                                    #region RecNoms
                                    var PipeDunsRec = Convert.ToString(row["Duns No"]);
                                    if(!PipelinesForUser.Any(a=>a.DUNSNo == PipeDunsRec))
                                    {
                                        //successMsg += Environment.NewLine+ " Contact to admin to have access to the pipeline " + PipeDunsRec;
                                        notAccessedPipeduns.Add(PipeDunsRec);
                                        continue;
                                    }
                                        accessedPipeduns.Add(PipeDunsRec);
                                    var bDateRec = Convert.ToDateTime(row["Start Date"]);
                                    var eDateRec = Convert.ToDateTime(row["End Date"]);
                                    batchRec.CreatedBy = UserID;
                                    batchRec.CreatedDate = DateTime.Now;
                                    batchRec.IsActive = true;
                                    string cycleCodeRec = Convert.ToString(row["Cycle"]);
                                    cycleId = 0;
                                    if (cycleCodeRec == timely)
                                    {
                                       // bDateRec = new DateTime(bDateRec.Year, bDateRec.Month, bDateRec.Day, 9, 0, 0);
                                       // eDateRec = new DateTime(eDateRec.Year, eDateRec.Month, eDateRec.Day, 9, 0, 0);
                                        cycleId = 1;
                                    }
                                    else if (cycleCodeRec == evening)
                                    {
                                      //  bDateRec = new DateTime(bDateRec.Year, bDateRec.Month, bDateRec.Day, 9, 0, 0);
                                      //  eDateRec = new DateTime(eDateRec.Year, eDateRec.Month, eDateRec.Day, 9, 0, 0);
                                        cycleId = 2;
                                    }
                                    else if (cycleCodeRec == intraDay1)
                                    {
                                      //  bDateRec = new DateTime(bDateRec.Year, bDateRec.Month, bDateRec.Day, 14, 0, 0);
                                      //  eDateRec = new DateTime(eDateRec.Year, eDateRec.Month, eDateRec.Day, 9, 0, 0);
                                        cycleId = 3;
                                    }
                                    else if (cycleCodeRec == intraDay2)
                                    {
                                      //  bDateRec = new DateTime(bDateRec.Year, bDateRec.Month, bDateRec.Day, 18, 0, 0);
                                      //  eDateRec = new DateTime(eDateRec.Year, eDateRec.Month, eDateRec.Day, 9, 0, 0);
                                        cycleId = 4;
                                    }
                                    else if (cycleCodeRec == intraDay3)
                                    {
                                      //  bDateRec = new DateTime(bDateRec.Year, bDateRec.Month, bDateRec.Day, 22, 0, 0);
                                      //  eDateRec = new DateTime(eDateRec.Year, eDateRec.Month, eDateRec.Day, 9, 0, 0);
                                        cycleId = 5;
                                    }
                                    batchRec.FlowEndDate = eDateRec;
                                    batchRec.FlowStartDate = bDateRec;
                                    batchRec.CycleId = cycleId;
                                    batchRec.Description = "Bulk Upload - NonPathed Batch- Receipt";
                                    batchRec.NomSubCycle = Convert.ToString(row["Roll Nom"]);
                                    batchRec.StatusID = (int)statusBatch.Draft;
                                    batchRec.SubmitDate = DateTime.MaxValue;
                                    batchRec.ScheduleDate = DateTime.MaxValue;
                                    
                                    //if (!string.IsNullOrEmpty(PipeDunsRec))
                                    //{
                                    //    pipeline = pipelineService.GetPipelineByDunsNo(PipeDunsRec);
                                    //}
                                    batchRec.PipelineDuns = PipeDunsRec;
                                    batchRec.ServiceRequester = shipperDuns;
                                    batchRec.ShowZeroCheck = false;
                                    batchRec.ShowZeroDn = false;
                                    batchRec.ShowZeroUp = false;
                                    batchRec.RankingCheck = false;
                                    batchRec.PakageCheck = false;
                                    batchRec.UpDnContractCheck = false;
                                    batchRec.UpDnPkgCheck = false;
                                    //Random ran = new Random((int)DateTime.Now.Ticks);
                                    batchRec.ReferenceNumber = ran.Next(999999999).ToString();
                                    string path = Path.GetRandomFileName();
                                    path = path.Replace(".", "");
                                    batchRec.TransactionSetControlNumber = path;
                                    batchRec.NomTypeID = (int)NomType.NonPathed;

                                    V4_Nomination nomRec = new V4_Nomination();
                                    int? recQty = null;
                                    int? delQty = null;
                                    decimal? fuelPercentage = null;
                                    nomRec.TransactionType = Convert.ToString(row["TransactionType"]);
                                    nomRec.NominationSubCycleIndicator = Convert.ToString(row["Roll Nom"]);
                                    nomRec.ContractNumber = Convert.ToString(row["Srv Req Contract"]);
                                    if (!DBNull.Value.Equals(row["Fuel Percentage"])) { fuelPercentage = Convert.ToDecimal(row["Fuel Percentage"]); }

                                    nomRec.FuelPercentage = fuelPercentage;
                                    if (!DBNull.Value.Equals(row["Rec Qty"])) { recQty = Convert.ToInt32(row["Rec Qty"]); }
                                    nomRec.Quantity = recQty;
                                    nomRec.DelQuantity = 0;
                                    nomRec.ReceiptLocationIdentifier = Convert.ToString(row["Rec Loc"]);
                                    nomRec.receiptLocationPropCode = Convert.ToString(row["Rec Loc"]);

                                    nomRec.UpstreamIdentifier = Convert.ToString(row["Up Identifier"]);
                                    nomRec.UpstreamContractIdentifier = Convert.ToString(row["Up Contract"]);

                                    nomRec.ReceiptRank = Convert.ToString(row["Rec Rank"]);
                                    nomRec.PackageId = Convert.ToString(row["Package Id"]);
                                    nomRec.NominatorTrackingId = NomTrackingID(9, random);
                                    nomRec.PathType = "NPR";
                                    nomRec.UnitOfMeasure = "BZ";
                                    nomRec.QuantityTypeIndicator = "R";
                                    batchRec.Nominations.Add(nomRec);
                                    batchList.Add(batchRec);

                                    #endregion
                                }
                                if (!DBNull.Value.Equals(row["Start Date1"]))
                                {
                                    #region delNoms
                                    var PipeDunsDel =Convert.ToString(row["Duns No1"]);
                                    if(!PipelinesForUser.Any(a=>a.DUNSNo == PipeDunsDel))
                                    {
                                        //successMsg += Environment.NewLine+ " Contact to admin to have access to the pipeline " + PipeDunsDel;
                                        notAccessedPipeduns.Add(PipeDunsDel);
                                        continue;
                                    }
                                    accessedPipeduns.Add(PipeDunsDel);
                                    var bDateDel = Convert.ToDateTime(row["Start Date1"]);
                                    var eDateDel = Convert.ToDateTime(row["End Date1"]);
                                    batchDel.CreatedBy = UserID;
                                    batchDel.CreatedDate = DateTime.Now;
                                    batchDel.IsActive = true;
                                    string cycleCodeDel =Convert.ToString(row["Cycle1"]);
                                    cycleId = 0;
                                    if (cycleCodeDel == timely)
                                    {
                                       // bDateDel = new DateTime(bDateDel.Year, bDateDel.Month, bDateDel.Day, 9, 0, 0);
                                       // eDateDel = new DateTime(eDateDel.Year, eDateDel.Month, eDateDel.Day, 9, 0, 0);
                                        cycleId = 1;
                                    }
                                    else if (cycleCodeDel == evening)
                                    {
                                      //  bDateDel = new DateTime(bDateDel.Year, bDateDel.Month, bDateDel.Day, 9, 0, 0);
                                      //  eDateDel = new DateTime(eDateDel.Year, eDateDel.Month, eDateDel.Day, 9, 0, 0);
                                        cycleId = 2;
                                    }
                                    else if (cycleCodeDel == intraDay1)
                                    {
                                      //  bDateDel = new DateTime(bDateDel.Year, bDateDel.Month, bDateDel.Day, 14, 0, 0);
                                      //  eDateDel = new DateTime(eDateDel.Year, eDateDel.Month, eDateDel.Day, 9, 0, 0);
                                        cycleId = 3;
                                    }
                                    else if (cycleCodeDel == intraDay2)
                                    {
                                      //  bDateDel = new DateTime(bDateDel.Year, bDateDel.Month, bDateDel.Day, 18, 0, 0);
                                      //  eDateDel = new DateTime(eDateDel.Year, eDateDel.Month, eDateDel.Day, 9, 0, 0);
                                        cycleId = 4;
                                    }
                                    else if (cycleCodeDel == intraDay3)
                                    {
                                       // bDateDel = new DateTime(bDateDel.Year, bDateDel.Month, bDateDel.Day, 22, 0, 0);
                                      //  eDateDel = new DateTime(eDateDel.Year, eDateDel.Month, eDateDel.Day, 9, 0, 0);
                                        cycleId = 5;
                                    }
                                    batchDel.FlowEndDate = eDateDel;
                                    batchDel.FlowStartDate = bDateDel;
                                    batchDel.CycleId = cycleId;
                                    batchDel.Description = "Bulk Upload - NonPathed Batch- Delivery";
                                    batchDel.NomSubCycle = Convert.ToString(row["Roll Nom1"]);
                                    batchDel.StatusID = (int)statusBatch.Draft;
                                    batchDel.SubmitDate = DateTime.MaxValue;
                                    batchDel.ScheduleDate = DateTime.MaxValue;

                                    

                                    //if (!string.IsNullOrEmpty(PipeDunsDel))
                                    //{
                                    //    pipeline = pipelineService.GetPipelineByDunsNo(PipeDunsDel);
                                    //}
                                    batchDel.PipelineDuns = PipeDunsDel;
                                    batchDel.ServiceRequester = shipperDuns;
                                    batchDel.ShowZeroCheck = false;
                                    batchDel.ShowZeroDn = false;
                                    batchDel.ShowZeroUp = false;
                                    batchDel.RankingCheck = false;
                                    batchDel.PakageCheck = false;
                                    batchDel.UpDnContractCheck = false;
                                    batchDel.UpDnPkgCheck = false;
                                    //Random ran = new Random((int)DateTime.Now.Ticks);
                                    batchDel.ReferenceNumber = ran.Next(999999999).ToString();
                                    string path1 = Path.GetRandomFileName();
                                    path1 = path1.Replace(".", "");
                                    batchDel.TransactionSetControlNumber = path1;
                                    batchDel.NomTypeID = (int)NomType.NonPathed;


                                    V4_Nomination nomDel = new V4_Nomination();
                                    decimal? fuel = null; int? delQ = null;
                                    nomDel.TransactionType = Convert.ToString(row["TransactionType1"]);
                                    nomDel.NominationSubCycleIndicator = Convert.ToString(row["Roll Nom1"]);
                                    nomDel.ContractNumber = Convert.ToString(row["Srv Req Contract1"]);
                                    if (!DBNull.Value.Equals(row["Fuel Percentage1"])) { fuel = Convert.ToDecimal(row["Fuel Percentage1"]); }

                                    nomDel.FuelPercentage = fuel;
                                    if (!DBNull.Value.Equals(row["Del Qty"])) { delQ = Convert.ToInt32(row["Del Qty"]); }
                                    nomDel.DelQuantity = delQ;
                                    nomDel.Quantity = 0;
                                    nomDel.DeliveryLocationIdentifer = Convert.ToString(row["Del Loc"]);
                                    nomDel.DeliveryLocationPropCode = Convert.ToString(row["Del Loc"]);

                                    nomDel.DownstreamIdentifier = Convert.ToString(row["Down Identifier"]);
                                    nomDel.DownstreamContractIdentifier = Convert.ToString(row["Down Contract"]);

                                    nomDel.DeliveryRank = Convert.ToString(row["Del Rank"]);
                                    nomDel.PackageId = Convert.ToString(row["Package Id1"]);
                                    nomDel.NominatorTrackingId = NomTrackingID(9, random);
                                    nomDel.PathType = "NPD";
                                    nomDel.UnitOfMeasure = "BZ";
                                    nomDel.QuantityTypeIndicator = "D";
                                    batchDel.Nominations.Add(nomDel);
                                    batchList.Add(batchDel);

                                    #endregion
                                }
                            }

                                
                            }
                            catch (Exception)
                            {
                                return Json("Something went wrong!! Please, Check in Non-Pathed tab.");
                            }
                            #endregion
                            InsertData(batchList);
                            if(accessedPipeduns.Count() > 0 && notAccessedPipeduns.Count() == 0)
                            {
                                successMsg = "All Noms uploaded successfully for pipeline DUNS " + string.Join("," + Environment.NewLine, accessedPipeduns.Distinct()) + ".";
                            }else if(notAccessedPipeduns.Distinct().ToList().Count == 1  && accessedPipeduns.Count > 0)
                            {
                                successMsg = "Noms uploaded successfully for pipeline DUNS " + string.Join("," + Environment.NewLine, accessedPipeduns.Distinct()) + "." + Environment.NewLine
                                    + "Noms Upload failed for pipeline DUNS " + string.Join("," + Environment.NewLine, notAccessedPipeduns.Distinct()) + " because you do not have access to this pipe";
                            }else if(notAccessedPipeduns.Distinct().ToList().Count == 1 && accessedPipeduns.Count == 0)
                            {
                                successMsg = "Noms Upload failed for pipeline DUNS " + string.Join("," + Environment.NewLine, notAccessedPipeduns.Distinct()) + " because you do not have access to this pipe";
                            }
                            else if (notAccessedPipeduns.Distinct().ToList().Count > 1 && accessedPipeduns.Count > 0)
                            {
                                successMsg = "Noms uploaded successfully for pipeline DUNS " + string.Join("," + Environment.NewLine, accessedPipeduns.Distinct()) + "." + Environment.NewLine
                                    + "Noms Upload failed for pipeline DUNS " + string.Join("," + Environment.NewLine, notAccessedPipeduns.Distinct()) + " because you do not have access to these pipes";
                            }
                            else if (notAccessedPipeduns.Distinct().ToList().Count > 1 && accessedPipeduns.Count == 0)
                            {
                                successMsg = "Noms Upload failed for pipeline DUNS " + string.Join("," + Environment.NewLine, notAccessedPipeduns.Distinct()) + " because you do not have access to these pipes";
                            }
                        }
                        else
                        {
                            //file is not with excel extension. 
                            return Json("file is not excel format.");
                        }
                        #endregion


                    }
                    else
                    {
                        return Json("file is empty");
                        // Empty file

                    }

                }               
                return Json(successMsg);
            }
            catch(Exception e)
            {               
                string text = string.IsNullOrEmpty(e.Message) ? "Something wrong!" : e.Message;
                return Json(text);
            }
            finally{
                if (excelConnection1 != null)
                {
                    excelConnection1.Close();
                }
                DeleteFile(fileLocation);
            }
            
        }

        public int? ApplyFormulasOnDelQty(int? RecQty, int? DelQty, decimal? FuelPercentage)
        {
            //  formula==>> Del Qty = Rec Qty * (1 - Fuel %)  
            int? resultQty = null;
            if (RecQty == null && DelQty == null)
            {
                resultQty = 0;
            }
            else if (RecQty == null && DelQty != null)
            {
                return DelQty;
            }
            else
            {
                resultQty =Convert.ToInt32(RecQty * (1 - (FuelPercentage / 100)));
            }
            return resultQty;
        }

        public int? ApplyFormulasOnRecQty(int? RecQty, int? DelQty, decimal? FuelPercentage)
        {
            //  formula==>> Del Qty = Rec Qty * (1 - Fuel %)  
            int? resultQty = null;
            if (RecQty != null) {
                return RecQty;
            }
            if (RecQty == null && DelQty == null)
            {
                resultQty = 0;
            }
            else if (RecQty == null && DelQty != null)
            {
                resultQty =Convert.ToInt32(DelQty / (1 - (FuelPercentage / 100)));
            }
            return resultQty;
        }


        private static string NomTrackingID(int Size, Random random)
        {          
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString().ToUpper();
        }


        public void InsertData(List<V4_Batch> batches)
        {
            
            uploadNominationService.AddBatches(batches);           
        }

       
       //[HttpPost]
       // public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, int Id)
       // {
       //     var timely = "TIM";
       //     var evening = "EVE";
       //     var intraDay1 = "ID1";
       //     var intraDay2 = "ID2";
       //     var intraDay3 = "ID3";
       //     var SONAT = "006900518";  // Sonat Duns. for Route
       //     var ElPaso = "008001703";  // El Paso Duns. for Route
       //     var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
       //     var PipelineId = Id;
       //     int emptyRowCount = 0;
       //     string UserID = identity.Claims.Where(c => c.Type == "UserId")
       //                        .Select(c => c.Value).SingleOrDefault();
       //     string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
       //                        .Select(c => c.Value).SingleOrDefault();
       //     string company = identity.Claims.Where(c => c.Type == "CompanyId")
       //                         .Select(c => c.Value).SingleOrDefault();

       //     int companyID = String.IsNullOrEmpty(company) ? 0 : int.Parse(company);
       //     using (MemoryStream stream = new MemoryStream())
       //     {
       //         try
       //         {
       //             foreach (HttpPostedFileBase file in files)
       //             {
       //                 int rowSkipped = 0;
       //                 dynamic workbook = null;
       //                 MemoryStream mem = new MemoryStream();
       //                 mem.SetLength((int)file.ContentLength);
       //                 file.InputStream.Read(mem.GetBuffer(), 0, (int)file.ContentLength);
       //                 if (Path.GetExtension(file.FileName) == ".xls")
       //                 {
       //                     workbook = new HSSFWorkbook(mem);
       //                 }
       //                 else if (Path.GetExtension(file.FileName) == ".xlsx")
       //                 {
       //                     workbook = new XSSFWorkbook(mem);
       //                 }
       //                 ISheet batchSheet = workbook.GetSheet("Batch");
       //                 ISheet supplysheet = workbook.GetSheet("Supply");
       //                 ISheet marketsheet = workbook.GetSheet("Market");
       //                 ISheet contractPathsheet = workbook.GetSheet("Contract Paths");
       //                 ISheet pathedSheet = workbook.GetSheet("Pathed");
       //                 ISheet nonPathedSheet = workbook.GetSheet("NonPathed");
       //                 #region
       //                 UploadedFile fileSave = new UploadedFile();
       //                 fileSave.AddedBy = UserID;
       //                 fileSave.CreatedDate = DateTime.Now;
       //                 fileSave.FileBytes = mem.GetBuffer();
       //                 fileSave.FileName = file.FileName;
       //                 fileSave.PipelineId = 0;
       //                 uploadNominationService.SaveUploadedFile(fileSave);
       //                 #endregion
       //                 #region Validations On Sheet PNT and pathed(check the column sequence)
       //                 var batchColsList = GetColsNameWithoutEmpty(batchSheet);
       //                 string batchInOrder = IsBatchColInSequence(batchColsList);
       //                 if (!string.IsNullOrEmpty(batchInOrder))
       //                 {
       //                     return Json("Error: " + batchInOrder);
       //                 }

       //                 var supplycolsList = GetColsNameWithoutEmpty(supplysheet);
       //                 string isInOrdered = IsSupplyColsInSquence(supplycolsList);
       //                 if (!string.IsNullOrEmpty(isInOrdered))
       //                 {
       //                     return Json("Error:" + isInOrdered);
       //                 }

       //                 var marketcolsList = GetColsNameWithoutEmpty(marketsheet);
       //                 string marketInOrdered = IsMarketColsInSquence(marketcolsList);
       //                 if (!string.IsNullOrEmpty(marketInOrdered))
       //                 {
       //                     return Json("Error:" + marketInOrdered);
       //                 }

       //                 var contractcolsList = GetColsNameWithoutEmpty(contractPathsheet);
       //                 string contractInOrdered = IsContractPathColsInSquence(contractcolsList);
       //                 if (!string.IsNullOrEmpty(contractInOrdered))
       //                 {
       //                     return Json("Error:" + contractInOrdered);
       //                 }

       //                 #region Pathed validation
       //                 // Pathed validation                      
       //                 var colsList = GetColsNameWithoutEmpty(pathedSheet);
       //                 string InOrdered = IsPathedColsInSquence(colsList);
       //                 if (!string.IsNullOrEmpty(InOrdered))
       //                 {
       //                     return Json("Error:" + InOrdered);
       //                 }
       //                 // Pathed validation ---End.   
       //                 #endregion
       //                 #region non pathed 
       //                 var nonPathedColsList = GetColsNameWithoutEmpty(nonPathedSheet);
       //                 string nonPathedColsInOrder = IsNonPathedColsInSequence(nonPathedColsList);
       //                 if (!string.IsNullOrEmpty(nonPathedColsInOrder))
       //                 {
       //                     return Json("Error: " + nonPathedColsInOrder);
       //                 }
       //                 #endregion
       //                 #endregion

       //                 #region Pnt nom DTO's
       //                 List<BatchDetailDTO> batchList = new List<BatchDetailDTO>();
       //                 List<BatchDetailSupplyDTO> supplyList = new List<BatchDetailSupplyDTO>();
       //                 List<BatchDetailContractPathDTO> contractPathList = new List<BatchDetailContractPathDTO>();
       //                 List<BatchDetailContractDTO> contractList = new List<BatchDetailContractDTO>();
       //                 List<BatchDetailMarketDTO> marketList = new List<BatchDetailMarketDTO>();
       //                 #endregion
       //                 #region Pathed nom DTO's
       //                 List<PathedDTO> pathedDTOList = new List<PathedDTO>();
       //                 #endregion
       //                 #region NonPathed nom DTO's
       //                 List<NonPathedDTO> nonPathedDtoList = new List<NonPathedDTO>();
       //                 List<NonPathedRecieptNom> nonPathedRecNomsList = new List<NonPathedRecieptNom>();
       //                 List<NonPathedDeliveryNom> nonPathedDelNomList = new List<NonPathedDeliveryNom>();
       //                 #endregion
       //                 #region PNT nom data extract from excel and fill DTO
       //                 for (int row = 1; row <= 251; row++)
       //                 {
       //                     if (batchSheet.GetRow(row) == null || batchSheet.GetRow(row).GetCell(0) == null || string.IsNullOrEmpty(batchSheet.GetRow(row).GetCell(0).ToString()))
       //                         continue;
       //                     var CycleId = 1;
       //                     var SDateTime = DateTime.Now;
       //                     var EDateTime = DateTime.Now;
       //                     PipelineDTO pipe = null;
       //                     var startDate = batchSheet.GetRow(row).GetCell(0) != null ? (batchSheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || batchSheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? DateTime.Parse(batchSheet.GetRow(row).GetCell(0).DateCellValue.ToString()) : DateTime.Parse(batchSheet.GetRow(row).GetCell(0).StringCellValue) : new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 9, 0, 0);
       //                     var endDate = batchSheet.GetRow(row).GetCell(1) != null ? (batchSheet.GetRow(row).GetCell(1).CellType == CellType.Numeric || batchSheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? DateTime.Parse(batchSheet.GetRow(row).GetCell(1).DateCellValue.ToString()) : DateTime.Parse(batchSheet.GetRow(row).GetCell(1).StringCellValue) : new DateTime(DateTime.Now.AddDays(3).Year, DateTime.Now.AddDays(3).Month, DateTime.Now.AddDays(3).Day, 9, 0, 0);
       //                     var CycleCode = batchSheet.GetRow(row).GetCell(2) != null ? batchSheet.GetRow(row).GetCell(2).StringCellValue : timely;
       //                     var PipeDuns = batchSheet.GetRow(row).GetCell(3) != null ? (batchSheet.GetRow(row).GetCell(3).CellType == CellType.Numeric) ? batchSheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : batchSheet.GetRow(row).GetCell(3).StringCellValue : string.Empty;
       //                     if (!string.IsNullOrEmpty(PipeDuns))
       //                     {
       //                         pipe = pipelineService.GetPipelineByDunsNo(PipeDuns);
       //                     }
       //                     if (CycleCode == timely)
       //                     {
       //                         SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
       //                         EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
       //                         CycleId = 1;
       //                     }
       //                     else if (CycleCode == evening)
       //                     {
       //                         SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
       //                         EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
       //                         CycleId = 2;
       //                     }
       //                     else if (CycleCode == intraDay1)
       //                     {
       //                         SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 14, 0, 0);
       //                         EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
       //                         CycleId = 3;
       //                     }
       //                     else if (CycleCode == intraDay2)
       //                     {
       //                         SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 18, 0, 0);
       //                         EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
       //                         CycleId = 4;
       //                     }
       //                     else if (CycleCode == intraDay3)
       //                     {
       //                         SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 22, 0, 0);
       //                         EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
       //                         CycleId = 5;
       //                     }

       //                     BatchDetailDTO batch = new BatchDetailDTO();
       //                     batch.SupplyList = new List<BatchDetailSupplyDTO>();
       //                     batch.Contract = new List<BatchDetailContractDTO>();
       //                     batch.ContractPath = new List<BatchDetailContractPathDTO>();
       //                     batch.MarketList = new List<BatchDetailMarketDTO>();
       //                     batch.BatchStatus = "";
       //                     batch.CreatedBy = identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault();
       //                     batch.CreatedDateTime = DateTime.UtcNow;
       //                     batch.CurrentContractRow = 0;
       //                     batch.CurrentSupplyRow = 0;
       //                     batch.Description = "Excel Upload " + DateTime.Now;
       //                     batch.Duns = pipe != null ? pipe.DUNSNo : string.Empty;
       //                     batch.IsPNT = true;
       //                     batch.PakageCheck = true;
       //                     batch.PipelineId = pipe != null ? pipe.ID : 0;
       //                     batch.PipeLineName = "";
       //                     batch.RankingCheck = true;
       //                     batch.ScheduleDate = DateTime.MaxValue;
       //                     batch.ShiperDuns = shipperDuns;
       //                     batch.ShowZeroCheck = true;
       //                     batch.ShowZeroDn = false;
       //                     batch.ShowZeroUp = false;
       //                     batch.StatusId = 11;
       //                     batch.SubmittedDate = DateTime.MaxValue;
       //                     batch.UpDnContractCheck = false;
       //                     batch.UpDnPkgCheck = false;
       //                     batch.ShipperCompanyId = companyID;
       //                     batch.StartDateTime = SDateTime;
       //                     batch.EndDateTime = EDateTime;//AddDays(1);
       //                     batch.CycleId = CycleId;
       //                     batchList.Add(batch);
       //                 }
       //                 int supEmpRow = 0;
       //                 for (int row = 1; row <= 251; row++)
       //                 {
       //                     #region Supply Part Declarations   
       //                     var ReceiptLoc = supplysheet.GetRow(row).GetCell(0) != null ? (supplysheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || (supplysheet.GetRow(row).GetCell(0).CellType == CellType.Formula && supplysheet.GetRow(row).GetCell(0).CachedFormulaResultType == CellType.Numeric)) ? supplysheet.GetRow(row).GetCell(0).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : string.Empty;

       //                     if (string.IsNullOrEmpty(ReceiptLoc))
       //                     {
       //                         rowSkipped++;
       //                         if (supEmpRow > 251)
       //                             break;
       //                         supEmpRow++;
       //                         continue;
       //                     }
       //                     var TransactionType = supplysheet.GetRow(row).GetCell(1) != null ? (supplysheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || (supplysheet.GetRow(row).GetCell(1).CellType == CellType.Formula && supplysheet.GetRow(row).GetCell(1).CachedFormulaResultType == CellType.Numeric)) ? supplysheet.GetRow(row).GetCell(1).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim() : string.Empty;
       //                     //supplysheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim() : string.Empty;
       //                     var SvcReq = supplysheet.GetRow(row).GetCell(2) != null ? (supplysheet.GetRow(row).GetCell(2).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(2).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(2).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(SvcReq))
       //                     {
       //                         rowSkipped++;
       //                         if (supEmpRow > 10)
       //                             break;
       //                         supEmpRow++;
       //                         continue;
       //                     }
       //                     var FuelPercentage = supplysheet.GetRow(row).GetCell(3) != null ? (supplysheet.GetRow(row).GetCell(3).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(3).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                     var UpIdProp = supplysheet.GetRow(row).GetCell(4) != null ? (supplysheet.GetRow(row).GetCell(4).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(4).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
       //                     var upId = supplysheet.GetRow(row).GetCell(5) != null ? (supplysheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
       //                     var RecQuantity = supplysheet.GetRow(row).GetCell(6) != null ? (supplysheet.GetRow(row).GetCell(6).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(6).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
       //                     var UpDelRank = supplysheet.GetRow(row).GetCell(7) != null ? (supplysheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PkgId = supplysheet.GetRow(row).GetCell(8) != null ? supplysheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                     var UpContractNo = supplysheet.GetRow(row).GetCell(9) != null ? (supplysheet.GetRow(row).GetCell(9).CellType == CellType.Numeric || supplysheet.GetRow(row).GetCell(9).CellType == CellType.Formula) ? supplysheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PipeDuns = supplysheet.GetRow(row).GetCell(11) != null ? (supplysheet.GetRow(row).GetCell(11).CellType == CellType.Numeric) ? supplysheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : supplysheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;

       //                     #region Filter Strings

       //                     PkgId = GetFilteredString(PkgId);
       //                     TransactionType = GetFilteredString(TransactionType);
       //                     SvcReq = GetFilteredString(SvcReq);
       //                     UpContractNo = GetFilteredString(UpContractNo);
       //                     UpDelRank = GetFilteredString(UpDelRank);
       //                     ReceiptLoc = GetFilteredString(ReceiptLoc);
       //                     RecQuantity = GetFilteredString(RecQuantity);
       //                     upId = GetFilteredString(upId);
       //                     UpIdProp = GetFilteredString(UpIdProp);
       //                     PipeDuns = GetFilteredString(PipeDuns);
       //                     FuelPercentage = GetFilteredString(FuelPercentage);
       //                     #endregion
       //                     if (String.IsNullOrEmpty(upId) || string.IsNullOrWhiteSpace(upId))
       //                     {
       //                         if (!string.IsNullOrEmpty(UpIdProp) && !string.IsNullOrWhiteSpace(UpIdProp))
       //                         {
       //                             upId = UpIdProp;
       //                         }
       //                     }
       //                     #region
       //                     BatchDetailSupplyDTO supply = new BatchDetailSupplyDTO();
       //                     supply.CreatedBy = Guid.Empty;
       //                     supply.CreatedDate = DateTime.Now;
       //                     supply.DeliveryQuantityNet = Convert.ToInt32(RecQuantity);
       //                     supply.FuelPercentage = FuelPercentage;
       //                     supply.FuelQunatity = "";
       //                     supply.IsActive = true;
       //                     supply.Location = ReceiptLoc;
       //                     supply.LocationProp = ReceiptLoc;
       //                     supply.PackageID = PkgId;
       //                     supply.ReceiptQuantityGross = Convert.ToInt32(RecQuantity);
       //                     supply.ServiceRequestNo = SvcReq;
       //                     supply.ServiceRequestType = "";
       //                     supply.TransactionType = TransactionType;
       //                     supply.TransactionTypeDescription = "";
       //                     supply.UpContractIdentifier = UpContractNo;
       //                     supply.UpPackageID = "";
       //                     supply.UpstreamID = upId;
       //                     supply.UpstreamIDName = "";
       //                     supply.UpstreamIDProp = UpIdProp;
       //                     supply.UpstreamRank = UpDelRank;
       //                     supply.PipeDuns = PipeDuns;
       //                     supplyList.Add(supply);

       //                     #endregion
       //                     #endregion
       //                 }
       //                 int conEmpRow = 0;
       //                 for (int row = 1; row <= 251; row++)
       //                 {
       //                     #region Transport Part Declarations
       //                     var SvcReq = contractPathsheet.GetRow(row).GetCell(0) != null ? (contractPathsheet.GetRow(row).GetCell(0).CellType == CellType.Numeric) ? contractPathsheet.GetRow(row).GetCell(0).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(SvcReq))
       //                     {
       //                         rowSkipped++;
       //                         if (conEmpRow > 251)
       //                             break;
       //                         conEmpRow++;
       //                         continue;
       //                     }
       //                     var FuelPercentage = contractPathsheet.GetRow(row).GetCell(1) != null ? (contractPathsheet.GetRow(row).GetCell(1).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(1).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim() : string.Empty;
       //                     var TT = contractPathsheet.GetRow(row).GetCell(2) != null ? (contractPathsheet.GetRow(row).GetCell(2).CellType == CellType.Numeric) ? contractPathsheet.GetRow(row).GetCell(2).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim() : string.Empty;

       //                     var RecLocProp = contractPathsheet.GetRow(row).GetCell(3) != null ? (contractPathsheet.GetRow(row).GetCell(3).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(3).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(RecLocProp))
       //                         continue;
       //                     var RecDnRank = contractPathsheet.GetRow(row).GetCell(4) != null ? (contractPathsheet.GetRow(row).GetCell(4).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(4).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
       //                     var RecQty = contractPathsheet.GetRow(row).GetCell(5) != null ? (contractPathsheet.GetRow(row).GetCell(5).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(5).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DelLocProp = contractPathsheet.GetRow(row).GetCell(6) != null ? (contractPathsheet.GetRow(row).GetCell(6).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(6).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(DelLocProp))
       //                     {
       //                         rowSkipped++;
       //                         if (conEmpRow > 251)
       //                             break;
       //                         conEmpRow++;
       //                         continue;
       //                     }
       //                     var UpDelRank = contractPathsheet.GetRow(row).GetCell(7) != null ? (contractPathsheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DelQty = contractPathsheet.GetRow(row).GetCell(8) != null ? (contractPathsheet.GetRow(row).GetCell(8).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(8).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PathRank = contractPathsheet.GetRow(row).GetCell(9) != null ? (contractPathsheet.GetRow(row).GetCell(9).CellType == CellType.Numeric || contractPathsheet.GetRow(row).GetCell(9).CellType == CellType.Formula) ? contractPathsheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PkgId = contractPathsheet.GetRow(row).GetCell(10) != null ? (contractPathsheet.GetRow(row).GetCell(10).CellType == CellType.Numeric) ? contractPathsheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;

       //                     var Route = contractPathsheet.GetRow(row).GetCell(12) != null ? (contractPathsheet.GetRow(row).GetCell(12).CellType == CellType.Numeric) ? contractPathsheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(12).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PipeDuns = contractPathsheet.GetRow(row).GetCell(13) != null ? (contractPathsheet.GetRow(row).GetCell(13).CellType == CellType.Numeric) ? contractPathsheet.GetRow(row).GetCell(13).NumericCellValue.ToString().Trim() : contractPathsheet.GetRow(row).GetCell(13).StringCellValue.ToString().Trim() : string.Empty;


       //                     #region Filter Strings

       //                     PkgId = GetFilteredString(PkgId);
       //                     SvcReq = GetFilteredString(SvcReq);
       //                     TT = GetFilteredString(TT);
       //                     RecLocProp = GetFilteredString(RecLocProp);
       //                     RecDnRank = GetFilteredString(RecDnRank);
       //                     RecQty = GetFilteredString(RecQty);
       //                     DelLocProp = GetFilteredString(DelLocProp);
       //                     UpDelRank = GetFilteredString(UpDelRank);
       //                     DelQty = GetFilteredString(DelQty);
       //                     PathRank = GetFilteredString(PathRank);
       //                     Route = GetFilteredString(Route);
       //                     PipeDuns = GetFilteredString(PipeDuns);
       //                     FuelPercentage = GetFilteredString(FuelPercentage);
       //                     #endregion
       //                     BatchDetailContractPathDTO contractPath = new BatchDetailContractPathDTO();
       //                     contractPath.CreatedDate = DateTime.Now;
       //                     contractPath.ServiceRequestNo = SvcReq;
       //                     contractPathList.Add(contractPath);

       //                     BatchDetailContractDTO transport = new BatchDetailContractDTO();
       //                     transport.CreatedDate = DateTime.Now;
       //                     transport.DeliveryDth = RecQty;
       //                     transport.DelLocation = DelLocProp;
       //                     transport.DelLocationProp = DelLocProp;
       //                     transport.ServiceRequestNo = SvcReq;
       //                     transport.DelRank = UpDelRank;
       //                     transport.DelZone = "";
       //                     transport.FuelDth = "";
       //                     transport.FuelPercentage = FuelPercentage;
       //                     //transport.ID
       //                     transport.IsActive = true;
       //                     transport.PackageID = PkgId;
       //                     transport.PathRank = PathRank;
       //                     transport.ReceiptDth = RecQty;
       //                     transport.RecLocation = RecLocProp;
       //                     transport.RecLocationProp = RecLocProp;
       //                     transport.RecRank = RecDnRank;
       //                     transport.RecZone = "";
       //                     transport.TransactionType = TT;
       //                     transport.TransactionTypeDescription = "";
       //                     transport.PipeDuns = PipeDuns;
       //                     // Only for SONAT
       //                     if (PipeDuns == SONAT || PipeDuns == ElPaso)
       //                     {
       //                         transport.Route = Route;
       //                     }
       //                     else
       //                     {
       //                         transport.Route = string.Empty;
       //                     }
       //                     contractList.Add(transport);
       //                     #endregion
       //                 }
       //                 int markEmpRow = 0;
       //                 for (int row = 1; row <= 251; row++)
       //                 {
       //                     #region Market Declarations
       //                     var DelLocProp = marketsheet.GetRow(row).GetCell(0) != null ? (marketsheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(0).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(DelLocProp))
       //                     {
       //                         rowSkipped++;
       //                         if (markEmpRow > 251)
       //                             break;
       //                         markEmpRow++;
       //                         continue;
       //                     }
       //                     var TT = marketsheet.GetRow(row).GetCell(1) != null ? (marketsheet.GetRow(row).GetCell(1).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(1).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim() : string.Empty;
       //                     var SvcReq = marketsheet.GetRow(row).GetCell(2) != null ? (marketsheet.GetRow(row).GetCell(2).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(2).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(2).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (string.IsNullOrEmpty(SvcReq))
       //                     {
       //                         rowSkipped++;
       //                         if (markEmpRow > 251)
       //                             break;
       //                         markEmpRow++;
       //                         continue;
       //                     }
       //                     var FuelPercentage = marketsheet.GetRow(row).GetCell(3) != null ? (marketsheet.GetRow(row).GetCell(3).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(3).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DwIdProp = marketsheet.GetRow(row).GetCell(4) != null ? (marketsheet.GetRow(row).GetCell(4).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(4).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DwId = marketsheet.GetRow(row).GetCell(5) != null ? (marketsheet.GetRow(row).GetCell(5).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(5).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DelQty = marketsheet.GetRow(row).GetCell(6) != null ? (marketsheet.GetRow(row).GetCell(6).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(6).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
       //                     var DnRank = marketsheet.GetRow(row).GetCell(7) != null ? (marketsheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PkId = marketsheet.GetRow(row).GetCell(8) != null ? (marketsheet.GetRow(row).GetCell(8).CellType == CellType.Numeric) ? marketsheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                     var UpDwContract = marketsheet.GetRow(row).GetCell(9) != null ? (marketsheet.GetRow(row).GetCell(9).CellType == CellType.Numeric || marketsheet.GetRow(row).GetCell(9).CellType == CellType.Formula) ? marketsheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
       //                     var PipeDuns = marketsheet.GetRow(row).GetCell(11) != null ? (marketsheet.GetRow(row).GetCell(11).CellType == CellType.Numeric) ? marketsheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : marketsheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;

       //                     #region Filter Strings
       //                     PkId = GetFilteredString(PkId);
       //                     SvcReq = GetFilteredString(SvcReq);
       //                     TT = GetFilteredString(TT);
       //                     DelLocProp = GetFilteredString(DelLocProp);
       //                     DnRank = GetFilteredString(DnRank);
       //                     DelQty = GetFilteredString(DelQty);
       //                     DwIdProp = GetFilteredString(DwIdProp);
       //                     DwId = GetFilteredString(DwId);
       //                     UpDwContract = GetFilteredString(UpDwContract);
       //                     PipeDuns = GetFilteredString(PipeDuns);
       //                     FuelPercentage = GetFilteredString(FuelPercentage);
       //                     #endregion
       //                     if (String.IsNullOrEmpty(DwId) || string.IsNullOrWhiteSpace(DwId))
       //                     {
       //                         if (!string.IsNullOrEmpty(DwIdProp) && !string.IsNullOrWhiteSpace(DwIdProp))
       //                         {
       //                             DwId = DwIdProp;
       //                         }
       //                     }
       //                     BatchDetailMarketDTO market = new BatchDetailMarketDTO();
       //                     market.CreatedDate = DateTime.Now;
       //                     market.DeliveryQuantityNet = 0;
       //                     market.DnContractIdentifier = UpDwContract;
       //                     market.DnPackageID = "";
       //                     market.DnstreamRank = DnRank;
       //                     market.DownstreamID = DwId;
       //                     market.DownstreamIDName = "";
       //                     market.DownstreamIDProp = DwIdProp;
       //                     market.FuelPercentage = FuelPercentage;
       //                     market.FuelQunatity = "";
       //                     market.IsActive = true;
       //                     market.Location = DelLocProp;
       //                     market.LocationProp = DelLocProp;
       //                     market.PackageID = PkId;
       //                     market.ReceiptQuantityGross = 0;
       //                     market.ServiceRequestNo = SvcReq;
       //                     market.TransactionType = TT;
       //                     market.TransactionTypeDescription = "";
       //                     market.ReceiptQuantityGross = Convert.ToInt32(DelQty);
       //                     market.DeliveryQuantityNet = Convert.ToInt32(DelQty);
       //                     market.PipeDuns = PipeDuns;
       //                     marketList.Add(market);
       //                     #endregion
       //                 }
       //                 #region add supply, market, contract to related batch
       //                 if (batchList.Count() > 0)
       //                 {
       //                     foreach (var supply in supplyList)
       //                     {
       //                         var relBatchList = batchList.Where(a => a.Duns == supply.PipeDuns).ToList();
       //                         if (relBatchList == null && relBatchList.Count <= 0)
       //                         {
       //                             return Json("In Supply Tab, the pipeline duns you have entered does not exist.");
       //                         }
       //                         foreach (var batch in relBatchList)
       //                         {
       //                             batch.SupplyList.Add(supply);
       //                             batchList.Remove(batchList.Where(a => a.Duns == batch.Duns && a.StartDateTime == batch.StartDateTime && a.EndDateTime == batch.EndDateTime).FirstOrDefault());
       //                             batchList.Add(batch);
       //                         }

       //                     }
       //                     foreach (var contract in contractList)
       //                     {
       //                         var relBatchList = batchList.Where(a => a.Duns == contract.PipeDuns).ToList();
       //                         if (relBatchList == null && relBatchList.Count < 0)
       //                         {
       //                             return Json("In Contract Tab, the pipeline duns you have entered does not exist.");
       //                         }
       //                         foreach (var batch in relBatchList)
       //                         {
       //                             batch.Contract.Add(contract);
       //                             batchList.Remove(batchList.Where(a => a.Duns == batch.Duns && a.StartDateTime == batch.StartDateTime && a.EndDateTime == batch.EndDateTime).FirstOrDefault());
       //                             batchList.Add(batch);
       //                         }
       //                     }
       //                     foreach (var market in marketList)
       //                     {
       //                         var relBatchList = batchList.Where(a => a.Duns == market.PipeDuns).ToList();
       //                         if (relBatchList == null && relBatchList.Count <= 0)
       //                         {
       //                             return Json("In Market Tab, the pipeline duns you have entered does not exist.");
       //                         }
       //                         foreach (var batch in relBatchList)
       //                         {
       //                             batch.MarketList.Add(market);
       //                             batchList.Remove(batchList.Where(a => a.Duns == batch.Duns && a.StartDateTime == batch.StartDateTime && a.EndDateTime == batch.EndDateTime).FirstOrDefault());
       //                             batchList.Add(batch);
       //                         }
       //                     }
       //                 }
       //                 #endregion
       //                 #endregion
       //                 #region Pathed nom data extract from excel and fill DTO
       //                 for (int row = 1; row <= 251; row++)
       //                 {
       //                     #region Pathed Nomination
       //                     bool isrowempty = false;
       //                     #region Empty row Validation
       //                     var rowofTable = pathedSheet.GetRow(row);
       //                     isrowempty = IsEmptyRow(rowofTable);
       //                     if (isrowempty)
       //                     {
       //                         emptyRowCount++;
       //                         continue;
       //                     }
       //                     #endregion

       //                     if (pathedSheet.GetRow(row) != null)
       //                     {
       //                         #region Pathed 

       //                         string UpK = string.Empty;
       //                         string SvcReq = string.Empty;
       //                         if (pathedSheet.GetRow(row).GetCell(1) != null)
       //                         {
       //                             var BegDate = pathedSheet.GetRow(row).GetCell(0) != null
       //                                 ? (pathedSheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(0).DateCellValue.ToString() : (pathedSheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(0).DateCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim()
       //                                 : string.Empty;
       //                             //var begDate = pathedSheet.GetRow(row).GetCell(0) != null ? (pathedSheet.GetRow(row).GetCell(0).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(0).NumericCellValue.ToString(): (pathedSheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim(): string.Empty;
       //                             //var BegDate = pathedSheet.GetRow(row).GetCell(0) != null ? pathedSheet.GetRow(row).GetCell(0).RichStringCellValue.ToString().Trim() : string.Empty;
       //                             DateTime bDate;
       //                             if (string.IsNullOrEmpty(BegDate))
       //                             {
       //                                 continue;
       //                             }
       //                             bDate = DateTime.Parse(BegDate);
       //                             var EndDate = pathedSheet.GetRow(row).GetCell(1) != null
       //                                 ? (pathedSheet.GetRow(row).GetCell(1).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(1).DateCellValue.ToString() : (pathedSheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(1).DateCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim()
       //                                 : string.Empty;
       //                             //var EndDate = pathedSheet.GetRow(row).GetCell(1) != null ? (pathedSheet.GetRow(row).GetCell(1).RichStringCellValue.ToString().Trim()) : string.Empty;
       //                             if (string.IsNullOrEmpty(EndDate))
       //                                 return Json("End Date missing in Pathed tab.");
       //                             DateTime eDate = DateTime.Parse(EndDate);
       //                             var cycleCode = pathedSheet.GetRow(row).GetCell(2) != null ? (pathedSheet.GetRow(row).GetCell(2).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(2).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim() : string.Empty;
       //                             int cycleId = 0;

       //                             if (cycleCode == timely)
       //                             {
       //                                 bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
       //                                 eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
       //                                 cycleId = 1;
       //                             }
       //                             else if (cycleCode == evening)
       //                             {
       //                                 bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 9, 0, 0);
       //                                 eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
       //                                 cycleId = 2;
       //                             }
       //                             else if (cycleCode == intraDay1)
       //                             {
       //                                 bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 14, 0, 0);
       //                                 eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
       //                                 cycleId = 3;
       //                             }
       //                             else if (cycleCode == intraDay2)
       //                             {
       //                                 bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 18, 0, 0);
       //                                 eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
       //                                 cycleId = 4;
       //                             }
       //                             else if (cycleCode == intraDay3)
       //                             {
       //                                 bDate = new DateTime(bDate.Year, bDate.Month, bDate.Day, 22, 0, 0);
       //                                 eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, 9, 0, 0);
       //                                 cycleId = 5;
       //                             }

       //                             if (pathedSheet.GetRow(row).GetCell(3) != null && pathedSheet.GetRow(row).GetCell(3).CellType == CellType.Formula)
       //                             {
       //                                 SvcReq = pathedSheet.GetRow(row).GetCell(3) != null ? (pathedSheet.GetRow(row).GetCell(3).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 SvcReq = pathedSheet.GetRow(row).GetCell(3) != null ? (pathedSheet.GetRow(row).GetCell(3).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(3).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var FuelPercentage = pathedSheet.GetRow(row).GetCell(4) != null ? (pathedSheet.GetRow(row).GetCell(4).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(4).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
       //                             var NomSubCycle = pathedSheet.GetRow(row).GetCell(5) != null ? pathedSheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
       //                             var ActCD = pathedSheet.GetRow(row).GetCell(6) != null ? pathedSheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
       //                             var TT = pathedSheet.GetRow(row).GetCell(7) != null ? (pathedSheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
       //                             var RecLoc2 = "";
       //                             if (pathedSheet.GetRow(row).GetCell(8) != null && pathedSheet.GetRow(row).GetCell(8).CellType == CellType.Formula)
       //                             {
       //                                 RecLoc2 = pathedSheet.GetRow(row).GetCell(8) != null ? (pathedSheet.GetRow(row).GetCell(8).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 RecLoc2 = pathedSheet.GetRow(row).GetCell(8) != null ? (pathedSheet.GetRow(row).GetCell(8).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(8).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var UpIdProp = pathedSheet.GetRow(row).GetCell(9) != null ? (pathedSheet.GetRow(row).GetCell(9).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(9).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(9).NumericCellValue.ToString() : pathedSheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
       //                             var UpId = pathedSheet.GetRow(row).GetCell(10) != null ? (pathedSheet.GetRow(row).GetCell(10).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(10).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;
       //                             if (pathedSheet.GetRow(row).GetCell(11) != null && pathedSheet.GetRow(row).GetCell(11).CellType == CellType.Formula)
       //                             {
       //                                 UpK = pathedSheet.GetRow(row).GetCell(11) != null ? (pathedSheet.GetRow(row).GetCell(11).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 UpK = pathedSheet.GetRow(row).GetCell(11) != null ? (pathedSheet.GetRow(row).GetCell(11).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var RecQty = pathedSheet.GetRow(row).GetCell(12) != null ? pathedSheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : "0";
       //                             var RecRank = pathedSheet.GetRow(row).GetCell(13) != null ? (pathedSheet.GetRow(row).GetCell(13).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(13).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(13).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(13).StringCellValue.ToString().Trim() : string.Empty;
       //                             var DelLoc = "";
       //                             if (pathedSheet.GetRow(row).GetCell(14) != null && pathedSheet.GetRow(row).GetCell(14).CellType == CellType.Formula)
       //                             {
       //                                 DelLoc = pathedSheet.GetRow(row).GetCell(14) != null ? (pathedSheet.GetRow(row).GetCell(14).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(14).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(14).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 DelLoc = pathedSheet.GetRow(row).GetCell(14) != null ? (pathedSheet.GetRow(row).GetCell(14).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(14).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(14).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var DnIdPrp = pathedSheet.GetRow(row).GetCell(15) != null ? (pathedSheet.GetRow(row).GetCell(15).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(15).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(15).NumericCellValue.ToString() : pathedSheet.GetRow(row).GetCell(15).StringCellValue.ToString().Trim() : string.Empty;
       //                             var DnId = pathedSheet.GetRow(row).GetCell(16) != null ? (pathedSheet.GetRow(row).GetCell(16).CellType == CellType.Numeric || pathedSheet.GetRow(row).GetCell(16).CellType == CellType.Formula) ? pathedSheet.GetRow(row).GetCell(16).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(16).StringCellValue.ToString().Trim() : string.Empty;
       //                             var DnK = "";
       //                             if (pathedSheet.GetRow(row).GetCell(17) != null && pathedSheet.GetRow(row).GetCell(17).CellType == CellType.Formula)
       //                             {
       //                                 DnK = pathedSheet.GetRow(row).GetCell(17) != null ? (pathedSheet.GetRow(row).GetCell(17).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(17).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(17).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 DnK = pathedSheet.GetRow(row).GetCell(17) != null ? (pathedSheet.GetRow(row).GetCell(17).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(17).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(17).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var DelQty = "";
       //                             if (pathedSheet.GetRow(row).GetCell(18) != null && pathedSheet.GetRow(row).GetCell(18).CellType == CellType.Formula)
       //                             {
       //                                 DelQty = pathedSheet.GetRow(row).GetCell(18) != null ? (pathedSheet.GetRow(row).GetCell(18).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(18).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(18).StringCellValue.ToString().Trim() : "0";
       //                             }
       //                             else
       //                             {
       //                                 DelQty = pathedSheet.GetRow(row).GetCell(18) != null ? (pathedSheet.GetRow(row).GetCell(18).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(18).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(18).StringCellValue.ToString().Trim() : "0";
       //                             }
       //                             var DelRank = "";
       //                             if (pathedSheet.GetRow(row).GetCell(19) != null && pathedSheet.GetRow(row).GetCell(19).CellType == CellType.Formula)
       //                             {
       //                                 DelRank = pathedSheet.GetRow(row).GetCell(19) != null ? (pathedSheet.GetRow(row).GetCell(19).CachedFormulaResultType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(19).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(19).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             else
       //                             {
       //                                 DelRank = pathedSheet.GetRow(row).GetCell(19) != null ? (pathedSheet.GetRow(row).GetCell(19).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(19).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(19).StringCellValue.ToString().Trim() : string.Empty;
       //                             }
       //                             var BegTime = "";
       //                             var PkgId = pathedSheet.GetRow(row).GetCell(20) != null ? pathedSheet.GetRow(row).GetCell(20).StringCellValue.ToString().Trim() : string.Empty;
       //                             var PipeDuns = pathedSheet.GetRow(row).GetCell(21) != null ? (pathedSheet.GetRow(row).GetCell(21).CellType == CellType.Numeric) ? pathedSheet.GetRow(row).GetCell(21).NumericCellValue.ToString().Trim() : pathedSheet.GetRow(row).GetCell(21).StringCellValue.ToString().Trim() : string.Empty;
       //                             //var TranTypeDesc = pathedSheet.GetRow(row).GetCell(22) != null ? pathedSheet.GetRow(row).GetCell(22).StringCellValue.ToString().Trim() : string.Empty;
       //                             #region String filtered
       //                             BegDate = GetFilteredString(BegDate);
       //                             EndDate = GetFilteredString(EndDate);
       //                             SvcReq = GetFilteredString(SvcReq);
       //                             NomSubCycle = GetFilteredString(NomSubCycle);
       //                             ActCD = GetFilteredString(ActCD);
       //                             TT = GetFilteredString(TT);
       //                             RecLoc2 = GetFilteredString(RecLoc2);
       //                             UpIdProp = GetFilteredString(UpIdProp);
       //                             UpId = GetFilteredString(UpId);
       //                             UpK = GetFilteredString(UpK);
       //                             //UpActCd = GetFilteredString(UpActCd);
       //                             //UpTT = GetFilteredString(UpTT);
       //                             RecQty = GetFilteredString(RecQty);
       //                             RecRank = GetFilteredString(RecRank);
       //                             DelLoc = GetFilteredString(DelLoc);
       //                             DnIdPrp = GetFilteredString(DnIdPrp);
       //                             DnId = GetFilteredString(DnId);
       //                             DnK = GetFilteredString(DnK);
       //                             DelQty = GetFilteredString(DelQty);
       //                             DelRank = GetFilteredString(DelRank);
       //                             BegTime = GetFilteredString(BegTime);
       //                             PkgId = GetFilteredString(PkgId);
       //                             PipeDuns = GetFilteredString(PipeDuns);
       //                             FuelPercentage = GetFilteredString(FuelPercentage);
       //                             CounterPartiesDTO counUp = null;
       //                             if (!string.IsNullOrEmpty(UpIdProp))
       //                                 counUp = counterPartyService.GetCounterPartyByPropCode(UpIdProp);
       //                             else if (!string.IsNullOrEmpty(UpId))
       //                                 counUp = counterPartyService.GetCounterPartyByIdentifier(UpId);

       //                             CounterPartiesDTO counDn = null;
       //                             if (!string.IsNullOrEmpty(DnIdPrp))
       //                                 counDn = counterPartyService.GetCounterPartyByPropCode(DnIdPrp);
       //                             else if (!string.IsNullOrEmpty(DnId))
       //                                 counDn = counterPartyService.GetCounterPartyByIdentifier(DnId);
       //                             #endregion

       //                             PipelineDTO pipe = null;
       //                             if (!string.IsNullOrEmpty(PipeDuns))
       //                                 pipe = pipelineService.GetPipelineByDunsNo(PipeDuns);

       //                             //int TranTypeMapId = pipelineService.GetTranTypeMapId(pipe.DUNSNo, TT, TranTypeDesc);

       //                             decimal recQty = string.IsNullOrEmpty(RecQty) ? 0 : Convert.ToDecimal(RecQty);
       //                             decimal delQty = string.IsNullOrEmpty(DelQty) ? 0 : Convert.ToDecimal(DelQty);
       //                             int fuelPercent = string.IsNullOrEmpty(FuelPercentage) ? 0 : Convert.ToInt32(FuelPercentage);
       //                             if (recQty > 0)
       //                             {
       //                                 delQty = (recQty * ((100 - fuelPercent) / 100));
       //                             }
       //                             if (recQty == 0 && delQty > 0)
       //                             {
       //                                 recQty = ((delQty / (100 - fuelPercent)) * 100);
       //                             }

       //                             PathedDTO pathed = new PathedDTO();
       //                             pathed.PathedNomsList = new List<PathedNomDetailsDTO>();
       //                             pathed.DunsNo = shipperDuns;
       //                             pathed.PipelineID = pipe != null ? pipe.ID : 0;
       //                             pathed.ShipperID = UserID != null ? Guid.Parse(UserID) : Guid.Empty;
       //                             PathedNomDetailsDTO nom = new PathedNomDetailsDTO();
       //                             //nom.TransTypeMapId = TranTypeMapId;
       //                             nom.ActCode = ActCD;
       //                             nom.AssocContract = "";
       //                             nom.BidTransportRate = "";
       //                             nom.BidUp = "";
       //                             nom.CanWrite = true;
       //                             nom.CapacityType = "";
       //                             nom.CompanyID = companyID;
       //                             nom.Contract = SvcReq;
       //                             nom.CycleID = cycleId;
       //                             nom.DealType = "";
       //                             nom.DelLoc = DelLoc;
       //                             nom.DelLocID = DelLoc;
       //                             nom.DelLocProp = DelLoc;
       //                             nom.DelQuantity = delQty;
       //                             nom.DelRank = DelRank;
       //                             nom.DownContract = DnK;
       //                             nom.FuelPercentage = fuelPercent;
       //                             if (counDn != null)
       //                             {
       //                                 nom.DownID = counDn.Identifier;
       //                                 nom.DownIDProp = counDn.PropCode;
       //                             }
       //                             else
       //                             {
       //                                 nom.DownID = DnId;
       //                                 nom.DownIDProp = DnIdPrp;
       //                             }
       //                             nom.DownName = "";
       //                             nom.DownPkgID = "";
       //                             nom.DownRank = "";
       //                             nom.EndDate = eDate;
       //                             nom.Export = "";
       //                             nom.MaxRate = "";
       //                             nom.NomSubCycle = NomSubCycle;
       //                             nom.NomUserData1 = "";
       //                             nom.NomUserData2 = "";
       //                             nom.PipelineID = PipelineId;
       //                             nom.PkgID = PkgId;
       //                             nom.ProcessingRights = "";
       //                             nom.RecLocation = RecLoc2;
       //                             nom.RecLocID = RecLoc2;
       //                             nom.RecLocProp = RecLoc2;
       //                             nom.RecQty = Convert.ToInt32(recQty).ToString();
       //                             nom.RecRank = RecRank;
       //                             nom.ShipperDuns = shipperDuns;
       //                             nom.StartDate = bDate;
       //                             nom.Status = "";
       //                             nom.StatusID = (int)statusBatch.Draft;
       //                             nom.TransType = TT;
       //                             if (counUp != null)
       //                             {
       //                                 nom.UpID = counUp.Identifier;
       //                                 nom.UpIDProp = counUp.PropCode;
       //                             }
       //                             else
       //                             {
       //                                 nom.UpID = UpId;
       //                                 nom.UpIDProp = UpIdProp;
       //                             }
       //                             nom.UpKContract = UpK;
       //                             nom.UpName = "";
       //                             nom.UpPkgID = "";
       //                             nom.UpRank = "";
       //                             nom.QuantityType = "R";
       //                             nom.CreatedDate = DateTime.Now;
       //                             nom.ScheduledDateTime = DateTime.Now;
       //                             pathed.PathedNomsList.Add(nom);
       //                             pathedDTOList.Add(pathed);
       //                         }
       //                         #endregion
       //                     }
       //                     #endregion
       //                 }
       //                 #endregion
       //                 #region NonPathed Nom data extract from excel and fill DTO
       //                 for (int row = 2; row <= 251; row++)
       //                 {
       //                     var rSDate = "";
       //                     if (nonPathedSheet.GetRow(row) != null)
       //                         rSDate = nonPathedSheet.GetRow(row).GetCell(0) != null ? (nonPathedSheet.GetRow(row).GetCell(0).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(0).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(0).DateCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (!string.IsNullOrEmpty(rSDate))
       //                     {
       //                         NonPathedRecieptNom recNom = new NonPathedRecieptNom();
       //                         var rEDate = nonPathedSheet.GetRow(row).GetCell(1) != null ? (nonPathedSheet.GetRow(row).GetCell(1).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(1).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(1).DateCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rCycleCode = nonPathedSheet.GetRow(row).GetCell(2) != null ? (nonPathedSheet.GetRow(row).GetCell(2).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(2).DateCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rTranTypeCode = nonPathedSheet.GetRow(row).GetCell(3) != null ? (nonPathedSheet.GetRow(row).GetCell(3).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(3).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(3).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rRollNom = nonPathedSheet.GetRow(row).GetCell(4) != null ? (nonPathedSheet.GetRow(row).GetCell(4).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rSrvRecCon = nonPathedSheet.GetRow(row).GetCell(5) != null ? (nonPathedSheet.GetRow(row).GetCell(5).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(5).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rFuelPercentage = nonPathedSheet.GetRow(row).GetCell(6) != null ? (nonPathedSheet.GetRow(row).GetCell(6).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(6).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rRecLoc = nonPathedSheet.GetRow(row).GetCell(7) != null ? (nonPathedSheet.GetRow(row).GetCell(7).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(7).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rUpIden = nonPathedSheet.GetRow(row).GetCell(8) != null ? (nonPathedSheet.GetRow(row).GetCell(8).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(8).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rUpCon = nonPathedSheet.GetRow(row).GetCell(9) != null ? (nonPathedSheet.GetRow(row).GetCell(9).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(9).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rRecQty = nonPathedSheet.GetRow(row).GetCell(10) != null ? (nonPathedSheet.GetRow(row).GetCell(10).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(10).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rRecRank = nonPathedSheet.GetRow(row).GetCell(11) != null ? (nonPathedSheet.GetRow(row).GetCell(11).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(11).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rPkgId = nonPathedSheet.GetRow(row).GetCell(12) != null ? (nonPathedSheet.GetRow(row).GetCell(12).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(12).StringCellValue.ToString().Trim() : string.Empty;
       //                         //var rTranTypeDesc = nonPathedSheet.GetRow(row).GetCell(13) != null ? (nonPathedSheet.GetRow(row).GetCell(13).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(13).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(13).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(13).StringCellValue.ToString().Trim() : string.Empty;
       //                         var rPipeDuns = nonPathedSheet.GetRow(row).GetCell(13) != null ? (nonPathedSheet.GetRow(row).GetCell(13).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(13).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(13).StringCellValue.ToString().Trim() : string.Empty;
       //                         rSDate = GetFilteredString(rSDate);
       //                         rEDate = GetFilteredString(rEDate);
       //                         rCycleCode = GetFilteredString(rCycleCode);
       //                         rTranTypeCode = GetFilteredString(rTranTypeCode);
       //                         //rTranTypeDesc = GetFilteredString(rTranTypeDesc);
       //                         rSrvRecCon = GetFilteredString(rSrvRecCon);
       //                         rRecLoc = GetFilteredString(rRecLoc);
       //                         rUpIden = GetFilteredString(rUpIden);
       //                         rUpCon = GetFilteredString(rUpCon);
       //                         rRecQty = GetFilteredString(rRecQty);
       //                         rRecRank = GetFilteredString(rRecRank);
       //                         rPkgId = GetFilteredString(rPkgId);
       //                         rPipeDuns = GetFilteredString(rPipeDuns);
       //                         rRollNom = GetFilteredString(rRollNom);
       //                         rFuelPercentage = GetFilteredString(rFuelPercentage);
       //                         int cycleId = 0;
       //                         DateTime rStartDate = DateTime.Parse(rSDate);
       //                         DateTime rEndDate = DateTime.Parse(rEDate);
       //                         if (rCycleCode == timely)
       //                         {
       //                             rStartDate = new DateTime(rStartDate.Year, rStartDate.Month, rStartDate.Day, 9, 0, 0);
       //                             rEndDate = new DateTime(rEndDate.Year, rEndDate.Month, rEndDate.Day, 9, 0, 0);
       //                             cycleId = 1;
       //                         }
       //                         else if (rCycleCode == evening)
       //                         {
       //                             rStartDate = new DateTime(rStartDate.Year, rStartDate.Month, rStartDate.Day, 9, 0, 0);
       //                             rEndDate = new DateTime(rEndDate.Year, rEndDate.Month, rEndDate.Day, 9, 0, 0);
       //                             cycleId = 2;
       //                         }
       //                         else if (rCycleCode == intraDay1)
       //                         {
       //                             rStartDate = new DateTime(rStartDate.Year, rStartDate.Month, rStartDate.Day, 14, 0, 0);
       //                             rEndDate = new DateTime(rEndDate.Year, rEndDate.Month, rEndDate.Day, 9, 0, 0);
       //                             cycleId = 3;
       //                         }
       //                         else if (rCycleCode == intraDay2)
       //                         {
       //                             rStartDate = new DateTime(rStartDate.Year, rStartDate.Month, rStartDate.Day, 18, 0, 0);
       //                             rEndDate = new DateTime(rEndDate.Year, rEndDate.Month, rEndDate.Day, 9, 0, 0);
       //                             cycleId = 4;
       //                         }
       //                         else if (rCycleCode == intraDay3)
       //                         {
       //                             rStartDate = new DateTime(rStartDate.Year, rStartDate.Month, rStartDate.Day, 22, 0, 0);
       //                             rEndDate = new DateTime(rEndDate.Year, rEndDate.Month, rEndDate.Day, 9, 0, 0);
       //                             cycleId = 5;
       //                         }
       //                         var upCp = counterPartyService.GetCounterPartyByIdentifier(rUpIden);
       //                         recNom.StartDateTime = rStartDate;
       //                         recNom.EndDateTime = rEndDate;
       //                         recNom.CreateDateTime = DateTime.UtcNow;
       //                         recNom.CreatedBy = UserID != null ? UserID : string.Empty;
       //                         recNom.Cycle = rCycleCode;
       //                         recNom.CycleId = cycleId;
       //                         recNom.FuelPercentage = string.IsNullOrEmpty(rFuelPercentage) ? 0 : Convert.ToDecimal(rFuelPercentage);
       //                         recNom.NomSubCycle = rRollNom;
       //                         recNom.NomTrackingId = "";
       //                         recNom.PackageId = rPkgId;
       //                         recNom.PipelineId = 0;
       //                         recNom.ReceiptLocId = rRecLoc;
       //                         recNom.ReceiptLocName = "";
       //                         recNom.ReceiptLocProp = rRecLoc;
       //                         recNom.ReceiptQty = Convert.ToDecimal(string.IsNullOrEmpty(rRecQty) ? "0" : rRecQty);
       //                         recNom.ReceiptRank = rRecRank;
       //                         recNom.ServiceRequesterContractCode = rSrvRecCon;
       //                         recNom.ServiceRequesterContractName = "";
       //                         recNom.ShipperDuns = shipperDuns;
       //                         recNom.Status = "";
       //                         recNom.StatusId = (int)statusBatch.Draft;
       //                         recNom.TransactionType = rTranTypeCode;
       //                         //recNom.TransactionTypeDesc = rTranTypeDesc;
       //                         recNom.UpstreamId = upCp != null ? upCp.Identifier : rUpIden;
       //                         recNom.UpstreamK = rUpCon;
       //                         recNom.UpstreamProp = upCp != null ? upCp.PropCode : rUpIden;

       //                         var nonPathedBatch = nonPathedDtoList.Where(a => a.PipelineDuns == rPipeDuns).FirstOrDefault();
       //                         if (nonPathedBatch != null)
       //                         {
       //                             nonPathedDtoList.Remove(nonPathedBatch);
       //                             nonPathedBatch.ReceiptNoms.Add(recNom);
       //                             nonPathedDtoList.Add(nonPathedBatch);
       //                         }
       //                         else
       //                         {
       //                             NonPathedDTO nonPathedDTO = new NonPathedDTO();
       //                             var pipe = pipelineService.GetPipelineByDunsNo(rPipeDuns);
       //                             nonPathedDTO.PipelineDuns = rPipeDuns;
       //                             nonPathedDTO.PipelineId = pipe != null ? pipe.ID : 0;
       //                             nonPathedDTO.ShipperDuns = shipperDuns;
       //                             nonPathedDTO.UserId = UserID != null ? Guid.Parse(UserID) : Guid.Empty;
       //                             nonPathedDTO.ReceiptNoms.Add(recNom);
       //                             nonPathedDtoList.Add(nonPathedDTO);
       //                         }
       //                     }
       //                     var dSDate = "";
       //                     if (nonPathedSheet.GetRow(row) != null)
       //                         dSDate = nonPathedSheet.GetRow(row).GetCell(14) != null ? (nonPathedSheet.GetRow(row).GetCell(14).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(14).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(14).DateCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(14).StringCellValue.ToString().Trim() : string.Empty;
       //                     if (!string.IsNullOrEmpty(dSDate))
       //                     {
       //                         NonPathedDeliveryNom delNom = new NonPathedDeliveryNom();
       //                         var dEDate = nonPathedSheet.GetRow(row).GetCell(15) != null ? (nonPathedSheet.GetRow(row).GetCell(15).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(15).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(15).DateCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(15).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dCycleCode = nonPathedSheet.GetRow(row).GetCell(16) != null ? (nonPathedSheet.GetRow(row).GetCell(16).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(16).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(16).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dTranTypeCode = nonPathedSheet.GetRow(row).GetCell(17) != null ? (nonPathedSheet.GetRow(row).GetCell(17).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(17).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(17).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(17).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dRollNom = nonPathedSheet.GetRow(row).GetCell(18) != null ? (nonPathedSheet.GetRow(row).GetCell(18).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(18).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(18).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dSrvRecCon = nonPathedSheet.GetRow(row).GetCell(19) != null ? (nonPathedSheet.GetRow(row).GetCell(19).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(19).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(19).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(19).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dFuelPercentage = nonPathedSheet.GetRow(row).GetCell(20) != null ? (nonPathedSheet.GetRow(row).GetCell(20).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(20).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(20).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(20).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dDelLoc = nonPathedSheet.GetRow(row).GetCell(21) != null ? (nonPathedSheet.GetRow(row).GetCell(21).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(21).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(21).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(21).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dDwnIden = nonPathedSheet.GetRow(row).GetCell(22) != null ? (nonPathedSheet.GetRow(row).GetCell(22).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(22).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(22).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(22).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dDwnCon = nonPathedSheet.GetRow(row).GetCell(23) != null ? (nonPathedSheet.GetRow(row).GetCell(23).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(23).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(23).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(23).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dDelQty = nonPathedSheet.GetRow(row).GetCell(24) != null ? (nonPathedSheet.GetRow(row).GetCell(24).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(24).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(24).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(24).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dDelRank = nonPathedSheet.GetRow(row).GetCell(25) != null ? (nonPathedSheet.GetRow(row).GetCell(25).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(25).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(25).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(25).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dPkgId = nonPathedSheet.GetRow(row).GetCell(26) != null ? (nonPathedSheet.GetRow(row).GetCell(26).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(26).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(26).StringCellValue.ToString().Trim() : string.Empty;
       //                         //var dTranTypeDesc = nonPathedSheet.GetRow(row).GetCell(28) != null ? (nonPathedSheet.GetRow(row).GetCell(28).CellType == CellType.Numeric || nonPathedSheet.GetRow(row).GetCell(28).CellType == CellType.Formula) ? nonPathedSheet.GetRow(row).GetCell(28).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(28).StringCellValue.ToString().Trim() : string.Empty;
       //                         var dPipeDuns = nonPathedSheet.GetRow(row).GetCell(27) != null ? (nonPathedSheet.GetRow(row).GetCell(27).CellType == CellType.Numeric) ? nonPathedSheet.GetRow(row).GetCell(27).NumericCellValue.ToString().Trim() : nonPathedSheet.GetRow(row).GetCell(27).StringCellValue.ToString().Trim() : string.Empty;
       //                         dSDate = GetFilteredString(dSDate);
       //                         dEDate = GetFilteredString(dEDate);
       //                         dCycleCode = GetFilteredString(dCycleCode);
       //                         dTranTypeCode = GetFilteredString(dTranTypeCode);
       //                         //dTranTypeDesc = GetFilteredString(dTranTypeDesc);
       //                         dSrvRecCon = GetFilteredString(dSrvRecCon);
       //                         dDelLoc = GetFilteredString(dDelLoc);
       //                         dDwnIden = GetFilteredString(dDwnIden);
       //                         dDwnCon = GetFilteredString(dDwnCon);
       //                         dDelQty = GetFilteredString(dDelQty);
       //                         dDelRank = GetFilteredString(dDelRank);
       //                         dPkgId = GetFilteredString(dPkgId);
       //                         dPipeDuns = GetFilteredString(dPipeDuns);
       //                         dRollNom = GetFilteredString(dRollNom);
       //                         dFuelPercentage = GetFilteredString(dFuelPercentage);

       //                         DateTime dStartDate = DateTime.Parse(dSDate);
       //                         DateTime dEndDate = DateTime.Parse(dEDate);
       //                         int cycleId = 0;
       //                         if (dCycleCode == timely)
       //                         {
       //                             dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day, 9, 0, 0);
       //                             dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 9, 0, 0);
       //                             cycleId = 1;
       //                         }
       //                         else if (dCycleCode == evening)
       //                         {
       //                             dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day, 9, 0, 0);
       //                             dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 9, 0, 0);
       //                             cycleId = 2;
       //                         }
       //                         else if (dCycleCode == intraDay1)
       //                         {
       //                             dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day, 14, 0, 0);
       //                             dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 9, 0, 0);
       //                             cycleId = 3;
       //                         }
       //                         else if (dCycleCode == intraDay2)
       //                         {
       //                             dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day, 18, 0, 0);
       //                             dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 9, 0, 0);
       //                             cycleId = 4;
       //                         }
       //                         else if (dCycleCode == intraDay3)
       //                         {
       //                             dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day, 22, 0, 0);
       //                             dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 9, 0, 0);
       //                             cycleId = 5;
       //                         }
       //                         var downCp = counterPartyService.GetCounterPartyByIdentifier(dDwnIden);
       //                         delNom.CreateDateTime = DateTime.UtcNow;
       //                         delNom.CreatedBy = UserID != null ? UserID : string.Empty;
       //                         delNom.Cycle = dCycleCode;
       //                         delNom.CycleId = cycleId;
       //                         delNom.DeliveryLocId = dDelLoc;
       //                         delNom.DeliveryLocName = "";
       //                         delNom.DeliveryLocProp = dDelLoc;
       //                         delNom.DeliveryQty = Convert.ToDecimal(string.IsNullOrEmpty(dDelQty) ? "0" : dDelQty);
       //                         delNom.DeliveryRank = dDelRank;
       //                         delNom.DnstreamId = downCp != null ? downCp.Identifier : dDwnIden;
       //                         delNom.DnstreamK = dDwnCon;
       //                         delNom.DnstreamName = "";
       //                         delNom.DnstreamProp = downCp != null ? downCp.PropCode : dDwnIden;
       //                         delNom.EndDateTime = dEndDate;
       //                         delNom.FuelPercentage = string.IsNullOrEmpty(dFuelPercentage) ? 0 : Convert.ToDecimal(dFuelPercentage);
       //                         delNom.NomSubCycle = dRollNom;
       //                         delNom.NomTrackingId = "";
       //                         delNom.PackageId = dPkgId;
       //                         delNom.PipelineId = 0;
       //                         delNom.ServiceRequesterContractCode = dSrvRecCon;
       //                         delNom.ServiceRequesterContractName = "";
       //                         delNom.ShipperDuns = shipperDuns;
       //                         delNom.StartDateTime = dStartDate;
       //                         delNom.Status = "";
       //                         delNom.StatusId = (int)statusBatch.Draft;
       //                         delNom.TransactionId = Guid.Empty;
       //                         delNom.TransactionType = dTranTypeCode;
       //                         //delNom.TransactionTypeDesc = dTranTypeDesc;

       //                         var nonPathedBatch = nonPathedDtoList.Where(a => a.PipelineDuns == dPipeDuns).FirstOrDefault();
       //                         if (nonPathedBatch != null)
       //                         {
       //                             nonPathedDtoList.Remove(nonPathedBatch);
       //                             nonPathedBatch.DeliveryNoms.Add(delNom);
       //                             nonPathedDtoList.Add(nonPathedBatch);
       //                         }
       //                         else
       //                         {
       //                             NonPathedDTO nonPathedDTO = new NonPathedDTO();
       //                             var pipe = pipelineService.GetPipelineByDunsNo(dPipeDuns);
       //                             nonPathedDTO.PipelineDuns = dPipeDuns;
       //                             nonPathedDTO.PipelineId = pipe != null ? pipe.ID : 0;
       //                             nonPathedDTO.ShipperDuns = shipperDuns;
       //                             nonPathedDTO.UserId = UserID != null ? Guid.Parse(UserID) : Guid.Empty;
       //                             nonPathedDTO.DeliveryNoms.Add(delNom);
       //                             nonPathedDtoList.Add(nonPathedDTO);
       //                         }
       //                     }
       //                 }
       //                 #endregion

       //                 #region Save non-Pathed List
       //                 if (nonPathedDtoList.Count > 0)
       //                 {
       //                     foreach (var nonPathedDTO in nonPathedDtoList)
       //                         nonPathedService.SaveAllNonPathedNominations(nonPathedDTO);
       //                 }
       //                 #endregion

       //                 #region Save pathed nominations
       //                 if (pathedDTOList.Count > 0)
       //                 {
       //                     foreach (var pathedDTO in pathedDTOList)
       //                         uploadNominationService.SavePathedBulkUpload(pathedDTO, true);
       //                 }
       //                 #endregion
       //                 #region Save Pnt nominations
       //                 if (batchList.Count > 0)
       //                 {
       //                     foreach (var batch in batchList)
       //                         uploadNominationService.SavePNTBulkUpload(batch, true);
       //                 }
       //                 #endregion
       //             }
       //         }
       //         catch (Exception ex)
       //         {
       //             stream.Position = 0;
       //             return Json("Oops! Something went wrong.");
       //         }
       //     }

       //     return Json("All files have been successfully stored.");
       // }



        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                stream.ReadByte();
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
        public List<string> GetColumnsName(ISheet sheet)
        {
            List<string> colNames = new List<string>();
            IRow row;
            row = sheet.GetRow(0);
            if ((sheet != null) && (sheet.SheetName == "NonPathed"))
            {
                row = sheet.GetRow(1);
            }
            //if ((sheet != null) && (sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market" || sheet.SheetName == "Batch"))
            //{
            //    row = sheet.GetRow(0);
            //}
            //else
            //{
            //    row = sheet.GetRow(9);   // for pathed row number 10 is columns heading row.
            //}

            for (int index = 0; index < row.PhysicalNumberOfCells; index++)
            {
                try
                {
                    string colname = row.GetCell(index).StringCellValue.ToString();
                    colNames.Add(colname);
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            return colNames;

            //if ((sheet != null) && (sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market" || sheet.SheetName=="Batch"))
            //{
            //    return colNames;
            //}
            //else
            //{
            //    colNames = colNames; // columns name starts from 21 column index in pathed.
            //}
        }
        public List<int> GetEmptyColIndex(List<string> colNamesList)
        {
            List<int> emptyColsIndex = new List<int>();
            foreach (var str in colNamesList)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    int index = colNamesList.IndexOf(str);
                    emptyColsIndex.Add(index);
                }
            }
            return emptyColsIndex;
        }
        public List<string> GetColsNameWithoutEmpty(ISheet sheet)
        {
            List<string> resultantList = new List<string>();
            List<string> colsNames = GetColumnsName(sheet);
            foreach (var item in colsNames)
            {
                if (!(item == "" || string.IsNullOrWhiteSpace(item) || string.IsNullOrEmpty(item)))
                {
                    resultantList.Add(item);
                }
            }
            return resultantList;
        }

        private string ValideBatchColumnHeadins(System.Data.DataSet BatchDataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Start Date",
                    "End Date",
                    "CycleCode",
                    "DunsNo"
            };
            if (BatchDataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in the Batch tab.";
            }
            var i = 0;
            foreach (DataColumn column in BatchDataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim()) {
                    errorMsg = "Column sequence is not correct in Batch tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }
       

        private string IsBatchColInSequence(List<string> batchColsList)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Start Date",
                    "End Date",
                    "CycleCode",
                    "DunsNo"
            };
            if (batchColsList.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate in the Batch tab.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = batchColsList.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {

                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in Batch tab. ";
                }
            }
            return wrongStr;
        }


        private string ValidateSupplyColumnHeadings(System.Data.DataSet dataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Rec Loc",
                    "TT",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Up ID Prop",
                    "Up ID",
                    "Rec Qty (Gross)",
                    "Up Rank/Del Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month",
                    "DunsNo"
            };
            if (dataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in the Supply tab.";
            }
            var i = 0;
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim())
                {
                    errorMsg = "Column sequence is not correct in Supply tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }



        public string IsSupplyColsInSquence(List<string> supplywithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Rec Loc",
                    "TT",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Up ID Prop",
                    "Up ID",
                    "Rec Qty (Gross)",
                    "Up Rank/Del Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month",
                    "DunsNo"
            };
            if (supplywithColsName.Count != colNames.Count)
            {
                wrongStr = "Column count is not accurate in Supply tab.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = supplywithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not accurate in Supply tab.";
                }
            }

            return wrongStr;
        }


        private string ValidateMarketColumnHeadings(System.Data.DataSet dataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Del Loc",
                    "TT",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Dn Id Prop",
                    "Dn ID",
                    "Rec Qty",
                    "Dn Rank/Rec Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month",
                    "DunsNo"
            };
            if (dataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in the Market tab.";
            }
            var i = 0;
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim())
                {
                    errorMsg = "Column sequence is not correct in Market tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }




        public string IsMarketColsInSquence(List<string> marketwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Del Loc",
                    "TT",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Dn Id Prop",
                    "Dn ID",
                    "Del Qty (Net)",
                    "Dn Rank/Rec Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month",
                    "DunsNo"
            };
            if (marketwithColsName.Count != colNames.Count)
            {
                wrongStr = "Column count is not accurate in Market tab.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = marketwithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not accurate in Market tab.";
                }
            }

            return wrongStr;
        }

        private string ValidateContractPathColumnHeadings(System.Data.DataSet dataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Svc Req K",
                    "Fuel Percentage",
                    "TT",
                    "Rec Loc",
                    "Dn Rank/Rec Rank",
                    "Rec Qty (Gross)",
                    "Del Loc",
                    "Up Rank/Del Rank",
                    "Del Qty (Net)",
                    "Path Rank",
                    "Pkg ID",
                    "Imbalance Month",
                // "ROUTE (Required Only for Sonat or El Paso, otherwise Leave Blank). For Sonat, value Must be either 'Default' or 'Cypress'. For EP, enter 1 acceptable Description ID e.g. North or South",
                "ROUTE (Required Only for Sonat or El Paso, otherwise Leave Blank",
                "DunsNo"
            };
            if (dataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in Contract Path tab.";
            }
            var i = 0;
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim())
                {
                    errorMsg = "Column sequence is not correct in Contract Path tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }




        public string IsContractPathColsInSquence(List<string> transportwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Svc Req K",
                    "Fuel Percentage",
                    "TT",
                    "Rec Loc",
                    "Dn Rank/Rec Rank",
                    "Rec Qty (Gross)",
                    "Del Loc",
                    "Up Rank/Del Rank",
                    "Del Qty (Net)",
                    "Path Rank",
                    "Pkg ID",
                    "Imbalance Month",
                    "ROUTE (Required Only for Sonat or El Paso, otherwise Leave Blank). For Sonat, value Must be either 'Default' or 'Cypress'. For EP, enter 1 acceptable Description ID e.g. North or South",
                    "DunsNo"
            };
            if (transportwithColsName.Count != colNames.Count)
            {
                wrongStr = "Column count is not accurate in Contract Path tab.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = transportwithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not accurate in Contract Path tab.";
                }
            }

            return wrongStr;
        }


        private string ValidatePathedColumnHeadings(System.Data.DataSet dataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Beg Date",
                    "End Date",
                    " Cycle",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Nom Sub Cycle",
                    "Act Cd",
                    "TT",
                    "Rec Loc",
                    "Up ID Prop",
                    "Up ID",
                    "Up K",
                    "Rec Qty",
                    "Rec Rank",
                    "Del Loc",
                    "Dn ID Prop",
                    "Dn ID",
                    "Dn K",
                    "Del Qty",
                    "Del Rank",
                    "Pkg ID",
                    "DunsNo"
            };
            if (dataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in Pathed tab.";
            }
            var i = 0;
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim())
                {
                    errorMsg = "Column sequence is not correct in Pathed tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }

        public string IsPathedColsInSquence(List<string> PathedwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Beg Date",
                    "End Date",
                    "Cycle",
                    "Svc Req K",
                    "Fuel Percentage",
                    "Nom Sub Cycle",
                    "Act Cd",
                    "TT",
                    "Rec Loc",
                    "Up ID Prop",
                    "Up ID",
                    "Up K",
                    "Rec Qty",
                    "Rec Rank",
                    "Del Loc",
                    "Dn ID Prop",
                    "Dn ID",
                    "Dn K",
                    "Del Qty",
                    "Del Rank",
                    "Pkg ID",
                    "DunsNo"
            };
            if (PathedwithColsName.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate in Pathed.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = PathedwithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in pathed. Check Column Names(Blank spaces not allowed) and Sequence for " + arrToCompare[index];
                }
            }

            return wrongStr;
        }


        private string ValidateNonPathedColumnHeadings(System.Data.DataSet dataSet)
        {
            string errorMsg = String.Empty;
            List<string> colNames = new List<string>() {
                    "Start Date",
                    "End Date",
                    " Cycle",
                    "TransactionType",
                    "Roll Nom",
                    "Srv Req Contract",
                    "Fuel Percentage",
                    "Rec Loc",
                    "Up Identifier",
                    "Up Contract",
                    "Rec Qty",
                    "Rec Rank",
                    "Package Id",
                    "Duns No",
                    "Start Date1",
                    "End Date1",
                    "Cycle1",
                    "TransactionType1",
                    "Roll Nom1",
                    "Srv Req Contract1",
                    "Fuel Percentage1",
                    "Del Loc",
                    "Down Identifier",
                    "Down Contract",
                    "Del Qty",
                    "Del Rank",
                    "Package Id1",
                    "Duns No1"
            };
            if (dataSet.Tables[0].Columns.Count != colNames.Count)
            {
                errorMsg = "Columns count is not accurate in NonPathed tab.";
            }

            
            var i = 0;
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (i == colNames.Count) { break; }
                if (column.ColumnName.ToLower().Trim() != colNames[i].ToLower().Trim())
                {
                    errorMsg = "Column sequence is not correct in NonPathed tab. ";
                    break;
                }
                i++;
                //Console.WriteLine(column.ColumnName);
            }
            return errorMsg;
        }



        private string IsNonPathedColsInSequence(List<string> nonPathedColsList)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Start Date",
                    "End Date",
                    "Cycle",
                    "TransactionType",
                    "Roll Nom",
                    "Srv Req Contract",
                    "Fuel Percentage",
                    "Rec Loc",
                    "Up Identifier",
                    "Up Contract",
                    "Rec Qty",
                    "Rec Rank",
                    "Package Id",
                    "Duns No",
                    "Start Date",
                    "End Date",
                    "Cycle",
                    "TransactionType",
                    "Roll Nom",
                    "Srv Req Contract",
                    "Fuel Percentage",
                    "Del Loc",
                    "Down Identifier",
                    "Down Contract",
                    "Del Qty",
                    "Del Rank",
                    "Package Id",
                    "Duns No"
            };
            if (nonPathedColsList.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate in non-Pathed.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = nonPathedColsList.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in non-pathed. Check Column Names(Blank spaces not allowed) and Sequence for " + arrToCompare[index];
                }
            }

            return wrongStr;
        }
        public bool isSheetEmpty(ISheet sheet)
        {
            bool hasContent = false;
            int rowIndex = 0;
            IRow row;
            DataFormatter formatter = new DataFormatter();
            while (sheet.GetRow(rowIndex) != null)
            {
                row = sheet.GetRow(rowIndex);
                //all cells are empty, so is a 'blank row'
                if (row.Cells.All(d => d.CellType == CellType.Blank || String.IsNullOrWhiteSpace(formatter.FormatCellValue(d)) || formatter.FormatCellValue(d) == ""))
                {
                    rowIndex++;
                    continue;
                }

                hasContent = true;
                break;
            }
            return !hasContent;
        }
        public bool IsEmptyRow(IRow row)
        {
            bool hasContent = true;
            DataFormatter formatter = new DataFormatter();
            //all cells are empty, so is a 'blank row'
            if (row.Cells.All(d => d.CellType == CellType.Blank || String.IsNullOrWhiteSpace(formatter.FormatCellValue(d)) || formatter.FormatCellValue(d) == ""))
            {
                hasContent = false;
            }
            return !hasContent;
        }
        public ActionResult Redirection(string pipelineDuns)
        {
            // var pipeline = pipelineService.GetPipeline(PipelineId);
            var pipes = GetPipelines();
            var pipe = pipes.Count > 0 ? pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault() : new PipelineDTO();


            if (pipe.ModelTypeID == 1)
            {
                return RedirectToAction("Index", "PathedNomination", new { pipelineDuns = pipelineDuns });
            }
            else
            {
                return RedirectToAction("Index", "Batch", new { pipelineDuns = pipelineDuns });
            }
        }
    }
    public class PNTModules
    {
        public string PNTStreams { get; set; }
    }
}