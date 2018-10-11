using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Nom.ViewModel;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using System.IO;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class PathedNominationService : IPathedNominationService
    {
        private readonly IPipelineRepository PipelineRepository;
        private readonly INominationsRepository NominationsRepository;
        private readonly IBatchRepository BatchRepository;
        private readonly ISQTSTrackOnNomRepository SQTSTrackOnNomRepository;
        private readonly ILocationRepository LocationRepository;
        private readonly ICounterPartyRepository counterpartyRepository;
        private readonly IContractService contractService;
        private readonly IModalFactory modelFactory;
        private readonly ImetadataTransactionTypeRepository metadataTransactionTypeRepository;
        private readonly IPipelineTransactionTypeMapRepository PipelineTransactionTypeMapRepository;


        public PathedNominationService(
            IPipelineTransactionTypeMapRepository PipelineTransactionTypeMapRepository,
             ImetadataTransactionTypeRepository metadataTransactionTypeRepository,
            ICounterPartyRepository counterpartyRepository, 
            INominationsRepository NominationsRepository, 
            IBatchRepository BatchRepository, 
            IPipelineRepository PipelineRepository, 
            ISQTSTrackOnNomRepository SQTSTrackOnNomRepository, 
            ILocationRepository LocationRepository,
            IContractService contractService,
            IModalFactory modelFactory)
        {
            this.PipelineTransactionTypeMapRepository = PipelineTransactionTypeMapRepository;
            this.metadataTransactionTypeRepository = metadataTransactionTypeRepository;
           this.NominationsRepository = NominationsRepository;
            this.BatchRepository = BatchRepository;
            this.PipelineRepository = PipelineRepository;
            this.SQTSTrackOnNomRepository = SQTSTrackOnNomRepository;
            this.LocationRepository = LocationRepository;
            this.counterpartyRepository = counterpartyRepository;
            this.contractService = contractService;
            this.modelFactory = modelFactory;
        }        

        //public bool CopyNomination(Guid TransactionId)
        //{
        //    try
        //    {
        //        Guid? newTransactionId = new Guid();
        //        V4_Batch batch = BatchRepository.GetAll().Where(a => a.TransactionID == TransactionId).FirstOrDefault();
        //        if (batch != null)
        //        {

        //            Random ran = new Random();
        //            string TranSetContNumb = Path.GetRandomFileName();
        //            TranSetContNumb = TranSetContNumb.Replace(".", "");
        //            batch.StatusID = (int)NomStatus.Draft;
        //            batch.ReferenceNumber = ran.Next(999999999).ToString();
        //            batch.TransactionSetControlNumber = TranSetContNumb;
        //            batch.TransactionID = Guid.NewGuid();
        //            batch.CreatedDate = DateTime.Now;
        //            BatchRepository.Add(batch);
        //            V4_Nomination nom = NominationsRepository.GetAll().Where(a => a.TransactionID == TransactionId).FirstOrDefault();
        //            if (nom != null && newTransactionId != Guid.Empty)
        //            {
        //                V4_Nomination newNom = new V4_Nomination();
        //                newNom.TransactionID = newTransactionId.Value;
        //                newNom.AssignIdentification = nom.AssignIdentification;
        //                newNom.AssociatedContract = nom.AssociatedContract;
        //                newNom.BidTransportationRate = nom.BidTransportationRate;
        //                newNom.BidupIndicator = nom.BidupIndicator;
        //                newNom.CapacityTypeIndicator = nom.CapacityTypeIndicator;
        //                newNom.ContractNumber = nom.ContractNumber;
        //                newNom.DealType = nom.DealType;
        //                newNom.DeliveryLocationIdentifer = nom.DeliveryLocationIdentifer;
        //                newNom.DeliveryLocationPropCode = nom.DeliveryLocationPropCode;
        //                newNom.DeliveryRank = nom.DeliveryRank;
        //                newNom.DelQuantity = nom.DelQuantity;
        //                newNom.DownstreamContractIdentifier = nom.DownstreamContractIdentifier;
        //                newNom.DownstreamIdentifier = nom.DownstreamIdentifier;
        //                newNom.DownstreamPackageId = nom.DownstreamPackageId;
        //                newNom.DownstreamPropCode = nom.DownstreamPropCode;
        //                newNom.DownstreamRank = nom.DownstreamRank;
        //                newNom.ExportDecleration = nom.ExportDecleration;
        //                newNom.ImbalancePeriod = nom.ImbalancePeriod;
        //                newNom.MaxRateIndicator = nom.MaxRateIndicator;
        //                newNom.NominationSubCycleIndicator = nom.NominationSubCycleIndicator;
        //                newNom.NominationUserData1 = nom.NominationUserData1;
        //                newNom.NominationUserData2 = nom.NominationUserData2;
        //                newNom.NominatorTrackingId = NomTrackingID(9);
        //                newNom.PackageId = nom.PackageId;
        //                newNom.PathRank = nom.PathRank;
        //                newNom.PathType = nom.PathType;
        //                newNom.ProcessingRightIndicator = nom.ProcessingRightIndicator;
        //                newNom.Quantity = nom.Quantity;
        //                newNom.QuantityTypeIndicator = nom.QuantityTypeIndicator;
        //                newNom.ReceiptLocationIdentifier = nom.ReceiptLocationIdentifier;
        //                newNom.receiptLocationPropCode = nom.receiptLocationPropCode;
        //                newNom.ReceiptRank = nom.ReceiptRank;
        //                newNom.Route = nom.Route;
        //                newNom.ServiceProviderActivityCode = nom.ServiceProviderActivityCode;
        //                newNom.TransactionType = nom.TransactionType;
        //                newNom.UnitOfMeasure = nom.UnitOfMeasure;
        //                newNom.UpstreamContractIdentifier = nom.UpstreamContractIdentifier;
        //                newNom.UpstreamIdentifier = nom.UpstreamIdentifier;
        //                newNom.UpstreamPackageId = nom.UpstreamPackageId;
        //                newNom.UpstreamPropCode = nom.UpstreamPropCode;
        //                newNom.UpstreamRank = nom.UpstreamRank;
        //                NominationsRepository.Add(newNom);
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public bool DeleteNominationData(Guid transactionId)
        {
            try
            {
                V4_Batch batch = BatchRepository.GetByTransactionID(transactionId);
                batch.IsActive = false;
                BatchRepository.Update(batch);
                BatchRepository.SaveChages();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Pipeline GetPipeline(int PipeLineId)
        {
            return PipelineRepository.GetById(PipeLineId);
        }
        
        public Guid? SaveAndUpdatePathedNomination(PathedDTO pathedNom, bool IsSave)
        {
            //if (IsSave)
            //    return SavePathedNomination(pathedNom);
            //else
                return UpdatePathedNomination(pathedNom);
        }
        private Guid? SavePathedNomination(PathedDTO pathedNom, PathedNomDetailsDTO pathed)
        {
            Guid transactionId = new Guid();
            try
            {
                ContractsDTO flyCon=new ContractsDTO();
                Contract con = contractService.GetContractByContractNo(pathed.Contract,pathedNom.DunsNo);
                if (con == null)
                {
                    flyCon = AddContractOnFly(pathed.Contract, pathedNom.PipelineDuns, pathedNom.companyId, pathedNom.ShipperID.ToString(),pathed.FuelPercentage);
                    //pathed.FuelPercentage = flyCon.FuelPercentage;
                }
                //else {
                //    pathed.FuelPercentage = con.FuelPercentage;
                //}
               

                Random ran = new Random();
                string path = Path.GetRandomFileName();
                path = path.Replace(".", "");
                V4_Batch batch = new V4_Batch();
                batch.CreatedBy = pathedNom.ShipperID.ToString();
                batch.CreatedDate = DateTime.Now;
                batch.CycleId = pathed.CycleID;
                batch.Description = "Pathed Nomination" + DateTime.Now.ToString();
                batch.FlowEndDate = pathed.EndDate;
                batch.FlowStartDate = pathed.StartDate;
                batch.NomSubCycle = pathed.NomSubCycle;
                batch.IsActive = true;

                //var nomType = PipelineTransactionTypeMapRepository.GetById(pathed.TransTypeMapId);
                //if (nomType==null || nomType.PathType=="P")
                //    batch.NomTypeID = (int)NomType.Pathed;
                //else if (nomType.PathType == "NP")
                //    batch.NomTypeID = (int)NomType.NonPathed;

                batch.NomTypeID = (int)NomType.Pathed;
                batch.PakageCheck = false;
               // batch.PipelineID = pathedNom.PipelineID;
                batch.PipelineDuns = pathedNom.PipelineDuns;
                batch.RankingCheck = false;
                batch.ReferenceNumber = ran.Next(999999999).ToString();
                batch.ScheduleDate = DateTime.MaxValue;
                batch.ServiceRequester = pathedNom.DunsNo;
                batch.ShowZeroCheck = false;
                batch.ShowZeroDn = false;
                batch.ShowZeroUp = false;
                batch.StatusID = (int)NomStatus.Draft;
                batch.SubmitDate = DateTime.MaxValue;
                batch.TransactionSetControlNumber = path;
                batch.UpDnContractCheck = false;
                batch.UpDnPkgCheck = false;
                BatchRepository.Add(batch);
                BatchRepository.SaveChages();
                //if (nomType == null || nomType.PathType == "P")
                //{
                    V4_Nomination nomination = new V4_Nomination();
                    nomination.AssignIdentification = pathed.AssocContract;
                    nomination.AssociatedContract = pathed.ActCode;
                    nomination.BidTransportationRate = pathed.BidTransportRate;
                    nomination.BidupIndicator = pathed.BidUp;
                    nomination.CapacityTypeIndicator = pathed.CapacityType;
                    nomination.ContractNumber = flyCon!=null ? flyCon.RequestNo : pathed.Contract;
                    nomination.DealType = pathed.DealType;
                    nomination.DeliveryLocationIdentifer = pathed.DelLocID;
                nomination.DeliveryLocationName = pathed.DelLoc;
                    nomination.DeliveryLocationPropCode = pathed.DelLocProp;
                    nomination.DeliveryRank = pathed.DelRank;
                    nomination.DelQuantity = string.IsNullOrEmpty(pathed.DelQuantity.ToString()) ? 0 : Convert.ToInt32(pathed.DelQuantity);
                    nomination.DownstreamContractIdentifier = pathed.DownContract;
                    nomination.DownstreamIdentifier = pathed.DownID;
                nomination.DownstreamName = pathed.DownName;
                    nomination.DownstreamPackageId = pathed.DownPkgID;
                    nomination.DownstreamPropCode = pathed.DownIDProp;
                    nomination.DownstreamRank = pathed.DownRank;
                    nomination.ExportDecleration = pathed.Export;
                    nomination.FuelPercentage = pathed.FuelPercentage;
                    //nomination.ImbalancePeriod= not in pathedDTO
                    nomination.MaxRateIndicator = pathed.MaxRate;
                    nomination.NominationSubCycleIndicator = pathed.NomSubCycle;
                    nomination.NominationUserData1 = pathed.NomUserData1;
                    nomination.NominationUserData2 = pathed.NomUserData2;
                    nomination.NominatorTrackingId = NomTrackingID(9);
                    nomination.PackageId = pathed.PkgID;
                    //nomination.PathRank=pathed.p
                    nomination.PathType = "P";
                    nomination.ProcessingRightIndicator = pathed.ProcessingRights;
                    nomination.Quantity = string.IsNullOrEmpty(pathed.RecQty) ? 0 : Convert.ToInt32(pathed.RecQty);
                    nomination.QuantityTypeIndicator = "R";   // By default value "Receipt"
                    nomination.ReceiptLocationIdentifier = pathed.RecLocID;
                nomination.ReceiptLocationName = pathed.RecLocation;
                    nomination.receiptLocationPropCode = pathed.RecLocProp;
                    nomination.ReceiptRank = pathed.RecRank;
                    //nomination.Route=pathed.
                    nomination.ServiceProviderActivityCode = pathed.ActCode;//duplicate nomination.AssignIdentification
                    nomination.TransactionID = batch.TransactionID;
                    nomination.TransactionType = pathed.TransType;

                    var ttPathed = metadataTransactionTypeRepository.GetTTUsingIdentifier(pathed.TransType, "P", pathedNom.PipelineDuns);
                    if(ttPathed!=null)
                      nomination.TransactionTypeDesc = ttPathed.Name;
                    else
                       nomination.TransactionTypeDesc = string.Empty;

                    nomination.UpstreamContractIdentifier = pathed.UpKContract;
                    nomination.UpstreamIdentifier = pathed.UpID;
                nomination.UpstreamName = pathed.UpName;
                    nomination.UpstreamPackageId = pathed.UpPkgID;
                    nomination.UpstreamPropCode = pathed.UpIDProp;
                    nomination.UpstreamRank = pathed.UpRank;
                    nomination.UnitOfMeasure = "BZ";
                    NominationsRepository.Add(nomination);
                    NominationsRepository.SaveChages();

                    transactionId = batch.TransactionID;
                //}
                //else if (nomType.PathType == "NP")
                //{
                //    var ttPathed = metadataTransactionTypeRepository.GetTTUsingIdentifier(pathed.TransType, "NP", pathedNom.PipelineID);
                //    V4_Nomination nomRec= new V4_Nomination
                //    {
                //        TransactionID = batch.TransactionID,
                //        AssignIdentification = "",
                //        AssociatedContract = pathed.AssocContract,
                //        BidTransportationRate = pathed.BidTransportRate,
                //        BidupIndicator = pathed.BidUp,
                //        CapacityTypeIndicator = pathed.CapacityType,
                //        ContractNumber = pathed.Contract,
                //        DealType = pathed.DealType,
                //        DeliveryLocationIdentifer = "",
                //        DeliveryLocationPropCode = "",
                //        DeliveryRank = "",
                //        DelQuantity = 0,
                //        DownstreamContractIdentifier = "",
                //        DownstreamIdentifier = "",
                //        DownstreamPackageId = "",
                //        DownstreamPropCode = "",
                //        DownstreamRank = "",
                //        ExportDecleration = pathed.Export,
                //        FuelPercentage = pathed.FuelPercentage,
                //        ImbalancePeriod = "",
                //        MaxDeliveredQty = 0,
                //        MaxRateIndicator = pathed.MaxRate,
                //        NominationSubCycleIndicator = pathed.NomSubCycle,
                //        NominationUserData1 = "",
                //        NominationUserData2 = "",
                //        NominatorTrackingId = NomTrackingID(9),
                //        PackageId = pathed.PkgIDRec,
                //        PackageId2 = "",
                //        PathRank = "",
                //        PathType = "NPR",
                //        ProcessingRightIndicator = pathed.ProcessingRights,
                //        receiptLocationPropCode = pathed.RecLocProp,
                //        ReceiptLocationIdentifier = pathed.RecLocID,
                //        ReceiptRank = pathed.RecRank,
                //        Route = "",
                //        ServiceProviderActivityCode = "",
                //        TransactionType = pathed.TransType,
                //        TransactionTypeDesc = (ttPathed!=null)?ttPathed.Name:string.Empty,
                //        UnitOfMeasure = "BZ",
                //        UpstreamContractIdentifier = pathed.UpKContract,
                //        UpstreamIdentifier = pathed.UpID,
                //        UpstreamPackageId = pathed.UpPkgID,
                //        UpstreamPropCode = pathed.UpIDProp,
                //        UpstreamRank = pathed.UpRank,
                //        Quantity = string.IsNullOrEmpty(pathed.RecQty) ? 0 : Convert.ToInt32(pathed.RecQty),
                //        QuantityTypeIndicator = "R"
                //    };
                //    V4_Nomination nomDel = new V4_Nomination
                //    {
                //        TransactionID = batch.TransactionID,
                //        AssignIdentification = "",
                //        AssociatedContract = pathed.AssocContract,
                //        BidTransportationRate = pathed.BidTransportRate,
                //        BidupIndicator = pathed.BidUp,
                //        CapacityTypeIndicator = pathed.CapacityType,
                //        ContractNumber = pathed.Contract,
                //        DealType = pathed.DealType,
                //        DeliveryLocationIdentifer = pathed.DelLocID,
                //        DeliveryLocationPropCode = pathed.DelLocProp,
                //        DeliveryRank = pathed.DelRank,
                //        DelQuantity = Convert.ToInt32(pathed.DelQuantity),
                //        DownstreamContractIdentifier = pathed.DownContract,
                //        DownstreamIdentifier = pathed.DownID,
                //        DownstreamPackageId = pathed.DownPkgID,
                //        DownstreamPropCode = pathed.DownIDProp,
                //        DownstreamRank = pathed.DownRank,
                //        ExportDecleration = pathed.Export,
                //        FuelPercentage = pathed.FuelPercentage,
                //        ImbalancePeriod = "",
                //        MaxDeliveredQty = 0,
                //        MaxRateIndicator = pathed.MaxRate,
                //        NominationSubCycleIndicator = pathed.NomSubCycle,
                //        NominationUserData1 = "",
                //        NominationUserData2 = "",
                //        NominatorTrackingId = NomTrackingID(9),
                //        PackageId2 = "",
                //        PackageId = pathed.PkgID,
                //        PathRank = "",
                //        PathType = "NPD",
                //        ProcessingRightIndicator = pathed.ProcessingRights,
                //        receiptLocationPropCode = "",
                //        ReceiptLocationIdentifier = pathed.ProcessingRights,
                //        ReceiptRank = "",
                //        Route = "",
                //        ServiceProviderActivityCode = pathed.ActCode,
                //        TransactionType = pathed.TransType,
                //        TransactionTypeDesc = ttPathed!=null ? ttPathed.Name : string.Empty,
                //        UnitOfMeasure = "BZ",
                //        UpstreamContractIdentifier = "",
                //        UpstreamIdentifier = "",
                //        UpstreamPackageId = "",
                //        UpstreamPropCode = "",
                //        UpstreamRank = "",
                //        Quantity = 0,
                //        QuantityTypeIndicator = "D"
                //    };
                //   if (pathed.PathedHybridNonpathedType == 5 || pathed.PathedHybridNonpathedType == 7 || pathed.PathedHybridNonpathedType == 10)
                //    {
                //        NominationsRepository.Add(nomDel);
                //    }    //  5 7  10  delivery only   // receipt only- 3,9,   // both- 1,2,4,6,8 
                //   else if (pathed.PathedHybridNonpathedType == 3 || pathed.PathedHybridNonpathedType == 9)
                //    {
                //        NominationsRepository.Add(nomRec);
                //    }
                //   else
                //    {
                //        NominationsRepository.Add(nomRec);
                //        NominationsRepository.Add(nomDel);
                //    }                       
                //    NominationsRepository.SaveChages();
                //}
               
                return transactionId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private ContractsDTO AddContractOnFly(string contract, string pipelineDuns, int companyID, string userID,decimal fuelPercent)
        {
            ContractsDTO model = new ContractsDTO();
            model.RequestNo = contract;
            model.FuelPercentage = fuelPercent;
            model.PipeDuns = pipelineDuns;
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
        private Guid? UpdatePathedNomination(PathedDTO pathedNom)
        {
            try
            {
                bool isFly = false;
                Guid TransactionID = Guid.Empty;
                foreach (var nom in pathedNom.PathedNomsList)
                {
                    isFly = false;
                    V4_Batch batch = BatchRepository.GetByTransactionID(nom.TransactionId);
                    if (batch != null)
                    {
                        ContractsDTO flyCon = new ContractsDTO();
                        // Contract con = contractService.GetContractByContractNo(nom.Contract, pathedNom.PipelineID);
                        //if (con == null)
                        //{
                        //    isFly = true;
                        //    flyCon = AddContractOnFly(nom.Contract, pathedNom.PipelineID, pathedNom.companyId, pathedNom.ShipperID.ToString(),nom.FuelPercentage);
                        //   // nom.FuelPercentage = flyCon.FuelPercentage;
                        //}
                        //else
                        //{
                        //   // nom.FuelPercentage = con.FuelPercentage;
                        //}
                        TransactionID = batch.TransactionID;
                        batch.FlowStartDate = nom.StartDate;
                        batch.FlowEndDate = nom.EndDate;
                        batch.CycleId = nom.CycleID;
                        batch.NomTypeID = (int)NomType.Pathed;
                        batch.NomSubCycle = nom.NomSubCycle;
                        batch.CreatedBy = pathedNom.ShipperID.ToString();
                        BatchRepository.Update(batch);
                        BatchRepository.SaveChages();

                        List<V4_Nomination> storedNom = NominationsRepository.GetAllNomsByTransactionId(batch.TransactionID);
                        if (storedNom != null && storedNom.Count > 0)
                            NominationsRepository.deleteAll(storedNom);
                            V4_Nomination nomination = new V4_Nomination();
                            nomination.AssignIdentification = nom.AssocContract;
                            nomination.AssociatedContract = nom.ActCode;
                            nomination.BidTransportationRate = nom.BidTransportRate;
                            nomination.BidupIndicator = nom.BidUp;
                            nomination.CapacityTypeIndicator = nom.CapacityType;
                            // nomination.ContractNumber = !string.IsNullOrEmpty(flyCon.RequestNo) ? flyCon.RequestNo : nom.Contract;
                            nomination.ContractNumber = nom.Contract;
                            nomination.DealType = nom.DealType;
                            nomination.DeliveryLocationIdentifer = nom.DelLocID;
                            nomination.DeliveryLocationName = nom.DelLoc;
                            nomination.DeliveryLocationPropCode = nom.DelLocProp;
                            nomination.DeliveryRank = nom.DelRank;
                            nomination.DelQuantity = string.IsNullOrEmpty(nom.DelQuantity.ToString()) ? 0 : Convert.ToInt32(nom.DelQuantity);
                            nomination.DownstreamContractIdentifier = nom.DownContract;
                            nomination.DownstreamIdentifier = nom.DownID;
                            nomination.DownstreamName = nom.DownName;
                            nomination.DownstreamPackageId = nom.DownPkgID;
                            nomination.DownstreamPropCode = nom.DownIDProp;
                            nomination.DownstreamRank = nom.DownRank;
                            nomination.ExportDecleration = nom.Export;
                            nomination.FuelPercentage = nom.FuelPercentage;
                            //nomination.ImbalancePeriod= not in pathedDTO
                            nomination.MaxRateIndicator = nom.MaxRate;
                            nomination.NominationSubCycleIndicator = nom.NomSubCycle;
                            nomination.NominationUserData1 = nom.NomUserData1;
                            nomination.NominationUserData2 = nom.NomUserData2;
                            nomination.NominatorTrackingId = NomTrackingID(9);
                            nomination.PackageId = nom.PkgID;
                            nomination.PathType = "P";
                            nomination.ProcessingRightIndicator = nom.ProcessingRights;
                            nomination.Quantity = string.IsNullOrEmpty(nom.RecQty) ? 0 : Convert.ToInt32(nom.RecQty);
                            nomination.QuantityTypeIndicator = "R";    // by default value "Receipt"
                            nomination.ReceiptLocationIdentifier = nom.RecLocID;
                            nomination.ReceiptLocationName = nom.RecLocation;
                            nomination.receiptLocationPropCode = nom.RecLocProp;
                            nomination.ReceiptRank = nom.RecRank;
                            nomination.ServiceProviderActivityCode = nom.ActCode;//duplicate nomination.AssignIdentification
                            nomination.TransactionID = batch.TransactionID;
                            nomination.TransactionType = nom.TransType;
                            var ttPathed = metadataTransactionTypeRepository.GetTTUsingIdentifier(nom.TransType, "P", pathedNom.PipelineDuns);
                            nomination.TransactionTypeDesc = ttPathed != null ? ttPathed.Name : string.Empty;
                            nomination.UpstreamContractIdentifier = nom.UpKContract;
                            nomination.UpstreamIdentifier = nom.UpID;
                            nomination.UpstreamName = nom.UpName;
                            nomination.UpstreamPackageId = nom.UpPkgID;
                            nomination.UpstreamPropCode = nom.UpIDProp;
                            nomination.UpstreamRank = nom.UpRank;
                            nomination.UnitOfMeasure = "BZ";
                            NominationsRepository.Add(nomination);
                            NominationsRepository.SaveChages();
                    }
                    else {
                        SavePathedNomination(pathedNom, nom);
                    }
                }
                return TransactionID;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int GetPathedListTotalCount(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {
            return NominationsRepository.GetPathedListTotalCount(pipelineDuns,Status,StartDate,EndDate,shipperDuns,loginUser);
        }
        public List<PathedNomDetailsDTO> GetPathedListWithPaging(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser, SortingPagingInfo sortingPagingInfo)
        {
            return NominationsRepository.GetPathedListWithPaging(pipelineDuns,Status,StartDate,EndDate,shipperDuns,loginUser,sortingPagingInfo);
        }
        public List<PathedNomDetailsDTO> GetPathedList(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {
            return NominationsRepository.GetPathedList(pipelineDuns, Status, StartDate, EndDate, shipperDuns, loginUser);
           
            // PathedDTO pathedObj = new PathedDTO();
            // pathedObj.PathedNomsList =
            //new List<PathedNomDetailsDTO>();
            /*
            List<V4_Batch> batchList = new List<V4_Batch>();
            batchList = BatchRepository.GetBatchDataUsingStatusId(PipelieID,Status,StartDate,EndDate,shipperDuns,loginUser);
            
            foreach (var batch in batchList)
            {
                PathedNomDetailsDTO pathed = new PathedNomDetailsDTO();
                pathed.TransactionId = batch.TransactionID;
                pathed.CanWrite = batch.StatusID == (int)NomStatus.Draft ? true : false;
                pathed.PipelineID = batch.PipelineID;
                pathed.CycleID = batch.CycleId;
                pathed.EndDate = batch.FlowEndDate;
                pathed.ScheduledDateTime = batch.ScheduleDate.HasValue ? batch.ScheduleDate.Value : DateTime.MaxValue;
                pathed.ShipperDuns = batch.ServiceRequester;
                pathed.ReferenceNo = batch.ReferenceNumber;
                pathed.StartDate = batch.FlowStartDate;
                pathed.Status = GetFileStatusOnId(batch.StatusID);
                pathed.StatusID = batch.StatusID;
                pathed.CreatedDate = batch.CreatedDate;
                V4_Nomination nom = NominationsRepository.GetAll().Where(a => a.TransactionID == batch.TransactionID && a.PathType == "P").FirstOrDefault();
                if (nom != null)
                {
                    Location locRec = LocationRepository.GetAll().Where(a => a.PropCode == nom.receiptLocationPropCode && a.PipelineID == batch.PipelineID).FirstOrDefault();
                    Location locDel = LocationRepository.GetAll().Where(a => a.PropCode == nom.DeliveryLocationPropCode && a.PipelineID == batch.PipelineID).FirstOrDefault();
                    CounterParty conUp = null;
                    CounterParty conDwn = null;
                    if (!string.IsNullOrEmpty(nom.UpstreamIdentifier))
                        conUp = counterpartyRepository.GetAll().Where(a => a.Identifier == nom.UpstreamIdentifier && (a.PipelineID == batch.PipelineID || a.PipelineID == 0)).FirstOrDefault();
                    else if(!string.IsNullOrEmpty(nom.UpstreamPropCode))
                        conUp = counterpartyRepository.GetAll().Where(a => a.PropCode == nom.UpstreamPropCode && (a.PipelineID == batch.PipelineID || a.PipelineID == 0)).FirstOrDefault();
                    if(!string.IsNullOrEmpty(nom.DownstreamIdentifier))
                        conDwn = counterpartyRepository.GetAll().Where(a => a.Identifier == nom.DownstreamIdentifier && (a.PipelineID == batch.PipelineID || a.PipelineID == 0)).FirstOrDefault();
                    else if(!string.IsNullOrEmpty(nom.DownstreamPropCode))
                        conDwn = counterpartyRepository.GetAll().Where(a => a.PropCode == nom.DownstreamPropCode && (a.PipelineID == batch.PipelineID || a.PipelineID == 0)).FirstOrDefault();

                    pathed.ActCode = nom.AssociatedContract;
                    pathed.AssocContract = nom.AssociatedContract;
                    pathed.BidTransportRate = nom.BidTransportationRate;
                    pathed.BidUp = nom.BidupIndicator;
                    pathed.CapacityType = nom.CapacityTypeIndicator;
                    //pathed.CompanyID=batch.
                    pathed.Contract = nom.ContractNumber;
                    pathed.DealType = nom.DealType;
                    pathed.DelLoc = locDel != null ? locDel.Name : "";
                    pathed.DelLocID = nom.DeliveryLocationIdentifer;
                    pathed.DelLocProp = nom.DeliveryLocationPropCode;
                    pathed.DelQuantity = nom.DelQuantity.HasValue ? Convert.ToDecimal(nom.DelQuantity.Value) : decimal.MinValue;
                    pathed.DelRank = nom.DeliveryRank;
                    pathed.DownContract = nom.DownstreamContractIdentifier;
                    pathed.DownID = conDwn!=null && !string.IsNullOrEmpty(conDwn.Identifier) ? conDwn.Identifier:nom.DownstreamIdentifier;
                    pathed.DownIDProp = conDwn!=null && !string.IsNullOrEmpty(conDwn.PropCode)?conDwn.PropCode: nom.DownstreamPropCode;
                    pathed.DownName = conDwn != null ? conDwn.Name : "";
                    pathed.DownPkgID = nom.DownstreamPackageId;
                    pathed.DownRank = nom.DownstreamRank;
                    pathed.Export = nom.ExportDecleration;
                    pathed.MaxRate = nom.MaxRateIndicator;
                    pathed.NomSubCycle = nom.NominationSubCycleIndicator;
                    pathed.NomTrackingId = nom.NominatorTrackingId;
                    pathed.NomUserData1 = nom.NominationUserData1;
                    pathed.NomUserData2 = nom.NominationUserData2;
                    //pathed.PipelineID = batch.PipelineID;
                    pathed.PkgID = nom.PackageId;
                    pathed.ProcessingRights = nom.ProcessingRightIndicator;
                    pathed.QuantityType = nom.QuantityTypeIndicator;
                    pathed.RecLocation = locRec != null ? locRec.Name : "";
                    pathed.RecLocID = nom.ReceiptLocationIdentifier;
                    pathed.RecLocProp = nom.receiptLocationPropCode;
                    pathed.RecRank = nom.ReceiptRank;
                    pathed.RecQty = nom.Quantity.HasValue ? nom.Quantity.Value.ToString() : "0";
                    pathed.TransType = nom.TransactionType;
                    pathed.UpID = conUp!=null && !string.IsNullOrEmpty(conUp.Identifier)?conUp.Identifier:  nom.UpstreamIdentifier;
                    pathed.UpIDProp = conUp != null && !string.IsNullOrEmpty(conUp.PropCode) ? conUp.PropCode : nom.UpstreamPropCode;
                    pathed.UpKContract = nom.UpstreamContractIdentifier;
                    pathed.UpName = conUp != null ? conUp.Name : "";
                    pathed.UpPkgID = nom.UpstreamPackageId;
                    pathed.UpRank = nom.UpstreamRank;
                    pathed.CycleName = "";
                    pathed.TransTypeName = "";
                    SQTSTrackOnNom sqts = GetSqtsResult(nom.NominatorTrackingId, nom.TransactionID);
                    if (sqts != null)
                    {
                        pathed.DelPointQty = sqts.DeliveryPointQuantity;
                        pathed.RecPointQty = sqts.ReceiptPointQuantity;
                        pathed.ReductionReason = sqts.ReductionReason;
                    }
                }
                pathedObj.PathedNomsList.Add(pathed);

            }
             return pathedObj;
         */
        }
        private static string NomTrackingID(int Size)
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
        private string GetFileStatusOnId(int statusId)
        {
            if (statusId == (int)NomStatus.Draft)
                return "Draft";
            else if (statusId == (int)NomStatus.Rejected)
                return "Rejected";
            else if (statusId == (int)NomStatus.Accepted)
                return "Accepted";
            else if (statusId == (int)NomStatus.Error)
                return "Error";
            else if (statusId == (int)NomStatus.InProcess)
                return "InProcess";
            else if (statusId == (int)NomStatus.Submitted)
                return "Submitted";
            else
                return string.Empty;

            //else if()

            //if (statusId == (int)BLLmetadataFileStatus.Type.Decrypted)
            //    return "In Process";
            //if (statusId == (int)BLLmetadataFileStatus.Type.EDI)
            //    return "In Process";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Encrypted)
            //    return "In Process";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Failure_Ack)
            //    return "Error";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Failure_Gisb)
            //    return "Error";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Failure_NMQR)
            //    return "Rejected";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Success_Ack)
            //    return "submitted";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Success_Gisb)
            //    return "Submitted";
            //if (statusId == (int)BLLmetadataFileStatus.Type.Success_NMQR)
            //    return "Accepted";
            //if (statusId == (int)BLLmetadataFileStatus.Type.XML)
            //    return "In Process";
        }

        public int GetStatusOnTransactionId(Guid tranId)
        {
            return BatchRepository.GetStatusOnTransactionId(tranId);
        }
    }
}
