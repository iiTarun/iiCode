using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Nom.ViewModel;
using Nom1Done.Model;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model.Models;
using System.Globalization;

namespace Nom1Done.Service
{
    public class ModalFactory : IModalFactory
    {
        private readonly IPipelineRepository pipelineRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IShipperCompanyRepository shipperCompanyRepository;
        private readonly IContractRepository contractRepository;
        private readonly ImetadataRequestTypeRepository metadataRequestTypeRepository;
        private readonly ImetadataTransactionTypeRepository metadataTransactionTypeRepository;
        private readonly ISQTSTrackOnNomRepository SQTSTrackOnNomRepository;
        private readonly ImetadataCycleRepository metadataCycleRepository;
        private readonly ICounterPartyRepository CounterPartyRepository;       
        private readonly ITransactionalReportingRepository transactionalRepository;
        IPipelineEDISettingRepository pipelineEDISettingRepository;
      


        public ModalFactory(           
            IPipelineRepository pipelineRepository,
            ITransactionalReportingRepository transactionalRepository,
            ICounterPartyRepository CounterPartyRepository,
            ILocationRepository locationRepository,
            IShipperCompanyRepository shipperCompanyRepository,
            IContractRepository contractRepository,
            ImetadataRequestTypeRepository metadataRequestTypeRepository,
            ImetadataTransactionTypeRepository metadataTransactionTypeRepository,
            ISQTSTrackOnNomRepository SQTSTrackOnNomRepository,
            ImetadataCycleRepository metadataCycleRepository,
            IPipelineEDISettingRepository pipelineEDISettingRepository)
        {
           
            this.transactionalRepository = transactionalRepository;
            this.pipelineRepository = pipelineRepository;
            this.locationRepository = locationRepository;
            this.shipperCompanyRepository = shipperCompanyRepository;
            this.contractRepository = contractRepository;
            this.metadataRequestTypeRepository = metadataRequestTypeRepository;
            this.metadataTransactionTypeRepository = metadataTransactionTypeRepository;
            this.SQTSTrackOnNomRepository = SQTSTrackOnNomRepository;
            this.metadataCycleRepository = metadataCycleRepository;
            this.CounterPartyRepository = CounterPartyRepository;
            this.pipelineEDISettingRepository = pipelineEDISettingRepository;
        }

        public DTO.Route Parse(Model.Route model) {
            return new DTO.Route {
                EDIRouteId=model.EDIRouteId,
                RouteDescription=model.RouteDescription,
                DirectionId=model.DirectionId,
                DirectionDescription=model.DirectionDescription
            };
        }

