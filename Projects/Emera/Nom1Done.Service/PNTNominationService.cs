using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.Model;
using System.IO;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class PNTNominationService : IPNTNominationService
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private readonly INominationsRepository NominationsRepository;
        private readonly IBatchRepository BatchRepository;
        private readonly IModalFactory modelFactory;
        private readonly IShipperCompanyRepository ShipperCompanyRepository;
        private readonly IContractRepository contractRepository;
        private readonly ICounterPartyRepository counterPartyRepository;
        private readonly ILocationRepository locationRepository;
        private readonly INominationStatusRepository NominationStatusRepository;
        private readonly INMQRPerTransactionRepository NMQRPerTransactionRepository;
        private readonly ITransactionLogRepository TransactionLogRepository;
        private readonly IOutboxRepository OutboxRepository;
        private readonly ITaskMgrJobsRepository TaskMgrJobsRepository;
        private readonly ImetadataTransactionTypeRepository metadataTransactionTypeRepository;
        private readonly IPipelineTransactionTypeMapRepository PipelineTransactionTypeMapRepository;
        private readonly ITaskMgrReceiveMultipuleFileRepository TaskmgrRecMultiFileRepository;
        private readonly IContractService contractService;
        private readonly IPipelineService pipelineService;
        private readonly IRouteRepository routeRepo;

        public PNTNominationService(
            IRouteRepository routeRepo,
            IPipelineService pipelineService,
            INominationsRepository NominationsRepository,
            IBatchRepository BatchRepository,
            IModalFactory modelFactory,
            IShipperCompanyRepository ShipperCompanyRepository,
            IContractRepository contractRepository,
            ICounterPartyRepository counterPartyRepository,
            ILocationRepository locationRepository,
            INominationStatusRepository NominationStatusRepository,
            INMQRPerTransactionRepository NMQRPerTransactionRepository,
            ITransactionLogRepository TransactionLogRepository,
            IOutboxRepository OutboxRepository,
            ITaskMgrJobsRepository TaskMgrJobsRepository,
            ImetadataTransactionTypeRepository metadataTransactionTypeRepository,
            IPipelineTransactionTypeMapRepository PipelineTransactionTypeMapRepository,
            ITaskMgrReceiveMultipuleFileRepository TaskmgrRecMultiFileRepository,
            IContractService contractService)
        {
            this.routeRepo = routeRepo;
            this.pipelineService = pipelineService;
            this.PipelineTransactionTypeMapRepository = PipelineTransactionTypeMapRepository;
            this.metadataTransactionTypeRepository = metadataTransactionTypeRepository;
            this.TaskMgrJobsRepository = TaskMgrJobsRepository;
            this.OutboxRepository = OutboxRepository;
            this.TransactionLogRepository = TransactionLogRepository;
            this.NominationsRepository = NominationsRepository;
            this.BatchRepository = BatchRepository;
            this.modelFactory = modelFactory;
            this.ShipperCompanyRepository = ShipperCompanyRepository;
            this.contractRepository = contractRepository;
            this.counterPartyRepository = counterPartyRepository;
            this.locationRepository = locationRepository;
            this.NominationStatusRepository = NominationStatusRepository;
            this.NMQRPerTransactionRepository = NMQRPerTransactionRepository;
            this.TaskmgrRecMultiFileRepository = TaskmgrRecMultiFileRepository;
            this.contractService = contractService;
        }


        public List<DTO.Route> GetRoutes()
        {
            return routeRepo.GetAll().Select(a => modelFactory.Parse(a)).ToList();
        }



        public Guid? SaveAndUpdatePNTBatchDetail(BatchDetailDTO batchDetail,bool IsSave)
        {
            Guid? TransactionId = new Guid();
            if (IsSave)
            {
                if (batchDetail.Id == Guid.Empty)
                {
                    TransactionId = SavePNTBatch(batchDetail);
                    batchDetail.Id = TransactionId.Value;
                    SavePNTNomination(batchDetail);
                }
                else
                {
                    var NomList = NominationsRepository.GetAllNomsByTransactionId(batchDetail.Id);


                    if (NomList.Count > 0)
                    {
                        NominationsRepository.deleteAll(NomList);
                    }
                    UpdatePNTBatch(batchDetail);
                    SavePNTNomination(batchDetail);
                }
            }
            else
            {
                var NomList = NominationsRepository.GetAllNomsByTransactionId(batchDetail.Id);//.GetAll().Where(a => a.TransactionID == batchDetail.Id);
                if (NomList.Count()>0)
                {
                    NominationsRepository.deleteAll(NomList);
                }
                SavePNTNomination(batchDetail);
            }
            return TransactionId;
        }
        public bool CopyNomination(Guid TransactionId)
        {
            try
            {
                V4_Batch batch = BatchRepository.GetByTransactionID(TransactionId);
                if (batch != null)
                {
                    V4_Batch newBatch = new V4_Batch();
                    Random ran = new Random();
                    string TranSetContNumb = Path.GetRandomFileName();
                    TranSetContNumb = TranSetContNumb.Replace(".", "");
                    newBatch.TransactionID = Guid.NewGuid();
                    newBatch.CreatedBy = batch.CreatedBy;
                    newBatch.CreatedDate = DateTime.Now;
                    newBatch.CycleId = batch.CycleId;
                    newBatch.Description = "Copy from " + batch.Description;
                    //newBatch.FlowEndDate = new DateTime(DateTime.Now.AddDays(3).Year, DateTime.Now.AddDays(3).Month, DateTime.Now.AddDays(3).Day, 9, 0, 0);
                    // newBatch.FlowStartDate = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 9, 0, 0);
                    newBatch.FlowStartDate = batch.FlowStartDate.Date < DateTime.Now.Date ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0) : batch.FlowStartDate;
                    newBatch.FlowEndDate = batch.FlowEndDate.Date < DateTime.Now.Date ? new DateTime(DateTime.Now.AddDays(2).Year, DateTime.Now.AddDays(2).Month, DateTime.Now.AddDays(2).Day, 9, 0, 0) : batch.FlowEndDate;
                    if (newBatch.FlowStartDate > newBatch.FlowEndDate) {
                        newBatch.FlowEndDate = newBatch.FlowStartDate.AddDays(1);
                    }
                    newBatch.IsActive = true;
                    newBatch.NomTypeID = (int)NomType.PNT;
                    newBatch.PakageCheck = batch.PakageCheck;
                    newBatch.PipelineID = batch.PipelineID;
                    newBatch.RankingCheck = batch.RankingCheck;
                    newBatch.ReferenceNumber = ran.Next(999999999).ToString();
                    newBatch.ScheduleDate = DateTime.MaxValue;
                    newBatch.ServiceRequester = batch.ServiceRequester;
                    newBatch.ShowZeroCheck = batch.ShowZeroCheck;
                    newBatch.ShowZeroDn = batch.ShowZeroDn;
                    newBatch.ShowZeroUp = batch.ShowZeroUp;
                    newBatch.StatusID = (int)NomStatus.Draft;
                    newBatch.SubmitDate = DateTime.MaxValue;
                    newBatch.TransactionSetControlNumber = TranSetContNumb;
                    newBatch.UpDnContractCheck = batch.UpDnContractCheck;
                    newBatch.UpDnPkgCheck = batch.UpDnPkgCheck;
                    BatchRepository.Add(newBatch);
                    BatchRepository.SaveChages();

                    //entities.V4_SaveBatchData(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), batch.CycleId, (int)BLLmetadataFileStatus.Type.Draft, null, null, batch.PipelineID, batch.ServiceRequester, "Copied " + DateTime.Now, false, false, false, false, false, false, false, true, batch.CreatedBy, DateTime.Now, ran.Next(999999999).ToString(), 2, TranSetContNumb).FirstOrDefault();
                    if (newBatch.TransactionID == Guid.Empty)
                        return false;
                    else
                    {
                        List<V4_Nomination> nomList = NominationsRepository.GetAllNomsByTransactionId(batch.TransactionID);
                        if (nomList != null && nomList.Count > 0)
                        {
                            foreach (var nom in nomList)
                            {
                                V4_Nomination newNom = new V4_Nomination();
                                newNom.TransactionID = newBatch.TransactionID;
                                newNom.AssignIdentification = nom.AssignIdentification;
                                newNom.AssociatedContract = nom.AssociatedContract;
                                newNom.BidTransportationRate = nom.BidTransportationRate;
                                newNom.BidupIndicator = nom.BidupIndicator;
                                newNom.CapacityTypeIndicator = nom.CapacityTypeIndicator;
                                newNom.ContractNumber = nom.ContractNumber;
                                newNom.DealType = nom.DealType;
                                newNom.DeliveryLocationIdentifer = nom.DeliveryLocationIdentifer;
                                newNom.DeliveryLocationName = nom.DeliveryLocationName;
                                newNom.DeliveryLocationPropCode = nom.DeliveryLocationPropCode;
                                newNom.DeliveryRank = nom.DeliveryRank;
                                newNom.DelQuantity = nom.DelQuantity;
                                newNom.DownstreamContractIdentifier = nom.DownstreamContractIdentifier;
                                newNom.DownstreamIdentifier = nom.DownstreamIdentifier;
                                newNom.DownstreamPackageId = nom.DownstreamPackageId;
                                newNom.DownstreamPropCode = nom.DownstreamPropCode;
                                newNom.DownstreamName = nom.DownstreamName;
                                newNom.DownstreamRank = nom.DownstreamRank;
                                newNom.ExportDecleration = nom.ExportDecleration;
                                newNom.ImbalancePeriod = nom.ImbalancePeriod;
                                newNom.MaxRateIndicator = nom.MaxRateIndicator;
                                newNom.NominationSubCycleIndicator = nom.NominationSubCycleIndicator;
                                newNom.NominationUserData1 = nom.NominationUserData1;
                                newNom.NominationUserData2 = nom.NominationUserData2;
                                newNom.NominatorTrackingId = NomTrackingID(9);
                                newNom.PackageId = nom.PackageId;
                                newNom.PathRank = nom.PathRank;
                                newNom.PathType = nom.PathType;
                                newNom.ProcessingRightIndicator = nom.ProcessingRightIndicator;
                                newNom.Quantity = nom.Quantity;
                                newNom.QuantityTypeIndicator = nom.QuantityTypeIndicator;
                                newNom.ReceiptLocationIdentifier = nom.ReceiptLocationIdentifier;
                                newNom.receiptLocationPropCode = nom.receiptLocationPropCode;
                                newNom.ReceiptLocationName = nom.ReceiptLocationName;
                                newNom.ReceiptRank = nom.ReceiptRank;
                                newNom.Route = nom.Route;
                                newNom.ServiceProviderActivityCode = nom.ServiceProviderActivityCode;
                                newNom.TransactionType = nom.TransactionType;
                                newNom.UnitOfMeasure = nom.UnitOfMeasure;
                                newNom.UpstreamContractIdentifier = nom.UpstreamContractIdentifier;
                                newNom.UpstreamIdentifier = nom.UpstreamIdentifier;
                                newNom.UpstreamPackageId = nom.UpstreamPackageId;
                                newNom.UpstreamPropCode = nom.UpstreamPropCode;
                                newNom.UpstreamName = nom.UpstreamName;
                                newNom.UpstreamRank = nom.UpstreamRank;
                                NominationsRepository.Add(newNom);
                                NominationsRepository.SaveChages();
                            }
                            return true;
                        }
                        else
                            return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private Guid SavePNTBatch(BatchDetailDTO batch)
        {
            try
            {
                Random ran = new Random();
                string path = Path.GetRandomFileName();
                path = path.Replace(".", "");
                V4_Batch b = new V4_Batch();
                b.CreatedBy = batch.CreatedBy;
                b.CreatedDate = DateTime.Now;
                b.CycleId = batch.CycleId;
                b.Description = batch.Description;
                b.FlowEndDate = batch.EndDateTime;
                b.FlowStartDate = batch.StartDateTime;
                b.IsActive = true;
                b.NomTypeID = (int)NomType.PNT;
                b.PakageCheck = false;
                b.PipelineID = batch.PipelineId;
                b.RankingCheck = false;
                b.ReferenceNumber = ran.Next(999999999).ToString();
                b.ServiceRequester = batch.ShiperDuns;
                b.ShowZeroCheck = false;
                b.ShowZeroDn = false;
                b.ShowZeroUp = false;
                b.StatusID = (int)NomStatus.Draft;
                b.SubmitDate = DateTime.MaxValue;
                b.ScheduleDate = DateTime.MaxValue;
                b.TransactionSetControlNumber = path;
                b.UpDnContractCheck = false;
                b.UpDnPkgCheck = false;
                b.NomSubCycle = batch.NomSubCycle;
                BatchRepository.Add(b);
                BatchRepository.SaveChages();
                return b.TransactionID;
            }catch(Exception ex)
            {
                return Guid.Empty;
            }
        }

        public Guid UpdatePNTBatch(BatchDetailDTO batch)
        {
            try
            {
                var b = BatchRepository.GetByTransactionID(batch.Id);//GetAll().ToList().Where(a => a.TransactionID == batch.Id).FirstOrDefault();
                if (b != null)
                {
                    Random ran = new Random();
                    string path = Path.GetRandomFileName();
                    path = path.Replace(".", "");
                    b.CreatedBy = batch.CreatedBy;
                    b.CreatedDate = DateTime.Now;
                    b.CycleId = batch.CycleId;
                    b.Description = batch.Description;
                    b.FlowEndDate = batch.EndDateTime;
                    b.FlowStartDate = batch.StartDateTime;
                    b.IsActive = true;
                    b.NomTypeID = (int)NomType.PNT;
                    b.PakageCheck = false;
                    b.PipelineID = batch.PipelineId;
                    b.RankingCheck = false;
                    b.ReferenceNumber = ran.Next(999999999).ToString();
                    b.ServiceRequester = batch.ShiperDuns;
                    b.ShowZeroCheck = false;
                    b.ShowZeroDn = false;
                    b.ShowZeroUp = false;
                    b.StatusID = (int)NomStatus.Draft;
                    b.SubmitDate = DateTime.MaxValue;
                    b.ScheduleDate = DateTime.MaxValue;
                    b.TransactionSetControlNumber = path;
                    b.UpDnContractCheck = false;
                    b.UpDnPkgCheck = false;
                    b.NomSubCycle = batch.NomSubCycle;
                    BatchRepository.Update(b);
                    BatchRepository.SaveChages();
                    return b.TransactionID;
                }
                else
                    return Guid.Empty;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public TransactionTypesDTO GetTTUsingttMapId(int TTmapid)
        {
            return metadataTransactionTypeRepository.GetTTUsingMapId(TTmapid);
        }

        public TransactionTypesDTO GetTTUsingTTCodeTTName(string TTIdentifier, string TTName, string PipelineDuns)
        {
            return metadataTransactionTypeRepository.GetTTUsingttnameTTCode(TTIdentifier,TTName,PipelineDuns);
        }

        private void SavePNTNomination(BatchDetailDTO batchDetail)
        {
            try
            {
                List<V4_Nomination> BatchNoms = new List<V4_Nomination>();
                if (batchDetail.SupplyList != null)
                {
                   // BatchNoms = new List<V4_Nomination>();
                    foreach (var supply in batchDetail.SupplyList)
                    {

                       
                        ContractsDTO flyCon = new ContractsDTO();                        
                        Contract con = contractService.GetContractByContractNo(supply.ServiceRequestNo, batchDetail.PipelineId);
                        if (con == null)
                        {
                            flyCon = AddContractOnFly(supply.ServiceRequestNo, batchDetail.PipelineId, batchDetail.ShipperCompanyId, batchDetail.CreatedBy,string.IsNullOrEmpty(supply.FuelPercentage)?0:Convert.ToDecimal(supply.FuelPercentage));
                            //supply.FuelPercentage = flyCon.FuelPercentage.ToString();
                        }
                        //else {
                        //    supply.FuelPercentage = string.IsNullOrEmpty(supply.FuelPercentage) ? "0" :supply.FuelPercentage;
                        //}
                        CounterParty cp = counterPartyRepository.GetCounterPartyByPropCode(supply.UpstreamIDProp);
                        V4_Nomination nom = new V4_Nomination();
                        nom.TransactionID = batchDetail.Id;
                        nom.TransactionType = supply.TransactionType;

                        //var ttsupply = metadataTransactionTypeRepository.GetTTUsingIdentifier(supply.TransactionType, "S",batchDetail.PipelineId);
                        
                        var ttsupply = GetTTUsingttMapId(supply.TransTypeMapId);

                        nom.TransactionTypeDesc = ttsupply.Name;
                        nom.FuelPercentage = Convert.ToDecimal(String.IsNullOrEmpty(supply.FuelPercentage)?"0": supply.FuelPercentage);                  
                        nom.AssignIdentification = "";
                        nom.AssociatedContract = "";
                        nom.BidTransportationRate = "";
                        nom.BidupIndicator = "";
                        nom.CapacityTypeIndicator = "";
                        nom.ContractNumber = !string.IsNullOrEmpty(flyCon.RequestNo)? flyCon.RequestNo:supply.ServiceRequestNo;
                        nom.DealType = "";
                        nom.DeliveryLocationIdentifer = "";
                        nom.DeliveryLocationName = "";
                        nom.DeliveryLocationPropCode = "";
                        nom.DeliveryRank = "";
                        nom.DelQuantity = string.IsNullOrEmpty(supply.DeliveryQuantityNet + "") ? 0 : Convert.ToInt32(supply.DeliveryQuantityNet);
                        nom.DownstreamContractIdentifier = "";
                        nom.DownstreamRank = "";
                        nom.DownstreamPackageId = "";
                        nom.DownstreamIdentifier = "";
                        nom.DownstreamName = "";
                        nom.DownstreamPropCode = "";
                        nom.ExportDecleration = "";
                        nom.ImbalancePeriod = "";
                        nom.MaxRateIndicator = "";
                        nom.NominationSubCycleIndicator = batchDetail.NomSubCycle;
                        nom.NominationUserData1 = "";
                        nom.NominationUserData2 = "";
                        nom.NominatorTrackingId = NomTrackingID(9);
                        nom.PackageId = supply.PackageID;
                        nom.PathRank = "";                        
                        nom.PathType = "S";
                        nom.ProcessingRightIndicator = "";
                        nom.Quantity = string.IsNullOrEmpty(supply.ReceiptQuantityGross + "") ? 0 : Convert.ToInt32(supply.ReceiptQuantityGross);
                        nom.QuantityTypeIndicator = "R";
                        nom.ReceiptLocationIdentifier = supply.Location;
                        nom.ReceiptLocationName = supply.LocationName;
                        nom.receiptLocationPropCode = supply.LocationProp;
                        nom.ReceiptRank = "";
                        nom.Route = "";
                        nom.ServiceProviderActivityCode = "";
                        nom.UnitOfMeasure = "BZ";
                        nom.UpstreamContractIdentifier = supply.UpContractIdentifier;
                        //transaction type Loan
                        nom.UpstreamIdentifier = (!string.IsNullOrEmpty(supply.TransactionType) && supply.TransactionType == "28") ? batchDetail.ShiperDuns : cp!=null?cp.Identifier:supply.UpstreamID;
                        nom.UpstreamPackageId = supply.UpPackageID;
                        nom.UpstreamPropCode = (!string.IsNullOrEmpty(supply.TransactionType) && supply.TransactionType == "28") ? batchDetail.ShiperDuns : cp != null ? cp.PropCode : supply.UpstreamIDProp;
                        nom.UpstreamName = (!string.IsNullOrEmpty(supply.TransactionType) && supply.TransactionType == "28") ? batchDetail.ShiperDuns : cp != null ? cp.Name : supply.UpstreamIDName;
                        nom.UpstreamRank = supply.UpstreamRank;
                        nom.TransactionID = batchDetail.Id;
                        BatchNoms.Add(nom);
                    }
                   // NominationsRepository.AddBulkNoms(BatchNoms);
                    //NominationsRepository.SaveChages();
                }
                if (batchDetail.Contract != null)
                {
                   // BatchNoms = new List<V4_Nomination>();
                    foreach (var contract in batchDetail.Contract)
                    {
                        ContractsDTO flyCon = new ContractsDTO();
                        Contract con = contractService.GetContractByContractNo(contract.ServiceRequestNo, batchDetail.PipelineId);// && a.PipelineID == batchDetail.PipelineId).FirstOrDefault();
                        if (con == null)
                        {
                            flyCon = AddContractOnFly(contract.ServiceRequestNo, batchDetail.PipelineId, batchDetail.ShipperCompanyId, batchDetail.CreatedBy,string.IsNullOrEmpty(contract.FuelPercentage)?0:Convert.ToDecimal(contract.FuelPercentage));
                            //contract.FuelPercentage = flyCon.FuelPercentage.ToString();
                        }
                        //else {
                        //    contract.FuelPercentage = con.FuelPercentage.ToString();
                        //}
                        V4_Nomination nom = new V4_Nomination();
                        nom.TransactionID = batchDetail.Id;
                        nom.TransactionType = contract.TransactionType;
                        var ttcontract = metadataTransactionTypeRepository.GetTTUsingIdentifier(contract.TransactionType, "C", batchDetail.PipelineId);
                        nom.TransactionTypeDesc = ttcontract != null ? ttcontract.Name : string.Empty;
                        nom.FuelPercentage =Convert.ToDecimal(String.IsNullOrEmpty(contract.FuelPercentage)?"0": contract.FuelPercentage);
                        nom.AssignIdentification = "";
                        nom.AssociatedContract = "";
                        nom.BidTransportationRate = "";
                        nom.BidupIndicator = "";
                        nom.CapacityTypeIndicator = "";
                        nom.ContractNumber = !string.IsNullOrEmpty(flyCon.RequestNo) ? flyCon.RequestNo:contract.ServiceRequestNo;//contractNumber;
                        nom.DealType = "";
                        nom.DeliveryLocationIdentifer = contract.DelLocation;
                        nom.DeliveryLocationName = contract.DelLocationName;
                        nom.DeliveryLocationPropCode = contract.DelLocationProp;
                        nom.DeliveryRank = contract.DelRank;
                        nom.DelQuantity = string.IsNullOrEmpty(contract.DeliveryDth) ? 0 : Convert.ToInt32(Convert.ToDecimal(contract.DeliveryDth));
                        nom.DownstreamContractIdentifier = "";
                        nom.DownstreamRank = "";
                        nom.DownstreamPackageId = "";
                        nom.DownstreamIdentifier = "";
                        nom.DownstreamName = "";
                        nom.DownstreamPropCode = "";
                        nom.ExportDecleration = "";
                        nom.ImbalancePeriod = "";
                        nom.MaxRateIndicator = "";
                        nom.NominationSubCycleIndicator = batchDetail.NomSubCycle;
                        nom.NominationUserData1 = "";
                        nom.NominationUserData2 = "";
                        nom.NominatorTrackingId = NomTrackingID(9);
                        nom.PackageId = contract.PackageID;
                        nom.PathRank = contract.PathRank != null ? contract.PathRank : "";
                        nom.PathType = "T";
                        nom.ProcessingRightIndicator = "";
                        nom.Quantity = string.IsNullOrEmpty(contract.ReceiptDth) ? 0 : Convert.ToInt32(contract.ReceiptDth);
                        nom.QuantityTypeIndicator = "R";
                        nom.ReceiptLocationIdentifier = contract.RecLocation;
                        nom.ReceiptLocationName = contract.RecLocationName;
                        nom.receiptLocationPropCode = contract.RecLocationProp;
                        nom.ReceiptRank = contract.RecRank;
                        nom.Route = contract.Route;
                        nom.ServiceProviderActivityCode = "";
                        nom.UnitOfMeasure = "BZ";
                        nom.UpstreamContractIdentifier = "";
                        nom.UpstreamIdentifier = "";
                        nom.UpstreamName = "";
                        nom.UpstreamPackageId = "";
                        nom.UpstreamPropCode = "";
                        nom.UpstreamRank = "";
                        nom.TransactionID = batchDetail.Id;
                        BatchNoms.Add(nom);
                        //NominationsRepository.Add(nom);
                        //NominationsRepository.SaveChages();

                    }
                   // NominationsRepository.AddBulkNoms(BatchNoms);
                }
                if (batchDetail.MarketList != null)
                {
                   // BatchNoms = new List<V4_Nomination>();
                    foreach (var market in batchDetail.MarketList)
                    {
                        ContractsDTO flyCon = new ContractsDTO();
                        Contract con = contractService.GetContractByContractNo(market.ServiceRequestNo,batchDetail.PipelineId);
                        if (con == null)
                        {
                            flyCon = AddContractOnFly(market.ServiceRequestNo, batchDetail.PipelineId, batchDetail.ShipperCompanyId, batchDetail.CreatedBy, string.IsNullOrEmpty(market.FuelPercentage) ? 0 : Convert.ToDecimal(market.FuelPercentage));
                            //market.FuelPercentage = flyCon.FuelPercentage.ToString();
                        }
                        //else {
                        //    market.FuelPercentage = con.FuelPercentage.ToString();
                        //}
                        CounterParty cp = counterPartyRepository.GetCounterPartyByPropCode(market.DownstreamIDProp);

                        V4_Nomination nom = new V4_Nomination();
                        nom.TransactionID = batchDetail.Id;
                        nom.TransactionType = market.TransactionType;

                       // var ttmarket = metadataTransactionTypeRepository.GetTTUsingIdentifier(market.TransactionType, "M", batchDetail.PipelineId);
                        var ttmarket = GetTTUsingttMapId(market.TransTypeMapId);

                        nom.TransactionTypeDesc = ttmarket.Name;
                        nom.FuelPercentage =Convert.ToDecimal( String.IsNullOrEmpty( market.FuelPercentage)?"0":market.FuelPercentage);
                        nom.AssignIdentification = "";
                        nom.AssociatedContract = "";
                        nom.BidTransportationRate = "";
                        nom.BidupIndicator = "";
                        nom.CapacityTypeIndicator = "";
                        nom.ContractNumber = !string.IsNullOrEmpty(flyCon.RequestNo) ? flyCon.RequestNo : market.ServiceRequestNo;
                        nom.DealType = "";
                        nom.DeliveryLocationIdentifer = market.Location;
                        nom.DeliveryLocationName = market.LocationName;
                        nom.DeliveryLocationPropCode = market.LocationProp;
                        nom.DeliveryRank = "";
                        nom.DelQuantity = string.IsNullOrEmpty(market.DeliveryQuantityNet + "") ? 0 : Convert.ToInt32(Convert.ToDecimal(market.DeliveryQuantityNet));
                        nom.DownstreamContractIdentifier = market.DnContractIdentifier;
                        nom.DownstreamRank = market.DnstreamRank;
                        nom.DownstreamPackageId = market.DnPackageID;
                        //transaction type Park
                        nom.DownstreamIdentifier = (!string.IsNullOrEmpty(market.TransactionType) && (market.TransactionType == "26" || market.TransactionType == "06")) ? batchDetail.ShiperDuns : cp != null ? cp.Identifier : market.DownstreamID;
                        nom.DownstreamName = (!string.IsNullOrEmpty(market.TransactionType) && (market.TransactionType == "26" || market.TransactionType == "06")) ? batchDetail.ShiperDuns : cp != null ? cp.Name : market.DownstreamIDName;
                        nom.DownstreamPropCode = (!string.IsNullOrEmpty(market.TransactionType) && (market.TransactionType == "26" || market.TransactionType == "06")) ? batchDetail.ShiperDuns : cp != null ? cp.PropCode : market.DownstreamIDProp;
                        nom.ExportDecleration = "";
                        nom.ImbalancePeriod = "";
                        nom.MaxRateIndicator = "";
                        nom.NominationSubCycleIndicator = batchDetail.NomSubCycle;
                        nom.NominationUserData1 = "";
                        nom.NominationUserData2 = "";
                        nom.NominatorTrackingId = NomTrackingID(9);
                        nom.PackageId = market.PackageID;
                        nom.PathRank = "";
                        nom.PathType = "M";
                        nom.ProcessingRightIndicator = "";
                        nom.Quantity = string.IsNullOrEmpty(market.ReceiptQuantityGross + "") ? 0 : Convert.ToInt32(market.ReceiptQuantityGross);
                        nom.QuantityTypeIndicator = "D";
                        nom.ReceiptLocationIdentifier = "";
                        nom.ReceiptLocationName = "";
                        nom.receiptLocationPropCode = "";
                        nom.ReceiptRank = "";
                        nom.Route = "";
                        nom.ServiceProviderActivityCode = "";
                        nom.UnitOfMeasure = "BZ";
                        nom.UpstreamContractIdentifier = "";
                        nom.UpstreamIdentifier = "";
                        nom.UpstreamName = "";
                        nom.UpstreamPackageId = "";
                        nom.UpstreamPropCode = "";
                        nom.UpstreamRank = "";
                        nom.TransactionID = batchDetail.Id;
                        BatchNoms.Add(nom);
                        //NominationsRepository.Add(nom);
                        //NominationsRepository.SaveChages();
                    }

                }
                if(BatchNoms.Count > 0)
                NominationsRepository.AddBulkNoms(BatchNoms);
            }
            catch (Exception ex)
            {

            }
        }


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
        public bool DirectSent(BatchDetailDTO batchDetail, bool sendToTest)
        {
            Guid? transactionid;
            ShipperCompany shipComp = ShipperCompanyRepository.GetAll().Where(a => a.DUNS == batchDetail.ShiperDuns).FirstOrDefault();
            if (batchDetail.Id == Guid.Empty)
            {
                transactionid = SaveAndUpdatePNTBatchDetail(batchDetail, true);
                batchDetail.Id = transactionid.HasValue ? transactionid.Value : Guid.Empty;
                SavePNTNomination(batchDetail);
                if (ValidateNomination(batchDetail.Id, batchDetail.PipelineId))
                {
                    SendNominationTransaction(batchDetail.Id, shipComp.ID, sendToTest);
                }
                else
                    return false;
            }
            else
            {
                SaveAndUpdatePNTBatchDetail(batchDetail, false);
                if (ValidateNomination(batchDetail.Id,batchDetail.PipelineId))
                {
                    SendNominationTransaction(batchDetail.Id, shipComp.ID, sendToTest);
                }
                else
                    return false;
            }
            return true;
        }
        public IEnumerable<ContractsDTO> GetContracts(string Keyword, int ShipperID, int PipelineID, int PageNo, int PageSize)
        {
            List<ContractsDTO> items = new List<ContractsDTO>();
            List<Contract> conList = contractRepository.GetAll().ToList().Where(a => a.PipelineID == PipelineID && a.ShipperID == ShipperID).ToList();
            foreach (var item in conList)
            {
                items.Add(modelFactory.Parse(item));
            }
            return items;
        }       


        public IEnumerable<CounterPartiesDTO> GetCounterParties(string Keyword, int PipelineID, int PageNo, int PageSize,string order, string orderDir)
        {
            //return counterPartyRepository.GetCounterParties(Keyword,PipelineID);
            return counterPartyRepository.GetCounterPartiesUsingPaging(Keyword,PipelineID,PageNo,PageSize,order,orderDir);
        }

        public int GetTotalCounterPartiesCount(string Keyword, int PipelineID)
        {
            return counterPartyRepository.GetTotalCounterParties(Keyword,PipelineID);
        }


        public int GetTotalLocationCount(string Keyword, string PipelineDuns, string PopUpFor, bool IsSpecialDelCase)
        {
            return locationRepository.GetTotalLocationCount(Keyword, PipelineDuns, PopUpFor, IsSpecialDelCase);
        }

        public bool FindIsSpecialLocsUsingTTPipeMapId(int ttPipeMapId) {
            return metadataTransactionTypeRepository.FindIsSpecialLocsUsingTTPipeMapId(ttPipeMapId);
        }

        public List<LocationsDTO> GetLocationsForSpecialCases(string TransTypeMapId, int PipelineID)
        {
            List<LocationsDTO> locationList = new List<LocationsDTO>();
            int ttMapid = Convert.ToInt32(TransTypeMapId ?? "0");
            var isSpecialCase = FindIsSpecialLocsUsingTTPipeMapId(ttMapid);
            var Pipelineduns = pipelineService.GetDunsByPipelineID(PipelineID);
            locationList = locationRepository.GetLocations(string.Empty, Pipelineduns);
            if (isSpecialCase)
            {

                if (Pipelineduns == "006958581")  // ANR Pipeline Company
                {
                    locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
                }
                else if (Pipelineduns == "007933047") //Transwestern Pipeline Company, LLC
                {
                    locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                }
                else if (Pipelineduns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                {
                    locationList = locationList.Where(a => a.PropCode == "58744").ToList();                    
                }
            }        

            return locationList;
        }   


        public IEnumerable<LocationsDTO> GetLocations(string Keyword, string PipelineDuns, int PageNo, int PageSize,string PopUpFor, bool IsSpecialDelCase,string order,string orderDir)
        {            
            return locationRepository.GetLocationsWithPaging(Keyword,PipelineDuns,PageNo,PageSize, PopUpFor, IsSpecialDelCase,order,orderDir);
        }
        public BatchDetailDTO GetNominationDetailByBatchID(Guid BatchID)
        {
            BatchDetailDTO batchdetailDTO = new BatchDetailDTO();
            V4_Batch batch = BatchRepository.GetAll().Where(a => a.TransactionID == BatchID && a.NomTypeID == (int)NomType.PNT).FirstOrDefault();
            IEnumerable<V4_Nomination> list = NominationsRepository.GetAll().Where(a => a.TransactionID == BatchID);
            IEnumerable<V4_Nomination> supList = list.Where(a => a.PathType.Contains("S"));
            IEnumerable<V4_Nomination> tranList = list.Where(a => a.PathType.Contains("T"));
            IEnumerable<V4_Nomination> marList = list.Where(a => a.PathType.Contains("M"));
            batchdetailDTO = modelFactory.Parse(batch, list);
            return batchdetailDTO;
        }
        public IEnumerable<RejectionReasonDTO> GetRejectionReason(Guid transactionId)
        {
            List<RejectionReasonDTO> RejectionResonList = new List<RejectionReasonDTO>();
            List<NMQRPerTransaction> nmqrList = new List<NMQRPerTransaction>();
            var code = "";
            var nmqr = NominationStatusRepository.GetRejectedAndErroredNomStatus(transactionId);//.GetAll().Where(a => a.NOM_ID == transactionId && (a.StatusID==(int)NomStatus.Rejected || a.StatusID == (int)NomStatus.Error)).FirstOrDefault();
            if (nmqr != null)
            {
                nmqrList = NMQRPerTransactionRepository.GetNmqrOnNmqrTranId(Guid.Parse(nmqr.NMQR_ID)).ToList();//.GetAll().Where(b => b.Transactionid == Guid.Parse(nmqr.NMQR_ID)).ToList();
                foreach (var item in nmqrList)
                {
                    RejectionReasonDTO reason = new RejectionReasonDTO();
                    reason.ValidationCode = item.ValidationCode;
                    reason.ValidationMessage = item.ValidationMessage;
                    reason.NomTrackingId = item.NominationTrackingId;
                    RejectionResonList.Add(reason);
                    //if (item.ValidationCode != code)
                    //{
                    //    RejectionResonList.Add(reason);
                    //    code = item.ValidationCode;
                    //}

                }
            }
            var Result = (from a in RejectionResonList 
                         group a by new
                         {
                             a.ValidationCode,
                             a.ValidationMessage,
                             a.NomTrackingId
                         } into b
                         select new RejectionReasonDTO
                         {
                             ValidationCode = b.Key.ValidationCode,
                             ValidationMessage = b.Key.ValidationMessage,
                             NomTrackingId = b.Key.NomTrackingId
                         }).ToList();
            return Result;
        }

        public IEnumerable<TransactionTypesDTO> GetTransactionsTypes(int pipeLineId, string Keyword, string popUpfrom)
        {
            List<TransactionTypesDTO> items = new List<TransactionTypesDTO>();
            IEnumerable<metadataTransactionType> ttList = new List<metadataTransactionType>();         
            var pipeLineDuns = pipelineService.GetDunsByPipelineID(pipeLineId);

            NomType pathType = pipelineService.GetPathTypeByPipelineDuns(pipeLineDuns);
                if (popUpfrom == "Supply")
                    items= metadataTransactionTypeRepository.GetTTByMappingPipeline(pipeLineDuns, "S"); 
                else if (popUpfrom == "Market")
                    items= metadataTransactionTypeRepository.GetTTByMappingPipeline(pipeLineDuns, "M");
                else if (popUpfrom == "ContractPath")
                    items= metadataTransactionTypeRepository.GetTTByMappingPipeline(pipeLineDuns, "C");
                else if (popUpfrom == "P")
                    items= metadataTransactionTypeRepository.GetTTByMappingPipeline(pipeLineDuns, "P");
                else if (popUpfrom == "Receipt" || popUpfrom == "Delivery")
                    items= metadataTransactionTypeRepository.GetTTByMappingPipeline(pipeLineDuns, "NP");
         
            return items;          
        }


        

        //public IEnumerable<TransactionTypesDTO> GetTransactionsTypes(int PipelineId, string Keyword, int PageNo, int PageSize, string popUpfrom)
        // {
        //     //Note:- popUpfrom ="P"| "Supply"| "Market" | "ContractPath"
        //     List<TransactionTypesDTO> items = new List<TransactionTypesDTO>();
        //     IEnumerable<metadataTransactionType> ttList = new List<metadataTransactionType>();
        //     IEnumerable<Pipeline_TransactionType_Map> ttm = new List<Pipeline_TransactionType_Map>();
        //     if(popUpfrom == "P")
        //         ttm = PipelineTransactionTypeMapRepository.GetAll().Where(a => a.IsActive && a.PipelineID == PipelineId && a.PathType.Trim()=="P");
        //     if (popUpfrom == "Supply")
        //         ttm = PipelineTransactionTypeMapRepository.GetAll().Where(a => a.IsActive && a.PipelineID == PipelineId && a.PathType.Trim() == "S");
        //     if (popUpfrom == "Market")
        //         ttm = PipelineTransactionTypeMapRepository.GetAll().Where(a => a.IsActive && a.PipelineID == PipelineId && a.PathType.Trim() == "M");
        //     if (popUpfrom == "ContractPath")
        //         ttm = PipelineTransactionTypeMapRepository.GetAll().Where(a => a.IsActive && a.PipelineID == PipelineId && a.PathType.Trim() == "C");
        //     if (popUpfrom == "Receipt" ||popUpfrom== "Delivery")
        //         ttm = PipelineTransactionTypeMapRepository.GetAll().Where(a => a.IsActive && a.PipelineID == PipelineId && a.PathType.Trim() == "NP");


        //     if (!string.IsNullOrEmpty(Keyword))
        //         ttList = metadataTransactionTypeRepository.GetAll()
        //             .Where(a=>(a.Name.Contains(Keyword) || a.Identifier.Contains(Keyword))
        //             && a.IsActive
        //             && ttm.Any(b=>b.TransactionTypeID==a.ID));
        //     else
        //         ttList = metadataTransactionTypeRepository.GetAll()
        //             .Where(a => a.IsActive
        //             && ttm.Any(b => b.TransactionTypeID == a.ID));
        //     foreach (var tt in ttList)
        //     {
        //         items.Add(modelFactory.Parse(tt));
        //     }
        //     return items;
        // }


        public void SaveBulkUpload(BatchDetailDTO batchDetail, bool IsSave)
        {
            SaveAndUpdatePNTBatchDetail(batchDetail, IsSave);
        }
        public bool SendNominationTransaction(Guid transactionId, int ShipperCompanyID,bool sendToTest)
        {
            try
            {
                V4_Batch batch = BatchRepository.GetByTransactionID(transactionId);//.GetAll().Where(a => a.TransactionID == transactionId).FirstOrDefault();
                if (batch != null)
                {
                    batch.SubmitDate = DateTime.Now;
                    batch.ScheduleDate = DateTime.Now;
                    batch.StatusID = Convert.ToInt32(NomStatus.InProcess);
                    BatchRepository.Update(batch);
                    BatchRepository.SaveChages();

                    #region add outbox
                    Outbox outbox = new Outbox();
                    outbox.MessageID = transactionId;
                    outbox.CompanyID = ShipperCompanyID;
                    outbox.CreatedBy = batch.CreatedBy;
                    outbox.CreatedDate = DateTime.Now;
                    outbox.DatasetID = (int)DataSet.Nomination_PNT;
                    outbox.GISB = "";
                    outbox.IsActive = true;
                    outbox.IsScheduled = false;
                    outbox.IsTest = sendToTest;
                    outbox.ModifiedBy = "";
                    outbox.ModifiedDate = DateTime.MaxValue;
                    outbox.PipelineID = batch.PipelineID;
                    outbox.ScheduledDate = DateTime.Now;
                    outbox.StatusID = (int)NomStatus.Draft;
                    OutboxRepository.Add(outbox);
                    OutboxRepository.Save();
                    #endregion
                    #region add nomination status
                    NominationStatu nomStatus = new NominationStatu();
                    nomStatus.NOM_ID = batch.TransactionID;
                    nomStatus.NMQR_ID = string.Empty;
                    nomStatus.ReferenceNumber = batch.ReferenceNumber;
                    nomStatus.StatusDetail = string.Empty;
                    nomStatus.StatusID = (int)NomStatus.Draft;
                    nomStatus.CreatedDate = DateTime.Now;
                    NominationStatusRepository.Add(nomStatus);
                    NominationStatusRepository.Save();
                    #endregion
                    #region transactionLog
                    TransactionLog log = new TransactionLog();
                    log.TransactionId = transactionId.ToString();
                    log.CreatedAt = DateTime.Now;
                    log.StartDate = DateTime.Now;
                    log.StatusId = (int)JobStatus.NotProcessed;
                    log.EndDate = DateTime.MaxValue;
                    TransactionLogRepository.Add(log);
                    TransactionLogRepository.Save();
                    #endregion
                    #region add task manager job
                    TaskMgrJob job = new TaskMgrJob();
                    job.CreatedAt = DateTime.Now;
                    job.DatasetId = (int)DataSet.Nomination_PNT;
                    job.EDICheckCount = 0;
                    job.StageId = 1;
                    job.StatusId = 1;
                    job.IsSending = true;
                    job.NoOfXmlInEDI = 0;
                    job.StartDate = DateTime.Now;
                    job.EndDate = DateTime.MaxValue;
                    job.TransactionId = transactionId.ToString();
                    TaskMgrJobsRepository.Add(job);
                    TaskMgrJobsRepository.Save();
                    #endregion
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public List<BatchDTO> GetBatchAccordingToStatus(List<BatchDTO> oldList, int statusId)
        {
            List<BatchDTO> newList = new List<BatchDTO>();
            switch (statusId)
            {
                case 1:
                    newList = oldList.Where(a => a.StatusID == 1 || a.StatusID == 2 || a.StatusID == 3 || a.StatusID == 4).ToList();
                    break;
                case 5:
                    newList = oldList.Where(a => a.StatusID == 5 || a.StatusID == 6).ToList();
                    break;
                case 7:
                    newList = oldList.Where(a => a.StatusID == 7).ToList();
                    break;
                case 8:
                    newList = oldList.Where(a => a.StatusID == 8 || a.StatusID == 9).ToList();
                    break;
                case 10:
                    newList = oldList.Where(a => a.StatusID == 10).ToList();
                    break;
                case 11:
                    newList = oldList.Where(a => a.StatusID == 11).ToList();
                    break;
                case 0:
                    newList = oldList.Where(a => a.StatusID == 0).ToList();
                    break;
                default:
                    newList = oldList;
                    break;
            }
            return newList;
        }



        public IEnumerable<BatchDTO> GetBatches(string Keyword, int PipelineID, int statusID, DateTime StartDate, DateTime EndDate, int PageNo, int PageSize, Guid _providerUserKey, string shipperDuns)
        {
            IEnumerable<BatchDTO> batchList = new List<BatchDTO>();
            try
            {
                batchList = NominationsRepository.GetAllNominationBatch().ToList();
                
                if (batchList != null)
                {
                    if(Guid.Empty == _providerUserKey)
                    {
                        if (statusID == -1)
                            batchList = batchList.Where(a => a.IsActive == true && a.PipelineId == PipelineID && ((a.DateBeg.Date) >= StartDate.Date && (a.DateEnd.Date) <= EndDate.AddDays(1).Date) && a.ServiceRequester == shipperDuns );
                        else
                        {
                            batchList = batchList.Where(a => a.IsActive == true && a.PipelineId == PipelineID && ((a.DateBeg.Date) >= StartDate.Date && (a.DateEnd.Date) <= EndDate.AddDays(1).Date) && a.ServiceRequester == shipperDuns ).ToList();
                            batchList = GetBatchAccordingToStatus(batchList.ToList(), statusID);
                        }
                    }
                    else
                    {
                        if (statusID == -1)
                            batchList = batchList.Where(a => a.IsActive == true && a.PipelineId == PipelineID && ((a.DateBeg.Date) >= StartDate.Date && (a.DateEnd.Date) <= EndDate.AddDays(1).Date) && a.ServiceRequester == shipperDuns && a.CreatedBy == _providerUserKey.ToString());
                        else
                        {
                            batchList = batchList.Where(a => a.IsActive == true && a.PipelineId == PipelineID && ((a.DateBeg.Date) >= StartDate.Date && (a.DateEnd.Date) <= EndDate.AddDays(1).Date) && a.ServiceRequester == shipperDuns && a.CreatedBy == _providerUserKey.ToString()).ToList();
                            batchList = GetBatchAccordingToStatus(batchList.ToList(), statusID);
                        }
                    }
                }
                //foreach (var batch in batchList)
                //{
                //    batch.DateEnd = batch.DateEnd;
                //    batch.CreatedDate = batch.CreatedDate;//.ToLocalTime();
                //}
            }
            catch (Exception ex)
            {
            }
            return batchList;
        }



        public bool ValidateNomination(Guid transactioId, int pipeId)
        {
            bool reqFields = true;
            bool pathComplete = true;
            BatchDetailDTO batchDetail = NominationsRepository.GetNominationDetail(transactioId, pipeId);//GetNominationDetailByBatchID(transactioId);
            if (batchDetail.StatusId == 11 &&(batchDetail.MarketList != null && batchDetail.MarketList.Count > 0) && (batchDetail.SupplyList != null && batchDetail.SupplyList.Count > 0))
            {
                int marketRows, supplyRows, transportRows, transportPathRows;
                marketRows = batchDetail.MarketList.Count;
                supplyRows = batchDetail.SupplyList.Count;
                transportRows = batchDetail.Contract.Count;
                transportPathRows = batchDetail.ContractPath.Count;
                #region Mandatory fields validation
                #region Market
                do
                {
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].Location))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].LocationProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DownstreamID))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DownstreamIDProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].ServiceRequestNo))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].TransactionType))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DnstreamRank))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].ReceiptQuantityGross + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.MarketList[marketRows - 1].DeliveryQuantityNet + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    marketRows--;
                } while (marketRows > 0);
                #endregion
                #region Supply
                do
                {
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].Location))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].LocationProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamID))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamIDProp))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].ServiceRequestNo))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].TransactionType))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].UpstreamRank))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].ReceiptQuantityGross + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(batchDetail.SupplyList[supplyRows - 1].DeliveryQuantityNet + ""))
                    {
                        reqFields = false;
                        break;
                    }
                    supplyRows--;
                } while (supplyRows > 0);
                #endregion
                #region Contract Path
                if ((batchDetail.ContractPath != null && batchDetail.ContractPath.Count > 0) && (batchDetail.Contract != null && batchDetail.Contract.Count > 0))
                {
                    #region Transport
                    do
                    {
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecLocation))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecLocationProp))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelLocation))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelLocationProp))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].ServiceRequestNo))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].TransactionType))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].RecRank))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DelRank))
                        {
                            reqFields = false;
                            break;
                        }
                        //if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].PackageID))
                        //{
                        //    reqFields = false;
                        //    break;
                        //}
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].PathRank))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].ReceiptDth))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.Contract[transportRows - 1].DeliveryDth))
                        {
                            reqFields = false;
                            break;
                        }
                        transportRows--;
                    } while (transportRows > 0);
                    #endregion
                    #region TransportPath
                    do
                    {
                        if (string.IsNullOrEmpty(batchDetail.ContractPath[transportPathRows - 1].ServiceRequestNo))
                        {
                            reqFields = false;
                            break;
                        }
                        if (string.IsNullOrEmpty(batchDetail.ContractPath[transportPathRows - 1].FuelPercentage))
                        {
                            reqFields = false;
                            break;
                        }
                        transportPathRows--;
                    } while (transportPathRows > 0);
                    #endregion
                }
                #endregion

                #endregion
                #region Nom Matrix Path validation
                if (reqFields)
                {
                    foreach (var item in batchDetail.Contract)
                    {
                        if (!batchDetail.SupplyList.Any(a => a.Location == item.RecLocation))
                        {
                            pathComplete = false;
                            break;
                        }
                        if (!batchDetail.MarketList.Any(a => a.Location == item.DelLocation))
                        {
                            pathComplete = false;
                            break;
                        }
                        if (!batchDetail.ContractPath.Any(a => a.ServiceRequestNo == item.ServiceRequestNo))
                        {
                            pathComplete = false;
                            break;
                        }
                    }
                }
                else
                    return false;
                #endregion
            }
            else
                return false;

            if (reqFields && pathComplete)
                return true;
            else
                return false;
        }

        public BatchDetailDTO GetNomDetail(Guid batchId, int pipeId)
        {
            var query = NominationsRepository.GetNominationDetail(batchId, pipeId);
            return query;
        }

        public NomType GetBatchNomType(Guid TransactionId) {
            return NominationsRepository.GetBatchNomType(TransactionId);
        }
    }
}
