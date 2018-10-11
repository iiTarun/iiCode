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
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class NonPathedController : BaseController
    {
        // IPipelineService pipelineService;
        IPNTNominationService IPNTNominationService;
        IPathedNominationService pathedNominationService;
        ICycleIndicator ICycleIndicator;
        INonPathedService nonPathedService;
        IBatchService batchService;
        ImetadataFileStatusService metadataFileStatusService;
        INotifierEntityService _notifierEntityService;


        public NonPathedController(INotifierEntityService _notifierEntityService, IPathedNominationService pathedNominationService,ImetadataFileStatusService metadataFileStatusService,ICycleIndicator ICycleIndicator, IPNTNominationService IPNTNominationService, IPipelineService pipelineService, INonPathedService nonPathedService, IBatchService batchService) :base(pipelineService)
        {
            this._notifierEntityService = _notifierEntityService;
            this.pathedNominationService = pathedNominationService;
            this.metadataFileStatusService = metadataFileStatusService;
            this.ICycleIndicator = ICycleIndicator;
            this.IPNTNominationService = IPNTNominationService;
           // this.pipelineService = pipelineService;
            this.nonPathedService = nonPathedService;
            this.batchService = batchService;

        }

        public ActionResult Index(string pipelineDuns)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
           
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

            if (pipe != null && (pipe.ModelTypeID == (int)NomType.Pathed || pipe.ModelTypeID == (int)NomType.HyPathedNonPathed))
                return RedirectToAction("Index", "PathedNomination", new { pipelineDuns = pipe.DUNSNo });
            else if (pipe != null && (pipe.ModelTypeID == (int)NomType.PNT || pipe.ModelTypeID == (int)NomType.HyNonPathedPNT || pipe.ModelTypeID == (int)NomType.HyPathedPNT))
                return RedirectToAction("Index", "Batch", new { pipelineDuns = pipe.DUNSNo });
                   

            NonPathedDTO model = new NonPathedDTO();

            DateTime todayDate = DateTime.Now.Date;
            model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.UserId = Guid.Parse(currentIdentityValues.UserId);
            int status = -1;
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.PipelineDuns = pipe.DUNSNo;           
            model = nonPathedService.GetNonPathedNominations(pipe.DUNSNo, status,model.StartDate,model.EndDate,model.ShipperDuns,model.UserId.GetValueOrDefault());
            model = UpdateCounterPartyAndLocNameInNonPathed(model);           
            model.PipelineDuns = pipe.DUNSNo;
            ViewBag.StatusID = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = ICycleIndicator.GetCycles();
            var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);
            ViewBag.NotifierEntity = notifier;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(NonPathedDTO model,string Search)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.UserId = Guid.Parse(currentIdentityValues.UserId);
            string pipelineDuns = model.PipelineDuns;
            if (Search == null)
            {                
                model.CompanyId = Convert.ToInt32(currentIdentityValues.CompanyId ?? "0");
                bool result = false;
                var Id = nonPathedService.SaveAllNonPathedNominations(model);
                result = (Id == Guid.Empty) ? false : true;
                ViewBag.Cycles = ICycleIndicator.GetCycles();
                ViewBag.SubmitStatus = result;
                return RedirectToAction("Index", new { pipelineDuns = model.PipelineDuns });
            }
            else {
                model = nonPathedService.GetNonPathedNominations(model.PipelineDuns, model.StatusId, model.StartDate, model.EndDate, model.ShipperDuns, model.UserId.GetValueOrDefault());
                model = UpdateCounterPartyAndLocNameInNonPathed(model);
                model.PipelineDuns = pipelineDuns;
                ViewBag.StatusID = metadataFileStatusService.GetNomStatus();
                ViewBag.Cycles = ICycleIndicator.GetCycles();
                var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);//.GetNotifierEntityofBatchTable();
                ViewBag.NotifierEntity = notifier;
                return View(model);

            }
        }
       

        [HttpPost]
        public bool SendNomination(List<string> transactionIDs)
        {
            bool sendToTest = Convert.ToBoolean(ConfigurationManager.AppSettings["SendToTest"]);
            bool results = false;

            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            int companyId = String.IsNullOrEmpty(currentIdentity.CompanyId) ? 0 : int.Parse(currentIdentity.CompanyId);

            List<bool> tempBool = new List<bool>();
            int statusId = 0;
            foreach (var transactionID in transactionIDs)
            {
                statusId = pathedNominationService.GetStatusOnTransactionId(new Guid(transactionID));
                if (statusId == 11) {
                    var result = IPNTNominationService.SendNominationTransaction(new Guid(transactionID), companyId, sendToTest);
                    tempBool.Add(result);
                }
                else {
                    tempBool.Add(false);
                }
            }
            if ((tempBool.Where(a => a == false).Count()) == 0)
            {
                results = true;
            }
            return results;
        }


        public PartialViewResult AddReceiptRow(string pipelineDuns)
        {
            NonPathedDTO model = new NonPathedDTO();
            model.PipelineDuns = pipelineDuns;
           // model.Duns = pipelineService.GetDunsByPipelineID(PipelineID);           
            model.ReceiptNoms.Add(new NonPathedRecieptNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.StatusID = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = ICycleIndicator.GetCycles();
            return PartialView("_AddReceiptRow", model);
        }
        public PartialViewResult AddDeliveryRow(string pipelineDuns)
        {
            NonPathedDTO model = new NonPathedDTO();
            model.PipelineDuns = pipelineDuns;
            // model.Duns = pipelineService.GetDunsByPipelineID(PipelineID);           
            model.DeliveryNoms.Add(new NonPathedDeliveryNom() { StartDateTime= DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime=DateTime.Now.Date });
            ViewBag.StatusID = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = ICycleIndicator.GetCycles();
            return PartialView("_AddDeliveryRow", model);
        }
        public PartialViewResult NotimationsPartials(string partial, string clickedRow, string popUpFor, string pipelineDuns)
        {
            NominationPartialDTO model = new NominationPartialDTO();
            string partialView = string.Empty;
            model.ForRow = clickedRow;

            var currentidentity = GetValueFromIdentity();            
            int companyId =int.Parse(currentidentity.CompanyId ?? "0");
            model.PopUpFor = popUpFor;
            model.PipelineDuns = pipelineDuns;
            if (partial.ToLower() == "locations")
            {
                List<LocationsDTO> locationList = new List<LocationsDTO>();
                model.Locations = new List<LocationsDTO>();
                // var Pipelineduns = pipelineService.GetDunsByPipelineID(PipelineID);
                // locationList = IPNTNominationService.GetLocations(string.Empty, Pipelineduns, 1, 1000, string.Empty).ToList();
                //if (popUpFor == "ContractPath" || popUpFor == "Supply" ||popUpFor== "Receipt")
                //{
                //    model.Locations = locationList.Where(a => (a.RDB == "R" || a.RDB == "B")).ToList();

                //}
                //else if (popUpFor == "DelContractPath" || popUpFor == "Market"|| popUpFor == "Delivery")
                //{
                //    model.Locations = locationList.Where(a => (a.RDB == "D" || a.RDB == "B")).ToList();
                //}
                ViewBag.IsSpecialDelCase = false;
                ViewBag.PopUpFor = popUpFor;
                partialView = "~/Views/PNTNominations/_LocationPopUp.cshtml";
            }
            else if (partial == "TransactionType")
            {
                model.TransactionTypes = new List<TransactionTypesDTO>();
               
                model.TransactionTypes = IPNTNominationService.GetTransactionsTypes(pipelineDuns, "", model.PopUpFor).ToList();
                model.TransactionTypes = model.TransactionTypes.OrderBy(a => Convert.ToInt32(a.Identifier)).ToList();
                partialView = "~/Views/PNTNominations/_TransactionTypePopUp.cshtml";
            }
            else if (partial == "CounterParties")
            {
                model.CounterParties = new List<CounterPartiesDTO>();                
                partialView = "~/Views/PNTNominations/_CounterPartyPopUp.cshtml";
            }
            else if (partial == "Contract")
            {
                model.Contracts = new List<ContractsDTO>();
              
                model.Contracts = IPNTNominationService.GetContracts( companyId, pipelineDuns).ToList();
                model.PipelineDuns = pipelineDuns;
                partialView = "~/Views/PNTNominations/_ContractPopUp.cshtml";
            }
            else if (partial == "StatusReason")
            {
                var transactionId = popUpFor;
                model.StatusReason = IPNTNominationService.GetRejectionReason(Guid.Parse(transactionId)).ToList();
                partialView = "~/Views/PNTNominations/_StatusReasonPopUp.cshtml";
            }
            return PartialView(partialView, model);
        }
       

        //public ActionResult NonPathedBatches(int pipelineId)
        //{
        //    if (TempData["status"] != null)
        //    {
        //        ViewBag.Status = TempData["status"] + "";
        //    }
        //    var pipe = pipelineService.GetPipeline(pipelineId);
        //    if (pipe != null && pipe.ModelTypeID == (int)NomType.Pathed)
        //        return RedirectToAction("Index", "PathedNomination", new { pipelineId = pipe.ID });
        //    else if (pipe != null && pipe.ModelTypeID == (int)NomType.PNT)
        //        return RedirectToAction("Index", "Batch", new { pipelineId = pipe.ID });
        //    NonPathedBatchListDTO model = new NonPathedBatchListDTO();
        //    model.PipelineId = pipelineId;
        //    ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
        //    var shipperDuns = currentIdentity.ShipperDuns;
        //    var userId = currentIdentity.UserId;

        //    DateTime todayDate = DateTime.Now.Date;
        //    var currentmonth = todayDate.Month;
        //    model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    model.EndDate = model.StartDate.AddMonths(2);
        //    model.StatusId = -1;
        //    model.BatchDtoList = IPNTNominationService.GetBatches("", pipe.DUNSNo, 0, todayDate.AddDays(-15), todayDate.AddDays(15), 1, 1000, new Guid(userId), shipperDuns).ToList(); // new List<BatchDTO>();// = IWatchlistService.GetWatchListByUserId(currentIdentity.UserId);
        //    var Status = metadataFileStatusService.GetNomStatus();
        //    ViewBag.StatusID = Status;
        //    return this.View(model);
        //}
       

        public bool DeleteNonPathedNoms(List<string> transactionIDs)
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


        //[HttpPost]
        //public ActionResult NonPathedBatches(NonPathedBatchListDTO model, string Search)
        //{
        //    //if (TempData["status"] != null)
        //    //{
        //    //    ViewBag.Status = TempData["status"] + "";
        //    //}
        //    //var pipe = pipelineService.GetPipeline(model.PipelineId);
        //    //if (pipe != null && pipe.ModelTypeID == (int)NomType.Pathed)
        //    //    return RedirectToAction("Index", "PathedNomination", new { pipelineId = pipe.ID });
        //    //else if (pipe != null && pipe.ModelTypeID == (int)NomType.PNT)
        //    //    return RedirectToAction("Index", "Batch", new { pipelineId = pipe.ID });
        //    //NonPathedBatchListDTO model = new NonPathedBatchListDTO();
        //    // model.PipelineId = pipelineId;
        //    //ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
        //    //var shipperDuns = currentIdentity.ShipperDuns;
        //    //var userId = currentIdentity.UserId;

        //    //DateTime todayDate = DateTime.Now.Date;
        //    //var currentmonth = todayDate.Month;
        //    //model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    //model.EndDate = model.StartDate.AddMonths(2);
        //    //model.StatusId = 0;
        //    // model.BatchDtoList = IPNTNominationService.GetBatches("", pipelineId, 0, todayDate.AddDays(-15), todayDate.AddDays(15), 1, 1000, new Guid(userId), shipperDuns).ToList(); // new List<BatchDTO>();// = IWatchlistService.GetWatchListByUserId(currentIdentity.UserId);
        //    var Status = metadataFileStatusService.GetNomStatus();
        //    ViewBag.StatusID = Status;
        //    return this.View(model);
        //}



        //public ActionResult GetBatch(int pipelineId, DateFilter datefilter)
        //{
        //    DateTime startDate = datefilter.StartDate;
        //    DateTime endDate = datefilter.EndDate.AddHours(24);
        //    string Status = string.IsNullOrEmpty(datefilter.status) ? "-1" : datefilter.status;
        //    int StatusId = Convert.ToInt32(Status);

        //    JsonResult result = new JsonResult();
        //    try
        //    {
        //        // Initialization.
        //        string search = Request.Form.GetValues("search[value]")[0];
        //        string draw = Request.Form.GetValues("draw")[0];
        //        string order = Request.Form.GetValues("order[0][column]")[0];
        //        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
        //        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
        //        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

        //        // Loading.

        //        DateTime todayDate = DateTime.Now.Date;               
        //        ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
        //        var shipperDuns = currentIdentity.ShipperDuns;
        //        var UserKey = Guid.Parse(currentIdentity.UserId);
        //        //TODO:
        //        List<BatchDTO> batchList = IPNTNominationService.GetBatches("", PipelineDuns, StatusId, startDate, endDate, 1, 1000, UserKey, shipperDuns).ToList();

        //        if (batchList != null)
        //            batchList = batchList.OrderByDescending(a => a.CreatedDate).ToList();
        //        // Total record count.
        //        int totalRecords = batchList.Count;

        //        // Verification.
        //        if (!string.IsNullOrEmpty(search) &&
        //            !string.IsNullOrWhiteSpace(search))
        //        {
        //            // Apply search
        //            batchList = batchList.Where(p => p.DateBeg.ToString().ToLower().Contains(search.ToLower()) ||
        //                                   p.DateEnd.ToString().ToLower().Contains(search.ToLower()) ||                                          
        //                                   p.Cycle.ToLower().Contains(search.ToLower()) ||                                         
        //                                   p.CreatedDate.ToString().ToLower().Contains(search.ToLower()) ||
        //                                   p.Status.ToString().ToLower().Contains(search.ToLower())).ToList();
        //        }

        //        // Sorting.
        //        batchList = this.SortByColumnWithOrder(order, orderDir, batchList);

        //        // Filter record count.
        //        int recFilter = batchList.Count;

        //        // Apply pagination.
        //        batchList = batchList.Skip(startRec).Take(pageSize).ToList();

        //        // Loading drop down lists.
        //        result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = batchList }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;
        //}

        //#region Sort by column with order method

        ///// <summary>
        ///// Sort by column with order method.
        ///// </summary>
        ///// <param name="order">Order parameter</param>
        ///// <param name="orderDir">Order direction parameter</param>
        ///// <param name="data">Data parameter</param>
        ///// <returns>Returns - Data</returns>
        //private List<BatchDTO> SortByColumnWithOrder(string order, string orderDir, List<BatchDTO> data)
        //{
        //    // Initialization.
        //    List<BatchDTO> lst = new List<BatchDTO>();

        //    try
        //    {
        //        // Sorting
        //        switch (order)
        //        {
        //            case "1":
        //                // Setting.
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StatusID).ToList()
        //                                                                                         : data.OrderBy(p => p.StatusID).ToList();
        //                break;

        //            case "2":
        //                // Setting.
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PipelineName).ToList()
        //                                                                                         : data.OrderBy(p => p.PipelineName).ToList();
        //                break;

        //            case "3":
        //                // Setting.
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateBeg).ToList()
        //                                                                                         : data.OrderBy(p => p.DateBeg).ToList();
        //                break;

        //            case "4":
        //                // Setting.
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateEnd).ToList()
        //                                                                                         : data.OrderBy(p => p.DateEnd).ToList();
        //                break;

                   
        //            case "5":
        //                // Setting.
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cycle).ToList()
        //                                                                                         : data.OrderBy(p => p.Cycle).ToList();
        //                break;

        //            default:
        //               // Setting.
        //               lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedDate).ToList()
        //                                                                                         : data.OrderBy(p => p.CreatedDate).ToList();
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

        //#endregion


        public ActionResult Delete(Guid id)
        {
            //TODO: delete NonPathed Nom
           // IWatchlistService.DeleteWatchListById(id);
           return RedirectToAction("List");
        }



        [HttpPost]
        public ActionResult GetData(NonPathedDTO model)
        {
            string pipelineDuns = model.PipelineDuns;
            // var pipe = pipelineService.GetPipeline(model.PipelineId);
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.UserId = Guid.Parse(currentIdentityValues.UserId);            
            model = nonPathedService.GetNonPathedNominations(model.PipelineDuns, model.StatusId, model.StartDate, model.EndDate, model.ShipperDuns, model.UserId.GetValueOrDefault());
            model.PipelineDuns = pipelineDuns;
            ViewBag.StatusID = metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = ICycleIndicator.GetCycles();
            return PartialView("_RecDelTables", model);
        }

    }
}