        public Location Create(LocationsDTO dto)
        {
            return new Location
            {
                CreatedBy = dto.CreatedBy,
                ID = dto.ID,
                CreatedDate = dto.CreatedDate,
                Identifier = dto.Identifier,
                IsActive = dto.IsActive,
                ModifiedBy = dto.ModifiedBy,
                ModifiedDate = dto.ModifiedDate,
                Name = dto.Name,
                PipelineID = dto.PipelineID,
                PropCode = dto.PropCode,
                RDUsageID = dto.RDUsageID
            };
        }
        public TransactionalReportDTO Parse(TransactionalReport model)
        {
            return new TransactionalReportDTO() {
                id=model.id,
                PipelineId = model.PipelineId,
                PipeLineDuns = model.PipeLineDuns,
                TSP = model.TSP,
                TSPName = model.TSPName,
                PostingDateTime = model.PostingDateTime,
                ContractHolderName = model.ContractHolderName,
                ContractHolderIdentifier = model.ContractHolderIdentifier,
                ContractHolderProp = model.ContractHolderProp,
                AffiliateIndicatorDesc = model.AffiliateIndicatorDesc,
                RateSchedule = model.RateSchedule,
                ServiceRequesterContract = model.ServiceRequesterContract,
                ContactStatus = model.ContactStatus,
                AmendmentReporting = model.AmendmentReporting,
                ContractBeginDate =model.ContractBeginDate,
                ContractEndDate =model.ContractEndDate,
                ContractEntitlementBeginDate=model.ContractEntitlementBeginDate,
                ContractEntitlementEndDate=model.ContractEntitlementEndDate,
                SurchargeIndicator =model.SurchargeIndicator,
                RateIdentificationCode =model.RateIdentificationCode,
                RateCharged =model.RateCharged,
                RateChargedReference=model.RateChargedReference,
                MaximumTariffRate =model.MaximumTariffRate,
                MaximumTariffRateReference=model.MaximumTariffRateReference,
                MarketBasedRateIndicator=model.MarketBasedRateIndicator,
                ReservationRateBasis =model.ReservationRateBasis,
                ContractualQuantityContract =model.ContractualQuantityContract,
                NegotiatedRateIndicator = model.NegotiatedRateIndicator,
                TermsNotesIndicator=model.TermsNotesIndicator,

                LocQTIPurpDesc1=model.LocQTIPurpDesc1,
                Location1 =model.Location1,
                Location1Name =model.Location1Name,
                LocationZone1 =model.LocationZone1,

                LocQTIPurpDesc2=model.LocQTIPurpDesc2,
                Location2 =model.Location2,
                Location2Name=model.Location2Name,
                LocationZone2 =model.LocationZone2,
                CapacityTypeIndicator=model.CapacityTypeIndicator,
                Comments=model.Comments,

                 //For KinderMorgon-NGPL
                PeakSeasonalStartDate=model.PeakSeasonalStartDate,
                PeakSeasonalEndDate =model.PeakSeasonalEndDate,
                OffPeakSeasonalStartDate =model.OffPeakSeasonalStartDate,
                OffPeakSeasonalEndDate=model.OffPeakSeasonalEndDate,

                NegotId=model.NegotId

    };
           
        }
        public TransactionalReport Create(TransactionalReportDTO model)
        {
            return new TransactionalReport() {
                id = model.id,
                PipelineId = model.PipelineId,
                PipeLineDuns = model.PipeLineDuns,
                TSP = model.TSP,
                TSPName = model.TSPName,
                PostingDateTime = model.PostingDateTime,
                ContractHolderName = model.ContractHolderName,
                ContractHolderIdentifier = model.ContractHolderIdentifier,
                ContractHolderProp = model.ContractHolderProp,
                AffiliateIndicatorDesc = model.AffiliateIndicatorDesc,
                RateSchedule = model.RateSchedule,
                ServiceRequesterContract = model.ServiceRequesterContract,
                ContactStatus = model.ContactStatus,
                AmendmentReporting = model.AmendmentReporting,
                ContractBeginDate = model.ContractBeginDate,
                ContractEndDate = model.ContractEndDate,
                ContractEntitlementBeginDate = model.ContractEntitlementBeginDate,
                ContractEntitlementEndDate = model.ContractEntitlementEndDate,
                SurchargeIndicator = model.SurchargeIndicator,
                RateIdentificationCode = model.RateIdentificationCode,
                RateCharged = model.RateCharged,
                RateChargedReference = model.RateChargedReference,
                MaximumTariffRate = model.MaximumTariffRate,
                MaximumTariffRateReference = model.MaximumTariffRateReference,
                MarketBasedRateIndicator = model.MarketBasedRateIndicator,
                ReservationRateBasis = model.ReservationRateBasis,
                ContractualQuantityContract = model.ContractualQuantityContract,
                NegotiatedRateIndicator = model.NegotiatedRateIndicator,
                TermsNotesIndicator = model.TermsNotesIndicator,

                LocQTIPurpDesc1 = model.LocQTIPurpDesc1,
                Location1 = model.Location1,
                Location1Name = model.Location1Name,
                LocationZone1 = model.LocationZone1,

                LocQTIPurpDesc2 = model.LocQTIPurpDesc2,
                Location2 = model.Location2,
                Location2Name = model.Location2Name,
                LocationZone2 = model.LocationZone2,
                CapacityTypeIndicator = model.CapacityTypeIndicator,
                Comments = model.Comments,

                //For KinderMorgon-NGPL
                PeakSeasonalStartDate = model.PeakSeasonalStartDate,
                PeakSeasonalEndDate = model.PeakSeasonalEndDate,
                OffPeakSeasonalStartDate = model.OffPeakSeasonalStartDate,
                OffPeakSeasonalEndDate = model.OffPeakSeasonalEndDate,
                NegotId = model.NegotId
            };

        }
        public LocationsDTO Parse(Location model)
        {
            var dtoObj = new LocationsDTO
            {
                CreatedBy = model.CreatedBy,
                ID = model.ID,
                CreatedDate = model.CreatedDate,
                Identifier = model.Identifier,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = model.ModifiedDate,
                Name = model.Name,
                PipelineID = model.PipelineID,
                PropCode = model.PropCode,
                RDUsageID = model.RDUsageID,
            };
            if (dtoObj.RDUsageID == 1)
                dtoObj.RDB = "R";
            else if (dtoObj.RDUsageID == 2)
                dtoObj.RDB = "D";
            else
                dtoObj.RDB = "B";
            return dtoObj;
        }
        public BatchDetailDTO Parse(V4_Batch batch,IEnumerable<V4_Nomination> nomList)
        {
            BatchDetailDTO batchDetailObject = new BatchDetailDTO();
            batchDetailObject.SupplyList = new List<BatchDetailSupplyDTO>();
            batchDetailObject.ContractPath = new List<BatchDetailContractPathDTO>();
            batchDetailObject.Contract = new List<BatchDetailContractDTO>();
            batchDetailObject.MarketList = new List<BatchDetailMarketDTO>();
            if (batch != null)
            {
                Pipeline pipe = pipelineRepository.GetPipelineByDuns(batch.PipelineDuns);
                ShipperCompany shipperComp = shipperCompanyRepository.GetAll().Where(a => a.DUNS == batch.ServiceRequester).FirstOrDefault();
                batchDetailObject.BatchStatus = UpdateBatchStatus(batch.StatusID);
                batchDetailObject.CreatedBy = batch.CreatedBy;
                batchDetailObject.CreatedDateTime = batch.CreatedDate;
                batchDetailObject.CycleId = batch.CycleId;
                batchDetailObject.Description = batch.Description;
                batchDetailObject.Duns = pipe != null ? pipe.DUNSNo : "";
                batchDetailObject.EndDateTime = batch.FlowEndDate;
                batchDetailObject.Id = batch.TransactionID;
                //batchDetailObject.IsPNT=batch
                batchDetailObject.PakageCheck = batch.PakageCheck;
              //  batchDetailObject.PipelineId = batch.PipelineID;
                batchDetailObject.PipeLineName = pipe.Name;
                batchDetailObject.RankingCheck = batch.RankingCheck;
                batchDetailObject.ScheduleDate = batch.ScheduleDate.HasValue ? batch.ScheduleDate.Value : DateTime.MaxValue;
                batchDetailObject.ShiperDuns = batch.ServiceRequester;
                batchDetailObject.ShiperName = shipperComp.Name;
                batchDetailObject.ShowZeroCheck = batch.ShowZeroCheck;
                batchDetailObject.ShowZeroDn = batch.ShowZeroDn;
                batchDetailObject.ShowZeroUp = batch.ShowZeroUp;
                //batchDetailObject.sh
                batchDetailObject.StartDateTime = batch.FlowStartDate;
                batchDetailObject.StatusId = batch.StatusID;
                batchDetailObject.SubmittedDate = batch.SubmitDate.HasValue ? batch.SubmitDate.Value : DateTime.MaxValue;
                batchDetailObject.UpDnContractCheck = batch.UpDnContractCheck;
                batchDetailObject.UpDnPkgCheck = batch.UpDnPkgCheck;
                //batchDetailObject.
                
                if (nomList != null)
                {
                    IEnumerable<V4_Nomination> supList = nomList.Where(a => a.PathType.Contains("S"));
                    IEnumerable<V4_Nomination> tranList = nomList.Where(a => a.PathType.Contains("T"));
                    IEnumerable<V4_Nomination> marList = nomList.Where(a => a.PathType.Contains("M"));
                    if (supList != null)
                        foreach (var item in supList)
                        {
                            Contract con = contractRepository.GetContractByContractNo(item.ContractNumber,batch.PipelineDuns);  
                            metadataRequestType reqType = null;
                            if (con != null)
                                reqType = metadataRequestTypeRepository.GetById(con.RequestTypeID);
                            List<metadataTransactionType> tranType = metadataTransactionTypeRepository.GetAll().Where(a =>a.Identifier == item.TransactionType).ToList();
                            BatchDetailSupplyDTO supplyObj = new BatchDetailSupplyDTO();
                            supplyObj.ID = item.ID;

                            supplyObj.LocationProp = item.receiptLocationPropCode;

                            supplyObj.Location = item.receiptLocationPropCode;
                            supplyObj.LocationName = item.ReceiptLocationName;
                            //supplyObj.ServiceRequestNo
                            supplyObj.TransactionTypeDescription = (tranType != null && tranType.Count > 0) ? tranType.Count > 1 ? tranType.Where(a => a.Name.ToLower().Contains("buy")).FirstOrDefault().Name : tranType.FirstOrDefault().Name : "";
                            supplyObj.TransactionType = item.TransactionType;

                            supplyObj.ServiceRequestNo = item.ContractNumber;
                            supplyObj.ServiceRequestType = reqType != null ? reqType.Name : "";

                            supplyObj.UpstreamIDProp = item.UpstreamPropCode;
                            supplyObj.UpstreamIDName = item.UpstreamName;
                            supplyObj.UpstreamID = item.UpstreamIdentifier;

                            supplyObj.DefaultInd = "";
                            supplyObj.ReceiptQuantityGross = item.Quantity.HasValue ? item.Quantity.Value : 0;
                            supplyObj.FuelPercentage = con != null ? con.FuelPercentage.ToString() : "0.0";
                            supplyObj.DeliveryQuantityNet = Convert.ToInt32(supplyObj.ReceiptQuantityGross) * (100 - Convert.ToInt32(supplyObj.FuelPercentage) / 100);
                            supplyObj.FuelQunatity = (Convert.ToDecimal(supplyObj.ReceiptQuantityGross) - Convert.ToDecimal(supplyObj.DeliveryQuantityNet)).ToString();
                            supplyObj.UpstreamRank = item.UpstreamRank;

                            supplyObj.PackageID = item.PackageId;

                            supplyObj.BatchID = item.TransactionID;

                            supplyObj.UpContractIdentifier = item.UpstreamContractIdentifier;

                            supplyObj.UpPackageID = item.UpstreamPackageId;

                            supplyObj.IsActive = true;

                            supplyObj.CreatedBy = Guid.Empty;

                            supplyObj.CreatedDate = batch.CreatedDate;

                            supplyObj.ModifiedBy = Guid.Empty;

                            supplyObj.ModifiedDate = DateTime.MinValue;
                            SQTSTrackOnNom sqts = GetSqtsResult(item.NominatorTrackingId, batchDetailObject.Id);
                            if (sqts != null)
                            {
                                supplyObj.DelPointQty = sqts.DeliveryPointQuantity;
                                supplyObj.RecPointQty = sqts.ReceiptPointQuantity;
                                supplyObj.ReductionReason = sqts.ReductionReason;
                            }
                            batchDetailObject.SupplyList.Add(supplyObj);
                            batchDetailObject.supplyRecTotal = batchDetailObject.supplyRecTotal + Convert.ToInt32(supplyObj.ReceiptQuantityGross);
                            batchDetailObject.supplyDelTotal = batchDetailObject.supplyDelTotal + Convert.ToInt32(supplyObj.DeliveryQuantityNet);
                        }
                    if (tranList != null)
                        foreach (var item in tranList)
                        {
                            var contractNumber = item.ContractNumber;
                            Contract con = contractRepository.GetContractByContractNo(item.ContractNumber, batch.PipelineDuns); 
                            if (con != null)
                            {
                                metadataRequestType reqType = metadataRequestTypeRepository.GetById(con.RequestTypeID);
                                if (!batchDetailObject.ContractPath.Any(a => a.ServiceRequestNo == contractNumber))
                                {
                                    BatchDetailContractPathDTO contractPathobj = new BatchDetailContractPathDTO();
                                    contractPathobj.ID = item.ID;
                                    contractPathobj.ServiceRequestNo = contractNumber;
                                    contractPathobj.ServiceRequestType = reqType != null ? reqType.Name : "";
                                    contractPathobj.FuelPercentage = con.FuelPercentage.ToString();
                                    contractPathobj.DefaultInd = "";
                                    contractPathobj.MaxDeliveredQuantity = con.MDQ.ToString();
                                    contractPathobj.NominatedQuantity = "";

                                    contractPathobj.OverrunQuantity = "";
                                    contractPathobj.BatchID = batch.TransactionID;
                                    contractPathobj.IsActive = true;

                                    contractPathobj.CreatedBy = Guid.Empty;
                                    contractPathobj.ModifiedBy = Guid.Empty;
                                    contractPathobj.CreatedDate = DateTime.MinValue;
                                    contractPathobj.ModifiedDate = DateTime.MinValue;
                                    batchDetailObject.ContractPath.Add(contractPathobj);
                                }

                                //Location locRec = locationRepository.GetAll().Where(a => (a.Identifier == item.ReceiptLocationIdentifier && a.PipelineID == batch.PipelineID)).FirstOrDefault();
                                //Location locDel = locationRepository.GetAll().Where(a => (a.Identifier == item.DeliveryLocationIdentifer && a.PipelineID == batch.PipelineID)).FirstOrDefault();
                                List<metadataTransactionType> tranType = metadataTransactionTypeRepository.GetAll().Where(a => (a.Identifier == item.TransactionType)).ToList();

                                BatchDetailContractDTO contractObj = new BatchDetailContractDTO();
                                contractObj.ID = item.ID;
                                contractObj.TransactionTypeDescription = (tranType != null && tranType.Count > 0) ? tranType.Count > 1 ? tranType.Where(a => a.Name.ToLower().Contains("transport")).FirstOrDefault().Name : tranType.FirstOrDefault().Name : "";
                                contractObj.TransactionType = item.TransactionType;
                                contractObj.RecLocationProp = item.receiptLocationPropCode;
                                contractObj.ServiceRequestNo = contractNumber;
                                contractObj.RecLocationName =item.ReceiptLocationName;
                                contractObj.RecLocation = item.ReceiptLocationIdentifier;

                                contractObj.DelRank = item.DeliveryRank;
                                contractObj.RecZone = "";

                                contractObj.DelLocationProp = item.DeliveryLocationPropCode;
                                contractObj.DelLocationName = item.DeliveryLocationName;
                                contractObj.DelLocation = item.DeliveryLocationIdentifer;

                                contractObj.RecRank = item.ReceiptRank;
                                contractObj.DelZone = "";
                                contractObj.ReceiptDth = item.Quantity.HasValue ? item.Quantity.Value.ToString() : "0";
                                contractObj.FuelPercentage = con != null ? con.FuelPercentage.ToString() : "0.0";

                                contractObj.DeliveryDth = (Math.Round(Convert.ToInt32(contractObj.ReceiptDth) * (100 - Convert.ToDecimal(contractObj.FuelPercentage)) / 100)).ToString();
                                contractObj.FuelDth = (Convert.ToDecimal(contractObj.ReceiptDth) - Convert.ToDecimal(contractObj.DeliveryDth)).ToString();
                                contractObj.PackageID = item.PackageId;
                                contractObj.Route = item.Route;
                                contractObj.PathRank = item.PathRank;
                                contractObj.TransportContractID = int.MinValue;
                                contractObj.BatchID = item.TransactionID;
                                contractObj.IsActive = true;
                                contractObj.CreatedBy = Guid.Empty;
                                contractObj.CreatedDate = DateTime.MinValue;
                                contractObj.ModifiedBy = Guid.Empty;
                                contractObj.ModifiedDate = DateTime.MinValue;
                                SQTSTrackOnNom sqts = GetSqtsResult(item.NominatorTrackingId, batchDetailObject.Id);
                                if (sqts != null)
                                {
                                    contractObj.DelPointQty = sqts.DeliveryPointQuantity;
                                    contractObj.RecPointQty = sqts.ReceiptPointQuantity;
                                    contractObj.ReductionReason = sqts.ReductionReason;
                                }
                                batchDetailObject.Contract.Add(contractObj);
                                batchDetailObject.ContractPath.Where(a => a.ServiceRequestNo == contractObj.ServiceRequestNo).FirstOrDefault().transportRecTotal =
                                    batchDetailObject.ContractPath.Where(a => a.ServiceRequestNo == contractObj.ServiceRequestNo).FirstOrDefault().transportRecTotal + Convert.ToInt32(contractObj.ReceiptDth);
                                batchDetailObject.ContractPath.Where(a => a.ServiceRequestNo == contractObj.ServiceRequestNo).FirstOrDefault().transportDelTotal =
                                    batchDetailObject.ContractPath.Where(a => a.ServiceRequestNo == contractObj.ServiceRequestNo).FirstOrDefault().transportDelTotal + Convert.ToInt32(contractObj.DeliveryDth);
                                //batchDetailObject.transportRecTotal = batchDetailObject.transportRecTotal + Convert.ToInt32(contractObj.ReceiptDth);
                                //batchDetailObject.transportDelTotal = batchDetailObject.transportDelTotal + Convert.ToInt32(contractObj.DeliveryDth);

                            }
                        }
                    if (marList != null)
                        foreach (var item in marList)
                        { 
                            List<metadataTransactionType> tranType = metadataTransactionTypeRepository.GetAll().Where(a => (a.Identifier == item.TransactionType)).ToList();
                            Contract con = contractRepository.GetContractByContractNo(item.ContractNumber, batch.PipelineDuns);  
                            metadataRequestType reqType = null;
                            if (con != null)
                                reqType = metadataRequestTypeRepository.GetById(con.RequestTypeID);
                            BatchDetailMarketDTO marketObj = new BatchDetailMarketDTO();
                            marketObj.ID = item.ID;

                            marketObj.LocationProp = item.DeliveryLocationPropCode;
                            marketObj.Location = item.DeliveryLocationIdentifer;
                            marketObj.LocationName = item.DeliveryLocationName;

                            marketObj.TransactionTypeDescription = (tranType != null && tranType.Count > 0) ? tranType.Count > 1 ? tranType.Where(a => a.Name.ToLower().Contains("sell")).FirstOrDefault().Name : tranType.FirstOrDefault().Name : "";
                            marketObj.TransactionType = item.TransactionType;

                            marketObj.ServiceRequestNo = item.ContractNumber;
                            marketObj.ServiceRequestType = reqType != null ? reqType.Name : "";

                            marketObj.DownstreamIDProp = item.DownstreamPropCode;
                            marketObj.DownstreamIDName = item.DownstreamName;
                            marketObj.DownstreamID = item.DownstreamIdentifier;

                            marketObj.DefaultInd = "";
                            marketObj.ReceiptQuantityGross = item.Quantity.HasValue ? item.Quantity.Value : 0;
                            marketObj.FuelPercentage = con != null ? con.FuelPercentage.ToString() : "0.0";
                            marketObj.DeliveryQuantityNet = (Convert.ToInt32(marketObj.ReceiptQuantityGross) * (100 - Convert.ToInt32(marketObj.FuelPercentage)) / 100);
                            marketObj.FuelQunatity = (Convert.ToDecimal(marketObj.ReceiptQuantityGross) - Convert.ToDecimal(marketObj.DeliveryQuantityNet)).ToString();
                            marketObj.DnstreamRank = item.DownstreamRank;

                            marketObj.PackageID = item.PackageId;

                            marketObj.BatchID = item.TransactionID;

                            marketObj.DnPackageID = item.DownstreamPackageId;

                            marketObj.DnContractIdentifier = item.DownstreamContractIdentifier;

                            marketObj.IsActive = true;

                            marketObj.CreatedBy = Guid.Empty;

                            marketObj.CreatedDate = DateTime.MinValue;

                            marketObj.ModifiedBy = Guid.Empty;
                            SQTSTrackOnNom sqts = GetSqtsResult(item.NominatorTrackingId, batchDetailObject.Id);
                            if (sqts != null)
                            {
                                marketObj.DelPointQty = sqts.DeliveryPointQuantity;
                                marketObj.RecPointQty = sqts.ReceiptPointQuantity;
                                marketObj.ReductionReason = sqts.ReductionReason;
                            }
                            marketObj.ModifiedDate = DateTime.MinValue;
                            batchDetailObject.MarketList.Add(marketObj);
                            batchDetailObject.marketRecTotal = batchDetailObject.marketRecTotal + Convert.ToInt32(marketObj.ReceiptQuantityGross);
                            batchDetailObject.marketDelTotal = batchDetailObject.marketDelTotal + Convert.ToInt32(marketObj.DeliveryQuantityNet);
                        }

                }
            }
            
            return batchDetailObject;
        }

