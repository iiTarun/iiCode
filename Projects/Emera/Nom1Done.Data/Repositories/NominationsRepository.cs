using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Nom1Done.Data.Repositories
{
    public class NominationsRepository : RepositoryBase<V4_Nomination>, INominationsRepository
    {
        public NominationsRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }
        public IEnumerable<BatchDTO> GetAllNominationBatch()
        {
            try
            {
                var batchs = (from a in this.DbContext.V4_Batch
                              join b in this.DbContext.Pipeline on a.PipelineID equals b.ID
                              join S in this.DbContext.Shipper on a.CreatedBy equals S.UserId
                              join c in this.DbContext.metadataCycle on a.CycleId equals c.ID
                              where a.NomTypeID == (int)NomType.PNT   // only PNT batches
                              orderby a.CreatedDate
                              select new BatchDTO
                              {
                                  Id = a.TransactionID,
                                  Description = a.Description,
                                  PipelineId = a.PipelineID,
                                  DateBeg = a.FlowStartDate,
                                  DateEnd = a.FlowEndDate,
                                  CycleId = a.CycleId,
                                  StatusID = a.StatusID,
                                  SubmittedDate = DateTime.Now,
                                  ScheduledDate = DateTime.Now,
                                  ServiceRequester = a.ServiceRequester,
                                  ShowZeroes = a.ShowZeroCheck,
                                  RankingChecked = a.RankingCheck,
                                  PackageChecked = a.PakageCheck,
                                  UpDnContract = a.UpDnContractCheck,
                                  ShowZeroesUp = a.ShowZeroUp,
                                  ShowZeroesDn = a.ShowZeroDn,
                                  UpDnPakgID = a.UpDnPkgCheck,
                                  IsActive = a.IsActive,
                                  CreatedBy = a.CreatedBy,
                                  CreatedDate = a.CreatedDate,
                                  PipelineName = b.Name,
                                  Cycle = c.Name,
                                  CreaterName = S.FirstName + " " + S.LastName
                              });
                return batchs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<RejectedNomModel> GetAllRejectedNoms(int pipelineId, string userId)
        {
            var rejectedNoms = (from a in this.DbContext.NominationStatus
                                join b in this.DbContext.V4_Batch on a.NOM_ID equals b.TransactionID
                                join c in this.DbContext.Pipeline on b.PipelineID equals c.ID
                                //join r in this.DbContext.NMQRPerTransactions on a.NMQR_ID equals r.Transactionid
                                where ((a.StatusID == (int)NomStatus.Rejected || a.StatusID == (int)NomStatus.Error)
                                && c.ID == pipelineId
                                && b.CreatedBy == userId)
                                orderby a.CreatedDate
                                select new RejectedNomModel
                                {
                                    NMQR_ID = a.NMQR_ID,
                                    FlowDate = (DateTime)a.CreatedDate.Value,
                                    PipelineID = b.PipelineID,
                                    PipelineName = c.Name,
                                    RejectionReason = a.StatusDetail
                                });
            return rejectedNoms;
        }
        public int GetPathedListTotalCount(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {

            DateTime SDate = StartDate.Date;
            DateTime EDate = EndDate.Date;
            List<int> statusIds = new List<int>();
            switch (Status)
            {
                case 1:
                    statusIds = new List<int>() { 1, 2, 3, 4 };
                    break;
                case 5:
                    statusIds = new List<int>() { 5, 6 };
                    break;
                case 7:
                    statusIds = new List<int>() { 7 };
                    break;
                case 8:
                    statusIds = new List<int>() { 8, 9 };
                    break;
                case 10:
                    statusIds = new List<int>() { 10 };
                    break;
                case 11:
                    statusIds = new List<int>() { 11 };
                    break;
                case 0:
                    statusIds = new List<int>() { 0 };
                    break;
                default:
                    statusIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0 };
                    break;
            }

            if (loginUser == Guid.Empty)
            {
                return DbContext.V4_Batch.Where(a => a.PipelineID == PipelieID
                                                         && a.NomTypeID == (int)NomType.Pathed
                                                         && (DbFunctions.TruncateTime(a.FlowStartDate) >= SDate && DbFunctions.TruncateTime(a.FlowEndDate) <= EDate)
                                                         && a.IsActive
                                                         && a.ServiceRequester == shipperDuns
                                                         //&& a.CreatedBy == loginUser.ToString()
                                                         && (statusIds.Contains(a.StatusID))).Count();
            }
            else
            {
                return DbContext.V4_Batch.Where(a => a.PipelineID == PipelieID
                                                         && a.NomTypeID == (int)NomType.Pathed
                                                         && (DbFunctions.TruncateTime(a.FlowStartDate) >= SDate && DbFunctions.TruncateTime(a.FlowEndDate) <= EDate)
                                                         && a.IsActive
                                                         && a.ServiceRequester == shipperDuns
                                                         && a.CreatedBy == loginUser.ToString()
                                                         && (statusIds.Contains(a.StatusID))).Count();
            }

        }

        public IQueryable<PathedNomDetailsDTO> GetPathedListSortByColumnWithOrder(IQueryable<PathedNomDetailsDTO> dataQuery, SortingPagingInfo sortingPagingInfo)
        {
            string orderDir = sortingPagingInfo.SortDirection;
            switch (sortingPagingInfo.SortField)
            {
                //case "1":  // status                                                                                                             : dataQuery.OrderBy(p => p.StatusID).ToList();
                //    break;
                //case "2": // view sqts                                                                                                            : dataQuery.OrderBy(p => p.StatusID).ToList();
                //    break;
                //case "3": // TransactionType                                                                                                          : dataQuery.OrderBy(p => p.TransactionId).ToList();
                //    break;
                case "4": // StartDate
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.StartDate)
                                                                                                              : dataQuery.OrderBy(p => p.StartDate);
                    break;
                case "5": // EndDate
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.EndDate)
                                                                                                                   : dataQuery.OrderBy(p => p.EndDate);
                    break;
                case "6": // CreateDate
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.CreatedDate)
                                                                                                                   : dataQuery.OrderBy(p => p.CreatedDate);
                    break;
                case "7": // Cycle
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.CycleID)
                                                                                                                     : dataQuery.OrderBy(p => p.CycleID);
                    break;
                case "8": // Contract
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.Contract)
                                                                                                                  : dataQuery.OrderBy(p => p.Contract);
                    break;
                case "9": // Roll Nom
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.NomSubCycle)
                                                                                                                 : dataQuery.OrderBy(p => p.NomSubCycle);
                    break;
                //case "10": // Receipt Loc name
                //    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.RecLocation)
                //                                                                                                  : dataQuery.OrderBy(p => p.RecLocation);
                //    break;
                case "11": //Receipt Loc Prop
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.RecLocProp)
                                                                                                                   : dataQuery.OrderBy(p => p.RecLocProp);
                    break;
                case "12": // Receipt Loc Id
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.RecLocID)
                                                                                                                  : dataQuery.OrderBy(p => p.RecLocID);
                    break;
                //case "13": // Up stream Name
                //    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.UpName)
                //                                                                                                   : dataQuery.OrderBy(p => p.UpName);
                //    break;
                case "14": // Up stream Prop
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.UpIDProp)
                                                                                                                   : dataQuery.OrderBy(p => p.UpIDProp);
                    break;
                case "15": // Up Stream Id
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.UpID)
                                                                                                                  : dataQuery.OrderBy(p => p.UpID);
                    break;
                case "16": // Up stream Contract
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.UpKContract)
                                                                                                                   : dataQuery.OrderBy(p => p.UpKContract);
                    break;
                case "17": // Rec quantity
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.RecQty)
                                                                                                                   : dataQuery.OrderBy(p => p.RecQty);
                    break;
                case "18": // Rec Rank
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.RecRank)
                                                                                                                   : dataQuery.OrderBy(p => p.RecRank);
                    break;
                //case "19": // DelLoc
                //    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DelLoc)
                //                                                                                                   : dataQuery.OrderBy(p => p.DelLoc);
                //    break;
                case "20": // Del Loc Prop
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DelLocProp)
                                                                                                                  : dataQuery.OrderBy(p => p.DelLocProp);
                    break;
                case "21": // Del Loc Id
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DelLocID)
                                                                                                                   : dataQuery.OrderBy(p => p.DelLocID);
                    break;
                //case "22": // Down Stream name
                //    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DownName)
                //                                                                                                   : dataQuery.OrderBy(p => p.DownName);
                //    break;
                case "23": //Down Stream Prop
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DownIDProp)
                                                                                                                  : dataQuery.OrderBy(p => p.DownIDProp);
                    break;
                case "24": // Down stream Id
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DownID)
                                                                                                                   : dataQuery.OrderBy(p => p.DownID);
                    break;
                case "25": // Down stream Contract
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DownContract)
                                                                                                                   : dataQuery.OrderBy(p => p.DownContract);
                    break;
                case "26": // Del Quantity
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DelQuantity)
                                                                                                                   : dataQuery.OrderBy(p => p.DelQuantity);
                    break;
                case "27": // Del rank
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DelRank)
                                                                                                                  : dataQuery.OrderBy(p => p.DelRank);
                    break;
                case "28": //Pkg Id
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.PkgID)
                                                                                                                    : dataQuery.OrderBy(p => p.PkgID);
                    break;
                case "29": // Fuel%
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.FuelPercentage)
                                                                                                                   : dataQuery.OrderBy(p => p.FuelPercentage);
                    break;
                case "30": // createrName
                    dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.createrName)
                                                                                                                   : dataQuery.OrderBy(p => p.createrName);
                    break;
                default:
                    dataQuery = dataQuery.OrderByDescending(p => p.CreatedDate);
                    break;

            }

            return dataQuery;
        }

        public List<PathedNomDetailsDTO> GetPathedListWithPaging(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser, SortingPagingInfo sortingPagingInfo)
        {
            List<V4_Batch> batchList = new List<V4_Batch>();

            #region Get Batches Using Pipeline,status and dates.

            DateTime SDate = StartDate.Date;
            DateTime EDate = EndDate.Date;
            List<int> statusIds = new List<int>();
            switch (Status)
            {
                case 1:
                    statusIds = new List<int>() { 1, 2, 3, 4 };
                    break;
                case 5:
                    statusIds = new List<int>() { 5, 6 };
                    break;
                case 7:
                    statusIds = new List<int>() { 7 };
                    break;
                case 8:
                    statusIds = new List<int>() { 8, 9 };
                    break;
                case 10:
                    statusIds = new List<int>() { 10 };
                    break;
                case 11:
                    statusIds = new List<int>() { 11 };
                    break;
                case 0:
                    statusIds = new List<int>() { 0 };
                    break;
                default:
                    statusIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0 };
                    break;
            }
            IQueryable<PathedNomDetailsDTO> dataQuery;
            if (loginUser == Guid.Empty)
            {
                dataQuery = (from B in DbContext.V4_Batch
                             join N in DbContext.V4_Nomination on B.TransactionID equals N.TransactionID
                             join P in DbContext.Pipeline on B.PipelineID equals P.ID
                             join S in DbContext.Shipper on B.CreatedBy equals S.UserId
                             join SC in DbContext.ShipperCompany on B.ServiceRequester equals SC.DUNS
                             join RL in DbContext.Location on N.ReceiptLocationIdentifier equals RL.Identifier into ORL
                             from ai in ORL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()
                             join DL in DbContext.Location on N.DeliveryLocationIdentifer equals DL.Identifier into ODL
                             from bi in ODL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()

                             //join cpu in DbContext.CounterParty on N.UpstreamIdentifier equals cpu.Identifier into cpuOter
                             //from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                             //join cpd in DbContext.CounterParty on N.DownstreamIdentifier equals cpd.Identifier into cpdOter
                             //from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                             where B.PipelineID == PipelieID
                             && B.NomTypeID == (int)NomType.Pathed
                             && B.ServiceRequester == shipperDuns
                            && (DbFunctions.TruncateTime(B.FlowStartDate) >= SDate && DbFunctions.TruncateTime(B.FlowEndDate) <= EDate)
                            && B.IsActive
                            //&& B.CreatedBy == loginUser.ToString()
                            && (statusIds.Contains(B.StatusID))
                             select new PathedNomDetailsDTO()
                             {
                                 CreatedDate = B.CreatedDate,
                                 TransactionId = N.TransactionID,
                                 ActCode = N.AssociatedContract,
                                 AssocContract = N.AssociatedContract,
                                 BidTransportRate = N.BidTransportationRate,
                                 BidUp = N.BidupIndicator,
                                 CapacityType = N.CapacityTypeIndicator,
                                 Contract = N.ContractNumber,
                                 CompanyID = SC.ID,
                                 DealType = N.DealType,
                                 DelLoc =  bi.Name, //N.DeliveryLocationName,
                                 DelLocID = N.DeliveryLocationIdentifer,
                                 DelLocProp = N.DeliveryLocationPropCode,
                                 DelQuantity = N.DelQuantity.Value,
                                 DelRank = N.DeliveryRank,
                                 DownContract = N.DownstreamContractIdentifier,
                                 DownID = N.DownstreamIdentifier,  // conDwn != null && !string.IsNullOrEmpty(conDwn.Identifier) ? conDwn.Identifier : nom.DownstreamIdentifier;
                                 DownIDProp = N.DownstreamPropCode, // conDwn != null && !string.IsNullOrEmpty(conDwn.PropCode) ? conDwn.PropCode : nom.DownstreamPropCode;
                                 //DownName = di.Name, //N.DownstreamName,
                                 DownPkgID = N.DownstreamPackageId,
                                 DownRank = N.DownstreamRank,
                                 Export = N.ExportDecleration,
                                 MaxRate = N.MaxRateIndicator,
                                 NomSubCycle = B.NomSubCycle, // N.NominationSubCycleIndicator,
                                 NomTrackingId = N.NominatorTrackingId,
                                 NomUserData1 = N.NominationUserData1,
                                 NomUserData2 = N.NominationUserData2,
                                 PipelineID = P.ID,
                                 PkgID = N.PackageId,
                                 ProcessingRights = N.ProcessingRightIndicator,
                                 QuantityType = N.QuantityTypeIndicator,
                                 RecLocation = ai.Name,
                                 RecLocID = N.ReceiptLocationIdentifier,
                                 RecLocProp = N.receiptLocationPropCode,
                                 RecRank = N.ReceiptRank,
                                 RecQty = N.Quantity.ToString(),
                                 TransType = N.TransactionType,
                                 UpID = N.UpstreamIdentifier,
                                 UpIDProp = N.UpstreamPropCode,
                                 UpKContract = N.UpstreamContractIdentifier,
                                 //UpName = ci.Name,
                                 UpPkgID = N.UpstreamPackageId,
                                 UpRank = N.UpstreamRank,
                                 CycleName = "",
                                 TransTypeName = N.TransactionTypeDesc,
                                 //  pathtype = N.PathType,
                                 // NomTypeId = B.NomTypeID,
                                 StatusID = B.StatusID,
                                 // Status = UpdateBatchStatus(B.StatusID),
                                 CycleID = B.CycleId,
                                 StartDate = B.FlowStartDate,
                                 EndDate = B.FlowEndDate,
                                 ScheduledDateTime = B.ScheduleDate.HasValue ? B.ScheduleDate.Value : DateTime.MaxValue,
                                 ShipperDuns = B.ServiceRequester,
                                 ReferenceNo = B.ReferenceNumber,
                                 FuelPercentage = N.FuelPercentage.HasValue ? N.FuelPercentage.Value : 0,
                                 CreatedBy = B.CreatedBy,
                                 createrName = S.FirstName + " " + S.LastName
                             });
            }
            else
            {
                dataQuery = (from B in DbContext.V4_Batch
                             join N in DbContext.V4_Nomination on B.TransactionID equals N.TransactionID
                             join P in DbContext.Pipeline on B.PipelineID equals P.ID
                             join S in DbContext.Shipper on B.CreatedBy equals S.UserId
                             join SC in DbContext.ShipperCompany on B.ServiceRequester equals SC.DUNS
                             //join RL in DbContext.Location on N.ReceiptLocationIdentifier equals RL.Identifier into ORL
                             //from ai in ORL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()
                             //join DL in DbContext.Location on N.DeliveryLocationIdentifer equals DL.Identifier into ODL
                             //from bi in ODL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()

                             //join cpu in DbContext.CounterParty on N.UpstreamIdentifier equals cpu.Identifier into cpuOter
                             //from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                             //join cpd in DbContext.CounterParty on N.DownstreamIdentifier equals cpd.Identifier into cpdOter
                             //from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                             where B.PipelineID == PipelieID
                             && B.NomTypeID == (int)NomType.Pathed
                             && B.ServiceRequester == shipperDuns
                            && (DbFunctions.TruncateTime(B.FlowStartDate) >= SDate && DbFunctions.TruncateTime(B.FlowEndDate) <= EDate)
                            && B.IsActive
                            && B.CreatedBy == loginUser.ToString()
                            && (statusIds.Contains(B.StatusID))
                             select new PathedNomDetailsDTO()
                             {
                                 CreatedDate = B.CreatedDate,
                                 TransactionId = N.TransactionID,
                                 ActCode = N.AssociatedContract,
                                 AssocContract = N.AssociatedContract,
                                 BidTransportRate = N.BidTransportationRate,
                                 BidUp = N.BidupIndicator,
                                 CapacityType = N.CapacityTypeIndicator,
                                 Contract = N.ContractNumber,
                                 CompanyID = SC.ID,
                                 DealType = N.DealType,
                                 //DelLoc = bi.Name,
                                 DelLocID = N.DeliveryLocationIdentifer,
                                 DelLocProp = N.DeliveryLocationPropCode,
                                 DelQuantity = N.DelQuantity.Value,
                                 DelRank = N.DeliveryRank,
                                 DownContract = N.DownstreamContractIdentifier,
                                 DownID = N.DownstreamIdentifier,  // conDwn != null && !string.IsNullOrEmpty(conDwn.Identifier) ? conDwn.Identifier : nom.DownstreamIdentifier;
                                 DownIDProp = N.DownstreamPropCode, // conDwn != null && !string.IsNullOrEmpty(conDwn.PropCode) ? conDwn.PropCode : nom.DownstreamPropCode;
                                 //DownName = di.Name,
                                 DownPkgID = N.DownstreamPackageId,
                                 DownRank = N.DownstreamRank,
                                 Export = N.ExportDecleration,
                                 MaxRate = N.MaxRateIndicator,
                                 NomSubCycle = B.NomSubCycle, // N.NominationSubCycleIndicator,
                                 NomTrackingId = N.NominatorTrackingId,
                                 NomUserData1 = N.NominationUserData1,
                                 NomUserData2 = N.NominationUserData2,
                                 PipelineID = P.ID,
                                 PkgID = N.PackageId,
                                 ProcessingRights = N.ProcessingRightIndicator,
                                 QuantityType = N.QuantityTypeIndicator,
                                 //RecLocation = ai.Name,
                                 RecLocID = N.ReceiptLocationIdentifier,
                                 RecLocProp = N.receiptLocationPropCode,
                                 RecRank = N.ReceiptRank,
                                 RecQty = N.Quantity.ToString(),
                                 TransType = N.TransactionType,
                                 UpID = N.UpstreamIdentifier,
                                 UpIDProp = N.UpstreamPropCode,
                                 UpKContract = N.UpstreamContractIdentifier,
                                 //UpName = ci.Name,
                                 UpPkgID = N.UpstreamPackageId,
                                 UpRank = N.UpstreamRank,
                                 CycleName = "",
                                 TransTypeName = N.TransactionTypeDesc,
                                 //  pathtype = N.PathType,
                                 // NomTypeId = B.NomTypeID,
                                 StatusID = B.StatusID,
                                 // Status = UpdateBatchStatus(B.StatusID),
                                 CycleID = B.CycleId,
                                 StartDate = B.FlowStartDate,
                                 EndDate = B.FlowEndDate,
                                 ScheduledDateTime = B.ScheduleDate.HasValue ? B.ScheduleDate.Value : DateTime.MaxValue,
                                 ShipperDuns = B.ServiceRequester,
                                 ReferenceNo = B.ReferenceNumber,
                                 FuelPercentage = N.FuelPercentage.HasValue ? N.FuelPercentage.Value : 0,
                                 CreatedBy = B.CreatedBy,
                                 createrName = S.FirstName + " " + S.LastName
                             });
            }

            var dataQueryWithOrder = GetPathedListSortByColumnWithOrder(dataQuery, sortingPagingInfo);
            int skipVal = sortingPagingInfo.CurrentPageIndex * sortingPagingInfo.PageSize;
            int takeVal = sortingPagingInfo.PageSize;
            //var skipQuery = dataQueryWithOrder.Skip(sortingPagingInfo.CurrentPageIndex * sortingPagingInfo.PageSize);
           // var takeQuery = skipQuery.Take(sortingPagingInfo.PageSize).ToList();
            var resultData =dataQueryWithOrder.Skip(()=> skipVal).Take(()=>takeVal).ToList();


            #endregion          

            List<PathedNomDetailsDTO> pathedList = new List<PathedNomDetailsDTO>();

            #region Data Fill in DTO

            foreach (var obj in resultData)
            {
                obj.CanWrite = obj.StatusID == (int)NomStatus.Draft ? true : false;
                obj.Status = UpdateBatchStatus(obj.StatusID);
                // Nom Data
                var trantypeName = obj.TransTypeName;
                var TransType = obj.TransType;
                if (TransType != null)
                {
                    var ttmap = (from tt in this.DbContext.metadataTransactionType
                                 join ttm in this.DbContext.Pipeline_TransactionType_Map on tt.ID equals ttm.TransactionTypeID
                                 where ttm.PipelineID == obj.PipelineID
                                 && tt.Name == trantypeName && tt.Identifier == TransType
                                 select ttm).FirstOrDefault();
                    if (ttmap != null) { obj.TransTypeMapId = ttmap.ID; }
                }
                int recQty = string.IsNullOrEmpty(obj.RecQty) ? 0 : Convert.ToInt32(obj.RecQty);
                obj.DelQuantity = recQty - (((recQty * obj.FuelPercentage) / 100));
                pathedList.Add(obj);
            }

            #endregion

            return pathedList;
        }



        public NonPathedDTO GetNonPathedNominations(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {
            try
            {
                List<V4_Batch> batchList = new List<V4_Batch>();

                #region Get Batches Using Pipeline,status and dates.

                DateTime SDate = StartDate.Date;
                DateTime EDate = EndDate.Date;
                List<int> statusIds = new List<int>();
                switch (Status)
                {
                    case 1:
                        statusIds = new List<int>() { 1, 2, 3, 4 };
                        break;
                    case 5:
                        statusIds = new List<int>() { 5, 6 };
                        break;
                    case 7:
                        statusIds = new List<int>() { 7 };
                        break;
                    case 8:
                        statusIds = new List<int>() { 8, 9 };
                        break;
                    case 10:
                        statusIds = new List<int>() { 10 };
                        break;
                    case 11:
                        statusIds = new List<int>() { 11 };
                        break;
                    case 0:
                        statusIds = new List<int>() { 0 };
                        break;
                    default:
                        statusIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0 };
                        break;
                }

                var resultData = (from B in DbContext.V4_Batch
                                  join N in DbContext.V4_Nomination on B.TransactionID equals N.TransactionID
                                  join P in DbContext.Pipeline on B.PipelineID equals P.ID
                                  join SC in DbContext.ShipperCompany on B.ServiceRequester equals SC.DUNS
                                  //join RL in DbContext.Location on N.ReceiptLocationIdentifier equals RL.Identifier into ORL
                                  //from ai in ORL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()
                                  //join DL in DbContext.Location on N.DeliveryLocationIdentifer equals DL.Identifier into ODL
                                  //from bi in ODL.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()

                                  //join cpu in DbContext.CounterParty on N.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                  //from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                  //join cpd in DbContext.CounterParty on N.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                  //from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                  where B.PipelineID == PipelieID
                                  && B.NomTypeID == (int)NomType.NonPathed  // only for NonPathed batch
                                  && B.ServiceRequester == shipperDuns
                                 && (DbFunctions.TruncateTime(B.FlowStartDate) >= SDate && DbFunctions.TruncateTime(B.FlowEndDate) <= EDate)
                                 && B.IsActive
                                 && B.CreatedBy == loginUser.ToString()
                                 && (statusIds.Contains(B.StatusID))
                                  select new
                                  {
                                      CreatedBy = B.CreatedBy,
                                      CreatedDate = B.CreatedDate,
                                      transactionID = N.TransactionID,
                                      Contract = N.ContractNumber,
                                      CompanyID = SC.ID,
                                      DelLoc = N.DeliveryLocationName, // bi.Name,
                                      DelLocID = N.DeliveryLocationIdentifer,
                                      DelLocProp = N.DeliveryLocationPropCode,
                                      DelQuantity = N.DelQuantity,
                                      DelRank = N.DeliveryRank,
                                      DownContract = N.DownstreamContractIdentifier,
                                      DownID = N.DownstreamIdentifier,  // conDwn != null && !string.IsNullOrEmpty(conDwn.Identifier) ? conDwn.Identifier : nom.DownstreamIdentifier;
                                      DownIDProp = N.DownstreamPropCode, // conDwn != null && !string.IsNullOrEmpty(conDwn.PropCode) ? conDwn.PropCode : nom.DownstreamPropCode;
                                      DownName = N.DownstreamName, // di.Name,
                                      NomSubCycle = B.NomSubCycle,
                                      NomTrackingId = N.NominatorTrackingId,
                                      PipelineID = P.ID,
                                      PkgID = N.PackageId,
                                      QuantityType = N.QuantityTypeIndicator,
                                      RecLocation = N.ReceiptLocationName, // ai.Name,
                                      RecLocID = N.ReceiptLocationIdentifier,
                                      RecLocProp = N.receiptLocationPropCode,
                                      RecRank = N.ReceiptRank,
                                      RecQty = N.Quantity,
                                      TransType = N.TransactionType,
                                      UpID = N.UpstreamIdentifier,
                                      UpIDProp = N.UpstreamPropCode,
                                      UpKContract = N.UpstreamContractIdentifier,
                                      UpName = N.UpstreamName,  // ci.Name,
                                      CycleName = "",
                                      TransTypeName = N.TransactionTypeDesc,
                                      pathtype = N.PathType,
                                      NomTypeId = B.NomTypeID,
                                      StatusId = B.StatusID,
                                      CycleId = B.CycleId,
                                      FlowEndDate = B.FlowEndDate,
                                      FlowStartDate = B.FlowStartDate,
                                      ScheduleDate = B.ScheduleDate,
                                      ServiceRequester = B.ServiceRequester,
                                      ReferenceNumber = B.ReferenceNumber,
                                      StatusID = B.StatusID,
                                      RecPkgId = N.PackageId2,
                                      FuelPercentage = N.FuelPercentage != null ? N.FuelPercentage.Value : 0
                                  }
                ).ToList().OrderByDescending(a => a.CreatedDate);


                #endregion


                NonPathedDTO nonPathedDTO = new NonPathedDTO();
                List<NonPathedRecieptNom> ReceiptNoms = new List<NonPathedRecieptNom>();
                List<NonPathedDeliveryNom> DeliveryNoms = new List<NonPathedDeliveryNom>();

                #region Data For ReceiptNoms

                foreach (var item in resultData.Where(a => a.QuantityType == "R"))
                {
                    NonPathedRecieptNom recNom = new NonPathedRecieptNom() { };
                    recNom.CreateDateTime = item.CreatedDate;
                    recNom.CreatedBy = item.CreatedBy;
                    recNom.CycleId = item.CycleId;
                    recNom.Cycle = "";
                    recNom.EndDateTime = item.FlowEndDate;
                    recNom.FuelPercentage = item.FuelPercentage;
                    recNom.NomSubCycle = item.NomSubCycle;
                    recNom.NomTrackingId = item.NomTrackingId;
                    recNom.PackageId = item.PkgID;
                    recNom.PipelineId = item.PipelineID;
                    recNom.ReceiptLocId = item.RecLocID;
                    recNom.ReceiptLocName = item.RecLocation;
                    recNom.ReceiptLocProp = item.RecLocProp;
                    recNom.ReceiptQty = item.RecQty.Value;
                    recNom.ReceiptRank = item.RecRank;
                    recNom.ServiceRequesterContractCode = item.Contract;
                    recNom.ServiceRequesterContractName = "";
                    recNom.ShipperDuns = shipperDuns;
                    recNom.StartDateTime = item.FlowStartDate;
                    recNom.StatusId = item.StatusId;
                    recNom.Status = "";
                    recNom.TransactionId = item.transactionID;
                    recNom.TransactionType = item.TransType;
                    recNom.TransactionTypeDesc = item.TransTypeName;
                    recNom.UpstreamId = item.UpID;
                    recNom.UpstreamK = item.UpKContract;
                    recNom.UpstreamName = item.UpName;
                    recNom.UpstreamProp = item.UpIDProp;

                    ReceiptNoms.Add(recNom);
                }

                #endregion

                #region Data For DeliveryNoms

                foreach (var item in resultData.Where(a => a.QuantityType == "D"))
                {
                    NonPathedDeliveryNom delNom = new NonPathedDeliveryNom() { };

                    delNom.CreateDateTime = item.CreatedDate;
                    delNom.CreatedBy = item.CreatedBy;
                    delNom.CycleId = item.CycleId;
                    delNom.Cycle = "";
                    delNom.EndDateTime = item.FlowEndDate;
                    delNom.FuelPercentage = item.FuelPercentage;
                    delNom.NomSubCycle = item.NomSubCycle;
                    delNom.NomTrackingId = item.NomTrackingId;
                    delNom.PackageId = item.PkgID;
                    delNom.PipelineId = item.PipelineID;
                    delNom.DeliveryLocId = item.DelLocID;
                    delNom.DeliveryLocName = item.DelLoc;
                    delNom.DeliveryLocProp = item.DelLocProp;
                    delNom.DeliveryQty = item.DelQuantity.Value;
                    delNom.DeliveryRank = item.DelRank;
                    delNom.ServiceRequesterContractCode = item.Contract;
                    delNom.ServiceRequesterContractName = "";
                    delNom.ShipperDuns = shipperDuns;
                    delNom.StartDateTime = item.FlowStartDate;
                    delNom.StatusId = item.StatusId;
                    delNom.Status = "";
                    delNom.TransactionId = item.transactionID;
                    delNom.TransactionType = item.TransType;
                    delNom.TransactionTypeDesc = item.TransTypeName;
                    delNom.DnstreamId = item.DownID;
                    delNom.DnstreamK = item.DownContract;
                    delNom.DnstreamName = item.DownName;
                    delNom.DnstreamProp = item.DownIDProp;

                    DeliveryNoms.Add(delNom);
                }

                #endregion

                nonPathedDTO.ReceiptNoms = ReceiptNoms;
                nonPathedDTO.DeliveryNoms = DeliveryNoms;
                nonPathedDTO.PipelineId = PipelieID;
                nonPathedDTO.ShipperDuns = shipperDuns;
                nonPathedDTO.StartDate = StartDate;
                nonPathedDTO.EndDate = EndDate;
                nonPathedDTO.UserId = loginUser;

                return nonPathedDTO;
            }
            catch(Exception ex)
            {
                NonPathedDTO nonPathedDTO = new NonPathedDTO();
                List<NonPathedRecieptNom> ReceiptNoms = new List<NonPathedRecieptNom>();
                List<NonPathedDeliveryNom> DeliveryNoms = new List<NonPathedDeliveryNom>();
                return nonPathedDTO;
            }
            
        }
        public List<PathedNomDetailsDTO> GetPathedList(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser)
        {
            List<V4_Batch> batchList = new List<V4_Batch>();

            #region Get Batches Using Pipeline,status and dates.

            DateTime SDate = StartDate.Date;
            DateTime EDate = EndDate.Date;
            List<int> statusIds = new List<int>();
            switch (Status)
            {
                case 1:
                    statusIds = new List<int>() { 1, 2, 3, 4 };
                    break;
                case 5:
                    statusIds = new List<int>() { 5, 6 };
                    break;
                case 7:
                    statusIds = new List<int>() { 7 };
                    break;
                case 8:
                    statusIds = new List<int>() { 8, 9 };
                    break;
                case 10:
                    statusIds = new List<int>() { 10 };
                    break;
                case 11:
                    statusIds = new List<int>() { 11 };
                    break;
                default:
                    statusIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                    break;
            }

            var resultData = DbContext.V4_Batch.Where(a => a.PipelineID == PipelieID
                                                         && a.NomTypeID == 1
                                                         && (DbFunctions.TruncateTime(a.FlowStartDate) >= SDate && DbFunctions.TruncateTime(a.FlowEndDate) <= EDate)
                                                         && a.IsActive
                                                         && a.ServiceRequester == shipperDuns
                                                         && a.CreatedBy == loginUser.ToString()
                                                         && (statusIds.Contains(a.StatusID)))
                                                          .GroupJoin((from a in DbContext.V4_Nomination

                                                                      join b in this.DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                                                                      join p in this.DbContext.Pipeline on b.PipelineID equals p.ID
                                                                      join sc in this.DbContext.ShipperCompany on b.ServiceRequester equals sc.DUNS

                                                                      join rl in DbContext.Location on a.ReceiptLocationIdentifier equals rl.Identifier into rloter
                                                                      from ai in rloter.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()

                                                                      join dl in DbContext.Location on a.DeliveryLocationIdentifer equals dl.Identifier into dloter
                                                                      from bi in dloter.Where(a => a.Identifier != null && a.PipelineID == PipelieID).DefaultIfEmpty()

                                                                      join cpu in this.DbContext.CounterParty on a.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                                      from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                                      join cpd in this.DbContext.CounterParty on a.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                                      from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                                          //join sqts in this.DbContext.SQTSTrackOnNoms on a.NominatorTrackingId equals sqts.NomTrackingId into sqtsOter
                                                                          // from ei in sqtsOter.Where(a => a.NomTrackingId != null && a.NomTransactionID == batchId).DefaultIfEmpty()

                                                                      where a.ReceiptLocationIdentifier != null && a.DeliveryLocationIdentifer != null //&& a.TransactionID == batchId
                                                                      select new
                                                                      {
                                                                          //////
                                                                          transactionID = a.TransactionID,
                                                                          ActCode = a.AssociatedContract,
                                                                          AssocContract = a.AssociatedContract,
                                                                          BidTransportRate = a.BidTransportationRate,
                                                                          BidUp = a.BidupIndicator,
                                                                          CapacityType = a.CapacityTypeIndicator,
                                                                          Contract = a.ContractNumber,
                                                                          CompanyID = sc.ID,
                                                                          DealType = a.DealType,
                                                                          DelLoc = bi.Name,
                                                                          DelLocID = a.DeliveryLocationIdentifer,
                                                                          DelLocProp = a.DeliveryLocationPropCode,
                                                                          DelQuantity = a.DelQuantity,
                                                                          DelRank = a.DeliveryRank,
                                                                          DownContract = a.DownstreamContractIdentifier,
                                                                          DownID = a.DownstreamIdentifier,  // conDwn != null && !string.IsNullOrEmpty(conDwn.Identifier) ? conDwn.Identifier : nom.DownstreamIdentifier;
                                                                          DownIDProp = a.DownstreamPropCode, // conDwn != null && !string.IsNullOrEmpty(conDwn.PropCode) ? conDwn.PropCode : nom.DownstreamPropCode;
                                                                          DownName = di.Name,
                                                                          DownPkgID = a.DownstreamPackageId,
                                                                          DownRank = a.DownstreamRank,
                                                                          Export = a.ExportDecleration,
                                                                          MaxRate = a.MaxRateIndicator,
                                                                          NomSubCycle = a.NominationSubCycleIndicator,
                                                                          NomTrackingId = a.NominatorTrackingId,
                                                                          NomUserData1 = a.NominationUserData1,
                                                                          NomUserData2 = a.NominationUserData2,
                                                                          PipelineID = p.ID,
                                                                          PkgID = a.PackageId,
                                                                          ProcessingRights = a.ProcessingRightIndicator,
                                                                          QuantityType = a.QuantityTypeIndicator,
                                                                          RecLocation = ai.Name,
                                                                          RecLocID = a.ReceiptLocationIdentifier,
                                                                          RecLocProp = a.receiptLocationPropCode,
                                                                          RecRank = a.ReceiptRank,
                                                                          RecQty = a.Quantity,
                                                                          TransType = a.TransactionType,
                                                                          UpID = a.UpstreamIdentifier,
                                                                          UpIDProp = a.UpstreamPropCode,
                                                                          UpKContract = a.UpstreamContractIdentifier,
                                                                          UpName = ci.Name,
                                                                          UpPkgID = a.UpstreamPackageId,
                                                                          UpRank = a.UpstreamRank,
                                                                          CycleName = "",
                                                                          TransTypeName = "",
                                                                          //////
                                                                      }),
                                                                       batch => batch.TransactionID,
                                                                       nom => nom.transactionID,
                                                                       (batchtable, nomtable) => new
                                                                       {
                                                                           BatchTable = batchtable,
                                                                           NomTable = nomtable
                                                                       }
                                                                      );



            #endregion

            List<PathedNomDetailsDTO> pathedList = new List<PathedNomDetailsDTO>();

            #region Batch-Nom DTO Maping
            foreach (var obj in resultData)
            {
                // Batch data
                PathedNomDetailsDTO pathed = new PathedNomDetailsDTO();
                pathed.TransactionId = obj.BatchTable.TransactionID;
                pathed.CanWrite = obj.BatchTable.StatusID == (int)NomStatus.Draft ? true : false;
                pathed.PipelineID = obj.BatchTable.PipelineID;
                pathed.CycleID = obj.BatchTable.CycleId;
                pathed.EndDate = obj.BatchTable.FlowEndDate;
                pathed.ScheduledDateTime = obj.BatchTable.ScheduleDate.HasValue ? obj.BatchTable.ScheduleDate.Value : DateTime.MaxValue;
                pathed.ShipperDuns = obj.BatchTable.ServiceRequester;
                pathed.ReferenceNo = obj.BatchTable.ReferenceNumber;
                pathed.StartDate = obj.BatchTable.FlowStartDate;
                pathed.Status = UpdateBatchStatus(obj.BatchTable.StatusID);
                pathed.StatusID = obj.BatchTable.StatusID;
                pathed.CreatedDate = obj.BatchTable.CreatedDate;

                // NomData
                if (obj.NomTable.FirstOrDefault() != null)
                {
                    pathed.ActCode = obj.NomTable.FirstOrDefault().ActCode;
                    pathed.AssocContract = obj.NomTable.FirstOrDefault().AssocContract;
                    pathed.BidTransportRate = obj.NomTable.FirstOrDefault().BidTransportRate;
                    pathed.BidUp = obj.NomTable.FirstOrDefault().BidUp;
                    pathed.CapacityType = obj.NomTable.FirstOrDefault().CapacityType;
                    pathed.CompanyID = obj.NomTable.FirstOrDefault().CompanyID;
                    pathed.Contract = obj.NomTable.FirstOrDefault().Contract;
                    pathed.DealType = obj.NomTable.FirstOrDefault().DealType;
                    pathed.DelLoc = obj.NomTable.FirstOrDefault().DelLoc;
                    pathed.DelLocID = obj.NomTable.FirstOrDefault().DelLocID;
                    pathed.DelLocProp = obj.NomTable.FirstOrDefault().DelLocProp;
                    pathed.DelQuantity = obj.NomTable.FirstOrDefault().DelQuantity.GetValueOrDefault();
                    pathed.DelRank = obj.NomTable.FirstOrDefault().DelRank;
                    pathed.DownContract = obj.NomTable.FirstOrDefault().DownContract;
                    pathed.DownID = obj.NomTable.FirstOrDefault().DownID;
                    pathed.DownIDProp = obj.NomTable.FirstOrDefault().DownIDProp;
                    pathed.DownName = obj.NomTable.FirstOrDefault().DownName;
                    pathed.DownPkgID = obj.NomTable.FirstOrDefault().DownPkgID;
                    pathed.DownRank = obj.NomTable.FirstOrDefault().DownRank;
                    pathed.Export = obj.NomTable.FirstOrDefault().Export;
                    pathed.MaxRate = obj.NomTable.FirstOrDefault().MaxRate;
                    pathed.NomSubCycle = obj.NomTable.FirstOrDefault().NomSubCycle;
                    pathed.NomTrackingId = obj.NomTable.FirstOrDefault().NomTrackingId;
                    pathed.NomUserData1 = obj.NomTable.FirstOrDefault().NomUserData1;
                    pathed.NomUserData2 = obj.NomTable.FirstOrDefault().NomUserData2;
                    pathed.PipelineID = obj.NomTable.FirstOrDefault().PipelineID;
                    pathed.PkgID = obj.NomTable.FirstOrDefault().PkgID;
                    pathed.ProcessingRights = obj.NomTable.FirstOrDefault().ProcessingRights;
                    pathed.QuantityType = obj.NomTable.FirstOrDefault().QuantityType;
                    pathed.RecLocation = obj.NomTable.FirstOrDefault().RecLocation;
                    pathed.RecLocID = obj.NomTable.FirstOrDefault().RecLocID;
                    pathed.RecLocProp = obj.NomTable.FirstOrDefault().RecLocProp;
                    pathed.RecRank = obj.NomTable.FirstOrDefault().RecRank;
                    pathed.RecQty = obj.NomTable.FirstOrDefault().RecQty.GetValueOrDefault().ToString();
                    pathed.TransType = obj.NomTable.FirstOrDefault().TransType;
                    pathed.UpID = obj.NomTable.FirstOrDefault().UpID;
                    pathed.UpIDProp = obj.NomTable.FirstOrDefault().UpIDProp;
                    pathed.UpKContract = obj.NomTable.FirstOrDefault().UpKContract;
                    pathed.UpName = obj.NomTable.FirstOrDefault().UpName;
                    pathed.UpPkgID = obj.NomTable.FirstOrDefault().UpPkgID;
                    pathed.UpRank = obj.NomTable.FirstOrDefault().UpRank;
                    pathed.CycleName = "";
                    pathed.TransTypeName = "";
                    SQTSTrackOnNom sqts = GetSqtsResult(obj.NomTable.FirstOrDefault().NomTrackingId, obj.NomTable.FirstOrDefault().transactionID);
                    if (sqts != null)
                    {
                        pathed.DelPointQty = sqts.DeliveryPointQuantity;
                        pathed.RecPointQty = sqts.ReceiptPointQuantity;
                        pathed.ReductionReason = sqts.ReductionReason;
                    }
                }
                else
                {

                }
                pathedList.Add(pathed);

            }

            #endregion

            return pathedList;
        }
        private SQTSTrackOnNom GetSqtsResult(string NomTrackingId, Guid TransactionId)
        {
            return DbContext.SQTSTrackOnNoms.Where(a => a.NomTrackingId == NomTrackingId && a.NomTransactionID == TransactionId).FirstOrDefault();
        }
        public BatchDetailDTO GetNominationDetail(Guid batchId, int pipeId)
        {
            var Batch = new BatchDetailDTO();
            if (DbContext.V4_Nomination.Where(a => a.TransactionID == batchId).Count() > 0)
            {
                #region Pnt Db Call with Joins
                var data = (from a in DbContext.V4_Nomination

                            join b in this.DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                            join p in this.DbContext.Pipeline on b.PipelineID equals p.ID
                            join sc in this.DbContext.ShipperCompany on b.ServiceRequester equals sc.DUNS

                            //join rl in DbContext.Location on a.ReceiptLocationIdentifier equals rl.Identifier into rloter
                            //from ai in rloter.Where(a => a.Identifier != null && a.PipelineID == pipeId).DefaultIfEmpty()

                            //join dl in DbContext.Location on a.DeliveryLocationIdentifer equals dl.Identifier into dloter
                            //from bi in dloter.Where(a => a.Identifier != null && a.PipelineID == pipeId).DefaultIfEmpty()

                            //join cpu in this.DbContext.CounterParty on a.UpstreamIdentifier equals cpu.Identifier into cpuOter
                            //from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                            //join cpd in this.DbContext.CounterParty on a.DownstreamIdentifier equals cpd.Identifier into cpdOter
                            //from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                            join sqts in this.DbContext.SQTSTrackOnNoms on a.NominatorTrackingId equals sqts.NomTrackingId into sqtsOter
                            from ei in sqtsOter.Where(a => a.NomTrackingId != null && a.NomTransactionID == batchId).DefaultIfEmpty()
                            where a.TransactionID == batchId
                            select new
                            {
                                //transport
                                ID = a.ID,
                                pathType = a.PathType,
                                TransactionTypeDescription = a.TransactionTypeDesc,
                                TransactionType = a.TransactionType,
                                RecLocationProp = a.receiptLocationPropCode,
                                ServiceRequestNo = a.ContractNumber,
                                //RecLocationName = ai.Name,
                                RecLocation = a.ReceiptLocationIdentifier,
                                DelRank = a.DeliveryRank,
                                RecZone = "",
                                DelLocationProp = a.DeliveryLocationPropCode,
                                //DelLocationName =  bi.Name,
                                DelLocation = a.DeliveryLocationIdentifer,
                                RecRank = a.ReceiptRank,
                                DelZone = "",
                                ReceiptDth = a.Quantity.HasValue ? a.Quantity.Value.ToString() : "0",
                                FuelPercentage = a.FuelPercentage,
                                DeliveryDth = "0",
                                FuelDth = "0",
                                PackageID = a.PackageId,
                                Route = a.Route,
                                PathRank = a.PathRank,
                                //Batch
                                BatchStatus = b.StatusID,
                                CreatedBy = b.CreatedBy,
                                CreatedDateTime = b.CreatedDate,
                                CycleId = b.CycleId,
                                Description = b.Description,
                                Duns = p.DUNSNo,
                                EndDateTime = b.FlowEndDate,
                                Id = b.TransactionID,
                                PakageCheck = b.PakageCheck,
                                PipelineId = b.PipelineID,
                                PipeLineName = p.Name,
                                RankingCheck = b.RankingCheck,
                                ScheduleDate = b.ScheduleDate.HasValue ? b.ScheduleDate.Value : DateTime.MaxValue,
                                ShiperDuns = b.ServiceRequester,
                                ShiperName = sc.Name,
                                ShowZeroCheck = b.ShowZeroCheck,
                                ShowZeroDn = b.ShowZeroDn,
                                ShowZeroUp = b.ShowZeroUp,
                                StartDateTime = b.FlowStartDate,
                                StatusId = b.StatusID,
                                SubmittedDate = b.SubmitDate.HasValue ? b.SubmitDate.Value : DateTime.MaxValue,
                                UpDnContractCheck = b.UpDnContractCheck,
                                UpDnPkgCheck = b.UpDnPkgCheck,
                                BatchNomType = b.NomTypeID.Value,
                                NomSubCycle = b.NomSubCycle,
                                //Supply
                                BatchID = batchId,
                                LocationProp = a.receiptLocationPropCode,
                                Location = a.ReceiptLocationIdentifier,
                                //LocationName =ai.Name,
                                ServiceRequestType = "",
                                UpstreamIDProp = a.UpstreamPropCode,
                                //UpstreamIDName =ci.Name,
                                UpstreamID = a.UpstreamIdentifier,
                                DefaultInd = "",
                                ReceiptQuantityGross = a.Quantity.HasValue ? a.Quantity.Value : 0,
                                DeliveryQuantityNet = 0,
                                FuelQunatity = "",
                                UpstreamRank = a.UpstreamRank,
                                UpContractIdentifier = a.UpstreamContractIdentifier,
                                UpPackageID = a.UpstreamPackageId,
                                IsActive = true,
                                DelPointQty = ei.DeliveryPointQuantity,
                                RecPointQty = ei.ReceiptPointQuantity,
                                ReductionReason = ei.ReductionReason,
                                //Market                               
                                MLocationProp = a.DeliveryLocationPropCode,
                                MLocation =a.DeliveryLocationIdentifer,
                                //MLocationName =  bi.Name,
                                DownstreamIDProp =  a.DownstreamPropCode,
                                //DownstreamIDName =di.Name,
                                DownstreamID = a.DownstreamIdentifier,
                                DnstreamRank = a.DownstreamRank,
                                DnContractIdentifier = a.DownstreamContractIdentifier,
                                DnPackageID = a.DownstreamPackageId,
                                //contract path                              
                                MaxDeliveredQuantity = "",
                                NominatedQuantity = "",
                                OverrunQuantity = "",
                                transactionId = a.TransactionID,
                                nominatortrackId = a.NominatorTrackingId
                            }).ToList();
                #endregion



                #region Batch                

                if (data != null && data.Count > 0)
                {
                    Batch.BatchStatus = UpdateBatchStatus(data.FirstOrDefault().BatchStatus);
                    Batch.CreatedBy = data.FirstOrDefault().CreatedBy;
                    Batch.CreatedDateTime = data.FirstOrDefault().CreatedDateTime;
                    Batch.CycleId = data.FirstOrDefault().CycleId;
                    Batch.Description = data.FirstOrDefault().Description;
                    Batch.Duns = data.FirstOrDefault().Duns;
                    Batch.EndDateTime = data.FirstOrDefault().EndDateTime;
                    Batch.Id = data.FirstOrDefault().Id;
                    Batch.PakageCheck = data.FirstOrDefault().PakageCheck;
                    Batch.PipelineId = data.FirstOrDefault().PipelineId;
                    Batch.PipeLineName = data.FirstOrDefault().PipeLineName;
                    Batch.RankingCheck = data.FirstOrDefault().RankingCheck;
                    Batch.ScheduleDate = data.FirstOrDefault().ScheduleDate;
                    Batch.ShiperDuns = data.FirstOrDefault().ShiperDuns;
                    Batch.ShiperName = data.FirstOrDefault().ShiperName;
                    Batch.ShowZeroCheck = data.FirstOrDefault().ShowZeroCheck;
                    Batch.ShowZeroDn = data.FirstOrDefault().ShowZeroDn;
                    Batch.ShowZeroUp = data.FirstOrDefault().ShowZeroUp;
                    Batch.StartDateTime = data.FirstOrDefault().StartDateTime;
                    Batch.StatusId = data.FirstOrDefault().StatusId;
                    Batch.SubmittedDate = data.FirstOrDefault().SubmittedDate;
                    Batch.UpDnContractCheck = data.FirstOrDefault().UpDnContractCheck;
                    Batch.UpDnPkgCheck = data.FirstOrDefault().UpDnPkgCheck;
                    Batch.BatchNomType = data.FirstOrDefault().BatchNomType;
                    Batch.NomSubCycle = data.FirstOrDefault().NomSubCycle;

                }
                #endregion

                #region Transport
                foreach (var item in data.Where(a => a.pathType == "T"))
                {
                    BatchDetailContractDTO model = new BatchDetailContractDTO();
                    model.ID = item.ID;
                    model.TransactionTypeDescription = item.TransactionTypeDescription;
                    model.TransactionType = item.TransactionType;
                    model.RecLocationProp = item.RecLocationProp;
                    model.ServiceRequestNo = item.ServiceRequestNo;
                    model.RecLocationName = "";
                    model.RecLocation = item.RecLocation;
                    model.DelRank = item.DelRank;
                    model.RecZone = "";
                    model.DelLocationProp = item.DelLocationProp;
                    model.DelLocationName = "";
                    model.DelLocation = item.DelLocation;
                    model.RecRank = item.RecRank;
                    model.DelZone = "";
                    model.ReceiptDth = item.ReceiptDth;
                    model.FuelPercentage = item.FuelPercentage + "";
                    model.DeliveryDth = (Math.Round(Convert.ToInt32(item.ReceiptDth) * (100 - Convert.ToDecimal(item.FuelPercentage)) / 100)).ToString();
                    decimal deliveryQtyNt = (Math.Round(Convert.ToInt32(item.ReceiptQuantityGross) * (100 - Convert.ToDecimal(item.FuelPercentage)) / 100));
                    model.FuelDth = (Math.Round(deliveryQtyNt * (100 - Convert.ToDecimal(item.FuelPercentage)) / 100)).ToString();
                    model.PackageID = item.PackageID;
                    model.Route = item.Route;
                    model.PathRank = item.PathRank;
                    model.DelPointQty = item.DelPointQty;
                    model.RecPointQty = item.RecPointQty;
                    model.ReductionReason = item.ReductionReason;

                    model.TransactionId = item.transactionId.ToString();
                    model.NominatorTrackingId = item.nominatortrackId;

                    Batch.Contract.Add(model);
                }
                #endregion

                #region Supply
                foreach (var item in data.Where(a => a.pathType == "S"))
                {
                    BatchDetailSupplyDTO model = new BatchDetailSupplyDTO();
                    model.BatchID = batchId;
                    model.LocationProp = item.LocationProp;
                    model.Location = item.Location;
                    model.LocationName = "";
                    model.TransactionType = item.TransactionType;
                    model.TransactionTypeDescription = item.TransactionTypeDescription;


                    if (item.TransactionType != null)
                    {
                        var ttmap = (from tt in this.DbContext.metadataTransactionType
                                     join ttm in this.DbContext.Pipeline_TransactionType_Map on tt.ID equals ttm.TransactionTypeID
                                     where ttm.PipelineID == Batch.PipelineId
                                     && tt.Name == item.TransactionTypeDescription && tt.Identifier == item.TransactionType
                                     select ttm).FirstOrDefault();
                        if (ttmap != null) { model.TransTypeMapId = ttmap.ID; model.PathType = ttmap.PathType; }
                    }

                    model.ServiceRequestNo = item.ServiceRequestNo;
                    model.ServiceRequestType = "";
                    model.UpstreamIDProp = item.UpstreamIDProp;
                    model.UpstreamIDName = "";
                    model.UpstreamID = item.UpstreamID;
                    model.DefaultInd = "";
                    model.ReceiptQuantityGross = item.ReceiptQuantityGross;
                    model.FuelPercentage = item.FuelPercentage.ToString();
                    decimal deliveryQtyNt = (Math.Round(Convert.ToInt32(item.ReceiptQuantityGross) * (100 - Convert.ToDecimal(item.FuelPercentage)) / 100));
                    model.DeliveryQuantityNet = Convert.ToInt32(deliveryQtyNt);
                    model.FuelQunatity = (Convert.ToDecimal(item.ReceiptQuantityGross) - Convert.ToDecimal(deliveryQtyNt)).ToString();
                    model.UpstreamRank = item.UpstreamRank;
                    model.PackageID = item.PackageID;
                    model.UpContractIdentifier = item.UpContractIdentifier;
                    model.UpPackageID = item.UpPackageID;
                    model.IsActive = true;
                    model.DelPointQty = item.DelPointQty;
                    model.RecPointQty = item.RecPointQty;
                    model.ReductionReason = item.ReductionReason;
                    model.TransactionId = item.transactionId.ToString();
                    model.NominatorTrackingId = item.nominatortrackId;

                    Batch.SupplyList.Add(model);
                }
                #endregion

                #region Market
                foreach (var item in data.Where(a => a.pathType == "M"))
                {
                    BatchDetailMarketDTO model = new BatchDetailMarketDTO();
                    model.BatchID = item.BatchID;
                    model.LocationProp = item.MLocationProp;
                    model.Location = item.MLocation;
                    model.LocationName = "";
                    model.TransactionType = item.TransactionType;
                    model.TransactionTypeDescription = item.TransactionTypeDescription;

                    if (item.TransactionType != null)
                    {
                        var ttmap = (from tt in this.DbContext.metadataTransactionType
                                     join ttm in this.DbContext.Pipeline_TransactionType_Map on tt.ID equals ttm.TransactionTypeID
                                     where ttm.PipelineID == Batch.PipelineId
                                     && tt.Name == item.TransactionTypeDescription && tt.Identifier == item.TransactionType
                                     select ttm).FirstOrDefault();
                        if (ttmap != null) { model.TransTypeMapId = ttmap.ID; model.PathType = ttmap.PathType; }
                    }

                    model.ServiceRequestNo = item.ServiceRequestNo;
                    model.ServiceRequestType = "";
                    model.DownstreamIDProp = item.DownstreamIDProp;
                    model.DownstreamIDName = "";
                    model.DownstreamID = item.DownstreamID;
                    model.DefaultInd = "";
                    model.ReceiptQuantityGross = item.ReceiptQuantityGross;
                    model.FuelPercentage = item.FuelPercentage.ToString();
                    decimal deliveryQtyNt = (Math.Round(Convert.ToInt32(item.ReceiptQuantityGross) * (100 - Convert.ToDecimal(item.FuelPercentage)) / 100));
                    model.DeliveryQuantityNet = Convert.ToInt32(deliveryQtyNt);
                    model.FuelQunatity = (Convert.ToDecimal(item.ReceiptQuantityGross) - Convert.ToDecimal(deliveryQtyNt)).ToString();
                    model.DnstreamRank = item.DnstreamRank;
                    model.PackageID = item.PackageID;
                    model.DnContractIdentifier = item.DnContractIdentifier;
                    model.DnPackageID = item.DnPackageID;
                    model.IsActive = true;
                    model.DelPointQty = item.DelPointQty;
                    model.RecPointQty = item.RecPointQty;
                    model.ReductionReason = item.ReductionReason;
                    model.TransactionId = item.transactionId.ToString();
                    model.NominatorTrackingId = item.nominatortrackId;

                    Batch.MarketList.Add(model);
                }
                #endregion

                #region ContractPath
                var ContractPath = (from nCP in data
                                    where nCP.pathType == "T"
                                    group nCP by nCP.ServiceRequestNo into g
                                    select new BatchDetailContractPathDTO
                                    {
                                        ID = g.FirstOrDefault().ID,
                                        ServiceRequestNo = g.Key,
                                        ServiceRequestType = "",
                                        FuelPercentage = g.FirstOrDefault().FuelPercentage + "",
                                        DefaultInd = "",
                                        MaxDeliveredQuantity = "",
                                        NominatedQuantity = "",
                                        OverrunQuantity = "",
                                        BatchID = g.FirstOrDefault().BatchID
                                    }).ToList();
                Batch.ContractPath = ContractPath;
                #endregion

                #region Footer Total
                Batch.marketRecTotal = Batch.MarketList.Sum(a => a.ReceiptQuantityGross);
                Batch.marketDelTotal = Batch.MarketList.Sum(a => a.DeliveryQuantityNet);
                Batch.supplyRecTotal = Batch.SupplyList.Sum(a => a.ReceiptQuantityGross);
                Batch.supplyDelTotal = Batch.SupplyList.Sum(a => a.DeliveryQuantityNet);
                #endregion
            }
            else
            {
                Batch = (from sNom in DbContext.V4_Batch
                         join p in this.DbContext.Pipeline on sNom.PipelineID equals p.ID
                         join sc in this.DbContext.ShipperCompany on sNom.ServiceRequester equals sc.DUNS
                         where sNom.TransactionID == batchId
                         select new BatchDetailDTO
                         {
                             BatchStatus = sNom.StatusID + "",
                             CreatedBy = sNom.CreatedBy,
                             CreatedDateTime = sNom.CreatedDate,
                             CycleId = sNom.CycleId,
                             Description = sNom.Description,
                             Duns = p.DUNSNo,
                             EndDateTime = sNom.FlowEndDate,
                             Id = sNom.TransactionID,
                             PakageCheck = sNom.PakageCheck,
                             PipelineId = sNom.PipelineID,
                             PipeLineName = p.Name,
                             RankingCheck = sNom.RankingCheck,
                             ScheduleDate = sNom.ScheduleDate.Value,
                             ShiperDuns = sNom.ServiceRequester,
                             ShiperName = sc.Name,
                             ShowZeroCheck = sNom.ShowZeroCheck,
                             ShowZeroDn = sNom.ShowZeroDn,
                             ShowZeroUp = sNom.ShowZeroUp,
                             StartDateTime = sNom.FlowStartDate,
                             StatusId = sNom.StatusID,
                             SubmittedDate = sNom.SubmitDate.Value,
                             UpDnContractCheck = sNom.UpDnContractCheck,
                             UpDnPkgCheck = sNom.UpDnPkgCheck,
                             BatchNomType = sNom.NomTypeID.Value,
                             NomSubCycle = sNom.NomSubCycle
                         }).FirstOrDefault();

                Batch.BatchStatus = UpdateBatchStatus(Batch.StatusId);
            }


            return Batch;
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
            else if (StatusId == Convert.ToInt32(NomStatus.Replaced))
            {
                return "Replaced";
            }
            else
                return string.Empty;
        }
        public bool AddBulkNoms(List<V4_Nomination> noms)
        {
            this.DbContext.V4_Nomination.AddRange(noms);
            this.DbContext.SaveChanges();
            return true;
        }
        public bool deleteAll(List<V4_Nomination> noms)
        {
            DbContext.V4_Nomination.RemoveRange(noms);
            DbContext.SaveChanges();
            return true;
        }
        public List<V4_Nomination> GetAllNomsByTransactionId(Guid transactionId)
        {
            return (from a in DbContext.V4_Nomination where a.TransactionID == transactionId select a).ToList();
        }
        public NomType GetBatchNomType(Guid transactionId)
        {
            try
            {
                NomType nomtype = NomType.PNT; // defaultvalue
                var nomtypeId = DbContext.V4_Batch.Where(a => a.TransactionID == transactionId).FirstOrDefault().NomTypeID;
                if (nomtypeId == (int)NomType.Pathed) { nomtype = NomType.Pathed; }
                else if (nomtypeId == (int)NomType.PNT) { nomtype = NomType.PNT; }
                else if (nomtypeId == (int)NomType.NonPathed) { nomtype = NomType.NonPathed; }
                else if (nomtypeId == (int)NomType.HyNonPathedPNT) { nomtype = NomType.HyNonPathedPNT; }
                else if (nomtypeId == (int)NomType.HyPathedNonPathed) { nomtype = NomType.HyPathedNonPathed; }
                else if (nomtypeId == (int)NomType.HyPathedPNT) { nomtype = NomType.HyPathedPNT; }
                return nomtype;
            }
            catch (Exception ex)
            {
                throw new Exception("Batch not found/some Error.");
            }
        }
        public dynamic NomOnSqtsMatchedFields(string ServiceRequestorContract
            , string ModelType
            , string TransactionType
            , string ReceiptLocation
            , string UpstreamContractIdentifier
            , string DeliveryLocation
            , string DownstreamContractIdentifier
            , string UpIden
            , string DwnIden)
        {
            dynamic data = null;
            if (ModelType == "U")
            {
                if (!string.IsNullOrEmpty(ReceiptLocation) && !string.IsNullOrEmpty(UpIden) && string.IsNullOrEmpty(DeliveryLocation) && string.IsNullOrEmpty(DwnIden))
                {
                    data = (from a in DbContext.V4_Nomination
                            join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                            orderby b.CreatedDate descending
                            where a.ContractNumber == ServiceRequestorContract
                            && (a.PathType == "S")
                            && a.TransactionType == TransactionType
                            && a.ReceiptLocationIdentifier == ReceiptLocation
                            && a.UpstreamIdentifier == UpIden
                            && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                            select a).FirstOrDefault();
                }
                else if (string.IsNullOrEmpty(ReceiptLocation) && string.IsNullOrEmpty(UpIden) && !string.IsNullOrEmpty(DeliveryLocation) && !string.IsNullOrEmpty(DwnIden))
                {
                    data = (from a in DbContext.V4_Nomination
                            join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                            orderby b.CreatedDate descending
                            where a.ContractNumber == ServiceRequestorContract
                            && (a.PathType == "M")
                            && a.TransactionType == TransactionType
                            && a.DeliveryLocationIdentifer == DeliveryLocation
                            && a.DownstreamIdentifier == DwnIden
                            && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                            select a).FirstOrDefault();
                }
            }
            else if (ModelType == "T" && !string.IsNullOrEmpty(ReceiptLocation) && !string.IsNullOrEmpty(DeliveryLocation) && string.IsNullOrEmpty(UpIden) && string.IsNullOrEmpty(DwnIden))
            {
                data = (from a in DbContext.V4_Nomination
                        join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                        orderby b.CreatedDate descending
                        where a.ContractNumber == ServiceRequestorContract
                        && (a.PathType == "T")
                        && a.TransactionType == TransactionType
                        && a.DeliveryLocationIdentifer == DeliveryLocation
                        && a.ReceiptLocationIdentifier == ReceiptLocation
                        && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                        select a).FirstOrDefault();
            }
            else if (ModelType == "N")
            {
                if (string.IsNullOrEmpty(ReceiptLocation) && !string.IsNullOrEmpty(DeliveryLocation) && string.IsNullOrEmpty(UpIden) && !string.IsNullOrEmpty(DwnIden))
                {
                    data = (from a in DbContext.V4_Nomination
                            join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                            orderby b.CreatedDate descending
                            where a.ContractNumber == ServiceRequestorContract
                            && (a.PathType == "NPD")
                            && a.TransactionType == TransactionType
                            && a.DeliveryLocationIdentifer == DeliveryLocation
                            && a.DownstreamIdentifier == DwnIden
                            && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                            select a).FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(ReceiptLocation) && string.IsNullOrEmpty(DeliveryLocation) && !string.IsNullOrEmpty(UpIden) && string.IsNullOrEmpty(DwnIden))
                {
                    data = (from a in DbContext.V4_Nomination
                            join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                            orderby b.CreatedDate descending
                            where a.ContractNumber == ServiceRequestorContract
                            && (a.PathType == "NPR")
                            && a.TransactionType == TransactionType
                            && a.ReceiptLocationIdentifier == ReceiptLocation
                            && a.UpstreamIdentifier == UpIden
                            && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                            select a).FirstOrDefault();
                }
            }
            else if (ModelType == "P" && !string.IsNullOrEmpty(ReceiptLocation) && !string.IsNullOrEmpty(DeliveryLocation) && !string.IsNullOrEmpty(UpIden) && !string.IsNullOrEmpty(DwnIden))
            {
                data = (from a in DbContext.V4_Nomination
                        join b in DbContext.V4_Batch on a.TransactionID equals b.TransactionID
                        orderby b.CreatedDate descending
                        where a.ContractNumber == ServiceRequestorContract
                       && (a.PathType == ModelType)
                       && a.TransactionType == TransactionType
                       && a.ReceiptLocationIdentifier == ReceiptLocation
                       && a.UpstreamIdentifier == UpIden
                       && a.DeliveryLocationIdentifer == DeliveryLocation
                       && a.DownstreamIdentifier == DwnIden
                       && (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                        select a).FirstOrDefault();
            }
            return data;
        }

        /// <summary>
        /// Pathed nominations
        /// </summary>
        /// <param name="month"></param>
        /// <param name="pipeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>


        //public List<SummaryDTO> GetAcceptedNomsOnDate(int month, int pipeId, string UserId, bool ByLoc)
        //{
        //    var sumaryNomNew = new List<SummaryDTO>();
        //    int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
        //    DateTime dtFrom = new DateTime(DateTime.Now.Year, month, 1);
        //    List<DateTime> dates = Enumerable.Range(0, daysInMonth)
        //     .Select(offset => dtFrom.AddDays(offset))
        //     .ToList();

        // sumaryNomNew = (from n in DbContext.V4_Nomination
        //     join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
        //     where (b.FlowStartDate.Month == 2)
        //               && n.ReceiptLocationIdentifier != ""
        //               && n.DeliveryLocationIdentifer != ""
        //     //&& n.NominatorTrackingId == "QM0NN6R4E"
        //     group new { n, b }
        //     by new
        //     { n.NominatorTrackingId, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, b.CycleId, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
        //                into z
        //     let ActualDates = (from d in dates
        //                        where d >= z.Key.FlowStartDate && d <= z.Key.FlowEndDate
        //                        select new SummaryValues
        //                        {
        //                            Qty = z.Key.Quantity.Value,
        //                            date = d
        //                        }).ToList()
        //     let EmptyDates = (from d in dates
        //                       where (!ActualDates.Select(a => a.date).Contains(d))
        //                       select new SummaryValues
        //                       {
        //                           Qty = 0,
        //                           date = d
        //                       }).ToList()

        //     let UnionOfTwo = EmptyDates.Union(ActualDates).OrderBy(a => a.date)
        //     select new SummaryDTO
        //     {
        //         nomStartDate = z.Key.FlowStartDate,
        //         nomEndDate = z.Key.FlowEndDate,
        //         RecLoc = z.Key.ReceiptLocationIdentifier,
        //         DelLoc = z.Key.DeliveryLocationIdentifer,
        //         SubmittedDate = z.Key.SubmitDate.Value,
        //         Cycle = z.Key.CycleId.ToString(),
        //         Qty = z.Key.Quantity.Value,
        //         NomTrackingId = z.Key.NominatorTrackingId,
        //         UpIdentifier = z.Key.UpstreamIdentifier,
        //         DwnIdentifier = z.Key.DownstreamIdentifier,
        //         AllDates = UnionOfTwo
        //     }).ToList();

        //    return sumaryNomNew;

        //}



        public List<SummaryDTO> GetAcceptedNomsOnDate(int month, int pipeId, string UserId,string shipperCompanyDuns, bool ByLoc,bool showZero)
        {
            var sumaryNomNew = new List<SummaryDTO>();
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
            DateTime dtFrom = new DateTime(DateTime.Now.Year, month, 1);
            List<DateTime> dates = Enumerable.Range(0, daysInMonth)
             .Select(offset => dtFrom.AddDays(offset))
             .ToList();

            try
            {
                if (ByLoc)
                {
                    if (!showZero)
                    {
                        if (UserId != string.Empty)
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();
                        }
                        else if (string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();
                        }
                    }
                    else
                    {
                        if (UserId != string.Empty)
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            && n.Quantity!=0
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();
                        }
                        else if (string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            && n.Quantity!=0
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();
                        }
                    }
                }
                else
                {
                    if (!showZero)
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                        else if (string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            && n.Quantity!=0
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                        else if (string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "P"
                                            && n.Quantity!=0
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sumaryNomNew;
        }

        /// <summary>
        /// Pnt/NonPathed nominations
        /// </summary>
        /// <param name="month"></param>
        /// <param name="PathType"></param>
        /// <param name="pipeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// 

        public List<SummaryDTO> GetAcceptedNomOnDate(int month, string PathType, int pipeId, string UserId,string shipperCompanyDuns, bool ByLoc,bool showZero)
        {
            var sumaryNomNew = new List<SummaryDTO>();

            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
            DateTime dtFrom = new DateTime(DateTime.Now.Year, month, 1);
            List<DateTime> dates = Enumerable.Range(0, daysInMonth)
             .Select(offset => dtFrom.AddDays(offset))
             .ToList();

            try
            {
                if (!showZero)
                {
                    if (PathType == "R")
                    {
                        if (ByLoc)
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z

                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                  ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z

                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                  ).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && n.ContractNumber != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, n.ContractNumber, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && n.ContractNumber != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, n.ContractNumber, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }

                        }

                    }
                    else if (PathType == "D")
                    {
                        if (ByLoc)
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                                where (b.FlowStartDate.Month == month)
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    // RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }


                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.ContractNumber != ""
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.ContractNumber, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                      into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.ContractNumber != ""
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.ContractNumber, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                      into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                ).ToList();
                            }
                        }
                    }
                }
                else
                {
                    if (PathType == "R")
                    {

                        if (ByLoc)
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z

                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                    
                                                }
                                  ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity != 0
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z

                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                  ).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && n.ContractNumber != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, n.ContractNumber, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                                join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                                from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()
                                                
                                                where (b.FlowStartDate.Month == month)
                                                && n.ReceiptLocationIdentifier != ""
                                                && n.ContractNumber != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "S" || n.PathType == "NPR")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, upname = ci.Name, n.ReceiptLocationIdentifier, n.ContractNumber, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    RecLoc = z.Key.ReceiptLocationIdentifier,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = z.Key.upname,
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = "",
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }

                        }
                    }
                    else if (PathType == "D")
                    {
                        if (ByLoc)
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                            into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                 ).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.ContractNumber != ""
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.CreatedBy == UserId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.ContractNumber, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                      into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                ).ToList();
                            }
                            else if (string.IsNullOrEmpty(UserId))
                            {
                                sumaryNomNew = (from n in DbContext.V4_Nomination
                                                join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                                join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                                join s in DbContext.Shipper on b.CreatedBy equals s.UserId
                                                
                                                join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                                from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                                where (b.FlowStartDate.Month == month)
                                                && n.ContractNumber != ""
                                                && n.DeliveryLocationIdentifer != ""
                                                && b.PipelineID == pipeId
                                                && b.ServiceRequester == shipperCompanyDuns
                                                && b.CreatedDate.Year == DateTime.Now.Year
                                                && b.StatusID == 7
                                                && n.Quantity!=0
                                                && (n.PathType == "M" || n.PathType == "NPD")
                                                group new { n, b, s }
                                                by new
                                                { n.NominatorTrackingId, downName = di.Name, n.ContractNumber, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                      into z
                                                select new SummaryDTO
                                                {
                                                    nomEndDate = z.Key.FlowEndDate,
                                                    nomStartDate = z.Key.FlowStartDate,
                                                    ContractSVC = z.Key.ContractNumber,
                                                    DelLoc = z.Key.DeliveryLocationIdentifer,
                                                    Cycle = z.Key.Code.ToString(),
                                                    UpStreamName = "",
                                                    UpIdentifier = z.Key.UpstreamIdentifier,
                                                    DownStreamName = z.Key.downName,
                                                    DwnIdentifier = z.Key.DownstreamIdentifier,
                                                    NomTrackingId = z.Key.NominatorTrackingId,
                                                    PkgId = z.FirstOrDefault().n.PackageId,
                                                    SubmittedDate = z.Key.SubmitDate.Value,
                                                    Qty = z.Key.Quantity,
                                                    Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                                }
                                ).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sumaryNomNew;
        }

        public bool IsContractExist(string contractNo)
        {
            return DbContext.V4_Nomination.Any(a => a.ContractNumber == contractNo);
        }

        public List<SummaryDTO> GetNomQtyForContractPath(int month, int pipeId, string UserId,string shipperCompanyDuns, bool ByLoc, bool showZero)
        {
            var sumaryNomNew = new List<SummaryDTO>();
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
            DateTime dtFrom = new DateTime(DateTime.Now.Year, month, 1);
            List<DateTime> dates = Enumerable.Range(0, daysInMonth)
             .Select(offset => dtFrom.AddDays(offset))
             .ToList();
            try
            {
                if (!showZero)
                {
                    if (ByLoc)
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();

                        }
                        else
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();

                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                        else
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            //&& b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }

                    }
                }
                else
                {
                    if (ByLoc)
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.Quantity!=0
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z

                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();

                        }
                        else
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            where (b.FlowStartDate.Month == month)
                                            && n.ReceiptLocationIdentifier != ""
                                            && n.DeliveryLocationIdentifer != ""
                                            && b.PipelineID == pipeId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.Quantity!=0
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z
                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                  ).ToList();

                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()


                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            && b.CreatedBy == UserId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.Quantity!=0
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z


                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                        else
                        {
                            sumaryNomNew = (from n in DbContext.V4_Nomination
                                            join b in DbContext.V4_Batch on n.TransactionID equals b.TransactionID
                                            join c in DbContext.metadataCycle on b.CycleId equals c.ID
                                            join s in DbContext.Shipper on b.CreatedBy equals s.UserId

                                            join cpu in DbContext.CounterParty on n.UpstreamIdentifier equals cpu.Identifier into cpuOter
                                            from ci in cpuOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            join cpd in DbContext.CounterParty on n.DownstreamIdentifier equals cpd.Identifier into cpdOter
                                            from di in cpdOter.Where(a => a.Identifier != null).DefaultIfEmpty()

                                            where (b.FlowStartDate.Month == month)
                                            && n.ContractNumber != ""
                                            && b.PipelineID == pipeId
                                            && b.ServiceRequester == shipperCompanyDuns
                                            && b.CreatedDate.Year == DateTime.Now.Year
                                            && b.StatusID == 7
                                            && n.Quantity!=0
                                            && n.PathType == "T"
                                            group new { n, b, s }
                                            by new
                                            { n.NominatorTrackingId, upname = ci.Name, downName = di.Name, n.ReceiptLocationIdentifier, n.DeliveryLocationIdentifer, b.SubmitDate, c.Code, n.Quantity, n.UpstreamIdentifier, n.DownstreamIdentifier, b.FlowStartDate, b.FlowEndDate }
                                        into z
                                            select new SummaryDTO
                                            {
                                                nomEndDate = z.Key.FlowEndDate,
                                                nomStartDate = z.Key.FlowStartDate,
                                                ContractSVC = z.FirstOrDefault().n.ContractNumber,
                                                RecLoc = z.Key.ReceiptLocationIdentifier,
                                                DelLoc = z.Key.DeliveryLocationIdentifer,
                                                Cycle = z.Key.Code.ToString(),
                                                UpStreamName = z.Key.upname,
                                                UpIdentifier = z.Key.UpstreamIdentifier,
                                                DownStreamName = z.Key.downName,
                                                DwnIdentifier = z.Key.DownstreamIdentifier,
                                                NomTrackingId = z.Key.NominatorTrackingId,
                                                PkgId = z.FirstOrDefault().n.PackageId,
                                                SubmittedDate = z.Key.SubmitDate.Value,
                                                Qty = z.Key.Quantity,
                                                Username = z.FirstOrDefault().s.FirstName + " " + z.FirstOrDefault().s.LastName
                                            }
                                ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sumaryNomNew;
        }
    }
    public interface INominationsRepository : IRepository<V4_Nomination>
    {
        bool IsContractExist(string contractNo);
        NomType GetBatchNomType(Guid transactionId);
        IEnumerable<RejectedNomModel> GetAllRejectedNoms(int pipelineId, string userId);
        IEnumerable<BatchDTO> GetAllNominationBatch();
        BatchDetailDTO GetNominationDetail(Guid batchId, int pipeId);
        void SaveChages();
        bool AddBulkNoms(List<V4_Nomination> noms);
        bool deleteAll(List<V4_Nomination> noms);
        List<V4_Nomination> GetAllNomsByTransactionId(Guid transactionId);
        List<PathedNomDetailsDTO> GetPathedList(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);
        int GetPathedListTotalCount(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);
        List<PathedNomDetailsDTO> GetPathedListWithPaging(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser, SortingPagingInfo sortingPagingInfo);
        dynamic NomOnSqtsMatchedFields(string ServiceRequestorContract, string ModelType, string TransactionType, string ReceiptLocation, string UpstreamContractIdentifier, string DeliveryLocation, string DownstreamContractIdentifier, string UpIden, string DwnIden);
        NonPathedDTO GetNonPathedNominations(int PipelieID, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);
        List<SummaryDTO> GetAcceptedNomsOnDate(int month, int pipeId, string UserId, string shipperCompanyDuns, bool ByLoc, bool showZero);
        List<SummaryDTO> GetAcceptedNomOnDate(int month, string PathType, int pipeId, string UserId, string shipperCompanyDuns, bool ByLoc, bool showZero);
        List<SummaryDTO> GetNomQtyForContractPath(int month, int pipeId, string UserId, string shipperCompanyDuns, bool ByLoc, bool showZero);
    }
}
