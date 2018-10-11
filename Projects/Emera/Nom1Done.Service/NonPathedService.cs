using Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service
{
    public class NonPathedService : INonPathedService
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        IBatchRepository _batchRepo;
        INominationsRepository _nomRepo;
        IModalFactory _modelFactory;
        IContractService contractService;
        INominationsRepository _nominationRepo;
        ImetadataTransactionTypeRepository metadataTransactionTypeRepository;

        public NonPathedService(IContractService contractService, ImetadataTransactionTypeRepository metadataTransactionTypeRepository,INominationsRepository nominationRepo,IBatchRepository batchRepo, INominationsRepository nomRepo, IModalFactory modelFactory)
        {
            this.contractService = contractService;
            this.metadataTransactionTypeRepository=metadataTransactionTypeRepository;
            this._nominationRepo = nominationRepo;
             _batchRepo = batchRepo;
            _nomRepo = nomRepo;
            _modelFactory = modelFactory;
        }
        

        public bool UpdateNonPathedNomination(NonPathedDTO model)
        {
            throw new NotImplementedException();
        }

        public NonPathedDTO GetNonPathedNominationOnTransactionId(Guid TransactionId)
        {
            throw new NotImplementedException();
        }

        // private Guid? UpdateNonPathedReceiptNom() { }

        private ContractsDTO AddContractOnFly(string contract, int pipelineID, int companyID, string userID,decimal fuelPercent)
        {
            ContractsDTO model = new ContractsDTO();
            model.RequestNo = contract;
            model.FuelPercentage = fuelPercent;
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

        public Guid? SaveAllNonPathedNominations(NonPathedDTO NonpathedNom)
        {

            bool isFly = false;
            Guid TransactionID = Guid.Empty;
            #region Save Receipt Noms
            try
            { 

            foreach (var RecNom in NonpathedNom.ReceiptNoms)
            {
                    isFly = false;
                    ContractsDTO flyCon = new ContractsDTO();
                    Contract con = contractService.GetContractByContractNo(RecNom.ServiceRequesterContractCode, NonpathedNom.PipelineId); 
                    if (con == null)
                    {
                        isFly = true;
                        if(RecNom.ServiceRequesterContractCode!=null && RecNom.ServiceRequesterContractCode!="")
                            flyCon = AddContractOnFly(RecNom.ServiceRequesterContractCode, NonpathedNom.PipelineId, NonpathedNom.CompanyId, NonpathedNom.UserId.ToString(),RecNom.FuelPercentage);
                        //RecNom.FuelPercentage = flyCon.FuelPercentage;
                    }
                    //else
                    //{
                    //    RecNom.FuelPercentage = con.FuelPercentage;
                    //}
                    V4_Batch batch = null;
                    if (RecNom.TransactionId != null) {                   
                        batch = _batchRepo.GetByTransactionID((RecNom.TransactionId).Value);
                    }
                   
                    if (batch != null)
                    {

                        // Batch Update
                        TransactionID = batch.TransactionID;
                        batch.FlowStartDate = RecNom.StartDateTime;
                        batch.FlowEndDate = RecNom.EndDateTime;
                        batch.CycleId = RecNom.CycleId;
                        batch.NomTypeID = (int)NomType.NonPathed;
                        batch.NomSubCycle = RecNom.NomSubCycle;
                        _batchRepo.Update(batch);
                        _batchRepo.SaveChages();

                        // Exists Nominations Remove
                        List<V4_Nomination> storedNom = _nominationRepo.GetAllNomsByTransactionId(batch.TransactionID);

                        if (storedNom != null && storedNom.Count > 0)
                            _nominationRepo.deleteAll(storedNom);
                    }
                    else
                    {
                        // New Batch Create
                        Random ran = new Random();
                        string path = Path.GetRandomFileName();
                        path = path.Replace(".", "");
                        batch = new V4_Batch();
                        batch.CreatedBy = NonpathedNom.UserId.ToString();
                        batch.CreatedDate = DateTime.Now;
                        batch.CycleId = RecNom.CycleId;
                        batch.Description = "Non-Pathed Receipt Nomination" + DateTime.Now.ToString();
                        batch.FlowEndDate = RecNom.EndDateTime;
                        batch.FlowStartDate = RecNom.StartDateTime;
                        batch.IsActive = true;
                        batch.NomTypeID = (int)NomType.NonPathed;
                        batch.PakageCheck = false;
                        batch.PipelineID = NonpathedNom.PipelineId;
                        batch.RankingCheck = false;
                        batch.ReferenceNumber = ran.Next(999999999).ToString();
                        batch.ScheduleDate = DateTime.MaxValue;
                        batch.ServiceRequester = NonpathedNom.ShipperDuns;
                        batch.ShowZeroCheck = false;
                        batch.ShowZeroDn = false;
                        batch.ShowZeroUp = false;
                        batch.StatusID = (int)NomStatus.Draft;
                        batch.SubmitDate = DateTime.MaxValue;
                        batch.TransactionSetControlNumber = path;
                        batch.UpDnContractCheck = false;
                        batch.UpDnPkgCheck = false;
                        batch.NomSubCycle = RecNom.NomSubCycle;
                        _batchRepo.Add(batch);
                        _batchRepo.SaveChages();
                        TransactionID = batch.TransactionID;
                    }
                    var ttPathed = metadataTransactionTypeRepository.GetTTUsingIdentifier(RecNom.TransactionType, "NP", NonpathedNom.PipelineId);
                    V4_Nomination nomRec = new V4_Nomination
                    {
                        TransactionID = batch.TransactionID,
                        AssignIdentification = "",
                        AssociatedContract = "",
                        BidTransportationRate = "",
                        BidupIndicator = "",
                        CapacityTypeIndicator = "",
                        ContractNumber = isFly ? flyCon.RequestNo : RecNom.ServiceRequesterContractCode,
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
                        FuelPercentage = RecNom.FuelPercentage,
                        ImbalancePeriod = "",
                        MaxDeliveredQty = 0,
                        MaxRateIndicator ="",
                        NominationSubCycleIndicator = RecNom.NomSubCycle,
                        NominationUserData1 = "",
                        NominationUserData2 = "",
                        NominatorTrackingId = NomTrackingID(9),
                        PackageId = RecNom.PackageId,
                        PackageId2 = "",
                        PathRank = "",
                        PathType = "NPR",
                        ProcessingRightIndicator = "",
                        receiptLocationPropCode = RecNom.ReceiptLocProp,
                        ReceiptLocationName=RecNom.ReceiptLocName,
                        ReceiptLocationIdentifier = RecNom.ReceiptLocId,
                        ReceiptRank = RecNom.ReceiptRank,
                        Route = "",
                        ServiceProviderActivityCode = "",
                        TransactionType = RecNom.TransactionType,
                        TransactionTypeDesc = ttPathed != null ? ttPathed.Name : string.Empty,
                        UnitOfMeasure = "BZ",
                        UpstreamContractIdentifier = RecNom.UpstreamK,
                        UpstreamIdentifier = RecNom.UpstreamId,
                        UpstreamName=RecNom.UpstreamName,
                        UpstreamPackageId = "",
                        UpstreamPropCode = RecNom.UpstreamProp,
                        UpstreamRank = "",
                        Quantity =  Convert.ToInt32(RecNom.ReceiptQty),
                        QuantityTypeIndicator = "R"   // For receipt
                    };

                    _nominationRepo.Add(nomRec);
                    _nominationRepo.SaveChages();               
               
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Non-Pathed receive noms "+ex.Message+" innerEx:- "+ex.InnerException);
            }
            #endregion

            #region Save Delivery Noms
            try
            {
                foreach (var DeliveryNom in NonpathedNom.DeliveryNoms)
                {
                    isFly = false;
                    ContractsDTO flyCon = new ContractsDTO();
                    Contract con = contractService.GetContractByContractNo(DeliveryNom.ServiceRequesterContractCode, NonpathedNom.PipelineId); //contractService.GetAll().Where(a => a.RequestNo == nom.Contract && a.PipelineID == pathedNom.PipelineID).FirstOrDefault();
                    if (con == null)
                    {
                        isFly = true;
                        if (DeliveryNom.ServiceRequesterContractCode != null && DeliveryNom.ServiceRequesterContractCode != "")
                            flyCon = AddContractOnFly(DeliveryNom.ServiceRequesterContractCode, NonpathedNom.PipelineId, NonpathedNom.CompanyId, NonpathedNom.UserId.ToString(), DeliveryNom.FuelPercentage);
                        //DeliveryNom.FuelPercentage = flyCon.FuelPercentage;
                    }
                    //else
                    //{
                    //    DeliveryNom.FuelPercentage = con.FuelPercentage;
                    //}
                    V4_Batch batch = null;
                    if (DeliveryNom.TransactionId != null)
                    {
                        batch = _batchRepo.GetByTransactionID((DeliveryNom.TransactionId).Value);
                    }
                                        
                    if (batch != null)
                    {                      

                        // batch
                        TransactionID = batch.TransactionID;
                        batch.FlowStartDate = DeliveryNom.StartDateTime;
                        batch.FlowEndDate = DeliveryNom.EndDateTime;
                        batch.CycleId = DeliveryNom.CycleId;
                        batch.NomTypeID = (int)NomType.NonPathed;
                        batch.NomSubCycle = DeliveryNom.NomSubCycle;
                        _batchRepo.Update(batch);
                        _batchRepo.SaveChages();

                        // Exists Nomination Remove
                        List<V4_Nomination> storedNom = _nominationRepo.GetAllNomsByTransactionId(batch.TransactionID);
                        if (storedNom != null && storedNom.Count > 0)
                            _nominationRepo.deleteAll(storedNom);

                    }
                    else
                    {
                        // New Batch Create
                        Random ran = new Random();
                        string path = Path.GetRandomFileName();
                        path = path.Replace(".", "");
                        batch = new V4_Batch();
                        batch.CreatedBy = NonpathedNom.UserId.ToString();
                        batch.CreatedDate = DateTime.Now;
                        batch.CycleId = DeliveryNom.CycleId;
                        batch.Description = "Non-Pathed Receipt Nomination" + DateTime.Now.ToString();
                        batch.FlowEndDate = DeliveryNom.EndDateTime;
                        batch.FlowStartDate = DeliveryNom.StartDateTime;
                        batch.IsActive = true;
                        batch.NomTypeID = (int)NomType.NonPathed;
                        batch.PakageCheck = false;
                        batch.PipelineID = NonpathedNom.PipelineId;
                        batch.RankingCheck = false;
                        batch.ReferenceNumber = ran.Next(999999999).ToString();
                        batch.ScheduleDate = DateTime.MaxValue;
                        batch.ServiceRequester = NonpathedNom.ShipperDuns;
                        batch.ShowZeroCheck = false;
                        batch.ShowZeroDn = false;
                        batch.ShowZeroUp = false;
                        batch.StatusID = (int)NomStatus.Draft;
                        batch.SubmitDate = DateTime.MaxValue;
                        batch.TransactionSetControlNumber = path;
                        batch.UpDnContractCheck = false;
                        batch.UpDnPkgCheck = false;
                        batch.NomSubCycle = DeliveryNom.NomSubCycle;
                        _batchRepo.Add(batch);
                        _batchRepo.SaveChages();
                        TransactionID = batch.TransactionID;
                    }

                    // Add Nomination
                    var ttPathed = metadataTransactionTypeRepository.GetTTUsingIdentifier(DeliveryNom.TransactionType, "NP", NonpathedNom.PipelineId);
                    V4_Nomination nomDel = new V4_Nomination
                    {
                        TransactionID = batch.TransactionID,
                        AssignIdentification = "",
                        AssociatedContract = "",
                        BidTransportationRate = "",
                        BidupIndicator = "",
                        CapacityTypeIndicator = "",
                        ContractNumber = isFly ? flyCon.RequestNo : DeliveryNom.ServiceRequesterContractCode,
                        DealType = "",
                        DeliveryLocationIdentifer = DeliveryNom.DeliveryLocId,
                        DeliveryLocationName = DeliveryNom.DeliveryLocName,
                        DeliveryLocationPropCode = DeliveryNom.DeliveryLocProp,
                        DeliveryRank = DeliveryNom.DeliveryRank,
                        DelQuantity = Convert.ToInt32(DeliveryNom.DeliveryQty),
                        DownstreamContractIdentifier = DeliveryNom.DnstreamK,
                        DownstreamIdentifier = DeliveryNom.DnstreamId,
                        DownstreamName = DeliveryNom.DeliveryLocName,
                        DownstreamPackageId = "",
                        DownstreamPropCode = DeliveryNom.DnstreamProp,
                        DownstreamRank = "",
                        ExportDecleration = "",
                        FuelPercentage = DeliveryNom.FuelPercentage,
                        ImbalancePeriod = "",
                        MaxDeliveredQty = 0,
                        MaxRateIndicator = "",
                        NominationSubCycleIndicator = DeliveryNom.NomSubCycle,
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
                            UpstreamName="",
                            UpstreamPackageId = "",
                            UpstreamPropCode = "",
                            UpstreamRank = "",
                            Quantity = 0,
                            QuantityTypeIndicator = "D"
                        };

                        _nominationRepo.Add(nomDel);
                        _nominationRepo.SaveChages();
                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Non-Pathed delivery noms " + ex.Message + " innerEx:- " + ex.InnerException); ;
            }
            #endregion

            return TransactionID;

        }

        public NonPathedDTO GetNonPathedNominations(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {
           return  _nominationRepo.GetNonPathedNominations(PipelieID,Status,StartDate,EndDate,shipperDuns,loginUser);
        }

        //public Guid SaveNonPathedNomination(NonPathedDTO model)
        //{
        //    try
        //    {
        //        Random ran = new Random();
        //        string path = Path.GetRandomFileName();
        //        path = path.Replace(".", "");
        //        V4_Batch batch = new V4_Batch();
        //        // batch.CreatedBy = model.CreatedBy;
        //        batch.CreatedDate = DateTime.Now;
        //        //  batch.CycleId = model.CycleId;
        //        batch.Description = "";
        //        //   batch.FlowEndDate = model.EndDateTime;
        //        //   batch.FlowStartDate = model.StartDateTime;
        //        batch.IsActive = true;
        //        batch.NomTypeID = (int)NomType.NonPathed;
        //        batch.PipelineID = model.PipelineId;
        //        batch.ReferenceNumber = ran.Next(999999999).ToString();
        //        batch.ScheduleDate = DateTime.MaxValue;
        //        batch.ServiceRequester = model.ShipperDuns;
        //        batch.StatusID = (int)NomStatus.Draft;
        //        batch.SubmitDate = DateTime.MaxValue;
        //        batch.TransactionSetControlNumber = path;
        //        //  batch.NomSubCycle = model.NomSubCycle;
        //        _batchRepo.Add(batch);
        //        _batchRepo.SaveChages();
        //        List<V4_Nomination> NomList = new List<V4_Nomination>();
        //        foreach (var rec in model.ReceiptNoms)
        //        {
        //            rec.TransactionId = batch.TransactionID;
        //            rec.NomTrackingId = NomTrackingID(9);
        //            NomList.Add(new V4_Nomination
        //            {
        //                TransactionID = rec.TransactionId,
        //                AssignIdentification = "",
        //                AssociatedContract = "",
        //                BidTransportationRate = "",
        //                BidupIndicator = "",
        //                CapacityTypeIndicator = "",
        //                ContractNumber = rec.ServiceRequesterContractCode,
        //                DealType = "",
        //                DeliveryLocationIdentifer = "",
        //                DeliveryLocationPropCode = "",
        //                DeliveryRank = "",
        //                DelQuantity = 0,
        //                DownstreamContractIdentifier = "",
        //                DownstreamIdentifier = "",
        //                DownstreamPackageId = "",
        //                DownstreamPropCode = "",
        //                DownstreamRank = "",
        //                ExportDecleration = "",
        //                FuelPercentage = 0,
        //                ImbalancePeriod = "",
        //                MaxDeliveredQty = 0,
        //                MaxRateIndicator = "",
        //                NominationSubCycleIndicator = batch.NomSubCycle,
        //                NominationUserData1 = "",
        //                NominationUserData2 = "",
        //                NominatorTrackingId = rec.NomTrackingId,
        //                PackageId = rec.PackageId,
        //                PackageId2 = "",
        //                PathRank = "",
        //                PathType = "NPR",
        //                ProcessingRightIndicator = "",
        //                receiptLocationPropCode = rec.ReceiptLocProp,
        //                ReceiptLocationIdentifier = rec.ReceiptLocId,
        //                ReceiptRank = rec.ReceiptRank,
        //                Route = "",
        //                ServiceProviderActivityCode = "",
        //                TransactionType = rec.TransactionType,
        //                TransactionTypeDesc = rec.TransactionTypeDesc,
        //                UnitOfMeasure = "BZ",
        //                UpstreamContractIdentifier = rec.UpstreamK,
        //                UpstreamIdentifier = rec.UpstreamId,
        //                UpstreamPackageId = "",
        //                UpstreamPropCode = rec.UpstreamProp,
        //                UpstreamRank = "",
        //                Quantity = Convert.ToInt32(rec.ReceiptQty),
        //                QuantityTypeIndicator = "R"
        //            });
        //        }
        //        foreach (var del in model.DeliveryNoms)
        //        {
        //            del.TransactionId = batch.TransactionID;
        //            del.NomTrackingId = NomTrackingID(9);
        //            NomList.Add(new V4_Nomination
        //            {
        //                TransactionID = del.TransactionId,
        //                AssignIdentification = "",
        //                AssociatedContract = "",
        //                BidTransportationRate = "",
        //                BidupIndicator = "",
        //                CapacityTypeIndicator = "",
        //                ContractNumber = del.ServiceRequesterContractCode,
        //                DealType = "",
        //                DeliveryLocationIdentifer = del.DeliveryLocId,
        //                DeliveryLocationPropCode = del.DeliveryLocProp,
        //                DeliveryRank = del.DeliveryRank,
        //                DelQuantity = Convert.ToInt32(del.DeliveryQty),
        //                DownstreamContractIdentifier = del.DnstreamK,
        //                DownstreamIdentifier = del.DnstreamId,
        //                DownstreamPackageId = "",
        //                DownstreamPropCode = del.DnstreamProp,
        //                DownstreamRank = "",
        //                ExportDecleration = "",
        //                FuelPercentage = 0,
        //                ImbalancePeriod = "",
        //                MaxDeliveredQty = 0,
        //                MaxRateIndicator = "",
        //                NominationSubCycleIndicator = batch.NomSubCycle,
        //                NominationUserData1 = "",
        //                NominationUserData2 = "",
        //                NominatorTrackingId = del.NomTrackingId,
        //                PackageId = del.PackageId,
        //                PackageId2 = "",
        //                PathRank = "",
        //                PathType = "NPD",
        //                ProcessingRightIndicator = "",
        //                receiptLocationPropCode = "",
        //                ReceiptLocationIdentifier = "",
        //                ReceiptRank = "",
        //                Route = "",
        //                ServiceProviderActivityCode = "",
        //                TransactionType = del.TransactionType,
        //                TransactionTypeDesc = del.TransactionTypeDesc,
        //                UnitOfMeasure = "BZ",
        //                UpstreamContractIdentifier = "",
        //                UpstreamIdentifier = "",
        //                UpstreamPackageId = "",
        //                UpstreamPropCode = "",
        //                UpstreamRank = "",
        //                Quantity = Convert.ToInt32(del.DeliveryQty),
        //                QuantityTypeIndicator = "D"
        //            });
        //        }
        //        _nomRepo.AddBulkNoms(NomList);
        //        return batch.TransactionID;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Guid.Empty;
        //    }
        //}

        private static string NomTrackingID(int Size)
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

        public Guid SaveNonPathedNomination(NonPathedDTO model)
        {
            throw new NotImplementedException();
        }

        //public bool UpdateNonPathedNomination(NonPathedDTO model)
        //{
        //    try
        //    {
        //        V4_Batch batch = _batchRepo.GetByTransactionID(model.TransactionId.Value);
        //        if (batch != null && batch.SubmitDate.Value.Date==DateTime.MaxValue.Date)
        //        {
        //            batch.CycleId = model.CycleId;
        //            batch.Description = "";
        //            batch.FlowEndDate = model.EndDateTime;
        //            batch.FlowStartDate = model.StartDateTime;
        //            batch.ServiceRequester = model.ShipperDuns;
        //            batch.NomSubCycle = model.NomSubCycle;                   
        //            _batchRepo.Update(batch);
        //            _batchRepo.SaveChages();
        //            List<V4_Nomination> NomList = _nomRepo.GetAllNomsByTransactionId(batch.TransactionID);
        //            if (NomList.Count > 0)
        //                _nomRepo.deleteAll(NomList);
        //            NomList = new List<V4_Nomination>();
        //            foreach (var rec in model.ReceiptNoms)
        //            {
        //                rec.TransactionId = batch.TransactionID;
        //                // rec.ServiceRequesterContract = batch.ServiceRequester;                       
        //                rec.NomTrackingId = NomTrackingID(9);
        //                NomList.Add(new V4_Nomination
        //                {
        //                    TransactionID = rec.TransactionId,
        //                    AssignIdentification = "",
        //                    AssociatedContract = "",
        //                    BidTransportationRate = "",
        //                    BidupIndicator = "",
        //                    CapacityTypeIndicator = "",
        //                    ContractNumber = rec.ServiceRequesterContractCode,
        //                    DealType = "",
        //                    DeliveryLocationIdentifer = "",
        //                    DeliveryLocationPropCode = "",
        //                    DeliveryRank = "",
        //                    DelQuantity = 0,
        //                    DownstreamContractIdentifier = "",
        //                    DownstreamIdentifier = "",
        //                    DownstreamPackageId = "",
        //                    DownstreamPropCode = "",
        //                    DownstreamRank = "",
        //                    ExportDecleration = "",
        //                    FuelPercentage = 0,
        //                    ImbalancePeriod = "",
        //                    MaxDeliveredQty = 0,
        //                    MaxRateIndicator = "",
        //                    NominationSubCycleIndicator = batch.NomSubCycle,
        //                    NominationUserData1 = "",
        //                    NominationUserData2 = "",
        //                    NominatorTrackingId = rec.NomTrackingId,
        //                    PackageId = rec.PackageId,
        //                    PackageId2 = "",
        //                    PathRank = "",
        //                    PathType = "NPR",
        //                    ProcessingRightIndicator = "",
        //                    receiptLocationPropCode = rec.ReceiptLocProp,
        //                    ReceiptLocationIdentifier = rec.ReceiptLocId,
        //                    ReceiptRank = rec.ReceiptRank,
        //                    Route = "",
        //                    ServiceProviderActivityCode = "",
        //                    TransactionType = rec.TransactionType,
        //                    TransactionTypeDesc = rec.TransactionTypeDesc,
        //                    UnitOfMeasure = "BZ",
        //                    UpstreamContractIdentifier = rec.UpstreamK,
        //                    UpstreamIdentifier = rec.UpstreamId,
        //                    UpstreamPackageId = "",
        //                    UpstreamPropCode = rec.UpstreamProp,
        //                    UpstreamRank = "",
        //                    Quantity = Convert.ToInt32(rec.ReceiptQty),
        //                    QuantityTypeIndicator = "R"
        //                });
        //            }
        //            foreach (var del in model.DeliveryNoms)
        //            {
        //                del.TransactionId = batch.TransactionID;                      
        //                del.NomTrackingId = NomTrackingID(9);
        //                NomList.Add(new V4_Nomination
        //                {
        //                    TransactionID = del.TransactionId,
        //                    AssignIdentification = "",
        //                    AssociatedContract = "",
        //                    BidTransportationRate = "",
        //                    BidupIndicator = "",
        //                    CapacityTypeIndicator = "",
        //                    ContractNumber = del.ServiceRequesterContractCode,
        //                    DealType = "",
        //                    DeliveryLocationIdentifer = del.DeliveryLocId,
        //                    DeliveryLocationPropCode = del.DeliveryLocProp,
        //                    DeliveryRank = del.DeliveryRank,
        //                    DelQuantity = Convert.ToInt32(del.DeliveryQty),
        //                    DownstreamContractIdentifier = del.DnstreamId,
        //                    DownstreamIdentifier = del.DnstreamK,
        //                    DownstreamPackageId = "",
        //                    DownstreamPropCode = del.DnstreamProp,
        //                    DownstreamRank = "",
        //                    ExportDecleration = "",
        //                    FuelPercentage = 0,
        //                    ImbalancePeriod = "",
        //                    MaxDeliveredQty = 0,
        //                    MaxRateIndicator = "",
        //                    NominationSubCycleIndicator = batch.NomSubCycle,
        //                    NominationUserData1 = "",
        //                    NominationUserData2 = "",
        //                    NominatorTrackingId = del.NomTrackingId,
        //                    PackageId = del.PackageId,
        //                    PackageId2 = "",
        //                    PathRank = "",
        //                    PathType = "NPR",
        //                    ProcessingRightIndicator = "",
        //                    receiptLocationPropCode = "",
        //                    ReceiptLocationIdentifier = "",
        //                    ReceiptRank = "",
        //                    Route = "",
        //                    ServiceProviderActivityCode = "",
        //                    TransactionType = del.TransactionType,
        //                    TransactionTypeDesc = del.TransactionTypeDesc,
        //                    UnitOfMeasure = "BZ",
        //                    UpstreamContractIdentifier = "",
        //                    UpstreamIdentifier = "",
        //                    UpstreamPackageId = "",
        //                    UpstreamPropCode = "",
        //                    UpstreamRank = "",
        //                    Quantity = Convert.ToInt32(del.DeliveryQty),
        //                    QuantityTypeIndicator = "D"
        //                });
        //            }
        //            _nomRepo.AddBulkNoms(NomList);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        //public NonPathedDTO GetNonPathedNominationOnTransactionId(Guid TransactionId)
        //{
        //    NonPathedDTO model = new NonPathedDTO();
        //    var batch=_batchRepo.GetByTransactionID(TransactionId);
        //    if (batch != null)
        //    {
        //        model.CreateDateTime = batch.CreatedDate;
        //        model.CycleId = batch.CycleId;
        //        model.EndDateTime = batch.FlowEndDate;
        //        model.StartDateTime = batch.FlowStartDate;
        //        model.StatusId = batch.StatusID;             
        //        model.CreatedBy = batch.CreatedBy;
        //        model.TransactionId = TransactionId;
        //        model.PipelineId = batch.PipelineID;
        //        model.NomSubCycle = batch.NomSubCycle;
        //        var nomList = _nomRepo.GetAllNomsByTransactionId(TransactionId);
        //        if(nomList!=null && nomList.Count() > 0)
        //        {
        //            var nomListD = nomList.Where(a => a.PathType == "NPD");
        //            var nomListR = nomList.Where(a => a.PathType == "NPR");
        //            foreach(var nR in nomListR)
        //            {
        //                model.ReceiptNoms.Add(new NonPathedRecieptNom
        //                {
        //                    TransactionId=nR.TransactionID,                         
        //                    NomTrackingId=nR.NominatorTrackingId,
        //                    PackageId=nR.PackageId,
        //                    ReceiptLocId=nR.ReceiptLocationIdentifier,
        //                    ReceiptLocName="",
        //                    ReceiptLocProp=nR.receiptLocationPropCode,
        //                    ReceiptQty=nR.Quantity.Value,
        //                    ReceiptRank=nR.ReceiptRank,
        //                    ServiceRequesterContractCode=nR.ContractNumber,
        //                    TransactionType=nR.TransactionType,
        //                    TransactionTypeDesc=nR.TransactionTypeDesc,                         
        //                    UpstreamId=nR.UpstreamIdentifier,
        //                    UpstreamK=nR.UpstreamContractIdentifier,
        //                    UpstreamName="",
        //                    UpstreamProp=nR.UpstreamPropCode
        //                });
        //            }
        //            foreach(var nD in nomListD)
        //            {
        //                model.DeliveryNoms.Add(new NonPathedDeliveryNom
        //                {
        //                    TransactionId=nD.TransactionID,                          
        //                    DeliveryLocId=nD.DeliveryLocationIdentifer,
        //                    DeliveryLocName="",
        //                    DeliveryLocProp=nD.DeliveryLocationPropCode,
        //                    DeliveryQty=nD.Quantity.Value,
        //                    DeliveryRank=nD.DeliveryRank,                          
        //                    DnstreamId=nD.DownstreamIdentifier,
        //                    DnstreamK=nD.DownstreamContractIdentifier,
        //                    DnstreamName="",
        //                    DnstreamProp=nD.DownstreamPropCode,
        //                    NomTrackingId=nD.NominatorTrackingId,
        //                    PackageId=nD.PackageId,
        //                    ServiceRequesterContractCode=nD.ContractNumber,
        //                    TransactionType=nD.TransactionType,
        //                    TransactionTypeDesc=nD.TransactionTypeDesc
        //                });
        //            }
        //        }
        //    }
        //    return model;
        //}
    }
}