        public Contract Create(ContractsDTO conDTO)
        {
            Contract con = new Contract();
            if (conDTO != null)
            {
                con.CreatedBy = conDTO.CreatedBy;
                con.CreatedDate = conDTO.CreatedDate;
                con.DeliveryZone = conDTO.DeliveryZone;
                con.FuelPercentage = conDTO.FuelPercentage;
                con.ID = conDTO.ID;
                con.IsActive = conDTO.IsActive;
                con.LocationFromID = conDTO.LocationFromID;
                con.LocFromIdentifier = conDTO.LocationFromIdentifier;
                con.LocFromName = conDTO.LocationFrom;
                con.LocationToID = conDTO.LocationToID;
                con.LocToIdentifier = conDTO.LocationToIdentifier;
                con.LocToName = conDTO.LocationTo;
                con.MDQ = conDTO.MDQ;
                con.ModifiedBy = conDTO.ModifiedBy;
                con.ModifiedDate = conDTO.ModifiedDate;
                //con.PipelineID = conDTO.PipelineID;
                con.ReceiptZone = conDTO.ReceiptZone;
                con.RequestNo = conDTO.RequestNo;
                con.RequestTypeID =conDTO.RequestTypeID!=null ? conDTO.RequestTypeID.Value : 0;
                con.ShipperID = conDTO.ShipperID;
                con.ValidUpto = conDTO.ValidUpto;
                con.PipeDuns = conDTO.PipeDuns;
                con.RateSchedule = conDTO.RateSchedule;
            }
            return con;
        }

