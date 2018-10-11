using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Models;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class BatchController : BaseController
    {

        private readonly IPNTNominationService pntNominationService;
        private readonly IPathedNominationService pathedNominationService;
        ImetadataFileStatusService metadataFileStatusService;
        //IPipelineService pipelineService;
        ICycleIndicator ICycleIndicator;
        IBatchService batchService;
        INotifierEntityService _notifierEntityService;
        IPathedNominationService _pathedNominationService;
        ImetadataBidUpIndicatorService _metaDataBidUpIndicatorService;
        ImetadataCapacityTypeIndicatorService _metadataCapacityTypeIndicatorService;
        ImetadataQuantityTypeIndicatorService _metadataQuantityTypeIndicatorService;
        ImetadataExportDeclarationService _metadataExportDeclarationService;
        ICycleIndicator _cycleIndicator;
        INonPathedService nonPathedService;

        public BatchController(INonPathedService nonPathedService, ICycleIndicator _cycleIndicator, ImetadataExportDeclarationService _metadataExportDeclarationService, ImetadataQuantityTypeIndicatorService _metadataQuantityTypeIndicatorService, ImetadataCapacityTypeIndicatorService _metadataCapacityTypeIndicatorService, ImetadataBidUpIndicatorService _metaDataBidUpIndicatorService, IPathedNominationService _pathedNominationService, INotifierEntityService _notifierEntityService, ICycleIndicator ICycleIndicator,ImetadataFileStatusService metadataFileStatusService,IPNTNominationService pntNominationService, IPathedNominationService pathedNominationService, IPipelineService pipelineService, IBatchService batchService):base(pipelineService)
        {
            this._cycleIndicator = _cycleIndicator;
            this._metadataExportDeclarationService = _metadataExportDeclarationService;
            this._metadataQuantityTypeIndicatorService = _metadataQuantityTypeIndicatorService;
            this._metadataCapacityTypeIndicatorService = _metadataCapacityTypeIndicatorService;
            this._metaDataBidUpIndicatorService = _metaDataBidUpIndicatorService;
            this._notifierEntityService = _notifierEntityService;
            this.pntNominationService = pntNominationService;
            this.pathedNominationService = pathedNominationService;
            this.metadataFileStatusService=metadataFileStatusService;
            this.ICycleIndicator=ICycleIndicator;
            //this.pipelineService = pipelineService;
            this.batchService = batchService;
            this._pathedNominationService = _pathedNominationService;
            this.nonPathedService = nonPathedService;
        }


        public ActionResult Index(string pipelineDuns)
        {
            if (TempData["status"] != null)
            {
                ViewBag.Status = TempData["status"] + "";
            }
            PipelineDTO pipe = new PipelineDTO();
            if (Request["pipelineDuns"] == null || string.IsNullOrEmpty(pipelineDuns))
            {
                var pipes = GetPipelines();
                pipelineDuns = pipes.Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
                pipe = pipes.Count > 0 ? pipes.FirstOrDefault() : new PipelineDTO();
            }
            else
            {
                pipelineDuns = Request["pipelineDuns"] != null ? Request["pipelineDuns"].ToString() : pipelineDuns;
                var pipes = GetPipelines();
                pipe = pipes.Count > 0 ? pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault() : new PipelineDTO();
            }
            // var pipe = pipelineService.GetPipeline(pipelineId);
            if (pipe != null && (pipe.ModelTypeID == (int)NomType.Pathed || pipe.ModelTypeID == (int)NomType.HyPathedNonPathed))
               return RedirectToAction("Index", "PathedNomination", new { pipelineDuns = pipe.DUNSNo });
           else if (pipe != null && pipe.ModelTypeID == (int)NomType.NonPathed)
               return RedirectToAction("Index", "NonPathed", new { pipelineDuns = pipe.DUNSNo });

            ViewBag.PipelineDuns = pipelineDuns;
            ViewBag.ModelTypeID = pipe.ModelTypeID;
            ViewBag.Pipelinename = pipe.Name;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);//GetNotifierEntityofBatchTable();
            ViewBag.NotifierEntity = notifier;
            return View();
        }

        [HttpGet]
        public PartialViewResult _Batch(string pipelineDuns)
        {
           
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;         
            string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault();

            var UserKey = new Guid(identity.Claims.Where(c => c.Type == "UserId")
                             .Select(c => c.Value).SingleOrDefault());
           
            BatchNomCollection batchListModel = new BatchNomCollection();
            batchListModel.BatchList = new List<BatchDTO>();
            batchListModel.pipelineDuns = pipelineDuns;
            batchListModel.showMine = Session["showMinebatch"]==null? true : (bool)Session["showMinebatch"];
            DateTime todayDate = DateTime.Now.Date;
            var currentmonth = todayDate.Month;
            batchListModel.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            batchListModel.EndDate = batchListModel.StartDate.AddMonths(2);
            batchListModel.StatusId = -1;           
            List<BatchDTO> batchList = pntNominationService.GetBatches("", pipelineDuns, batchListModel.StatusId, todayDate.AddDays(-15), todayDate.AddDays(15), 1, 1000, batchListModel.showMine ? UserKey : Guid.Empty, shipperDuns).ToList();
            batchListModel.BatchList = batchList.OrderByDescending(a => a.CreatedDate).ToList();
            var Status = metadataFileStatusService.GetNomStatus();
            ViewBag.StatusID = Status;
            var notifier = _notifierEntityService.GetNotifierEntityForNoms(UserKey.ToString());//.GetNotifierEntityofBatchTable();
            ViewBag.NotifierEntity = notifier;
            return PartialView(batchListModel);
           // return this.View(batchListModel);
        }

        // [HttpPost]
        public ActionResult GetBatch(string pipelineDuns, DateFilter datefilter)
        {
            DateTime startDate = datefilter.StartDate;
            DateTime endDate = datefilter.EndDate;//.AddHours(24);
            string Status = string.IsNullOrEmpty(datefilter.status) ? "-1" : datefilter.status;
            int StatusId = Convert.ToInt32(Status);

            JsonResult result = new JsonResult();
            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = 20; //Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                
                DateTime todayDate = DateTime.Now.Date;
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                                   .Select(c => c.Value).SingleOrDefault();

                var UserKey = Guid.Parse(identity.Claims.Where(c => c.Type == "UserId")
                                 .Select(c => c.Value).SingleOrDefault());
                //TODO:
                List<BatchDTO> batchList = pntNominationService.GetBatches("", pipelineDuns, StatusId, startDate, endDate, 1, 1000, datefilter.showMine ? UserKey : Guid.Empty, shipperDuns).ToList();

                if (batchList != null) {
                    batchList = batchList.OrderByDescending(a => a.CreatedDate).ToList();
                    batchList = GetPipelineNames(batchList,pipelineDuns);
                }
                  
                // Total record count.
                int totalRecords = batchList.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    batchList = batchList.Where(p => p.DateBeg.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.DateEnd.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Description.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Cycle.ToLower().Contains(search.ToLower()) ||
                                           p.ScheduledDate.ToString().Contains(search.ToLower()) ||
                                           p.CreatedDate.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Status.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                batchList = this.SortByColumnWithOrder(order, orderDir, batchList);

                // Filter record count.
                int recFilter = batchList.Count;

                // Apply pagination.
                batchList = batchList.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = batchList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public List<BatchDTO> GetPipelineNames(List<BatchDTO> list,string pipelineDuns)
        {
            var pipelineList = GetPipelines();
            var pipeline = pipelineList.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault();
            if (pipeline != null) {
                foreach (var item in list) { item.PipelineName = pipeline.Name; }
            }
            return list;
        }



        #region Sort by column with order method

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        private List<BatchDTO> SortByColumnWithOrder(string order, string orderDir, List<BatchDTO> data)
        {
            // Initialization.
            List<BatchDTO> lst = new List<BatchDTO>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StatusID).ToList()
                                                                                                 : data.OrderBy(p => p.StatusID).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PipelineName).ToList()
                                                                                                 : data.OrderBy(p => p.PipelineName).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateBeg).ToList()
                                                                                                 : data.OrderBy(p => p.DateBeg).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateEnd).ToList()
                                                                                                 : data.OrderBy(p => p.DateEnd).ToList();
                        break;

                    case "5":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList()
                                                                                                   : data.OrderBy(p => p.Description).ToList();
                        break;

                    case "6":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cycle).ToList()
                                                                                                 : data.OrderBy(p => p.Cycle).ToList();
                        break;

                    case "7":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedDate).ToList()
                                                                                                 : data.OrderBy(p => p.CreatedDate).ToList();
                        break;
                    case "8":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreaterName).ToList()
                                                                                                 : data.OrderBy(p => p.CreaterName).ToList();
                        break;

                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledDate).ToList()
                                                                                                 : data.OrderBy(p => p.ScheduledDate).ToList();
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
        

        [HttpPost]
        public PartialViewResult _Batch(BatchNomCollection batchListModel, string Search)
        {
            Session["showMinebatch"] = batchListModel.showMine;
            if (Search == null)
            {
                return null;
            }
            else
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                                   .Select(c => c.Value).SingleOrDefault();
                var UserKey = Guid.Parse(identity.Claims.Where(c => c.Type == "UserId")
                                 .Select(c => c.Value).SingleOrDefault());
                batchListModel.BatchList = new List<BatchDTO>();
                List<BatchDTO> batchList = pntNominationService.GetBatches("", batchListModel.pipelineDuns, batchListModel.StatusId, batchListModel.StartDate, batchListModel.EndDate, 1, 1000, batchListModel.showMine? UserKey:Guid.Empty, shipperDuns).ToList();
                batchListModel.BatchList = batchList.OrderByDescending(a => a.CreatedDate).ToList();
                var Status = metadataFileStatusService.GetNomStatus();
                ViewBag.StatusID = Status;
                var notifier = _notifierEntityService.GetNotifierEntityForNoms(UserKey.ToString());//GetNotifierEntityofBatchTable();
                ViewBag.NotifierEntity = notifier;
                return PartialView("_Batch",batchListModel);
                //return this.View(batchListModel);
            }
        }

        public ActionResult Save(string pipelineDuns)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.Duns = pipelineDuns;
            model.Description = string.Empty;
            model.StartDateTime = DateTime.Now.Date;
            model.EndDateTime = DateTime.Now.Date;
            model.CycleId = 1;
            var CycleIndicator = ICycleIndicator.GetCycles();
            ViewBag.Cycles = new SelectList(CycleIndicator.AsEnumerable(), "Id", "Name", 1);


            var pipes = GetPipelines();
            var pipe = pipes.Count > 0 ? pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault() : new PipelineDTO();

           // NomType modelType = pipelineService.GetPathTypeByPipelineDuns(pipelineService.GetDunsByPipelineID(pipelineId));
            model.PipelineModelType = GetPathTypeByPipelineType(pipe.ModelTypeID);

            return this.View(model);
        }


        public NomType GetPathTypeByPipelineType(int ModelTypeID)
        {
            NomType modelType = new NomType();
            try
            {
                if (ModelTypeID != 0)
                {
                    if (ModelTypeID == 1) { modelType = NomType.Pathed; }
                    else if (ModelTypeID == 2) { modelType = NomType.PNT; }
                    else if (ModelTypeID == 3) { modelType = NomType.NonPathed; }
                    else if (ModelTypeID == 4) { modelType = NomType.HyPathedNonPathed; }
                    else if (ModelTypeID == 5) { modelType = NomType.HyPathedPNT; }
                    else if (ModelTypeID == 6) { modelType = NomType.HyNonPathedPNT; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Pathtype not found for ModelType.");
            }
            return modelType;
        }

        public bool SendNom(Guid TransactionId)
        {
            return true;
        }


        [HttpPost]
        public ActionResult Save(BatchDetailDTO model)
        {
            //if (model.CycleId == 1 || model.CycleId == 2) // timely
            //{
            //    model.BegginingTime = "09:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 3)//intrs day 1
            //{
            //    model.BegginingTime = "14:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 4)//intrs day 2
            //{
            //    model.BegginingTime = "18:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 5)//intrs day 3
            //{
            //    model.BegginingTime = "22:00";
            //    model.EndTime = "09:00";
            //}

            //var StartTime = TimeSpan.Parse(model.BegginingTime);
            //var EndTime = TimeSpan.Parse(model.EndTime);

           // model.StartDateTime = model.StartDateTime.Add(StartTime);
           // model.EndDateTime = model.EndDateTime.Add(EndTime);
            model.ShowZeroCheck = true;
            model.RankingCheck = true;
            model.PakageCheck = true;
            model.UpDnContractCheck = false;
            model.ShowZeroUp = true;
            model.ShowZeroDn = true;
            model.UpDnPkgCheck = false;
            model.ScheduleDate = DateTime.MaxValue;
            model.SubmittedDate = DateTime.MaxValue;
            //TODO:
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault();
            var UserKey = Guid.Parse(identity.Claims.Where(c => c.Type == "UserId")
                             .Select(c => c.Value).SingleOrDefault());
            model.StatusId = Convert.ToInt32(NomStatus.Draft);
            model.CreatedBy = UserKey.ToString();
            model.CreatedDateTime = DateTime.Now;
            model.ShiperDuns = shipperDuns;
            Guid? transactionId = pntNominationService.SaveAndUpdatePNTBatchDetail(model, true);//.SaveAndUpdateBatchWithDetail(model, true);
                        
            return RedirectToAction("Index", "PNTNominations", new { BatchId = transactionId, pipelineDuns = model.Duns });
        }

        public ActionResult Edit(string pipelineDuns, Guid batchId)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model = pntNominationService.GetNomDetail(batchId, pipelineDuns);
            model.Duns = pipelineDuns;
            var CycleIndicator = ICycleIndicator.GetCycles();
            ViewBag.Cycles = new SelectList(CycleIndicator.AsEnumerable(), "Id", "Name",model.CycleId);
            model.EndDateTime = model.EndDateTime;
            // in DB,Stored One day more than actual date of End date.

            //NomType modelType = pipelineService.GetPathTypeByPipelineDuns(pipelineService.GetDunsByPipelineID(pipelineId));
           // model.PipelineModelType = modelType;
            var pipes = GetPipelines();
            var pipe = pipes.Count > 0 ? pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault() : new PipelineDTO();           
            model.PipelineModelType = GetPathTypeByPipelineType(pipe.ModelTypeID);
            return this.PartialView(model);
        }


        [HttpPost]
        public ActionResult Edit(BatchDetailDTO model)
        {
            //if (model.CycleId == 1 || model.CycleId == 2) // timeley
            //{
            //    model.BegginingTime = "09:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 3)//intrs day 1
            //{
            //    model.BegginingTime = "14:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 4)//intrs day 2
            //{
            //    model.BegginingTime = "18:00";
            //    model.EndTime = "09:00";
            //}
            //else if (model.CycleId == 5)//intrs day 3
            //{
            //    model.BegginingTime = "22:00";
            //    model.EndTime = "09:00";
            //}

            //var StartTime = TimeSpan.Parse(model.BegginingTime);
            //var EndTime = TimeSpan.Parse(model.EndTime);

           // model.StartDateTime = model.StartDateTime.Add(StartTime);
           // model.EndDateTime = model.EndDateTime.Add(EndTime);
            model.ShowZeroCheck = true;
            model.RankingCheck = true;
            model.PakageCheck = true;
            model.UpDnContractCheck = false;
            model.ShowZeroUp = true;
            model.ShowZeroDn = true;
            model.UpDnPkgCheck = false;
            model.ScheduleDate = DateTime.MaxValue;
            model.SubmittedDate = DateTime.MaxValue;
            //TODO:
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault();
            var UserKey = Guid.Parse(identity.Claims.Where(c => c.Type == "UserId")
                             .Select(c => c.Value).SingleOrDefault());
            model.StatusId = Convert.ToInt32(NomStatus.Draft);
            model.CreatedBy = UserKey.ToString();
            model.CreatedDateTime = DateTime.Now;
            model.ShiperDuns = shipperDuns;
            Guid? transactionId = pntNominationService.UpdatePNTBatch(model); //SaveAndUpdatePNTBatchDetail(model, true);//.SaveAndUpdateBatchWithDetail(model, true);

            return RedirectToAction("Index", new { pipelineDuns = model.Duns });
        }


        public bool SendNomination(List<string> transactionIDs)
        {
            bool sendToTest = Convert.ToBoolean(ConfigurationManager.AppSettings["SendToTest"]);
            bool results = false;
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);
            List<bool> tempBool = new List<bool>();

            foreach (var transactionID in transactionIDs)
            {
                BatchDTO batch=batchService.GetBatch(Guid.Parse(transactionID));
                if (batch == null || batch.StatusID != (int)NomStatus.Draft || batch.DateBeg.Date < DateTime.Now.Date || batch.DateEnd.Date < DateTime.Now.Date)
                {
                    tempBool.Add(false);
                }
                else 
                {
                    bool result;
                    if (batchService.ValidateNomination(Guid.Parse(transactionID), batch.pipeDUNSNo))
                    {
                        result = pntNominationService.SendNominationTransaction(new Guid(transactionID), companyID, sendToTest);
                    }
                    else
                        result = false;
                    tempBool.Add(result);
                }               
            }
            if ((tempBool.Where(a => a == false).Count()) == 0)
            {
                results = true;
            }
            return results;
        }


        public bool DeleteBatchNom(List<string> transactionIDs)
        {
            bool isDelete = false;
            if (transactionIDs != null && transactionIDs.Count() > 0)
            {
                List<bool> tempBool = new List<bool>();
                //TODO:
                foreach (var transactionID in transactionIDs)
                {

                    var isDeleted = pathedNominationService.DeleteNominationData(new Guid(transactionID));
                    tempBool.Add(isDeleted);
                }
                if ((tempBool.Where(a => a == false).Count()) == 0)
                {
                    isDelete = true;
                }
            }
            return isDelete;
        }


        public bool CopyNomination(Guid transactionID, string pipelineDuns)
        {
            bool result = false;
            string message = ""; 
            result = pntNominationService.CopyNomination(transactionID);
            return result;
        }


        public PartialViewResult NotimationsPartials(string partial, string popUpFor)
        {
            NominationPartialDTO model = new NominationPartialDTO();
            //int companyId = Convert.ToInt32(Session["ShipperCompanyId"]);

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);


            string partialView = string.Empty;
            model.PopUpFor = popUpFor;
            if (partial == "StatusReason")
            {
                var transactionId = popUpFor;               
                model.StatusReason = pntNominationService.GetRejectionReason(Guid.Parse(transactionId)).ToList();
                partialView = "~/Views/PNTNominations/_StatusReasonPopUp.cshtml";
            }
            return PartialView(partialView, model);
        }


        [HttpGet]
        public ActionResult LoadHybridPathedPartial(string pipelineDuns)
        {
            PathedDTO model = new PathedDTO();
            model.PathedNomsList = new List<PathedNomDetailsDTO>() { new PathedNomDetailsDTO() { StartDate = DateTime.Now, EndDate = DateTime.Now, CycleID = 1 } };
            model.PipelineDuns = pipelineDuns;
            DateTime todayDate = DateTime.Now.Date;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            int status = -1;
            int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(model.PipelineDuns, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));

            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(model.PipelineDuns, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId), new SortingPagingInfo() { CurrentPageIndex = 0, PageSize = pathedListTotalCount== 0 ? 1: pathedListTotalCount });
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);

            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;
            return PartialView("_Pathedhybrid", model);
        }

        [HttpPost]
        public ActionResult LoadHybridPathedPartial(PathedDTO model)
        {          
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.DunsNo = currentIdentityValues.ShipperDuns;
            model.companyId = String.IsNullOrEmpty(currentIdentityValues.CompanyId) ? 0 : int.Parse(currentIdentityValues.CompanyId);
            model.ShipperID = new Guid(currentIdentityValues.UserId);
            if (model.PathedNomsList == null)
            {
                TempData["status"] = "Please fill row.";
                // return RedirectToAction("Index", new { BatchId = Mainmodel.Id, pipelineId = model.PipelineID });
                return RedirectToAction("Index", new { pipelineDuns = model.PipelineDuns });
            }
            Guid? transactionID = _pathedNominationService.SaveAndUpdatePathedNomination(model, true);
            if (transactionID != null)
                TempData["status"] = "Data saved successfully";
            else
            {
                TempData["status"] = "Data saving failed";
            }
            //  return RedirectToAction("Index", new { BatchId = Mainmodel.Id, pipelineId = model.PipelineID });
            return RedirectToAction("Index",new { pipelineDuns = model.PipelineDuns}); 
        }

        public ActionResult AddPathedHybrid(string pipelineDuns)
        {
            PathedDTO mainmodel = new PathedDTO();
            mainmodel.PipelineDuns = pipelineDuns;
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;

            mainmodel.PathedNomsList = new List<PathedNomDetailsDTO>();

            var item = new PathedNomDetailsDTO();
            DateTime todayDate = DateTime.Now.Date;
            item.StartDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 09, 00, 00);// todayDate;
            item.EndDate = new DateTime(todayDate.AddDays(1).Year, todayDate.AddDays(1).Month, (todayDate.AddDays(1).Day), 09, 00, 00);
            item.CycleID = 1;
            item.NomSubCycle = "Y";
            item.MaxRate = "Y";
            item.ProcessingRights = "Y";
            item.CreatedDate = DateTime.Now;
            item.QuantityType = "R";
            mainmodel.PathedNomsList.Add(item);

            return PartialView("_AddHybridPathedRow", mainmodel);
        }

        [HttpPost]
        public PartialViewResult CopyHybridPathedRow(List<PathedNomDetailsDTO> PathedRecordToCopy, string pipelineDuns)
        {
            PathedDTO model = new PathedDTO();
            model.PipelineDuns = pipelineDuns;
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status1 = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status1;

            var item = new PathedNomDetailsDTO();
            var today = DateTime.Now;
            if (PathedRecordToCopy[0].StartDate < today)
            {
                var time = PathedRecordToCopy[0].StartDate.TimeOfDay;
                item.StartDate = today.Date.Add(time);
            }
            else
            {
                item.StartDate = PathedRecordToCopy[0].StartDate;
            }

            if (PathedRecordToCopy[0].EndDate < today)
            {
                var nextDay = DateTime.Now.AddDays(1);
                var time1 = PathedRecordToCopy[0].EndDate.TimeOfDay;
                item.EndDate = nextDay.Date.Add(time1);
            }
            else
            {
                item.EndDate = PathedRecordToCopy[0].EndDate;
            }
            if (item.EndDate < item.StartDate)
            {
                var nextDay = item.StartDate.AddDays(1);
                var time1 = PathedRecordToCopy[0].EndDate.TimeOfDay;
                item.EndDate = nextDay.Date.Add(time1);
            }
            // item.StartDate = PathedRecordToCopy[0].StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : PathedRecordToCopy[0].StartDate.Date;
            // item.EndDate = PathedRecordToCopy[0].EndDate.Date < DateTime.Now.Date ? DateTime.Now.AddDays(1).Date : PathedRecordToCopy[0].EndDate.Date;
            item.CycleID = PathedRecordToCopy[0].CycleID;
            item.Contract = PathedRecordToCopy[0].Contract;
            item.NomSubCycle = PathedRecordToCopy[0].NomSubCycle;
            item.TransType = PathedRecordToCopy[0].TransType;
            item.TransTypeMapId = PathedRecordToCopy[0].TransTypeMapId;
            item.RecLocation = PathedRecordToCopy[0].RecLocation;
            item.RecLocProp = PathedRecordToCopy[0].RecLocProp;
            item.RecLocID = PathedRecordToCopy[0].RecLocID;
            item.UpName = PathedRecordToCopy[0].UpName;
            item.UpIDProp = PathedRecordToCopy[0].UpIDProp;
            item.UpID = PathedRecordToCopy[0].UpID;
            item.DelLoc = PathedRecordToCopy[0].DelLoc;
            item.DelLocID = PathedRecordToCopy[0].DelLocID;
            item.DelLocProp = PathedRecordToCopy[0].DelLocProp;
            item.DownID = PathedRecordToCopy[0].DownID;
            item.DownIDProp = PathedRecordToCopy[0].DownIDProp;
            item.DownName = PathedRecordToCopy[0].DownName;
            item.QuantityType = PathedRecordToCopy[0].QuantityType;
            item.MaxRate = PathedRecordToCopy[0].MaxRate;
            item.CapacityType = PathedRecordToCopy[0].CapacityType;
            item.BidUp = PathedRecordToCopy[0].BidUp;
            item.Export = PathedRecordToCopy[0].Export;
            item.ProcessingRights = PathedRecordToCopy[0].ProcessingRights;

            item.UpKContract = PathedRecordToCopy[0].UpKContract;
            item.RecQty = PathedRecordToCopy[0].RecQty;
            item.RecRank = PathedRecordToCopy[0].RecRank;

            item.DownContract = PathedRecordToCopy[0].DownContract;
            item.DelQuantity = PathedRecordToCopy[0].DelQuantity;
            item.DelRank = PathedRecordToCopy[0].DelRank;

            item.PkgID = PathedRecordToCopy[0].PkgID;
            item.NomUserData1 = PathedRecordToCopy[0].NomUserData1;
            item.NomUserData2 = PathedRecordToCopy[0].NomUserData2;
            item.ActCode = PathedRecordToCopy[0].ActCode;
            item.BidTransportRate = PathedRecordToCopy[0].BidTransportRate;
            item.AssocContract = PathedRecordToCopy[0].AssocContract;
            item.DealType = PathedRecordToCopy[0].DealType;
            item.FuelPercentage = PathedRecordToCopy[0].FuelPercentage;

            item.UpPkgID = PathedRecordToCopy[0].UpPkgID;
            item.DownPkgID = PathedRecordToCopy[0].DownPkgID;
            item.UpRank = PathedRecordToCopy[0].UpRank;
            item.DownRank = PathedRecordToCopy[0].DownRank;

            item.CreatedDate = DateTime.Now;

            model.PathedNomsList = new List<PathedNomDetailsDTO>();
            model.PathedNomsList.Add(item);

            return PartialView("_AddHybridPathedRow", model);
        }

        [HttpGet]
        public ActionResult LoadHybridNonPathedPartial(string pipelineDuns)
        {
            string PipelineDuns = "";
            DateTime todayDate = DateTime.Now.Date;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            int status = -1;
           
            NonPathedDTO model = nonPathedService.GetNonPathedNominations(PipelineDuns, status, DateTime.MinValue, DateTime.MaxValue, currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));
            model = UpdateCounterPartyAndLocNameInNonPathed(model);
            if (model == null) {
                model = new NonPathedDTO();
            }
            model.PipelineDuns = pipelineDuns;
            var CycleIndicator = _cycleIndicator.GetCycles();
            ViewBag.Cycles = CycleIndicator;
            return PartialView("_NonPathedHybrid", model);
        }


        [HttpPost]
        public ActionResult SaveNonPathedHybrid(NonPathedDTO model)
        {            
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.UserId = Guid.Parse(currentIdentityValues.UserId);
           // var pipe = pipelineService.GetPipeline(model.PipelineId);
            model.PipelineDuns = model.PipelineDuns;
            model.CompanyId = Convert.ToInt32(currentIdentityValues.CompanyId ?? "0");
            bool result = false;
            var Id = nonPathedService.SaveAllNonPathedNominations(model);
            result = (Id == Guid.Empty) ? false : true;
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            ViewBag.SubmitStatus = result;
            if (result)
                TempData["status"] = "Data saved successfully";
            else
            {
                TempData["status"] = "Data saving failed";
            }
            return RedirectToAction("Index", new {  pipelineDuns = model.PipelineDuns });
        }


        public PartialViewResult AddReceiptRow(string PipelineDuns)
        {
            NonPathedDTO model = new NonPathedDTO();
            model.PipelineDuns = PipelineDuns;
            model.ReceiptNoms.Add(new NonPathedRecieptNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddReceiptRowHybrid", model);
        }

        public PartialViewResult AddDeliveryRow(string PipelineDuns)
        {

            NonPathedDTO model = new NonPathedDTO();
            model.PipelineDuns = PipelineDuns;
            model.DeliveryNoms.Add(new NonPathedDeliveryNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddDeliveryRowHybrid", model);
        }

    }
}