using Microsoft.AspNet.Identity;
using Nom.ViewModel;
using Nom1Done.CustomSerialization;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Models;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class PathedNominationController : BaseController
    {
        IPathedNominationService _pathedNominationService;
        ILocationService _locationService;
        IPNTNominationService _pntNominationService;
        IPipelineService _pipelineService;
        ImetadataBidUpIndicatorService _metaDataBidUpIndicatorService;
        ImetadataCapacityTypeIndicatorService _metadataCapacityTypeIndicatorService;
        ImetadataQuantityTypeIndicatorService _metadataQuantityTypeIndicatorService;
        ImetadataExportDeclarationService _metadataExportDeclarationService;
        ImetadataFileStatusService _metadataFileStatusService;
        ICycleIndicator _cycleIndicator;
        INotifierEntityService _notifierEntityService;
        INonPathedService InonPathedService;
        int pageSize = 10;

        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private RestClient clientLocation = new RestClient(apiBaseUrl + "/api/Location/");
        private RestClient clientCounterParty = new RestClient(apiBaseUrl + "/api/CounterParty/");
         

        public PathedNominationController(INonPathedService InonPathedService, INotifierEntityService _notifierEntityService, ImetadataFileStatusService metadataFileStatusService, ImetadataExportDeclarationService ImetadataExportDeclarationService, ImetadataQuantityTypeIndicatorService metadataQuantityTypeIndicatorService, ICycleIndicator ICycleIndicator, ImetadataCapacityTypeIndicatorService metadataCapacityTypeIndicatorService, ImetadataBidUpIndicatorService metaDataBidUpIndicatorService, IPathedNominationService pathedNominationService, ILocationService locationService, IPNTNominationService pntNominationService, IPipelineService pipelineService) : base(pipelineService)
        {
            this._notifierEntityService = _notifierEntityService;
            this._pathedNominationService = pathedNominationService;
            this._locationService = locationService;
            this._pntNominationService = pntNominationService;
            this._metaDataBidUpIndicatorService = metaDataBidUpIndicatorService;
            this._metadataCapacityTypeIndicatorService = metadataCapacityTypeIndicatorService;
            this._metadataQuantityTypeIndicatorService = metadataQuantityTypeIndicatorService;
            this._metadataExportDeclarationService = ImetadataExportDeclarationService;
            this._metadataFileStatusService = metadataFileStatusService;
            this._cycleIndicator = ICycleIndicator;
            this._pipelineService = pipelineService;
            this.InonPathedService = InonPathedService;
            clientLocation.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
            clientCounterParty.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }
        [HttpGet]
        public ActionResult Index(int? pipelineId)
        {
            SortingPagingInfo info = new SortingPagingInfo();
            info.PageSize = pageSize;
            info.CurrentPageIndex = 0;
            if (TempData["status"] != null)
            {
                ViewBag.Status = TempData["status"] + "";
            }

            PathedDTO model = new PathedDTO();

            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            if (pipelineId == null)
                pipelineId = currentIdentityValues.FirstSelectedPipeIdByUser;
            PipelineDTO pipe = _pipelineService.GetPipeline(pipelineId.Value);

            if (pipe != null && (pipe.ModelTypeID == (int)NomType.PNT || pipe.ModelTypeID == (int)NomType.HyNonPathedPNT || pipe.ModelTypeID == (int)NomType.HyPathedPNT))
                return RedirectToAction("Index", "Batch", new { pipelineId = pipe.ID });
            else if (pipe != null && pipe.ModelTypeID == (int)NomType.NonPathed)
                return RedirectToAction("Index", "NonPathed", new { pipelineId = pipe.ID });

            DateTime todayDate = DateTime.Now.Date;
            model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.ShipperID = Guid.Parse(currentIdentityValues.UserId);
            int status = -1;
            model.DunsNo = currentIdentityValues.ShipperDuns;
            model.showMine =Session["showMine"]==null ? true : (bool)Session["showMine"];

            int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(pipelineId.Value, status, model.StartDate, model.EndDate, model.DunsNo,model.showMine? model.ShipperID.Value: Guid.Empty);

            info.PageCount = (pathedListTotalCount % info.PageSize) != 0 ? (pathedListTotalCount / info.PageSize) + 1 : (pathedListTotalCount / info.PageSize);

            model.SortingPagingInfo = info;
            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(pipelineId.Value, status, model.StartDate, model.EndDate, model.DunsNo,model.showMine ? model.ShipperID.Value: Guid.Empty, model.SortingPagingInfo);
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);
            var pipelineData = _pathedNominationService.GetPipeline(pipelineId.Value);
            model.PipelineDetails = pipelineData.Name + " (" + pipelineData.DUNSNo + ")";
            model.PipelineID = pipelineId.Value;
            model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.StatusId = -1;
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;

            NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(pipelineId.Value));
            model.PipelineNomType = modelType;

            var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);
            ViewBag.NotifierEntity = notifier;

            model.PathedNomsList = model.PathedNomsList.OrderByDescending(a => a.CreatedDate).ToList();
            //model.showMine = true;
            return this.View(model);
        }

        public PathedDTO GetPathedHybridNonPathedType(PathedDTO model)
        {
            var duns = _pipelineService.GetDunsByPipelineID(model.PipelineID);
            int tt = 0;
            string ttDesc = string.Empty;
            foreach (var item in model.PathedNomsList)
            {
                if (item.TransType != null)
                {
                    tt = int.Parse(item.TransType);
                    ttDesc = item.TransTypeName;
                    item.PathedHybridNonpathedType = NonPathedTypeVariance.GetNomModelType(duns, tt, ttDesc);
                }
                else
                {
                    item.PathedHybridNonpathedType = -1;   // by default-- Pathed Model
                }
            }
            return model;
        }
        [HttpPost]
        public ActionResult Index(PathedDTO model, string Search)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();

            Session["showMine"] = model.showMine;
            if (Search == null)
            {
                model.DunsNo = currentIdentityValues.ShipperDuns;// shipperDuns;                
                model.companyId = String.IsNullOrEmpty(currentIdentityValues.CompanyId) ? 0 : int.Parse(currentIdentityValues.CompanyId);
                model.ShipperID = new Guid(currentIdentityValues.UserId);
                if (model.PathedNomsList == null)
                {
                    TempData["status"] = "Please fill row.";
                    return RedirectToAction("Index", new { pipelineId = model.PipelineID });
                }

                model.PathedNomsList = model.PathedNomsList.Where(a => a.IsModify).ToList();

                Guid? transactionID = _pathedNominationService.SaveAndUpdatePathedNomination(model, true);//pathedService.SaveAndUpdatePathedNomination(model, true);
                if (transactionID != null)
                    TempData["status"] = "Data saved successfully";
                else
                {
                    TempData["status"] = "Data saving failed";
                }
                return RedirectToAction("Index", new { pipelineId = model.PipelineID });
            }
            else
            {

                var StartDate = model.StartDate;
                var EndDate = model.EndDate;
                var Status = model.StatusId;
                var PipelineID = model.PipelineID;
                model.DunsNo = currentIdentityValues.ShipperDuns;

                model.ShipperID = Guid.Parse(currentIdentityValues.UserId);
                int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(PipelineID, Status, StartDate, EndDate, model.DunsNo, model.showMine?model.ShipperID.Value:Guid.Empty);
                var info = model.SortingPagingInfo;
                model.SortingPagingInfo.PageCount = (pathedListTotalCount % info.PageSize) != 0 ? (pathedListTotalCount / info.PageSize) + 1 : (pathedListTotalCount / info.PageSize);
                model.SortingPagingInfo.CurrentPageIndex = 0; // for search result, return First-page always.
                model.SortingPagingInfo.SortDirection = string.Empty;
                model.SortingPagingInfo.SortField = string.Empty;
                var PathedNomsList = _pathedNominationService.GetPathedListWithPaging(PipelineID, Status, StartDate, EndDate, model.DunsNo, model.showMine ? model.ShipperID.Value : Guid.Empty, model.SortingPagingInfo);
                model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(PathedNomsList);
                var pipelineData = _pathedNominationService.GetPipeline(PipelineID);
                model.PipelineDetails = pipelineData.Name + "(" + pipelineData.DUNSNo + ")";
                model.PipelineID = PipelineID;
                var CycleIndicator = _cycleIndicator.GetCycles();
                var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
                var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
                var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
                var Export = _metadataExportDeclarationService.GetExportDeclarations();
                var Status1 = _metadataFileStatusService.GetNomStatus();
                ViewBag.Cycles = CycleIndicator;
                ViewBag.QuantityType = QuantityType;
                ViewBag.CapacityType = CapacityType;
                ViewBag.BidUp = BidUp;
                ViewBag.Export = Export;
                ViewBag.StatusID = Status1;
                var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);//GetNotifierEntityofBatchTable();
                ViewBag.NotifierEntity = notifier;

                NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(model.PipelineID));
                model.PipelineNomType = modelType;               
                model.PathedNomsList = model.PathedNomsList.OrderByDescending(a => a.CreatedDate).ToList();
                return this.View(model);
            }

        }
        public PartialViewResult GetLocationPopUpForSpecialHybrid(string TransTypeMapId, string clickedRow, string popUpFor, int PipelineID)
        {
            NominationPartialDTO model = new NominationPartialDTO();
            string partialView = string.Empty;
            model.ForRow = clickedRow;
            model.PopUpFor = popUpFor;
            model.PipelineId = PipelineID;
            model.Locations = new List<LocationsDTO>();
            int ttMapid = Convert.ToInt32(TransTypeMapId ?? "0");          
            bool isSpecialDeliveryCase = _pntNominationService.FindIsSpecialLocsUsingTTPipeMapId(ttMapid);
          
            ViewBag.PopUpFor = popUpFor;
            ViewBag.IsSpecialDelCase = isSpecialDeliveryCase;
            partialView = "~/Views/PNTNominations/_LocationPopUp.cshtml";
            return PartialView(partialView, model);
        }
        public PartialViewResult NotimationsPartials(string partial, string clickedRow, string popUpFor, int PipelineID)
        {
            int PageIndex = 1;
            NominationPartialDTO model = new NominationPartialDTO();
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            int companyId = String.IsNullOrEmpty(currentIdentity.CompanyId) ? 0 : int.Parse(currentIdentity.CompanyId);
            string partialView = string.Empty;
            model.ForRow = clickedRow;
            model.PopUpFor = popUpFor;
            model.PipelineId = PipelineID;
            if (partial.ToLower() == "locations")
            {
               
                model.Locations = new List<LocationsDTO>();                
                ViewBag.IsSpecialDelCase = false;
                ViewBag.PopUpFor = popUpFor;
                partialView = "~/Views/PNTNominations/_LocationPopUp.cshtml";
            }
            else if (partial == "TransactionType")
            {
                model.TransactionTypes = new List<TransactionTypesDTO>();
                model.TransactionTypes = _pntNominationService.GetTransactionsTypes(PipelineID, "", model.PopUpFor).ToList();
                model.TransactionTypes = model.TransactionTypes.OrderBy(a => Convert.ToInt32(a.Identifier)).ToList();
                partialView = "~/Views/PNTNominations/_TransactionTypePopUp.cshtml";
            }
            else if (partial == "CounterParties")
            {
                model.CounterParties = new List<CounterPartiesDTO>();
                //TODO:
                // model.CounterParties = _pntNominationService.GetCounterParties("", PipelineID, PageIndex, 1000).ToList();
                partialView = "~/Views/PNTNominations/_CounterPartyPopUp.cshtml";
            }
            else if (partial == "Contract")
            {
                model.Contracts = new List<ContractsDTO>();
                //TODO:
                model.Contracts = _pntNominationService.GetContracts("", companyId, PipelineID, PageIndex, 1000).ToList();
                model.PipelineId = PipelineID;
                partialView = "~/Views/PNTNominations/_ContractPopUp.cshtml";
            }
            else if (partial == "StatusReason")
            {
                var transactionId = popUpFor;
                //TODO:
                model.StatusReason = _pntNominationService.GetRejectionReason(new Guid(transactionId)).ToList();
                partialView = "~/Views/PNTNominations/_StatusReasonPopUp.cshtml";
            }
            return PartialView(partialView, model);
        }
        public PartialViewResult AddPathedNomRow(int RowCount, int pipelineid)
        {
            PathedDTO model = new PathedDTO();
            model.CurrentContractRow = RowCount;
            model.PipelineID = pipelineid;
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status1 = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status1;


            model.PathedNomsList = new List<PathedNomDetailsDTO>();

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
            model.PathedNomsList.Add(item);

            NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(model.PipelineID));
            if (modelType == NomType.HyPathedNonPathed)
            {
                // model = GetPathedHybridNonPathedType(model);
                model.PipelineModelType = "PathedNonPathedHybrid";
            }
            else
            {
                model.PipelineModelType = "Pathed";
            }


            return PartialView("_AddPathedNomRow", model);
        }
        public bool SendNomination(List<string> transactionIDs)
        {
            bool sendToTest = Convert.ToBoolean(ConfigurationManager.AppSettings["SendToTest"]);
            bool results = false;
            ShipperReturnByIdentity currentIdentity = GetValueFromIdentity();
            int companyId = String.IsNullOrEmpty(currentIdentity.CompanyId) ? 0 : int.Parse(currentIdentity.CompanyId);

            List<bool> tempBool = new List<bool>();

            int statusId = 0;

            foreach (var Id in transactionIDs)
            {
                statusId= _pathedNominationService.GetStatusOnTransactionId(new Guid(Id));
                if (statusId == 11)  // only for draft
                {                   
                    var result = _pntNominationService.SendNominationTransaction(new Guid(Id), companyId, sendToTest);
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
        public PartialViewResult CopyRow(List<PathedNomDetailsDTO> PathedRecordToCopy, int pipelineid)
        {
            PathedDTO model = new PathedDTO();
            model.PipelineID = pipelineid;

            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status1 = _metadataFileStatusService.GetNomStatus();
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
            if (item.EndDate < item.StartDate) {
                var nextDay = item.StartDate.AddDays(1);
                var time1 = PathedRecordToCopy[0].EndDate.TimeOfDay;
                item.EndDate = nextDay.Date.Add(time1);              
            }
            // item.StartDate = PathedRecordToCopy[0].StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : PathedRecordToCopy[0].StartDate;
            // item.EndDate = PathedRecordToCopy[0].EndDate.Date < DateTime.Now.Date ? DateTime.Now.AddDays(1).Date : PathedRecordToCopy[0].EndDate;

            item.CycleID = PathedRecordToCopy[0].CycleID;
            item.Contract = PathedRecordToCopy[0].Contract;
            item.NomSubCycle = PathedRecordToCopy[0].NomSubCycle;
            item.TransType = PathedRecordToCopy[0].TransType;
            item.TransTypeMapId = PathedRecordToCopy[0].TransTypeMapId;
            item.PathedHybridNonpathedType = PathedRecordToCopy[0].PathedHybridNonpathedType;
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
            item.PkgIDRec = PathedRecordToCopy[0].PkgIDRec;

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


            NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(model.PipelineID));
            if (modelType == NomType.HyPathedNonPathed)
            {
                // model = GetPathedHybridNonPathedType(model);
                model.PipelineModelType = "PathedNonPathedHybrid";
            }
            else
            {
                model.PipelineModelType = "Pathed";
            }

            return PartialView("_AddPathedNomRow", model);
        }
        public bool DeletePathedNom(List<string> transactionIDs)
        {
            bool isDelete = false;
            if (transactionIDs != null && transactionIDs.Count() > 0)
            {
                List<bool> tempBool = new List<bool>();

                //TODO:
                foreach (var transactionID in transactionIDs)
                {

                    var isDeleted = _pathedNominationService.DeleteNominationData(new Guid(transactionID));
                    tempBool.Add(isDeleted);
                }
                if ((tempBool.Where(a => a == false).Count()) == 0)
                {
                    isDelete = true;
                }
            }
            return isDelete;
        }

        [HttpPost]
        public ActionResult GetDataForSortingPaging(PathedFilters pathedFilters)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            PathedDTO model = new PathedDTO();
            model.showMine = pathedFilters.Showmine;
           // string search = Request.Form.GetValues("search[value]")[0];
           // string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
           // int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
           // int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            SortingPagingInfo info = new SortingPagingInfo();
            info.CurrentPageIndex = pathedFilters.CurrentPage;
            info.PageSize = pathedFilters.PageSize;
            info.SortDirection = orderDir;
            info.SortField = order;
            info.PageCount = pathedFilters.PageCount;
            model.SortingPagingInfo = info;
            model.PipelineID = pathedFilters.PipelineId;
            model.StartDate = pathedFilters.StartDate;
            model.EndDate = pathedFilters.EndDate;
            model.StatusId = pathedFilters.StatusId;

            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(model.PipelineID, model.StatusId, model.StartDate, model.EndDate, currentIdentityValues.ShipperDuns, model.showMine ? Guid.Parse(currentIdentityValues.UserId) : Guid.Empty , model.SortingPagingInfo);
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);

            switch (order) {
                case "10": // Receipt Loc name
                    model.PathedNomsList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? (model.PathedNomsList).OrderByDescending(p => p.RecLocation).ToList()
                                                                                                                  : (model.PathedNomsList).OrderBy(p => p.RecLocation).ToList();
                    break;

                case "13": // Up stream Name
                    model.PathedNomsList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? (model.PathedNomsList).OrderByDescending(p => p.UpName).ToList()
                                                                                                                   : (model.PathedNomsList).OrderBy(p => p.UpName).ToList();
                    break;

                case "19": // DelLoc
                    model.PathedNomsList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? (model.PathedNomsList).OrderByDescending(p => p.DelLoc).ToList()
                                                                                                                   : (model.PathedNomsList).OrderBy(p => p.DelLoc).ToList();
                    break;

                case "22": // Down Stream name
                    model.PathedNomsList = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? (model.PathedNomsList).OrderByDescending(p => p.DownName).ToList()
                                                                                                                   : (model.PathedNomsList).OrderBy(p => p.DownName).ToList();
                    break;
            }
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;
            return PartialView("_PathedNomTable", model);

        }


        [HttpPost]
        public ActionResult GetData(PathedDTO model)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();

            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(model.PipelineID, model.StatusId, model.StartDate, model.EndDate, currentIdentityValues.ShipperDuns, model.showMine ? Guid.Parse(currentIdentityValues.UserId) : Guid.Empty, model.SortingPagingInfo);
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);
            // model.PathedNomsList = model.PathedNomsList.OrderByDescending(a => a.CreatedDate).ToList();
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;
            return PartialView("_PathedNomTable", model);
        }
        [HttpPost]
        public JsonResult GetNonPathedTypeByTT(int tt, string ttDesc, int pipelineID)
        {
            int resultType = 0;
            var duns = _pipelineService.GetDunsByPipelineID(pipelineID);
            resultType = NonPathedTypeVariance.GetNomModelType(duns, tt, ttDesc);
            //  var propertiesList= PathedNonpathedHybridProperties.GetPropertiesByTypes(resultType);
            return Json(resultType);
        }

        public ActionResult GetLocationForPopUp(int pipelineId, string PopUpFor, bool IsSpecialDelCase)
        {
            List<LocationsDTO> lstLocations = new List<LocationsDTO>();
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
                int pageno = (startRec / pageSize) + 1;


                 var Pipelineduns = _pipelineService.GetDunsByPipelineID(pipelineId);
                // Loading.

                LocDataCriteria criteria = new LocDataCriteria();
                criteria.keyword = search;
                criteria.PipelineDuns = Pipelineduns;
                criteria.page = pageno;
                criteria.size = pageSize;
                criteria.PopupFor = PopUpFor;
                criteria.IsSpecialDelCase = IsSpecialDelCase;
                criteria.order = GetColumnNameToSort(order);
                criteria.orderDir = orderDir;

                var request = new RestRequest(string.Format("GetLocationsByCriteria"), Method.POST) {RequestFormat = DataFormat.Json};
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(criteria);
                var response = clientLocation.Execute<LocationsResultDTO>(request);

                // var dataList = _pntNominationService.GetLocations(search, Pipelineduns, pageno, pageSize, PopUpFor, IsSpecialDelCase, order, orderDir).ToList();

               
                int totalRecords = 0;
                if (response.Data != null)
                {
                    lstLocations = response.Data.locationsDTO;
                    totalRecords = response.Data.RecordCount;
                }
                else
                {
                    lstLocations = new List<LocationsDTO>();
                    totalRecords = 0;
                }
            
                // Filter record count.
                int recFilter = totalRecords; // response.Data.RecordCount;

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = lstLocations }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
               // throw (ex);
            }

            return result;
        }


        public ActionResult GetCounterParties(int pipelineId)
        {
            List<CounterPartiesDTO> lstCounterParties = new List<CounterPartiesDTO>();
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
                int pageno = (startRec / pageSize) +1;


                var PipeDuns = _pipelineService.GetDunsByPipelineID(pipelineId);
                // Loading.


                CounterPartyFilter filter = new CounterPartyFilter();
                filter.keyword = search;
                filter.PipeDuns = PipeDuns;
                filter.page = pageno;
                filter.size = pageSize;
                filter.order = GetColumnNameToSort(order);  
                filter.orderDir = orderDir;

                //var dataList = _pntNominationService.GetCounterParties(search, pipelineId, pageno, pageSize,order,orderDir);
                var requestCounterParty = new RestRequest(string.Format("GetCounterPartyByCriteria"), Method.POST) { RequestFormat = DataFormat.Json };
                requestCounterParty.JsonSerializer = NewtonsoftJsonSerializer.Default;
                requestCounterParty.AddJsonBody(filter);
                  
                var responseCounterParty = clientCounterParty.Execute<CounterPartiesResultDTO>(requestCounterParty);


                //  int totalRecords = _pntNominationService.GetTotalCounterPartiesCount(search, pipelineId);
                int totalRecords = 0;
                if (responseCounterParty.Data != null)
                {
                    lstCounterParties = responseCounterParty.Data.CounterParties;
                    totalRecords = responseCounterParty.Data.RecordCount;
                }
                else
                {
                    lstCounterParties = new List<CounterPartiesDTO>();
                     totalRecords = 0;
                }

                // Filter record count.
                int recFilter = totalRecords;// responseCounterParty.Data.RecordCount;

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = lstCounterParties }, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {

            }

            return result;
        }

        [HttpGet]
        public ActionResult LoadPartialPathed(int pipelineId)
        {
            SortingPagingInfo info = new SortingPagingInfo();
            info.PageSize = pageSize;
            info.CurrentPageIndex = 0;

            if (TempData["status"] != null)
            {
                ViewBag.Status = TempData["status"] + "";
            }
            PathedDTO model = new PathedDTO();
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            PipelineDTO pipe = _pipelineService.GetPipeline(pipelineId);
            DateTime todayDate = DateTime.Now.Date;
            model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.ShipperID = Guid.Parse(currentIdentityValues.UserId);
            int status = -1;
            model.DunsNo = currentIdentityValues.ShipperDuns;
            int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(pipelineId, status, model.StartDate, model.EndDate, model.DunsNo,model.showMine? model.ShipperID.Value: Guid.Empty);

            info.PageCount = (pathedListTotalCount % info.PageSize) != 0 ? (pathedListTotalCount / info.PageSize) + 1 : (pathedListTotalCount / info.PageSize);

            model.SortingPagingInfo = info;
            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(pipelineId, status, model.StartDate, model.EndDate, model.DunsNo, model.showMine ? model.ShipperID.Value: Guid.Empty, model.SortingPagingInfo);
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);
            var pipelineData = _pathedNominationService.GetPipeline(pipelineId);
            model.PipelineDetails = pipelineData.Name + "(" + pipelineData.DUNSNo + ")";
            model.PipelineID = pipelineId;
            model.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.EndDate = model.StartDate.AddMonths(2);
            model.StatusId = status;
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;

            NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(pipelineId));
            if (modelType == NomType.HyPathedNonPathed)
            {
                model = GetPathedHybridNonPathedType(model);
                model.PipelineModelType = "PathedNonPathedHybrid";
            }
            else
            {
                model.PipelineModelType = "Pathed";
            }


            var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);//GetNotifierEntityofBatchTable();
            ViewBag.NotifierEntity = notifier;

            model.PathedNomsList = model.PathedNomsList.OrderByDescending(a => a.CreatedDate).ToList();
            return PartialView("_PathedNoms", model);
        }

        [HttpGet]
        public ActionResult LoadNonPathedPartial(int pipelineId)
        {
            PathedDTO model = new PathedDTO();
            model.PipelineID = pipelineId;
            DateTime todayDate = DateTime.Now.Date;
            DateTime min = todayDate.AddYears(-1);
            DateTime max = todayDate.AddYears(1);
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            int status = -1;
            var data = InonPathedService.GetNonPathedNominations(model.PipelineID, status, min, max, currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));
            if (data != null)
            {
                data = UpdateCounterPartyAndLocNameInNonPathed(data);
                model.ReceiptNoms = data.ReceiptNoms;
                model.DeliveryNoms = data.DeliveryNoms;
            }
            else {
                model.ReceiptNoms = new List<NonPathedRecieptNom>();
                model.DeliveryNoms = new List<NonPathedDeliveryNom>();
            }
            
            var CycleIndicator = _cycleIndicator.GetCycles();
            ViewBag.Cycles = CycleIndicator;
            return PartialView("_NonPathedHybrid", model);
        }

        public PartialViewResult AddDeliveryRow(int PipelineID)
        {

            PathedDTO model = new PathedDTO();
            model.PipelineID = PipelineID;
            model.DeliveryNoms.Add(new NonPathedDeliveryNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddDeliveryRowHybrid", model);
        }


        public PartialViewResult AddReceiptRow(int PipelineID)
        {
            PathedDTO model = new PathedDTO();
            model.PipelineID = PipelineID;
            model.ReceiptNoms.Add(new NonPathedRecieptNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddReceiptRowHybrid", model);
        }


        [HttpPost]
        public ActionResult SaveNonPathedHybrid(PathedDTO Mainmodel)
        {
            NonPathedDTO model = new NonPathedDTO();
            model.PipelineId = Mainmodel.PipelineID;
            model.ReceiptNoms = Mainmodel.ReceiptNoms;
            model.DeliveryNoms = Mainmodel.DeliveryNoms;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.UserId = Guid.Parse(currentIdentityValues.UserId);
            var pipe = _pipelineService.GetPipeline(model.PipelineId);
            model.PipelineDuns = pipe.DUNSNo;
            model.CompanyId = Convert.ToInt32(currentIdentityValues.CompanyId ?? "0");
            bool result = false;
            var Id = InonPathedService.SaveAllNonPathedNominations(model);
            result = (Id == Guid.Empty) ? false : true;
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            ViewBag.SubmitStatus = result;
            if (result)
                TempData["status"] = "Data saved successfully";
            else
            {
                TempData["status"] = "Data saving failed";
            }
            return RedirectToAction("Index", new { pipelineId = Mainmodel.PipelineID });
        }
        [HttpPost]
        public JsonResult GetStatusOnTransactionId(List<StatusUpdateViewModel> objList)
        {
            if (objList != null && objList.Count > 0)
            {
                var newList = new List<StatusUpdateViewModel>();
                foreach (var obj in objList)
                {
                    obj.StatusId = _pathedNominationService.GetStatusOnTransactionId(Guid.Parse(obj.value));
                    newList.Add(obj);
                }
                return Json(newList);
            }
            return null;
        }

        public JsonResult GetStatus(List<string> objList)
        {
            if (objList != null && objList.Count > 0)
            {
                var newList = new List<StatusUpdateViewModel>();
                foreach (var obj in objList)
                {
                    StatusUpdateViewModel responseObj = new StatusUpdateViewModel();
                    responseObj.StatusId = _pathedNominationService.GetStatusOnTransactionId(Guid.Parse(obj));
                    switch(responseObj.StatusId)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            responseObj.StatusName = "In-Process";
                            responseObj.value = "label-info";
                            break;
                        case 5:
                            responseObj.StatusName = "Submitted";
                            responseObj.value = "label-success";
                            break;
                        case 6:
                            responseObj.StatusName = "Submitted";
                            responseObj.value = "label-success";
                            break;
                        case 7:
                            responseObj.StatusName = "Accepted";
                            responseObj.value = "label-success";
                            break;
                        case 8:
                            responseObj.StatusName = "Exception Occured";
                            responseObj.value = "label-warning";
                            break;
                        case 9:
                            responseObj.StatusName = "Exception Occured";
                            responseObj.value = "label-warning";
                            break;
                        case 10:
                            responseObj.StatusName = "Rejected";
                            responseObj.value = "label-warning";
                            break;
                        case 11:
                            responseObj.StatusName = "Draft";
                            responseObj.value = "label-warning";
                            break;
                        case 12:
                            responseObj.StatusName = "Replaced";
                            responseObj.value = "label-info";
                            break;
                    }

                    responseObj.Key = obj;
                    newList.Add(responseObj);
                }
                return Json(newList);
            }
            return null;
        }

        public string GetColumnNameToSort(string order)
        {
            string result = string.Empty;
            switch (order)
            {
                case "1":
                    result = "Name";
                    break;
                case "2":
                    result = "Identifier";
                    break;
                case "3":
                    result = "PropCode";
                    break;
                case "4":
                    result = "RDUsageID";
                    break;
            }
            return result;
        }

    }



  public class LocDataCriteria
    {
        public string PipelineDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public string order { get; set; } //SortField 
        public string orderDir { get; set; }
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex
        public string PopupFor { get; set; }
        public bool IsSpecialDelCase { get; set; }

    }

    public class CounterPartyFilter
    {
        public string PipeDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public string order { get; set; } //SortField
        public string orderDir { get; set; }
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex

    }


}