        public ContractsDTO Parse(Contract item)
        {
            metadataRequestType recType = metadataRequestTypeRepository.GetByRequestorTypeId(item.RequestTypeID).FirstOrDefault();//.GetAll().Where(a => a.ID == item.RequestTypeID).FirstOrDefault();
            
            //var locRec = locationRepository.GetById(item.LocationFromID);
            //var locDel = locationRepository.GetById(item.LocationToID);

            ContractsDTO con = new ContractsDTO();
            con.CreatedBy = item.CreatedBy;
            con.CreatedDate = item.CreatedDate;
            con.DeliveryZone = item.DeliveryZone;
            con.FuelPercentage = item.FuelPercentage;
            con.ID = item.ID;
            con.IsActive = item.IsActive;
            con.LocationFromID = item.LocationFromID;
            con.LocationFromIdentifier = item.LocFromIdentifier;
            con.LocationFrom = item.LocFromName;
            con.LocationTo = item.LocToName;
            //con.LocationFrom = locRec != null ? locRec.Name : string.Empty;
            con.LocationToID = item.LocationToID;
            con.LocationToIdentifier = item.LocToIdentifier;
           // con.LocationTo = locDel != null ? locDel.Name : string.Empty;
            con.MDQ = item.MDQ;
            con.ModifiedBy = item.ModifiedBy;
            con.ModifiedDate = item.ModifiedDate;
            con.PipelineID = item.PipelineID;
            con.PipeDuns = item.PipeDuns;
            con.ReceiptZone = item.ReceiptZone;
            con.RequestNo = item.RequestNo;
            con.RequestType = recType != null ? recType.Name : string.Empty;
            con.RequestTypeID = item.RequestTypeID;
            con.ShipperID = item.ShipperID;
            con.ValidUpto = item.ValidUpto;
            con.RateSchedule = item.RateSchedule;
            return con;
        }
        public CounterPartiesDTO Parse(CounterParty item)
        {
            CounterPartiesDTO itemObj = new CounterPartiesDTO();
            itemObj.ID = item.ID;
            itemObj.Name = item.Name;
            itemObj.Identifier = item.Identifier;
            itemObj.PropCode = item.PropCode;
            itemObj.PipelineID = item.PipelineID;
            itemObj.IsActive = item.IsActive;
            itemObj.CreatedBy = item.CreatedBy;
            itemObj.CreatedDate = item.CreatedDate;
            itemObj.ModifiedBy = item.ModifiedBy;
            itemObj.ModifiedDate = item.ModifiedDate;
            return itemObj;
        }
        public TransactionTypesDTO Parse(metadataTransactionType tt)
        {
            TransactionTypesDTO itemObj = new TransactionTypesDTO();
            itemObj.ID = tt.ID;
            itemObj.Identifier = tt.Identifier;
            itemObj.Name = tt.Name;
            itemObj.SequenceNo = tt.SequenceNo;
            itemObj.IsActive = tt.IsActive;
            return itemObj;
        }
        public UploadFileDTO Parse(UploadedFile a)
        {
            UploadFileDTO dto = new UploadFileDTO();
            dto.ID = a.ID;
            dto.FileName = a.FileName;
            dto.AddedBy = a.AddedBy;
            dto.CreatedDate = a.CreatedDate;
            dto.pipelineDuns = a.PipelineDuns;
            return dto;
        }
        public BatchDTO Parse(V4_Batch batch)
        {
            Pipeline pipe = pipelineRepository.GetPipelineByDuns(batch.PipelineDuns);
            metadataCycle cycle = metadataCycleRepository.GetById(batch.CycleId);
            BatchDTO itemObj = new BatchDTO();
            itemObj.Id = batch.TransactionID;
            itemObj.Description = batch.Description;
            itemObj.pipeDUNSNo = batch.PipelineDuns;
            itemObj.DateBeg = batch.FlowStartDate;
            itemObj.DateEnd = batch.FlowEndDate;
            itemObj.CycleId = batch.CycleId;
            itemObj.StatusID = batch.StatusID;
            itemObj.SubmittedDate = batch.SubmitDate != null ? batch.SubmitDate.Value : DateTime.MaxValue;
            itemObj.ScheduledDate = batch.ScheduleDate != null ? batch.SubmitDate.Value : DateTime.MaxValue;
            itemObj.ServiceRequester = batch.ServiceRequester;
            itemObj.ShowZeroes = batch.ShowZeroCheck;
            itemObj.RankingChecked = batch.RankingCheck;
            itemObj.PackageChecked = batch.PakageCheck;
            itemObj.UpDnContract = batch.UpDnContractCheck;
            itemObj.ShowZeroesUp = batch.ShowZeroUp;
            itemObj.ShowZeroesDn = batch.ShowZeroDn;
            itemObj.UpDnPakgID = batch.UpDnPkgCheck;
            itemObj.IsActive = batch.IsActive;
            itemObj.CreatedBy = batch.CreatedBy;
            itemObj.CreatedDate = batch.CreatedDate;
            itemObj.PipelineName = pipe != null ? pipe.Name : string.Empty;
            itemObj.Cycle = cycle!=null?cycle.Name:string.Empty;
            itemObj.CycleId = batch.CycleId;
            itemObj.Status = UpdateBatchStatus(batch.StatusID);
            itemObj.NomTypeID = batch.NomTypeID.Value;
            itemObj.ReferenceNumber = batch.ReferenceNumber;
            itemObj.pipeDUNSNo = batch.PipelineDuns;
            itemObj.CycleCode = cycle != null ? cycle.Code : string.Empty;
            itemObj.NomSubCycle = batch.NomSubCycle;
            return itemObj;
        }
        public PipelineDTO Parse(Pipeline pipe)
        {
            PipelineDTO dto = new PipelineDTO();
            dto.CreatedBy = pipe.CreatedBy;
            dto.CreatedDate = pipe.CreatedDate;
            dto.DUNSNo = pipe.DUNSNo;
            dto.ID = pipe.ID;
            dto.IsActive = pipe.IsActive;
            dto.ModelTypeID = pipe.ModelTypeID;
            dto.ModifiedBy = pipe.ModifiedBy;
            dto.ModifiedDate = pipe.ModifiedDate;
            dto.Name = pipe.Name + " (" + pipe.DUNSNo + ")";
            dto.NameWithoutDuns = pipe.Name;
            dto.ToUseTSPDUNS = pipe.ToUseTSPDUNS;
            dto.TSPId = pipe.TSPId;
            dto.TempItem = pipe.DUNSNo + "-" + pipe.ModelTypeID; // pipe.ID + "-" + pipe.ModelTypeID;
            dto.IsUprdActive = pipe.IsUprdActive;
            return dto;
        }
        public SettingDTO Parse(Setting a)
        {
            SettingDTO s = new SettingDTO();
            s.ID = a.ID;
            s.Value = a.Value;
            s.IsActive = a.IsActive;
            s.CreatedBy = a.CreatedBy;
            s.CreatedDate = a.CreatedDate;
            s.ModifiedBy = a.ModifiedBy;
            s.ModifiedDate = a.ModifiedDate;
            s.Name = a.Name;
            return s;
        }
        public PipelineEDISettingDTO Parse(PipelineEDISetting pipeSetting)
        {
            if (pipeSetting != null)
                return new PipelineEDISettingDTO
                {
                    id = pipeSetting.id,
                    DataSeparator = pipeSetting.DataSeparator,
                    DatasetId = pipeSetting.DatasetId,
                    GS01_Segment = pipeSetting.GS01_Segment,
                    GS02_Segment = pipeSetting.GS02_Segment,
                    GS03_Segment = pipeSetting.GS03_Segment,
                    GS07_Segment = pipeSetting.GS07_Segment,
                    GS08_Segment = pipeSetting.GS08_Segment,
                    SegmentSeperator = pipeSetting.SegmentSeperator,
                    ShipperCompDuns = pipeSetting.ShipperCompDuns,
                    ST01_Segment = pipeSetting.ST01_Segment,
                    ISA06_Segment = pipeSetting.ISA06_Segment,
                    ISA08_segment = pipeSetting.ISA08_segment,
                    ISA11_Segment = pipeSetting.ISA11_Segment,
                    ISA12_Segment = pipeSetting.ISA12_Segment,
                    ISA16_Segment = pipeSetting.ISA16_Segment,
                    PipeDuns = pipeSetting.PipeDuns,
                    StartDate = pipeSetting.StartDate.HasValue ? pipeSetting.StartDate.Value : DateTime.MinValue,
                    EndDate = pipeSetting.EndDate.HasValue ? pipeSetting.EndDate.Value : DateTime.MinValue,
                    SendManually = pipeSetting.SendManually,
                    ForOacy = pipeSetting.ForOacy,
                    ForUnsc = pipeSetting.ForUnsc,
                    ForSwnt = pipeSetting.ForSwnt
                };
            else
                return null;
        }
        private SQTSTrackOnNom GetSqtsResult(string NomTrackingId, Guid TransactionId)
        {
            return SQTSTrackOnNomRepository.GetAll().Where(a => a.NomTransactionID == TransactionId && a.NomTrackingId == NomTrackingId).FirstOrDefault();
        }
        private string UpdateBatchStatus(int StatusId)
        {
            if (StatusId == (int)NomStatus.Draft)
            {
                return "Draft";
            }
            else if (StatusId == Convert.ToInt32(NomStatus.Error))
            {
                return "Exception Occured";
            }
            else if (StatusId == Convert.ToInt32(NomStatus.Submitted))
            {
                return "Submitted";
            }
            else if (StatusId == Convert.ToInt32(NomStatus.Rejected))
            {
                return "Rejected ";
            }
            else if (StatusId == Convert.ToInt32(NomStatus.Accepted))
            {
                return "Accepted";
            }
            else if (StatusId == Convert.ToInt32(NomStatus.InProcess))
            {
                return "In Process";
            }
            else
                return string.Empty;
        }
        public List<V4_NominationDTO> Parse(List<V4_Nomination> a)
        {
            List<V4_NominationDTO> nDTO = new List<V4_NominationDTO>();
            foreach (var l in a)
                nDTO.Add(Parse(l));
            return nDTO;
        }
        public V4_NominationDTO Parse(V4_Nomination a)
        {
            V4_NominationDTO n = new V4_NominationDTO();
            n.ID = a.ID;
            n.ImbalancePeriod = a.ImbalancePeriod;
            n.MaxDeliveredQty = a.MaxDeliveredQty;
            n.MaxRateIndicator = a.MaxRateIndicator;
            n.NominationSubCycleIndicator = a.NominationSubCycleIndicator;
            n.NominationUserData1 = a.NominationUserData1;
            n.NominationUserData2 = a.NominationUserData2;
            n.NominatorTrackingId = a.NominatorTrackingId;
            n.PackageId = a.PackageId;
            n.PathRank = a.PathRank;
            n.PathType = a.PathType;
            n.ProcessingRightIndicator = a.ProcessingRightIndicator;
            n.Quantity = a.Quantity;
            n.QuantityTypeIndicator = a.QuantityTypeIndicator;
            n.ReceiptLocationIdentifier = a.ReceiptLocationIdentifier;
            n.receiptLocationPropCode = a.receiptLocationPropCode;
            n.ReceiptRank = a.ReceiptRank;
            n.Route = a.Route;
            n.ServiceProviderActivityCode = a.ServiceProviderActivityCode;
            n.TransactionID = a.TransactionID;
            n.TransactionType = a.TransactionType;
            n.TransactionTypeDesc = a.TransactionTypeDesc;
            n.UnitOfMeasure = a.UnitOfMeasure;
            n.UpstreamContractIdentifier = a.UpstreamContractIdentifier;
            n.UpstreamIdentifier = a.UpstreamIdentifier;
            n.UpstreamPackageId = a.UpstreamPackageId;
            n.UpstreamPropCode = a.UpstreamPropCode;
            n.UpstreamRank = a.UpstreamRank;
            n.AssignIdentification = a.AssignIdentification;
            n.AssociatedContract = a.AssociatedContract;
            n.BidTransportationRate = a.BidTransportationRate;
            n.BidupIndicator = a.BidupIndicator;
            n.CapacityTypeIndicator = a.CapacityTypeIndicator;
            n.ContractNumber = a.ContractNumber;
            n.DealType = a.DealType;
            n.DeliveryLocationIdentifer = a.DeliveryLocationIdentifer;
            n.DeliveryLocationPropCode = a.DeliveryLocationPropCode;
            n.DeliveryRank = a.DeliveryRank;
            n.DelQuantity = a.DelQuantity;
            n.DownstreamContractIdentifier = a.DownstreamContractIdentifier;
            n.DownstreamIdentifier = a.DownstreamIdentifier;
            n.DownstreamPackageId = a.DownstreamPackageId;
            n.DownstreamPropCode = a.DownstreamPropCode;
            n.DownstreamRank = a.DownstreamRank;
            n.ExportDecleration = a.ExportDecleration;
            n.FuelPercentage = a.FuelPercentage;
            return n;
        }
        public PipelineEDISetting Create(PipelineEDISettingDTO a)
        {
            return new PipelineEDISetting
            {
                SendManually = a.SendManually,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                ForOacy = a.ForOacy,
                ForSwnt = a.ForSwnt,
                ForUnsc = a.ForUnsc,
                ShipperCompDuns=a.ShipperCompDuns
            };
        }
        public List<NMQRPerTransaction> Create(List<NMQRPerTransactionDTO> nmqrList)
        {
            List<NMQRPerTransaction> nmqrlist = new List<NMQRPerTransaction>();
            foreach(var a in nmqrList)
            {
                nmqrlist.Add(Create(a));
            }
            return nmqrlist;
        }
        private NMQRPerTransaction Create(NMQRPerTransactionDTO a)
        {
            NMQRPerTransaction nmqr = new NMQRPerTransaction();
            nmqr.CreatedDate = a.CreatedDate;
            nmqr.ID = a.ID;
            nmqr.NominationTrackingId = a.NominationTrackingId;
            nmqr.ReferenceNumber = a.ReferenceNumber;
            nmqr.StatusCode = a.StatusCode;
            nmqr.Transactionid = a.Transactionid;
            nmqr.ValidationCode = a.ValidationCode;
            nmqr.ValidationMessage = a.ValidationMessage;
            return nmqr;
        }
      
