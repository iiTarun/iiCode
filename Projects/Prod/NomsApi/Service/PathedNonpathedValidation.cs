using Ninject;
using Nom.ViewModel;
using Nom1Done.Common;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using NomsApi.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NomsApi.Service
{
    public class PathedNonpathedValidation//:Ninject.Modules.NinjectModule
    {
          // User="Jinsong.Zhang@kochind.com" .
        static string shipperDuns = "832703297";  // static for NNG
        static int shipperId = 5;  // static for Koch
        static List<CycleIndicatorDTO> cycleList = new List<CycleIndicatorDTO>();
        static int pipelineId = 100;   // static for NNG


        
        IPNTNominationService _pntNominationService;
        ICycleIndicator _cycleRepo;
        IBatchRepository _batchRepository;
        ImetadataTransactionTypeRepository _metadataTransactionTypeRepository;
        INominationsRepository _nominationsRepository;
        private readonly IContractService contractService;
        public PathedNonpathedValidation()
        {
            StandardKernel Kernel = new StandardKernel(new APIModules());
            _batchRepository = Kernel.Get<BatchRepository>();
            _cycleRepo = Kernel.Get<CycleIndicatorService>();
           _pntNominationService = Kernel.Get<PNTNominationService>();
            _metadataTransactionTypeRepository = Kernel.Get<metadataTransactionTypeRepository>();
            _nominationsRepository = Kernel.Get<NominationsRepository>();
            contractService = Kernel.Get<ContractService>();
            cycleList = _cycleRepo.GetCycles();
        }
       
        public List<NominationsResponseDTO> ValidationSaveSendNoms(PathedNonPathedHybridDTO hybridObj,string userId)
        {
            var pathedList = hybridObj.PathedNomList;
            var nonpathedList = hybridObj.NonPathedNomList;
            
            List<NominationsResponseDTO> InvalideNoms = new List<NominationsResponseDTO>();
            DateTime min = DateTime.MinValue;
            DateTime max = DateTime.MaxValue;

            List<string> currentKochIds = new List<string>();
            List<string> currentPathedKochIds = pathedList.Select(a => a.KochId).ToList();
            List<string> currentNonPathedKochIds = new List<string>();
            foreach (var nonPathedBatch in nonpathedList) {
                var recListKochId = nonPathedBatch.NonPathedRecNomList.Select(a => a.KochId).ToList();
                var delListKochId = nonPathedBatch.NonPathedDelNomList.Select(a => a.KochId).ToList();
                currentNonPathedKochIds.AddRange(recListKochId);
                currentNonPathedKochIds.AddRange(delListKochId);
            }
            currentKochIds.AddRange(currentPathedKochIds);
            currentKochIds.AddRange(currentNonPathedKochIds);
            var duplicateCurrentKochIds = currentKochIds.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);

            List<string> existingKochIds = new List<string>();
            existingKochIds = _batchRepository.GetKochIds();

            #region Pathed Validation

            foreach (var pathednom in pathedList)
            {
                var kochId = pathednom.KochId;

                
                var error = string.Empty;
                
                if(pathednom.StartDateTime == min || pathednom.StartDateTime == max ) {
                    error += "Start date ";
                }
                if (pathednom.EndDateTime == min || pathednom.EndDateTime == max )
                {
                    error += "End date ";
                }
                if (pathednom.StartDateTime > pathednom.EndDateTime || pathednom.StartDateTime < DateTime.Today || pathednom.EndDateTime < DateTime.Today)
                {
                    error += "Start/End date ";
                }
                if (string.IsNullOrEmpty(pathednom.CycleCode)) { error += "CycleCode "; }
                if (string.IsNullOrEmpty(pathednom.ServiceRequesterContractCode)) { error += "ServiceRequesterContractCode "; }
                if (string.IsNullOrEmpty(pathednom.NomSubCycle)) { error += "NomSubCycle "; }
                if (string.IsNullOrEmpty(pathednom.TransactionType) || pathednom.TransactionType == "1") { error += "TransactionType "; }
                if (string.IsNullOrEmpty(pathednom.ReceiptLocId)) { error += "ReceiptLocId "; }
                if (string.IsNullOrEmpty(pathednom.UpstreamId)) { error += "UpstreamId "; }
                if (string.IsNullOrEmpty(pathednom.UpstreamK)) { error += "UpstreamK "; }
                if (string.IsNullOrEmpty(pathednom.ReceiptRank)) { error += "ReceiptRank "; }
                if (string.IsNullOrEmpty(pathednom.DeliveryLocId)) { error += "DeliveryLocId "; }
                if (string.IsNullOrEmpty(pathednom.DnstreamId)) { error += "DnstreamId "; }
                if (string.IsNullOrEmpty(pathednom.DnstreamK)) { error += "DnstreamK "; }
                if (string.IsNullOrEmpty(pathednom.DeliveryRank)) { error += "DeliveryRank "; }
                if (pathednom.RecQty < 0) { error += "RecQty "; }
                if (pathednom.DelQuantity < 0) { error += "DelQuantity "; }
                if (string.IsNullOrEmpty(pathednom.KochId)) { error += "KochId "; } else {
                    if (existingKochIds.Contains(pathednom.KochId) == true) { error += "KochId_Already_Exists "; }
                    else
                    if (duplicateCurrentKochIds.Contains(pathednom.KochId)==true) { error += "Duplicate_KochId "; }
                }               
                
              
                if (string.IsNullOrEmpty(error))
                {
                    pathednom.IsValideNom = true;
                } else {
                    error = "This nomination is invalid. Please, Check " + error;
                    var invalidNom = new NominationsResponseDTO()
                    {
                        KochId = kochId,
                        ResponseMessage = error
                    };
                    InvalideNoms.Add(invalidNom);
                }
            }

            #endregion

            #region SaveNSend Pathed
            pathedList = pathedList.Where(a => a.IsValideNom == true).ToList();
            SaveNSendPathedNoms(pathedList, userId);
            #endregion


            #region NonPathed

            foreach (var nonpathedNom in nonpathedList) {

                string BatchError = string.Empty;
                if (nonpathedNom.StartDateTime == min || nonpathedNom.StartDateTime == max) { BatchError += "Start date "; }
                if (nonpathedNom.EndDateTime == min || nonpathedNom.EndDateTime == max) { BatchError += "End date "; }
                if (nonpathedNom.StartDateTime > nonpathedNom.EndDateTime || nonpathedNom.StartDateTime < DateTime.Today || nonpathedNom.EndDateTime < DateTime.Today)
                {
                    BatchError += "Start/End date ";
                }
                if (string.IsNullOrEmpty(nonpathedNom.CycleCode)) { BatchError += "CycleCode "; }
                if (string.IsNullOrEmpty(nonpathedNom.NomSubCycle)) { BatchError += "NomSubCycle "; }
                if (string.IsNullOrEmpty(nonpathedNom.ServiceRequesterContractCode)) { BatchError += "ServiceRequesterContractCode "; }

                #region NonPathed Rec Nom validation

                foreach (var npRecNom in nonpathedNom.NonPathedRecNomList)
                {
                    var kochId = npRecNom.KochId;
                    var error = string.Empty;

                    if (string.IsNullOrEmpty(npRecNom.ReceiptLocId)) { error += "ReceiptLocId "; }
                    if (string.IsNullOrEmpty(npRecNom.TransactionType) || npRecNom.TransactionType == "1") { error += "TransactionType "; }
                    if (string.IsNullOrEmpty(npRecNom.UpstreamId)) { error += "UpstreamId "; }
                    if (string.IsNullOrEmpty(npRecNom.ReceiptRank)) { error += "ReceiptRank "; }
                    if (string.IsNullOrEmpty(npRecNom.UpstreamK)) { error += "UpstreamK "; }
                    if (npRecNom.ReceiptQty < 0) { error += "ReceiptQty "; }
                    if (string.IsNullOrEmpty(npRecNom.KochId)) { error += "KochId "; }
                    else
                    {
                        if (existingKochIds.Contains(npRecNom.KochId) == true) { error += "KochId_Already_Exists "; }
                        else
                        if (duplicateCurrentKochIds.Contains(npRecNom.KochId) == true) { error += "Duplicate_KochId "; }
                    }

                       
                    
                    if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(BatchError))
                    {
                        npRecNom.IsValideNom = true;
                    }
                    else
                    {
                        error = "This nomination is invalid. Please, Check " + BatchError + error;
                        var invalidNom = new NominationsResponseDTO()
                        {
                            KochId = kochId,
                            ResponseMessage = error
                        };
                        InvalideNoms.Add(invalidNom);
                    }
                }

                #endregion

                
                #region NonPathed Del Nom validation

                foreach (var npDelNom in nonpathedNom.NonPathedDelNomList)
                {
                    var kochId = npDelNom.KochId;
                    var error = string.Empty;
                    if (string.IsNullOrEmpty(npDelNom.DeliveryLocId)) { error += "DeliveryLocId "; }
                    if (string.IsNullOrEmpty(npDelNom.TransactionType) || npDelNom.TransactionType == "1") { error += "TransactionType "; }
                    if (string.IsNullOrEmpty(npDelNom.DnstreamId)) { error += "DnstreamId "; }
                    if (string.IsNullOrEmpty(npDelNom.DeliveryRank)) { error += "DeliveryRank "; }
                    if (string.IsNullOrEmpty(npDelNom.DnstreamK)) { error += "DnstreamK "; }
                    if (npDelNom.DeliveryQty < 0) { error += "DeliveryQty "; }
                    if (string.IsNullOrEmpty(npDelNom.KochId)) { error += "KochId "; }
                    else
                    {
                        if (existingKochIds.Contains(npDelNom.KochId) == true) { error += "KochId_Already_Exists "; }
                        else
                        if (duplicateCurrentKochIds.Contains(npDelNom.KochId) == true) { error += "Duplicate_KochId "; }
                    }
                    if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(BatchError))
                    {
                        npDelNom.IsValideNom = true;
                    }
                    else
                    {
                        error = "This nomination is invalid. Please, Check " + BatchError + error;
                        var invalidNom = new NominationsResponseDTO()
                        {
                            KochId = kochId,
                            ResponseMessage = error
                        };
                        InvalideNoms.Add(invalidNom);
                    }
                }

                #endregion

            }

            #endregion

            #region SaveNSend NonPathed 

            SaveNSendNonPathedNoms(nonpathedList, userId);

            #endregion


            if (InvalideNoms.Count == 0) {
                var invalidNom = new NominationsResponseDTO()
                {
                    KochId = string.Empty,
                    ResponseMessage = "All Noms Uploaded Successfully."
                };
                InvalideNoms.Add(invalidNom);
            }

            return InvalideNoms;
        }

        public bool SaveNSendPathedNoms(List<PathedNomDTO> pathedNomList,string userId)
        {
            Guid transactionId = new Guid();
           

            foreach (var pathed in pathedNomList)
            {          
                try {
                    ContractsDTO flyCon = new ContractsDTO();
                    Contract con = contractService.GetContractByContractNo(pathed.ServiceRequesterContractCode,pipelineId);
                    if (con == null)
                    {
                        flyCon = AddContractOnFly(pathed.ServiceRequesterContractCode, pipelineId, shipperId, userId, pathed.FuelPercentage);
                      
                    }

                Random ran = new Random();
                string path = Path.GetRandomFileName();
                path = path.Replace(".", "");
                V4_Batch batch = new V4_Batch();
                batch.CreatedBy = userId.ToString();
                batch.CreatedDate = DateTime.Now;
                batch.CycleId = cycleList.Where(a => a.Code == pathed.CycleCode).Select(a=>a.Id).FirstOrDefault();
                batch.Description = "Pathed Nomination" + DateTime.Now.ToString();
                batch.FlowEndDate = pathed.EndDateTime;
                batch.FlowStartDate = pathed.StartDateTime;
                batch.NomSubCycle = pathed.NomSubCycle;
                batch.IsActive = true;

                batch.NomTypeID = (int)NomType.Pathed;
                batch.PakageCheck = false;
                batch.PipelineID = pipelineId;
                batch.RankingCheck = false;
                batch.ReferenceNumber = ran.Next(999999999).ToString();
                batch.ScheduleDate = DateTime.MaxValue;
                batch.ServiceRequester = shipperDuns;
                batch.ShowZeroCheck = false;
                batch.ShowZeroDn = false;
                batch.ShowZeroUp = false;
                batch.StatusID = (int)NomStatus.Draft;
                batch.SubmitDate = DateTime.MaxValue;
                batch.TransactionSetControlNumber = path;
                batch.UpDnContractCheck = false;
                batch.UpDnPkgCheck = false;
                 _batchRepository.Add(batch);
                 _batchRepository.SaveChages();
               
                V4_Nomination nomination = new V4_Nomination();
                nomination.AssignIdentification = string.Empty;
                    nomination.AssociatedContract = string.Empty;
                    nomination.BidTransportationRate = string.Empty;
                    nomination.BidupIndicator = string.Empty;
                    nomination.CapacityTypeIndicator = string.Empty;
                    nomination.ContractNumber = !string.IsNullOrEmpty(flyCon.RequestNo) ? flyCon.RequestNo : pathed.ServiceRequesterContractCode;
                nomination.DealType = string.Empty;
                    nomination.DeliveryLocationIdentifer = pathed.DeliveryLocId;
                nomination.DeliveryLocationName = string.Empty;
                nomination.DeliveryLocationPropCode = string.Empty;
                    nomination.DeliveryRank = pathed.DeliveryRank;
                nomination.DelQuantity = Convert.ToInt32(pathed.DelQuantity);
                nomination.DownstreamContractIdentifier = pathed.DnstreamK;
                nomination.DownstreamIdentifier = pathed.DnstreamId;
                nomination.DownstreamName = string.Empty;
                    nomination.DownstreamPackageId = string.Empty;
                    nomination.DownstreamPropCode = string.Empty;
                    nomination.DownstreamRank = string.Empty;
                    nomination.ExportDecleration = string.Empty;
                    nomination.FuelPercentage = Convert.ToDecimal(pathed.FuelPercentage);              
                nomination.MaxRateIndicator = string.Empty;
                    nomination.NominationSubCycleIndicator = pathed.NomSubCycle;
                nomination.NominationUserData1 = string.Empty;
                    nomination.NominationUserData2 = string.Empty;
                    nomination.NominatorTrackingId = NomTrackingID(9);
                nomination.PackageId = pathed.PackageId;                
                nomination.PathType = "P";
                nomination.ProcessingRightIndicator = string.Empty;
                    nomination.Quantity = Convert.ToInt32(pathed.RecQty);
                nomination.QuantityTypeIndicator = "R";   // By default value "Receipt"
                nomination.ReceiptLocationIdentifier = pathed.ReceiptLocId;
                nomination.ReceiptLocationName = string.Empty;
                    nomination.receiptLocationPropCode = string.Empty;
                    nomination.ReceiptRank = pathed.ReceiptRank;               
                nomination.ServiceProviderActivityCode = string.Empty;
                    nomination.TransactionID = batch.TransactionID;
                nomination.TransactionType = pathed.TransactionType;

                var ttPathed = _metadataTransactionTypeRepository.GetTTUsingIdentifier(pathed.TransactionType, "P", pipelineId);
                if (ttPathed != null)
                    nomination.TransactionTypeDesc = ttPathed.Name;
                else
                    nomination.TransactionTypeDesc = string.Empty;

                nomination.UpstreamContractIdentifier = pathed.UpstreamK;
                nomination.UpstreamIdentifier = pathed.UpstreamId;
                nomination.UpstreamName = string.Empty;
                    nomination.UpstreamPackageId = string.Empty;
                    nomination.UpstreamPropCode = string.Empty;
                    nomination.UpstreamRank = string.Empty;
                    nomination.UnitOfMeasure = "BZ";
                    nomination.KochId = pathed.KochId;
                _nominationsRepository.Add(nomination);
                _nominationsRepository.SaveChages();
                transactionId = batch.TransactionID;
                _pntNominationService.SendNominationTransaction(transactionId,shipperId,true);
            }
            catch (Exception ex)
           {
                    throw new Exception("Pathed noms saving : " + ex.Message + " innerEx:- " + ex.InnerException);
           }
          }
            return true;
        }

        private ContractsDTO AddContractOnFly(string contract, int pipelineID, int companyID, string userID, double fuelPercent)
        {
            ContractsDTO model = new ContractsDTO();
            model.RequestNo = contract;
            model.FuelPercentage = Convert.ToDecimal(fuelPercent);
            model.PipelineID = pipelineID;
            model.CreatedBy = userID;
            model.ModifiedBy = model.CreatedBy;
            model.ModifiedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ShipperID = companyID;
            model.IsActive = false;
            model.ValidUpto = DateTime.Now.AddYears(1);
            bool isCreated = contractService.AddContract(model);
            if (isCreated)
                return model;
            else
                return null;
        }

        private string NomTrackingID(int Size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
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
        

        public bool SaveNSendNonPathedNoms(List<NonPathedBatch> NonPathedBatchList, string userId)
        {
            try { 

            foreach (var NonPathedbatch in NonPathedBatchList)
            {
                Guid TransactionID = new Guid();
                bool isFly = false;                
                ContractsDTO flyCon = new ContractsDTO();
                Contract con = contractService.GetContractByContractNo(NonPathedbatch.ServiceRequesterContractCode, pipelineId); //contractService.GetAll().Where(a => a.RequestNo == nom.Contract && a.PipelineID == pathedNom.PipelineID).FirstOrDefault();
                if (con == null)
                {
                    isFly = true;
                    if (NonPathedbatch.ServiceRequesterContractCode != null && NonPathedbatch.ServiceRequesterContractCode != "")
                        flyCon = AddContractOnFly(NonPathedbatch.ServiceRequesterContractCode, pipelineId, shipperId, userId, 0);
                }

                #region create new Nonpathedbatch


                V4_Batch batch = new V4_Batch();
                Random ran = new Random();
                string path = Path.GetRandomFileName();
                path = path.Replace(".", "");
                batch = new V4_Batch();
                batch.CreatedBy = userId;
                batch.CreatedDate = DateTime.Now;

                batch.CycleId = cycleList.Where(a => a.Code == NonPathedbatch.CycleCode).Select(a => a.Id).FirstOrDefault();
                batch.Description = "Non-Pathed Nomination using API, " + DateTime.Now.ToString();
                batch.FlowEndDate = NonPathedbatch.EndDateTime;
                batch.FlowStartDate = NonPathedbatch.StartDateTime;
                batch.IsActive = true;
                batch.NomTypeID = (int)NomType.NonPathed;
                batch.PakageCheck = false;
                batch.PipelineID = pipelineId;
                batch.RankingCheck = false;
                batch.ReferenceNumber = ran.Next(999999999).ToString();
                batch.ScheduleDate = DateTime.MaxValue;
                batch.ServiceRequester = shipperDuns;
                batch.ShowZeroCheck = false;
                batch.ShowZeroDn = false;
                batch.ShowZeroUp = false;
                batch.StatusID = (int)NomStatus.Draft;
                batch.SubmitDate = DateTime.MaxValue;
                batch.TransactionSetControlNumber = path;
                batch.UpDnContractCheck = false;
                batch.UpDnPkgCheck = false;
                batch.NomSubCycle = NonPathedbatch.NomSubCycle;
                _batchRepository.Add(batch);
                _batchRepository.SaveChages();
                TransactionID = batch.TransactionID;

                #endregion

                #region NonPathed Rec Nominations

                foreach (var RecNom in NonPathedbatch.NonPathedRecNomList.Where(a=>a.IsValideNom==true).ToList()) {

                    var ttPathed = _metadataTransactionTypeRepository.GetTTUsingIdentifier(RecNom.TransactionType, "NP", pipelineId);
                    V4_Nomination nomRec = new V4_Nomination
                    {
                        TransactionID = batch.TransactionID,
                        AssignIdentification = "",
                        AssociatedContract = "",
                        BidTransportationRate = "",
                        BidupIndicator = "",
                        CapacityTypeIndicator = "",
                        ContractNumber = isFly ? flyCon.RequestNo : NonPathedbatch.ServiceRequesterContractCode,
                        DealType = "",
                        DeliveryLocationIdentifer = "",
                        DeliveryLocationName = "",
                        DeliveryLocationPropCode = "",
                        DeliveryRank = "",
                        DelQuantity = 0,
                        DownstreamContractIdentifier = "",
                        DownstreamIdentifier = "",
                        DownstreamPackageId = "",
                        DownstreamPropCode = "",
                        DownstreamName = "",
                        DownstreamRank = "",
                        ExportDecleration = "",
                        FuelPercentage = Convert.ToDecimal(RecNom.FuelPercentage),
                        ImbalancePeriod = "",
                        MaxDeliveredQty = 0,
                        MaxRateIndicator = "",
                        NominationSubCycleIndicator = NonPathedbatch.NomSubCycle,
                        NominationUserData1 = "",
                        NominationUserData2 = "",
                        NominatorTrackingId = NomTrackingID(9),
                        PackageId = RecNom.PackageId,
                        PackageId2 = "",
                        PathRank = "",
                        PathType = "NPR",
                        ProcessingRightIndicator = "",
                        receiptLocationPropCode = "",
                        ReceiptLocationName = "",
                        ReceiptLocationIdentifier = RecNom.ReceiptLocId,
                        ReceiptRank = RecNom.ReceiptRank,
                        Route = "",
                        ServiceProviderActivityCode = "",
                        TransactionType = RecNom.TransactionType,
                        TransactionTypeDesc = ttPathed != null ? ttPathed.Name : string.Empty,
                        UnitOfMeasure = "BZ",
                        UpstreamContractIdentifier = RecNom.UpstreamK,
                        UpstreamIdentifier = RecNom.UpstreamId,
                        UpstreamName = "",
                        UpstreamPackageId = "",
                        UpstreamPropCode = "",
                        UpstreamRank = "",
                        Quantity = Convert.ToInt32(RecNom.ReceiptQty),
                        QuantityTypeIndicator = "R",  // For receipt
                        KochId = RecNom.KochId
                    };

                    _nominationsRepository.Add(nomRec);
                    _nominationsRepository.SaveChages();
                }

                #endregion

                #region NonPathed Del Nominations

                foreach (var DeliveryNom in NonPathedbatch.NonPathedDelNomList.Where(a=>a.IsValideNom==true).ToList()) {
                    var ttPathed = _metadataTransactionTypeRepository.GetTTUsingIdentifier(DeliveryNom.TransactionType, "NP", pipelineId);
                    V4_Nomination nomDel = new V4_Nomination
                    {
                        TransactionID = batch.TransactionID,
                        AssignIdentification = "",
                        AssociatedContract = "",
                        BidTransportationRate = "",
                        BidupIndicator = "",
                        CapacityTypeIndicator = "",
                        ContractNumber = isFly ? flyCon.RequestNo : NonPathedbatch.ServiceRequesterContractCode,
                        DealType = "",
                        DeliveryLocationIdentifer = DeliveryNom.DeliveryLocId,
                        DeliveryLocationName = string.Empty,
                        DeliveryLocationPropCode = string.Empty,
                        DeliveryRank = DeliveryNom.DeliveryRank,
                        DelQuantity = Convert.ToInt32(DeliveryNom.DeliveryQty),
                        DownstreamContractIdentifier = DeliveryNom.DnstreamK,
                        DownstreamIdentifier = DeliveryNom.DnstreamId,
                        DownstreamName = string.Empty,
                        DownstreamPackageId = "",
                        DownstreamPropCode = string.Empty,
                        DownstreamRank = "",
                        ExportDecleration = "",
                        FuelPercentage = Convert.ToDecimal(DeliveryNom.FuelPercentage),
                        ImbalancePeriod = "",
                        MaxDeliveredQty = 0,
                        MaxRateIndicator = "",
                        NominationSubCycleIndicator = NonPathedbatch.NomSubCycle,
                        NominationUserData1 = "",
                        NominationUserData2 = "",
                        NominatorTrackingId = NomTrackingID(9),
                        PackageId = DeliveryNom.PackageId,
                        PackageId2 = "",
                        PathRank = "",
                        PathType = "NPD",
                        ProcessingRightIndicator = "",
                        receiptLocationPropCode = "",
                        ReceiptLocationName = "",
                        ReceiptLocationIdentifier = "",
                        ReceiptRank = "",
                        Route = "",
                        ServiceProviderActivityCode = "",
                        TransactionType = DeliveryNom.TransactionType,
                        TransactionTypeDesc = ttPathed != null ? ttPathed.Name : string.Empty,
                        UnitOfMeasure = "BZ",
                        UpstreamContractIdentifier = "",
                        UpstreamIdentifier = "",
                        UpstreamName = "",
                        UpstreamPackageId = "",
                        UpstreamPropCode = "",
                        UpstreamRank = "",
                        Quantity = 0,
                        QuantityTypeIndicator = "D",
                        KochId = DeliveryNom.KochId
                    };

                    _nominationsRepository.Add(nomDel);
                    _nominationsRepository.SaveChages();

                }

                    #endregion

                    #region Send NonPathedbatch
                    int countRecNom = NonPathedbatch.NonPathedRecNomList.Where(a => a.IsValideNom == true).Count();
                    int countDelNom = NonPathedbatch.NonPathedDelNomList.Where(a => a.IsValideNom == true).Count();

                    if (countRecNom == 0 && countDelNom == 0)
                    {
                        // Nothing to Send.
                    }
                    else {
                        _pntNominationService.SendNominationTransaction(TransactionID, shipperId, true);
                    }

                    #endregion

                }
            return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SaveNSendNonPathedNoms: " + ex.Message + " innerEx:- " + ex.InnerException);
            }

        }
            }

    public class APIModules : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IDbFactory>().To<DbFactory>();
            Bind<IInboxRepository>().To<InboxRepository>();
            Bind<IJobWorkflowRepository>().To<JobWorkflowRepository>();
            Bind<ISettingRepository>().To<SettingRepository>();
            Bind<IPipelineRepository>().To<PipelineRepository>();
            Bind<IIncomingDataRepository>().To<IncomingDataRepository>();
            Bind<ITaskMgrJobsRepository>().To<TaskMgrJobsRepository>();
            Bind<IGISBInboxRepository>().To<GISBInboxRepository>();
            Bind<IApplicationLogRepository>().To<ApplicationLogRepository>();
            Bind<ImetadataErrorCodeRepository>().To<metadataErrorCodeRepository>();
            Bind<IManageIncomingRequestService>().To<ManageIncomingRequests>();
            Bind<IShipperCompanyRepository>().To<ShipperCompanyRepository>();
            Bind<INominationsRepository>().To<NominationsRepository>();
            Bind<IDashboardService>().To<DashboardService>();
            Bind<ILocationService>().To<LocationService>();
            Bind<ImetadataRequestTypeService>().To<metadataRequestTypeService>();
            Bind<IContractService>().To<ContractService>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserService>().To<UserService>();
            Bind<IShipperRepository>().To<ShipperRepository>();
            Bind<IPathedNominationService>().To<PathedNominationService>();
            Bind<IPNTNominationService>().To<PNTNominationService>();
            Bind<ImetadataCapacityTypeIndicatorRepository>().To<metadataCapacityTypeIndicatorRepository>();
            Bind<IOutboxRepository>().To<OutboxRepository>();
            Bind<IPipelineTransactionTypeMapRepository>().To<PipelineTransactionTypeMapRepository>();
            Bind<ITransactionLogRepository>().To<TransactionLogRepository>();
            Bind<ImetadataFileStatusRepositpry>().To<metadataFileStatusRepository>();
            Bind<ImetadataQuantityTypeRepository>().To<metadataQuantityTypeIndicatorRepository>();
            Bind<ImetadataBidUpIndicatorRepository>().To<metadataBidUpIndicatorRepository>();
            Bind<ImetadataExportDeclarationRepository>().To<metadataExportDeclarationRepository>();
            Bind<ImetadataExportDeclarationService>().To<metadataExportDeclarationService>();
            Bind<ImetadataBidUpIndicatorService>().To<metadataBidUpIndicatorService>();
            Bind<ImetadataCapacityTypeIndicatorService>().To<metadataCapacityTypeIndicatorService>();
            Bind<ImetadataFileStatusService>().To<metadataFileStatusService>();
            Bind<ImetadataQuantityTypeIndicatorService>().To<metadataQuantityTypeIndicatorService>();
            Bind<ICycleIndicator>().To<CycleIndicatorService>();
            Bind<IEmailQueueRepository>().To<EmailQueueRepository>();
            Bind<IEmailTemplateRepository>().To<EmailTemplateRepository>();
            Bind<IEmailQueueService>().To<EmailQueuService>();
            Bind<IEmailTemplateService>().To<EmailTemplateService>();
            Bind<IUploadedNominationRepository>().To<UploadNominationRepository>();
            Bind<IUploadNominationService>().To<UploadNominationService>();
            Bind<IBatchService>().To<BatchService>();
            Bind<ITaskMgrReceiveMultipuleFileRepository>().To<TaskMgrReceiveMultipuleFileRepository>();
            Bind<ICounterPartiesService>().To<CounterPartyService>();
            Bind<ITransactionalReportingService>().To<TransactionalReportingService>();
            Bind<INotifierEntityService>().To<NotifierEntityService>();
            Bind<IPipelineEDISettingService>().To<PipelineEDISettingService>();
            Bind<IUprdStatusService>().To<UprdStatusService>();
            Bind<IUPRDStatuRepository>().To<UPRDStatuRepository>();
            Bind<IIdentityUsersRepo>().To<IdentityUsersRepo>();
            Bind<INonPathedService>().To<NonPathedService>();
            Bind<ISQTSRepository>().To<SQTSRepository>();
            Bind<ISQTSOPPerTransactionRepository>().To<SQTSOPPerTransactionRepository>();
            Bind<IShipperCompSubShipperCompRepository>().To<ShipperCompSubShipperCompRepository>();
            Bind<IPasswordHistoryRepository>().To<PasswordHistoryRepository>();
            Bind<IPasswordHistoryService>().To<PasswordHistoryService>();
            Bind<IModalFactory>().To<ModalFactory>();
            Bind<ITransactionalReportingRepository>().To<TransactionalReportingRepository>();
            Bind<ICounterPartyRepository>().To<CounterPartyRepository>();
            Bind<IContractRepository>().To<ContractRepository>();
            Bind<ImetadataRequestTypeRepository>().To<metadataRequestTypeRepository>();
            Bind<ImetadataTransactionTypeRepository>().To<metadataTransactionTypeRepository>();
            Bind<ISQTSTrackOnNomRepository>().To<SQTSTrackOnNomRepository>();
            Bind<ImetadataCycleRepository>().To<metadataCycleRepository>();
            Bind<IPipelineEDISettingRepository>().To<PipelineEDISettingRepository>();
            Bind<INominationStatusRepository>().To<NominationStatusRepository>();
            Bind<IBatchRepository>().To<BatchRepository>();
            Bind<INMQRPerTransactionRepository>().To<NMQRPerTransactionRepository>();
            Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();
            Bind<ISQTSPerTransactionRepository>().To<SQTSPerTransactionRepository>();
            Bind<IRouteRepository>().To<RouteRepository>();
            Bind<IPipelineService>().To<PipelineService>();
            Bind<ILocationRepository>().To<LocationRepository>();
        }
    }
}