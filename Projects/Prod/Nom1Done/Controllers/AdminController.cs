using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    public class AdminController : BaseController
    {
        char specialChar = '_';
        List<PNTModules> lst = new List<PNTModules>();
        Guid transactionIdForRaiseNomination = new Guid();
        private readonly IUploadNominationService uploadNominationService;
        private readonly IPipelineService pipelineService;
        private readonly ICounterPartiesService counterPartyService;

        bool IsExit = false;

        public AdminController(IPipelineService pipelineService, IUploadNominationService uploadNominationService, ICounterPartiesService counterPartyService):base(pipelineService)
        {
            this.uploadNominationService = uploadNominationService;
            this.pipelineService = pipelineService;
            this.counterPartyService = counterPartyService;
        }

        public ActionResult Index()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;          
            UploadFilesListDTO model = new UploadFilesListDTO();
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            model.UploadedFilesList = uploadNominationService.GetAllUploadedFiles( UserId).ToList();//new List<UploadFileDTO>();//_uploadfileService.GetUploadedFiles(PipelineId, Convert.ToString(Session["UserId"]));
            return View(model);
        }

        // [HttpPost]
        public void DownloadFile(int FileID)
        {
            UploadedFile file;
            try
            {
                file = uploadNominationService.DownloadFile(FileID);
                if (file != null)
                {
                    
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";                 
                    Response.AddHeader("content-disposition", "attachment;filename=" + file.FileName);
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);                   
                    Response.BinaryWrite(file.FileBytes);
                    Response.Flush();
                    Response.Close();
                    
                }
                
            }
            catch (Exception ex)
            {
             
            }

        }

        [HttpPost]
        public ActionResult GetFiles()
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
                IEnumerable<UploadFileDTO> uploadedFilesList = uploadNominationService.GetAllUploadedFiles(UserId);

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

        //TODO : Replce Modified by and created by with the logged in user


        public BatchDetailDTO GetBatchDateTimeAndCycleID(BatchDetailDTO batch, ISheet sheet)
        {

            var timely = "TIM";
            var evening = "EVE";
            var intraDay1 = "ID1";
            var intraDay2 = "ID2";
            var intraDay3 = "ID3";


            #region GET Pipeline ID/Duns

            var Pipelineduns = sheet.GetRow(1).GetCell(0).StringCellValue;
            var PipelineId = pipelineService.GetPipelineIdUsingDuns(Pipelineduns);


            batch.PipelineId = PipelineId;
            batch.Duns = Pipelineduns;

            #endregion


            #region CycleCode and DateTime Reading
            var CycleId = 1;
            var SDateTime = DateTime.Now;
            var EDateTime = DateTime.Now;

            var startDate = sheet.GetRow(1).GetCell(1) != null ? sheet.GetRow(1).GetCell(1).DateCellValue : new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 9, 0, 0);
            var endDate = sheet.GetRow(1).GetCell(2) != null ? sheet.GetRow(1).GetCell(2).DateCellValue : new DateTime(DateTime.Now.AddDays(3).Year, DateTime.Now.AddDays(3).Month, DateTime.Now.AddDays(3).Day, 9, 0, 0);
            var CycleCode = sheet.GetRow(1).GetCell(3) != null ? sheet.GetRow(1).GetCell(3).StringCellValue : timely;
            batch.StartDateTime = startDate;
            batch.EndDateTime = endDate;
            if (CycleCode == timely)
            {
                SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
                EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
                CycleId = 1;
            }
            else if (CycleCode == evening)
            {
                SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
                EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
                CycleId = 2;
            }
            else if (CycleCode == intraDay1)
            {
                SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 14, 0, 0);
                EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
                CycleId = 3;
            }
            else if (CycleCode == intraDay2)
            {
                SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 18, 0, 0);
                EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
                CycleId = 4;
            }
            else if (CycleCode == intraDay3)
            {
                SDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 22, 0, 0);
                EDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, 9, 0, 0);
                CycleId = 5;
            }
            batch.StartDateTime = SDateTime;
            batch.EndDateTime = EDateTime.AddDays(1);
            batch.CycleId = CycleId;

            #endregion

            return batch;
        }



        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            var timely = "TIM";
            var evening = "EVE";
            var intraDay1 = "ID1";
            var intraDay2 = "ID2";
            var intraDay3 = "ID3";
            var SONAT = "006900518";  // Sonat Duns. for Route
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //  var PipelineId = Id;
            // var isPathed = false;
           // var pipeline = pipelineService.GetPipelineIdUsingDuns(PipelineId);
            //var Pipelineduns = pipelineService.GetDunsByPipelineID(PipelineId);
            int emptyRowCount = 0;
            string UserID = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault();
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                                .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0 : int.Parse(company);
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {

                    var ServiceRequestorDuns = shipperDuns;
                   /// lst.Add(new PNTModules { PNTStreams = "Batch" });
                    lst.Add(new PNTModules { PNTStreams = "Supply" });
                    lst.Add(new PNTModules { PNTStreams = "Market" });
                    lst.Add(new PNTModules { PNTStreams = "Contract Paths" });
                    // stream.Position = 0;
                    foreach (HttpPostedFileBase file in files)
                    {                       

                        int rowSkipped = 0;

                        dynamic workbook = null;

                        MemoryStream mem = new MemoryStream();
                        mem.SetLength((int)file.ContentLength);
                        file.InputStream.Read(mem.GetBuffer(), 0, (int)file.ContentLength);

                      
                        if (Path.GetExtension(file.FileName) == ".xls")
                        {
                            workbook = new HSSFWorkbook(mem);
                        }
                        else if (Path.GetExtension(file.FileName) == ".xlsx")
                        {
                            workbook = new XSSFWorkbook(mem);

                        }
                      
                        UploadedFile fileSave = new UploadedFile();
                        fileSave.AddedBy = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
                        fileSave.CreatedDate = DateTime.Now;
                        fileSave.FileBytes = mem.GetBuffer();
                        fileSave.FileName = file.FileName;
                        //fileSave.PipelineId = PipelineId;
                       
                        uploadNominationService.SaveUploadedFile(fileSave);


                        ISheet sheet = workbook.GetSheetAt(0);

                        #region empty file validation
                        bool isempty = isSheetEmpty(sheet);
                        if (isempty)
                        {
                            return Json("Error: File is empty.");
                        }
                        #endregion

                        #region PathedOrPNTValidate

                        //if ((sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market" || sheet.SheetName == "Batch"))
                        //{
                        //    if (pipeline.ModelTypeID == 1)
                        //    {
                        //        return Json("Upload Failed. Please Select Correct Pipeline to upload this File.");
                        //    }

                        //}
                        //else
                        //{
                        //    if (pipeline.ModelTypeID == 2)
                        //    {
                        //        return Json("Upload Failed. Please Select Correct Pipeline to upload this File.");
                        //    }
                        //}
                        #endregion


                        if ((sheet != null) && (sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market" || sheet.SheetName == "Batch"))
                        {
                            #region Validations On Sheet

                            ISheet supplysheet = workbook.GetSheet("Supply");
                            ISheet marketsheet = workbook.GetSheet("Market");
                            ISheet contractPathsheet = workbook.GetSheet("Contract Paths");                           

                            var supplycolsList = GetColsNameWithoutEmpty(supplysheet);
                            string isInOrdered = IsSupplyColsInSquence(supplycolsList);
                            if (!string.IsNullOrEmpty(isInOrdered))
                            {
                                return Json("Error:" + isInOrdered);
                            }
                            var marketcolsList = GetColsNameWithoutEmpty(marketsheet);
                            string marketInOrdered = IsMarketColsInSquence(marketcolsList);
                            if (!string.IsNullOrEmpty(marketInOrdered))
                            {
                                return Json("Error:" + marketInOrdered);
                            }
                            var contractcolsList = GetColsNameWithoutEmpty(contractPathsheet);
                            string contractInOrdered = IsContractPathColsInSquence(contractcolsList);
                            if (!string.IsNullOrEmpty(contractInOrdered))
                            {
                                return Json("Error:" + contractInOrdered);
                            }
                            #endregion
                            #region CreateBatch
                            Random ran = new Random();
                            string path = Path.GetRandomFileName();
                            #endregion

                            BatchDetailDTO batch = new BatchDetailDTO();
                            batch.SupplyList = new List<BatchDetailSupplyDTO>();
                            batch.Contract = new List<BatchDetailContractDTO>();
                            batch.ContractPath = new List<BatchDetailContractPathDTO>();
                            batch.MarketList = new List<BatchDetailMarketDTO>();

                            batch.BatchStatus = "";
                            batch.CreatedBy = identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault();
                            batch.CreatedDateTime = DateTime.UtcNow;
                            batch.CurrentContractRow = 0;
                            batch.CurrentSupplyRow = 0;
                            batch.Description = "Excel Upload " + DateTime.Now;                          
                            batch.StartDateTime = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 9, 0, 0);
                            batch.EndDateTime = new DateTime(DateTime.Now.AddDays(3).Year, DateTime.Now.AddDays(3).Month, DateTime.Now.AddDays(3).Day, 9, 0, 0);
                            batch.IsPNT = true;
                            batch.PakageCheck = true;                           
                            batch.PipeLineName = "";
                            batch.RankingCheck = true;
                            batch.ScheduleDate = DateTime.MaxValue;
                            batch.ShiperDuns = ServiceRequestorDuns;
                            batch.ShowZeroCheck = true;
                            batch.ShowZeroDn = false;
                            batch.ShowZeroUp = false;
                            batch.StatusId = 11;
                            batch.SubmittedDate = DateTime.MaxValue;
                            batch.UpDnContractCheck = false;
                            batch.UpDnPkgCheck = false;
                            batch.ShipperCompanyId = companyID;

                            bool isemptyRow = false;
                            foreach (var item in lst)
                            {
                                sheet = workbook.GetSheet(item.PNTStreams);
                                #region Saving PNT Nominations
                                for (int row = 1; row <= sheet.LastRowNum; row++)
                                {
                                    #region Empty row Validation

                                    var rowofTable = sheet.GetRow(row);
                                    isemptyRow = IsEmptyRow(rowofTable);
                                    if (isemptyRow)
                                    {
                                        emptyRowCount++;
                                        continue;
                                    }
                                    #endregion

                                    if (sheet.GetRow(row) != null)
                                    {
                                        if (sheet.SheetName == "Supply")
                                        {

                                            batch = GetBatchDateTimeAndCycleID(batch,sheet);

                                            #region Supply Part Declarations                                           

                                            var PkgId = sheet.GetRow(row).GetCell(11) != null ? sheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
                                            var TransactionType = sheet.GetRow(row).GetCell(5) != null ? sheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;

                                            var SvcReq = sheet.GetRow(row).GetCell(6) != null ? (sheet.GetRow(row).GetCell(6).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(SvcReq))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var ContractNo = sheet.GetRow(row).GetCell(12) != null ? (sheet.GetRow(row).GetCell(12).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(12).StringCellValue.ToString().Trim() : string.Empty;
                                            var UpDelRank = sheet.GetRow(row).GetCell(10) != null ? (sheet.GetRow(row).GetCell(10).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;
                                            var RecQuantity = sheet.GetRow(row).GetCell(9) != null ? (sheet.GetRow(row).GetCell(9).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
                                            var ReceiptLocProp = sheet.GetRow(row).GetCell(4) != null ? (sheet.GetRow(row).GetCell(4).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(ReceiptLocProp))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var upId = sheet.GetRow(row).GetCell(8) != null ? (sheet.GetRow(row).GetCell(8).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
                                            var UpIdProp = sheet.GetRow(row).GetCell(7) != null ? (sheet.GetRow(row).GetCell(7).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;



                                            #region Filter Strings

                                            PkgId = GetFilteredString(PkgId);
                                            TransactionType = GetFilteredString(TransactionType);
                                            SvcReq = GetFilteredString(SvcReq);
                                            ContractNo = GetFilteredString(ContractNo);
                                            UpDelRank = GetFilteredString(UpDelRank);
                                            ReceiptLocProp = GetFilteredString(ReceiptLocProp);
                                            RecQuantity = GetFilteredString(RecQuantity);
                                            upId = GetFilteredString(upId);
                                            UpIdProp = GetFilteredString(UpIdProp);

                                            #endregion

                                            if (String.IsNullOrEmpty(upId) || string.IsNullOrWhiteSpace(upId))
                                            {
                                                if (!string.IsNullOrEmpty(UpIdProp) && !string.IsNullOrWhiteSpace(UpIdProp))
                                                {
                                                    upId = UpIdProp;
                                                }
                                            }


                                            #region
                                            BatchDetailSupplyDTO supply = new BatchDetailSupplyDTO();
                                            //supply.BatchID
                                            supply.CreatedBy = Guid.Empty;
                                            supply.CreatedDate = DateTime.Now;
                                            supply.DeliveryQuantityNet = RecQuantity;
                                            supply.FuelPercentage = "";
                                            supply.FuelQunatity = "";
                                            supply.IsActive = true;
                                            supply.Location = ReceiptLocProp;
                                            supply.LocationProp = ReceiptLocProp;
                                            //supply.ModifiedBy=
                                            supply.PackageID = PkgId;
                                            supply.ReceiptQuantityGross = RecQuantity;
                                            supply.ServiceRequestNo = SvcReq;
                                            supply.ServiceRequestType = "";
                                            supply.TransactionType = TransactionType;
                                            supply.TransactionTypeDescription = "";
                                            supply.UpContractIdentifier = "";
                                            supply.UpPackageID = "";
                                            supply.UpstreamID = upId;
                                            supply.UpstreamIDName = "";
                                            supply.UpstreamIDProp = UpIdProp;
                                            supply.UpstreamRank = UpDelRank;
                                            batch.SupplyList.Add(supply);

                                            #endregion
                                            #endregion
                                            #endregion
                                        }
                                        else if (sheet.SheetName == "Contract Paths")
                                        {
                                            batch = GetBatchDateTimeAndCycleID(batch, sheet);

                                            #region Transport Part Declarations
                                            var SvcReq = sheet.GetRow(row).GetCell(4) != null ? (sheet.GetRow(row).GetCell(4).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(4).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(SvcReq))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var TT = sheet.GetRow(row).GetCell(5) != null ? (sheet.GetRow(row).GetCell(5).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
                                           
                                            var RecLocProp = sheet.GetRow(row).GetCell(6) != null ? (sheet.GetRow(row).GetCell(6).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(RecLocProp))
                                                continue;
                                            var RecDnRank = sheet.GetRow(row).GetCell(7) != null ? (sheet.GetRow(row).GetCell(7).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
                                            var RecQty = sheet.GetRow(row).GetCell(8) != null ? (sheet.GetRow(row).GetCell(8).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;
                                            var DelLocProp = sheet.GetRow(row).GetCell(9) != null ? (sheet.GetRow(row).GetCell(9).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(DelLocProp))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var UpDelRank = sheet.GetRow(row).GetCell(10) != null ? (sheet.GetRow(row).GetCell(10).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;
                                            var DelQty = sheet.GetRow(row).GetCell(11) != null ? (sheet.GetRow(row).GetCell(11).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
                                            var PathRank = sheet.GetRow(row).GetCell(12) != null ? (sheet.GetRow(row).GetCell(12).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(12).StringCellValue.ToString().Trim() : string.Empty;
                                            var PkgId = sheet.GetRow(row).GetCell(13) != null ? (sheet.GetRow(row).GetCell(13).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(13).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(13).StringCellValue.ToString().Trim() : string.Empty;

                                            var Route = sheet.GetRow(row).GetCell(15) != null ? (sheet.GetRow(row).GetCell(15).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(15).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(15).StringCellValue.ToString().Trim() : string.Empty;


                                            #region Filter Strings

                                            PkgId = GetFilteredString(PkgId);
                                            SvcReq = GetFilteredString(SvcReq);
                                            TT = GetFilteredString(TT);
                                            RecLocProp = GetFilteredString(RecLocProp);
                                            RecDnRank = GetFilteredString(RecDnRank);
                                            RecQty = GetFilteredString(RecQty);
                                            DelLocProp = GetFilteredString(DelLocProp);
                                            UpDelRank = GetFilteredString(UpDelRank);
                                            DelQty = GetFilteredString(DelQty);
                                            PathRank = GetFilteredString(PathRank);
                                            Route = GetFilteredString(Route);

                                            #endregion
                                            BatchDetailContractPathDTO contractPath = new BatchDetailContractPathDTO();
                                            contractPath.CreatedDate = DateTime.Now;
                                            contractPath.ServiceRequestNo = SvcReq;
                                            batch.ContractPath.Add(contractPath);

                                            BatchDetailContractDTO transport = new BatchDetailContractDTO();
                                            transport.CreatedDate = DateTime.Now;
                                            transport.DeliveryDth = RecQty;
                                            transport.DelLocation = DelLocProp;
                                            transport.DelLocationProp = DelLocProp;
                                            transport.ServiceRequestNo = SvcReq;
                                            transport.DelRank = UpDelRank;
                                            transport.DelZone = "";
                                            transport.FuelDth = "";
                                            transport.FuelPercentage = "";
                                            //transport.ID
                                            transport.IsActive = true;
                                            transport.PackageID = PkgId;
                                            transport.PathRank = PathRank;
                                            transport.ReceiptDth = RecQty;
                                            transport.RecLocation = RecLocProp;
                                            transport.RecLocationProp = RecLocProp;
                                            transport.RecRank = RecDnRank;
                                            transport.RecZone = "";
                                            transport.TransactionType = TT;
                                            transport.TransactionTypeDescription = "";

                                            // Only for SONAT
                                            if (batch.Duns == SONAT)
                                            {
                                                transport.Route = Route;
                                            }
                                            else
                                            {
                                                transport.Route = string.Empty;
                                            }
                                            batch.Contract.Add(transport);
                                            #endregion

                                            #region Nom Party Identification(Transport)
                                            #endregion
                                        }
                                        else if (sheet.SheetName == "Market")
                                        {
                                            batch = GetBatchDateTimeAndCycleID(batch, sheet);

                                            #region Market Declarations
                                            var DelLocProp = sheet.GetRow(row).GetCell(4) != null ? (sheet.GetRow(row).GetCell(0).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(0).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(DelLocProp))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var SvcReq = sheet.GetRow(row).GetCell(6) != null ? (sheet.GetRow(row).GetCell(6).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(6).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(6).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(SvcReq))
                                            {
                                                rowSkipped++;
                                                continue;
                                            }
                                            var PkId = sheet.GetRow(row).GetCell(11) != null ? (sheet.GetRow(row).GetCell(11).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(11).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(11).StringCellValue.ToString().Trim() : string.Empty;
                                            var TT = sheet.GetRow(row).GetCell(5) != null ? (sheet.GetRow(row).GetCell(5).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(5).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(5).StringCellValue.ToString().Trim() : string.Empty;
                                          
                                            var DwIdProp = sheet.GetRow(row).GetCell(7) != null ? (sheet.GetRow(row).GetCell(7).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(7).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(7).StringCellValue.ToString().Trim() : string.Empty;
                                            var DwId = sheet.GetRow(row).GetCell(8) != null ? (sheet.GetRow(row).GetCell(8).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(8).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(8).StringCellValue.ToString().Trim() : string.Empty;

                                            var UpDwContract = sheet.GetRow(row).GetCell(12) != null ? (sheet.GetRow(row).GetCell(12).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(12).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(12).StringCellValue.ToString().Trim() : string.Empty;
                                            var DnRank = sheet.GetRow(row).GetCell(10) != null ? (sheet.GetRow(row).GetCell(10).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(10).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(10).StringCellValue.ToString().Trim() : string.Empty;
                                            var DelQty = sheet.GetRow(row).GetCell(9) != null ? (sheet.GetRow(row).GetCell(9).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(9).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(9).StringCellValue.ToString().Trim() : string.Empty;


                                            #region Filter Strings

                                            PkId = GetFilteredString(PkId);
                                            SvcReq = GetFilteredString(SvcReq);
                                            TT = GetFilteredString(TT);
                                            DelLocProp = GetFilteredString(DelLocProp);
                                            DnRank = GetFilteredString(DnRank);
                                            DelQty = GetFilteredString(DelQty);
                                            DwIdProp = GetFilteredString(DwIdProp);
                                            DwId = GetFilteredString(DwId);
                                            UpDwContract = GetFilteredString(UpDwContract);
                                            #endregion

                                            if (String.IsNullOrEmpty(DwId) || string.IsNullOrWhiteSpace(DwId))
                                            {
                                                if (!string.IsNullOrEmpty(DwIdProp) && !string.IsNullOrWhiteSpace(DwIdProp))
                                                {
                                                    DwId = DwIdProp;
                                                }
                                            }


                                            BatchDetailMarketDTO market = new BatchDetailMarketDTO();
                                            //market.CreatedBy
                                            market.CreatedDate = DateTime.Now;
                                            market.DeliveryQuantityNet = "";
                                            market.DnContractIdentifier = UpDwContract;
                                            market.DnPackageID = "";
                                            market.DnstreamRank = DnRank;
                                            market.DownstreamID = DwId;
                                            market.DownstreamIDName = "";
                                            market.DownstreamIDProp = DwIdProp;
                                            market.FuelPercentage = "";
                                            market.FuelQunatity = "";
                                            market.IsActive = true;
                                            market.Location = DelLocProp;
                                            market.LocationProp = DelLocProp;
                                            market.PackageID = PkId;
                                            market.ReceiptQuantityGross = "";
                                            market.ServiceRequestNo = SvcReq;
                                            market.TransactionType = TT;
                                            market.TransactionTypeDescription = "";
                                            market.ReceiptQuantityGross = DelQty;
                                            market.DeliveryQuantityNet = DelQty;

                                            batch.MarketList.Add(market);
                                            #endregion
                                        }
                                    }
                                }
                            }
                            //_pntNomService.SaveBulkUpload(batch, true);
                            uploadNominationService.SavePNTBulkUpload(batch, true);

                        }
                        else
                        {
                            if (!IsExit)
                            {

                                #region Pathed validation
                                // Pathed validation                      
                                var colsList = GetColsNameWithoutEmpty(sheet);
                                string InOrdered = IsPathedColsInSquence(colsList);
                                if (!string.IsNullOrEmpty(InOrdered))
                                {
                                    return Json("Error:" + InOrdered);
                                }
                                // Pathed validation ---End.   



                                #endregion

                                #region GET Pipeline ID/Duns

                                var Pipelineduns = sheet.GetRow(10).GetCell(45).StringCellValue;
                                var PipelineId = pipelineService.GetPipelineIdUsingDuns(Pipelineduns);

                                #endregion


                                PathedDTO pathed = new PathedDTO();
                                pathed.PathedNomsList = new List<PathedNomDetailsDTO>();
                                pathed.DunsNo = ServiceRequestorDuns;
                                pathed.PipelineID = PipelineId;
                                pathed.ShipperID = UserID != null ? Guid.Parse(UserID) : Guid.Empty;
                                
                                //pathed.PathedNomsList
                                #region Pathed Nomination

                                bool isrowempty = false;

                                for (int row = 10; row <= sheet.LastRowNum; row++)
                                {
                                    PathedNomDetailsDTO nom = new PathedNomDetailsDTO();

                                    #region Empty row Validation

                                    var rowofTable = sheet.GetRow(row);
                                    isrowempty = IsEmptyRow(rowofTable);
                                    if (isrowempty)
                                    {
                                        emptyRowCount++;
                                        continue;
                                    }

                                    #endregion

                                    if (sheet.GetRow(row) != null)
                                    {
                                        #region Pathed 
                                        #region
                                        string UpK = string.Empty;
                                        string SvcReq = string.Empty;
                                        if (sheet.GetRow(row).GetCell(1) != null)
                                        {
                                            var BegDate = sheet.GetRow(row).GetCell(20) != null ? (sheet.GetRow(row).GetCell(20).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(20).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(20).DateCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(20).StringCellValue.ToString().Trim() : string.Empty;
                                            if (string.IsNullOrEmpty(BegDate))
                                            {
                                                IsExit = true;
                                                continue;
                                            }
                                            var EndDate = sheet.GetRow(row).GetCell(21) != null ? (sheet.GetRow(row).GetCell(21).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(21).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(21).DateCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(21).StringCellValue.ToString().Trim() : string.Empty;

                                            if (sheet.GetRow(row).GetCell(22) != null && sheet.GetRow(row).GetCell(22).CellType == CellType.Formula)
                                            {
                                                SvcReq = sheet.GetRow(row).GetCell(22) != null ? (sheet.GetRow(row).GetCell(22).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(22).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(22).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                SvcReq = sheet.GetRow(row).GetCell(22) != null ? (sheet.GetRow(row).GetCell(22).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(22).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(22).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(22).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var NomSubCycle = sheet.GetRow(row).GetCell(23) != null ? sheet.GetRow(row).GetCell(23).StringCellValue.ToString().Trim() : string.Empty;
                                            var ActCD = sheet.GetRow(row).GetCell(24) != null ? sheet.GetRow(row).GetCell(24).StringCellValue.ToString().Trim() : string.Empty;
                                            var TT = sheet.GetRow(row).GetCell(25) != null ? sheet.GetRow(row).GetCell(25).CellType == CellType.Numeric ? sheet.GetRow(row).GetCell(25).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(25).StringCellValue.ToString().Trim() : string.Empty;
                                            var RecLoc2 = ""; //= (sheet.GetRow(row).GetCell(26).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(26).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(26).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(26).StringCellValue.ToString().Trim();
                                            if (sheet.GetRow(row).GetCell(26) != null && sheet.GetRow(row).GetCell(26).CellType == CellType.Formula)
                                            {
                                                RecLoc2 = sheet.GetRow(row).GetCell(26) != null ? (sheet.GetRow(row).GetCell(26).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(26).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(26).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                RecLoc2 = sheet.GetRow(row).GetCell(26) != null ? (sheet.GetRow(row).GetCell(26).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(26).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(26).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(26).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var UpIdProp = sheet.GetRow(row).GetCell(27) != null ? sheet.GetRow(row).GetCell(27).StringCellValue.ToString().Trim() : string.Empty;
                                            var UpId = sheet.GetRow(row).GetCell(28) != null ? sheet.GetRow(row).GetCell(28).CellType == CellType.Numeric ? sheet.GetRow(row).GetCell(28).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(28).StringCellValue.ToString().Trim() : string.Empty;

                                            if (sheet.GetRow(row).GetCell(29) != null && sheet.GetRow(row).GetCell(29).CellType == CellType.Formula)
                                            {
                                                UpK = sheet.GetRow(row).GetCell(29) != null ? (sheet.GetRow(row).GetCell(29).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(29).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(29).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                UpK = sheet.GetRow(row).GetCell(29) != null ? (sheet.GetRow(row).GetCell(29).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(29).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(29).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var UpActCd = sheet.GetRow(row).GetCell(30) != null ? sheet.GetRow(row).GetCell(30).StringCellValue.ToString().Trim() : string.Empty;
                                            var UpTT = sheet.GetRow(row).GetCell(31) != null ? sheet.GetRow(row).GetCell(31).StringCellValue.ToString().Trim() : string.Empty;
                                            var RecQty = sheet.GetRow(row).GetCell(32) != null ? sheet.GetRow(row).GetCell(32).StringCellValue.ToString().Trim() : string.Empty;
                                            var RecRank = sheet.GetRow(row).GetCell(33) != null ? sheet.GetRow(row).GetCell(33).CellType == CellType.Numeric ? sheet.GetRow(row).GetCell(33).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(33).StringCellValue.ToString().Trim() : string.Empty;
                                            var DelLoc = "";

                                            if (sheet.GetRow(row).GetCell(34) != null && sheet.GetRow(row).GetCell(34).CellType == CellType.Formula)
                                            {
                                                DelLoc = sheet.GetRow(row).GetCell(34) != null ? (sheet.GetRow(row).GetCell(34).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(34).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(34).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                DelLoc = sheet.GetRow(row).GetCell(34) != null ? (sheet.GetRow(row).GetCell(34).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(34).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(34).StringCellValue.ToString().Trim() : string.Empty;
                                            }


                                            var DnIdPrp = sheet.GetRow(row).GetCell(35) != null ? sheet.GetRow(row).GetCell(35).StringCellValue.ToString().Trim() : string.Empty;
                                            var DnId = sheet.GetRow(row).GetCell(36) != null ? sheet.GetRow(row).GetCell(36).CellType == CellType.Numeric ? sheet.GetRow(row).GetCell(36).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(36).StringCellValue.ToString().Trim() : string.Empty;
                                            var DnK = "";// (sheet.GetRow(row).GetCell(37).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(37).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(37).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(37).StringCellValue.ToString().Trim();
                                            if (sheet.GetRow(row).GetCell(37) != null && sheet.GetRow(row).GetCell(37).CellType == CellType.Formula)
                                            {
                                                DnK = sheet.GetRow(row).GetCell(37) != null ? (sheet.GetRow(row).GetCell(37).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(37).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(37).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                DnK = sheet.GetRow(row).GetCell(37) != null ? (sheet.GetRow(row).GetCell(37).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(37).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(37).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var DelQty = "";  // (sheet.GetRow(row).GetCell(40).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(40).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(40).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(40).StringCellValue.ToString().Trim();
                                            if (sheet.GetRow(row).GetCell(40) != null && sheet.GetRow(row).GetCell(40).CellType == CellType.Formula)
                                            {
                                                DelQty = sheet.GetRow(row).GetCell(40) != null ? (sheet.GetRow(row).GetCell(40).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(40).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(40).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                DelQty = sheet.GetRow(row).GetCell(40) != null ? (sheet.GetRow(row).GetCell(40).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(40).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(40).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var DelRank = "";// (sheet.GetRow(row).GetCell(41).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(41).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(41).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(41).StringCellValue.ToString().Trim();
                                            if (sheet.GetRow(row).GetCell(41) != null && sheet.GetRow(row).GetCell(41).CellType == CellType.Formula)
                                            {
                                                DelRank = sheet.GetRow(row).GetCell(41) != null ? (sheet.GetRow(row).GetCell(41).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(41).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(41).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                DelRank = sheet.GetRow(row).GetCell(41) != null ? (sheet.GetRow(row).GetCell(41).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(41).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(41).StringCellValue.ToString().Trim() : string.Empty;
                                            }

                                            var BegTime = "";// (sheet.GetRow(row).GetCell(42).CellType == CellType.Numeric || sheet.GetRow(row).GetCell(42).CellType == CellType.Formula) ? sheet.GetRow(row).GetCell(42).DateCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(42).StringCellValue.ToString().Trim();
                                            if (sheet.GetRow(row).GetCell(42) != null && sheet.GetRow(row).GetCell(42).CellType == CellType.Formula)
                                            {
                                                BegTime = sheet.GetRow(row).GetCell(42) != null ? (sheet.GetRow(row).GetCell(42).CachedFormulaResultType == CellType.Numeric) ? sheet.GetRow(row).GetCell(42).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(42).StringCellValue.ToString().Trim() : string.Empty;
                                            }
                                            else
                                            {
                                                if (DateUtil.IsCellDateFormatted(sheet.GetRow(row).GetCell(42)))
                                                {
                                                    TimeSpan time = sheet.GetRow(row).GetCell(42).DateCellValue.TimeOfDay;
                                                    BegTime = time.ToString();
                                                }
                                                else
                                                {
                                                    BegTime = sheet.GetRow(row).GetCell(42) != null ? (sheet.GetRow(row).GetCell(42).CellType == CellType.Numeric) ? sheet.GetRow(row).GetCell(42).NumericCellValue.ToString().Trim() : sheet.GetRow(row).GetCell(42).StringCellValue.ToString().Trim() : string.Empty;
                                                }
                                            }

                                            var PkgId = sheet.GetRow(row).GetCell(43) != null ? sheet.GetRow(row).GetCell(43).StringCellValue.ToString().Trim() : string.Empty;
                                            #endregion
                                            #region String filtered

                                            BegDate = GetFilteredString(BegDate);
                                            EndDate = GetFilteredString(EndDate);
                                            SvcReq = GetFilteredString(SvcReq);
                                            NomSubCycle = GetFilteredString(NomSubCycle);
                                            ActCD = GetFilteredString(ActCD);
                                            TT = GetFilteredString(TT);
                                            RecLoc2 = GetFilteredString(RecLoc2);
                                            UpIdProp = GetFilteredString(UpIdProp);
                                            UpId = GetFilteredString(UpId);
                                            UpK = GetFilteredString(UpK);
                                            UpActCd = GetFilteredString(UpActCd);
                                            UpTT = GetFilteredString(UpTT);
                                            RecQty = GetFilteredString(RecQty);
                                            RecRank = GetFilteredString(RecRank);
                                            DelLoc = GetFilteredString(DelLoc);
                                            DnIdPrp = GetFilteredString(DnIdPrp);
                                            DnId = GetFilteredString(DnId);
                                            DnK = GetFilteredString(DnK);
                                            DelQty = GetFilteredString(DelQty);
                                            DelRank = GetFilteredString(DelRank);
                                            BegTime = GetFilteredString(BegTime);
                                            PkgId = GetFilteredString(PkgId);

                                            CounterPartiesDTO counUp = null;
                                            if (!string.IsNullOrEmpty(UpIdProp))
                                                counUp = counterPartyService.GetAll().Where(a => a.PropCode == UpIdProp).FirstOrDefault();
                                            else if (!string.IsNullOrEmpty(UpId))
                                                counUp = counterPartyService.GetAll().Where(a => a.PropCode == UpId).FirstOrDefault();

                                            CounterPartiesDTO counDn = null;
                                            if (!string.IsNullOrEmpty(DnIdPrp))
                                                counDn = counterPartyService.GetAll().Where(a => a.PropCode == DnIdPrp).FirstOrDefault();
                                            else if (!string.IsNullOrEmpty(DnId))
                                                counDn = counterPartyService.GetAll().Where(a => a.PropCode == DnId).FirstOrDefault();


                                            #endregion
                                            nom.ActCode = ActCD;
                                            nom.AssocContract = "";
                                            nom.BidTransportRate = "";
                                            nom.BidUp = "";
                                            nom.CanWrite = true;
                                            nom.CapacityType = "";
                                            nom.CompanyID = companyID;
                                            nom.Contract = SvcReq;
                                            nom.CycleID = 1;
                                            nom.DealType = "";
                                            nom.DelLoc = DelLoc;
                                            nom.DelLocID = DelLoc;
                                            nom.DelLocProp = DelLoc;
                                            nom.DelQuantity = Convert.ToDecimal(DelQty);
                                            nom.DelRank = DelRank;
                                            nom.DownContract = DnK;

                                            if (counDn != null)
                                            {
                                                nom.DownID = counDn.Identifier;
                                                nom.DownIDProp = counDn.PropCode;
                                            }
                                            else
                                            {
                                                nom.DownID = DnId;
                                                nom.DownIDProp = DnIdPrp;
                                            }

                                            nom.DownName = "";
                                            nom.DownPkgID = "";
                                            nom.DownRank = "";
                                            nom.EndDate = new DateTime(DateTime.Parse(EndDate).Year, DateTime.Parse(EndDate).Month, DateTime.Parse(EndDate).Day, 9, 0, 0);// DateTime.Parse(EndDate);
                                            nom.Export = "";
                                            //nom.FuelPercentage = "";
                                            //nom.IsScheduled=
                                            nom.MaxRate = "";
                                            nom.NomSubCycle = NomSubCycle;
                                            //nom.NomTrackingId=
                                            nom.NomUserData1 = "";
                                            nom.NomUserData2 = "";
                                            nom.PipelineID = PipelineId;
                                            nom.PkgID = PkgId;
                                            nom.ProcessingRights = "";
                                            nom.RecLocation = RecLoc2;
                                            nom.RecLocID = RecLoc2;
                                            nom.RecLocProp = RecLoc2;
                                            nom.RecQty = RecQty;
                                            nom.RecRank = RecRank;
                                            //nom.re
                                            nom.ShipperDuns = ServiceRequestorDuns;
                                            nom.StartDate = new DateTime(DateTime.Parse(BegDate).Year, DateTime.Parse(BegDate).Month, DateTime.Parse(BegDate).Day, 9, 0, 0);
                                            nom.Status = "";
                                            nom.StatusID = 11;
                                            //nom.TransactionId=
                                            nom.TransType = TT;
                                            if (counUp != null)
                                            {
                                                nom.UpID = counUp.Identifier;
                                                nom.UpIDProp = counUp.PropCode;
                                            }
                                            else
                                            {
                                                nom.UpID = UpId;
                                                nom.UpIDProp = UpIdProp;
                                            }
                                            nom.UpKContract = UpK;
                                            nom.UpName = "";
                                            nom.UpPkgID = "";
                                            nom.UpRank = "";
                                            nom.QuantityType = "R";

                                            nom.CreatedDate = DateTime.Now;
                                            nom.StartTime = DateTime.Parse(BegTime);
                                            nom.ScheduledDateTime = DateTime.Now;

                                            pathed.PathedNomsList.Add(nom);

                                            #region Save Pathed Nomination
                                            #endregion
                                        }
                                        #endregion
                                    }

                                }
                                // _pathedService.SaveAndUpdatePathedNomination(pathed,true);
                                uploadNominationService.SavePathedBulkUpload(pathed, true);
                                #endregion

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    stream.Position = 0;
                    return Json("Oops! Something went wrong.");
                }
            }

            return Json("All files have been successfully stored.");
        }
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
            if ((sheet != null) && (sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market"))
            {
                row = sheet.GetRow(0);
            }
            else
            {
                row = sheet.GetRow(9);   // for pathed row number 10 is columns heading row.
            }

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
            if ((sheet != null) && (sheet.SheetName == "Supply" || sheet.SheetName == "Contract Paths" || sheet.SheetName == "Market"))
            {
                return colNames;
            }
            else
            {
                colNames = colNames.Skip(20).ToList(); // columns name starts from 21 column index in pathed.
                return colNames;
            }
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

        public string IsSupplyColsInSquence(List<string> supplywithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                   "Pipeline Duns",
                   "Start Date",
                   "End Date",
                   "CycleCode",
                    "Rec Loc Prop",
                    "TT",
                    "Svc Req K",
                    "Up ID Prop",
                    "Up ID",
                    "Rec Qty (Gross)",
                    "Up Rank/Del Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month"
            };
            if (supplywithColsName.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate for Supply.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = supplywithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in Supply. Check Column Names(Blank spaces not allow) and Sequence = " + arrToCompare[index];
                }
            }

            return wrongStr;
        }


        public string IsMarketColsInSquence(List<string> marketwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Pipeline Duns",
                    "Start Date",
                    "End Date",
                    "CycleCode",
                    "Del Loc Prop",
                    "TT",
                    "Svc Req K",
                    "Dn Id Prop",
                    "Dn ID",
                    "Del Qty (Net)",
                    "Dn Rank/Rec Rank",
                    "Pkg ID",
                    "Up/Dn Contract",
                    "Imbalance Month"
            };
            if (marketwithColsName.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate in Market.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = marketwithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in Market. Check Column Names(Blank spaces not allow) and Sequence = " + arrToCompare[index];
                }
            }

            return wrongStr;
        }


        public string IsContractPathColsInSquence(List<string> transportwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Pipeline Duns",
                    "Start Date",
                    "End Date",
                    "CycleCode",
                    "Svc Req K",
                    "TT",
                    "Rec Loc Prop",
                    "Dn Rank/Rec Rank",
                    "Rec Qty (Gross)",
                    "Del Loc Prop",
                    "Up Rank/Del Rank",
                    "Del Qty (Net)",
                    "Path Rank",
                    "Pkg ID",
                    "Imbalance Month",
                    "ROUTE (Required Only for Sonat, otherwise Leave Blank). Value Must be either 'Default' or 'Cypress'"
            };
            if (transportwithColsName.Count != colNames.Count)
            {
                wrongStr = "Columns count is not accurate in ContractPath.";
            }
            string[] accurateNamesArr = colNames.ToArray();
            string[] arrToCompare = transportwithColsName.ToArray();
            for (int index = 0; index < accurateNamesArr.Length; index++)
            {
                if (accurateNamesArr[index] != arrToCompare[index].Trim())
                {
                    return wrongStr = "Column sequence is not correct in ContractPath. Check Column Names(Blank spaces not allow) and Sequence = " + arrToCompare[index];
                }
            }

            return wrongStr;
        }

        public string IsPathedColsInSquence(List<string> PathedwithColsName)
        {
            string wrongStr = String.Empty;
            List<string> colNames = new List<string>() {
                    "Beg Date",
                    "End Date",
                    "Svc Req K",
                    "Nom Sub Cycle",
                    "*Act Cd",
                    "*TT",
                    "Rec Loc",
                    "Up ID Prop",
                    "Up ID",
                    "Up K",
                    "Up Act Cd",
                    "Up TT",
                    "Rec Qty",
                    "Rec Rank",
                    "Del Loc",
                    "Dn ID Prop",
                    "Dn ID",
                    "Dn K",
                    "Dn Act Cd",
                    "Dn TT",
                    "Del Qty",
                    "Del Rank",
                    "Beg Time",
                    "Pkg ID",
                    "Bid Up",
                    "Pipeline Duns"
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
                    return wrongStr = "Column sequence is not correct in pathed. Check Column Names(Blank spaces not allow) and Sequence for " + arrToCompare[index];
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



        public ActionResult Redirection(int PipelineId)
        {
            var pipeline = pipelineService.GetPipeline(PipelineId);
            if (pipeline.ModelTypeID == 1)
            {
                return RedirectToAction("Index", "PathedNomination", new { pipelineId = PipelineId });
            }
            else
            {
                return RedirectToAction("Index", "Batch", new { pipelineId = PipelineId });
            }
        }

    }   

}