        public List<UPRDStatu> Create(List<UPRDStatusDTO> uprdStatusList)
        {
            List<UPRDStatu> uprdstatuslist = new List<UPRDStatu>();
            foreach(var a in uprdStatusList)
            {
                uprdstatuslist.Add(Create(a));
            }
            return uprdstatuslist;
        }
        private UPRDStatu Create(UPRDStatusDTO a)
        {
            UPRDStatu u = new UPRDStatu();
            u.CreatedDate = a.CreatedDate;
            u.DatasetRequested = a.DatasetRequested;
            u.DatasetSummary = a.DatasetSummary;
            u.RequestID = a.RequestID;
            u.RURD_ID = a.RURD_ID;
            u.TransactionId = a.TransactionId;
            u.UPRD_ID = a.UPRD_ID;
            u.IsRURDReceived = a.IsRURDReceived;
            u.IsDataSetAvailable = a.IsDataSetAvailable;
            u.IsDatasetReceived = a.IsDatasetReceived;
            return u;
        }
        public List<SQTSPerTransaction> Create(List<SQTSPerTransactionDTO> sqtsList)
        {
            List<SQTSPerTransaction> sqtslist = new List<SQTSPerTransaction>();
            foreach(var a in sqtsList)
            {
                sqtslist.Add(Create(a));
            }
            return sqtslist;
        }
        private SQTSPerTransaction Create(SQTSPerTransactionDTO a)
        {
            SQTSPerTransaction s = new SQTSPerTransaction();
            s.AssociatedContract = a.AssociatedContract;
            s.BeginingDateTime = a.BeginingDateTime;
            s.BidTransportationRate = a.BidTransportationRate;
            s.CapacityTypeIndicator = a.CapacityTypeIndicator;
            s.CycleIndicator = a.CycleIndicator;
            s.DealType = a.DealType;
            s.DeliveryLocation = a.DeliveryLocation;
            s.DeliveryQuantity = a.DeliveryQuantity;
            s.DeliveryRank = a.DeliveryRank;
            s.DownstreamContractIdentifier = a.DownstreamContractIdentifier;
            s.DownstreamID = a.DownstreamID;
            s.DownstreamPackageId = a.DownstreamPackageId;
            s.EndingDateTime = a.EndingDateTime;
            s.ExportDecleration = a.ExportDecleration;
            s.FuelQuantity = a.FuelQuantity;
            s.Id = a.Id;
            s.ModelType = a.ModelType;
            s.NominationUserData1 = a.NominationUserData1;
            s.NominationUserData2 = a.NominationUserData2;
            s.NomSubsequentCycleIndicator = a.NomSubsequentCycleIndicator;
            s.NomTrackingId = a.NomTrackingId;
            s.PackageId = a.PackageId;
            s.pipelineId = a.pipelineId;
            s.ProcessingRightsIndicator = a.ProcessingRightsIndicator;
            s.ReceiptLocation = a.ReceiptLocation;
            s.ReceiptQuantity = a.ReceiptQuantity;
            s.ReceiptRank = a.ReceiptRank;
            s.ReductionReason = a.ReductionReason;
            s.Route = a.Route;
            s.ServiceProviderActivityCode = a.ServiceProviderActivityCode;
            s.ServiceRequestor = a.ServiceRequestor;
            s.ServiceRequestorContract = a.ServiceRequestorContract;
            s.StatementDate = a.StatementDate;
            s.TranasactionId = a.TranasactionId;
            s.TransactionType = a.TransactionType;
            s.TSPCode = a.TSPCode;
            s.UpstreamContractIdentifier = a.UpstreamContractIdentifier;
            s.UpstreamId = a.UpstreamId;
            s.UpstreamPackageId = a.UpstreamPackageId;
            s.ReductionReasonDescription = a.ReductionReasonDescription;
            return s;
        }
        public List<UPRDStatusDTO> Parse(List<UPRDStatu> uprdStatusList)
        {
            List<UPRDStatusDTO> uprdDTOList=new List<UPRDStatusDTO>();
            foreach(var a in uprdStatusList)
            {
                uprdDTOList.Add(Parse(a));
            }
            return uprdDTOList;
        }
        private UPRDStatusDTO Parse(UPRDStatu a)
        {
            UPRDStatusDTO d = new UPRDStatusDTO();
            d.CreatedDate = a.CreatedDate;
            d.DatasetRequested = a.DatasetRequested;
            d.DatasetSummary = a.DatasetSummary;
            d.IsDataSetAvailable = a.IsDataSetAvailable;
            d.IsDatasetReceived = a.IsDatasetReceived;
            d.IsRURDReceived = a.IsRURDReceived;
            d.RequestID = a.RequestID;
            d.RURD_ID = a.RURD_ID;
            d.TransactionId = a.TransactionId;
            d.UPRD_ID = a.UPRD_ID;
            return d;
        }

        public V4_Nomination Create(NonPathedRecieptNom rec)
        {
            return new V4_Nomination
            {
               // TransactionID=rec.TransactionId,
                AssignIdentification="",
                AssociatedContract="",
                BidTransportationRate="",
                BidupIndicator="",
                CapacityTypeIndicator="",
                ContractNumber=rec.ServiceRequesterContractCode,
                DealType="",
                DeliveryLocationIdentifer="",
                DeliveryLocationPropCode="",
                DeliveryRank="",
                DelQuantity=0,
                DownstreamContractIdentifier="",
                DownstreamIdentifier="",
                DownstreamPackageId="",
                DownstreamPropCode="",
                DownstreamRank="",
                ExportDecleration="",
                FuelPercentage=0,
                ImbalancePeriod="",
                MaxDeliveredQty=0,
                MaxRateIndicator="",
                NominationSubCycleIndicator="",
                NominationUserData1="",
                NominationUserData2="",
                NominatorTrackingId=rec.NomTrackingId,
                PackageId=rec.PackageId,
                PackageId2="",
                PathRank="",
                PathType="NPR",
                ProcessingRightIndicator="",
                receiptLocationPropCode=rec.ReceiptLocProp,
                ReceiptLocationIdentifier=rec.ReceiptLocId,
                ReceiptRank=rec.ReceiptRank,
                Route="",
                ServiceProviderActivityCode="",
                TransactionType=rec.TransactionType,
                TransactionTypeDesc=rec.TransactionTypeDesc,
                UnitOfMeasure="BZ",
                UpstreamContractIdentifier=rec.UpstreamK,
                UpstreamIdentifier=rec.UpstreamId,              
                UpstreamPropCode=rec.UpstreamProp,
                UpstreamRank="",
                Quantity=Convert.ToInt32(rec.ReceiptQty),
                QuantityTypeIndicator=""
            };
        }

        public List<SQTSOPPerTransaction> Create(List<SQTSOPPerTransactionDTO> list)
        {
            List<SQTSOPPerTransaction> sqtsList = new List<SQTSOPPerTransaction>();
            if(list.Count()>0)
                foreach(var item in list)
                {
                    sqtsList.Add(Create(item));
                }
            return sqtsList;
        }

        private SQTSOPPerTransaction Create(SQTSOPPerTransactionDTO item)
        {
            SQTSOPPerTransaction sqtsOp = new SQTSOPPerTransaction();
            sqtsOp.Id = item.Id;
            sqtsOp.TransactionId = item.TransactionId;
            sqtsOp.ConfirmationRole = item.ConfirmationRole;
            sqtsOp.ConfirmationSusequenceCycleIndicator = item.ConfirmationSusequenceCycleIndicator;
            sqtsOp.ConfirmationTrackingID = item.ConfirmationTrackingID;
            sqtsOp.ConfirmationUserData1 = item.ConfirmationUserData1;
            sqtsOp.ConfirmationUserData2 = item.ConfirmationUserData2;
            sqtsOp.ContractualFLowIndicator = item.ContractualFLowIndicator;
            sqtsOp.CycleIndicator = item.CycleIndicator;
            sqtsOp.DownPkgId = item.DownPkgId;
            sqtsOp.DownstreamParty = item.DownstreamParty;
            sqtsOp.DwnStreamShipperContract = item.DwnStreamShipperContract;
            sqtsOp.EffectiveEndDate = item.EffectiveEndDate;
            sqtsOp.EffectiveStartDate = item.EffectiveStartDate;
            sqtsOp.Location = item.Location;
            sqtsOp.LocationCapacityFlowIndicator = item.LocationCapacityFlowIndicator;
            sqtsOp.LocationNetCapacity = item.LocationNetCapacity;
            sqtsOp.PackageId = item.PackageId;
            sqtsOp.PreparerID = item.PreparerID;
            sqtsOp.Quantity = item.Quantity;
            sqtsOp.ReductionQuantity = item.ReductionQuantity;
            sqtsOp.ReductionReason = item.ReductionReason;
            sqtsOp.SchedulingStatus = item.SchedulingStatus;
            sqtsOp.ServiceContract = item.ServiceContract;
            sqtsOp.ServiceIdentifierCode = item.ServiceIdentifierCode;
            sqtsOp.ServiceRequester = item.ServiceRequester;
            sqtsOp.ServiceRequesterContract = item.ServiceRequesterContract;
            sqtsOp.StatementDate = item.StatementDate;
            sqtsOp.Statement_ReceipentID = item.Statement_ReceipentID;
            sqtsOp.UpstreamParty = item.UpstreamParty;
            sqtsOp.UpstrmPkgId = item.UpstrmPkgId;
            sqtsOp.UpstrmShipperContract = item.UpstrmShipperContract;
            return sqtsOp;
        }
    }
}
