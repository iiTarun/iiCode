using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using CentralisedUprd.Api.Helpers;
namespace CentralisedUprd.Api.Repositories
{
    public class UprdOACYRepository
    {
        UprdDbEntities1 dbcontext = new UprdDbEntities1();
        ModalFactory modalFactory = new ModalFactory();

        public OacyResultDTO GetOACYListWithPaging(string PipelineDuns, string keyword, DateTime? postStartDate, DateTime? effectiveGasDate, string cycle,SortingPagingInfo sortingPagingInfo)
        {
          OacyResultDTO oacyResultDTO = new OacyResultDTO();
            //Get oacy list using above parameters
            //IQueryable<OACYPerTransaction> result;
            IQueryable<OACYPerTransactionDTO> OacyDTO =null;

            try
            {
                var result = GetByPostDateEffDate(PipelineDuns, postStartDate, effectiveGasDate, keyword, cycle);
                oacyResultDTO.oacyPerTransactionDTO = (result != null || result.Count > 0) ? result : new List<OACYPerTransactionDTO>();
                oacyResultDTO.RecordCount = (result != null || result.Count > 0) ? result.Count : 0;
                //if (result.Count() > 0)
                //{
                //    OacyDTO = GetOacyData(result).AsQueryable();                    
                //} 
                //var dataQueryWithOrder = SortByColumnWithOrder(OacyDTO, sortingPagingInfo);
                //oacyResultDTO.RecordCount = dataQueryWithOrder.Count();
                //var resultData = dataQueryWithOrder.Skip((sortingPagingInfo.CurrentPageIndex - 1) * sortingPagingInfo.PageSize).Take(sortingPagingInfo.PageSize).ToList();
                //if (resultData.Count() > 0)
                //{
                //    var oacyResult = resultData.Select(a => modalFactory.ParseDTO(a)).ToList();
                //    oacyResultDTO.oacyPerTransactionDTO = oacyResult;
                //    return oacyResultDTO;
                //}

                //return oacyResultDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oacyResultDTO;
        }


        public IQueryable<OACYPerTransactionDTO> GetOacyData(IQueryable<OACYPerTransaction> result)
        {
            var FinalResult = (from o in result
                               join l in dbcontext.Locations on o.TransactionServiceProvider equals l.PipeDuns into oacy
                               from oacyResult in oacy.Where(x => x.PropCode == o.Loc).DefaultIfEmpty()
                               select new OACYPerTransactionDTO
                               {
                                   OACYID = o.OACYID,
                                   TransactionID = o.TransactionID,
                                   ReceiceFileID = o.ReceiceFileID,
                                   CreatedDate = o.CreatedDate,
                                   TransactionServiceProviderPropCode = o.TransactionServiceProviderPropCode,
                                   TransactionServiceProvider = o.TransactionServiceProvider,
                                   PostingDate = o.PostingDateTime,
                                   EffectiveGasDay = o.EffectiveGasDayTime,
                                   Loc = o.Loc,
                                   LocName = (o.LocName =="" ? oacyResult.Name : o.LocName),
                                   LocZn = o.LocZn,
                                   FlowIndicator = o.FlowIndicator,
                                   LocPropDesc = o.LocPropDesc,
                                   LocQTIDesc = o.LocQTIDesc,
                                   MeasurementBasis = o.MeasurementBasis,
                                   ITIndicator = o.ITIndicator,
                                   AllQtyAvailableIndicator = o.AllQtyAvailableIndicator,
                                   DesignCapacity = o.DesignCapacity,
                                   OperatingCapacity = o.OperatingCapacity,
                                   TotalScheduleQty = o.TotalScheduleQty,
                                   OperationallyAvailableQty = o.OperationallyAvailableQty,
                                   PipelineID = o.PipelineID,
                                   CycleIndicator = o.CycleIndicator,
                                   AvailablePercentage = o.AvailablePercentage
                               });


            return FinalResult;


        }

        public int GetTotalCountOACYList(string PipelineDuns, string keyword, DateTime? postStartDate, DateTime? effectiveGasDate, string cycle)
        {
            //Get oacy list using above parameters
            int records = 0;
            try
            {
                records = GetByPostDateEffDate(PipelineDuns, postStartDate, effectiveGasDate, keyword, cycle).Count();

                return records;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        #region Sort by column with order method

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        public IQueryable<OACYPerTransactionDTO> SortByColumnWithOrder(IQueryable<OACYPerTransactionDTO> dataQuery, Helpers.SortingPagingInfo sortingPagingInfo)
        {
            if (sortingPagingInfo != null)
            {
                // Sorting
                string orderDir = sortingPagingInfo.SortDirection;
                switch (sortingPagingInfo.SortField)
                {
                    case "Loc":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.Loc)
                                                                                                 : dataQuery.OrderBy(p => p.Loc);
                        break;
                    case "LocName":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.LocName)
                                                                                                 : dataQuery.OrderBy(p => p.LocName);
                        break;

                    case "CycleIndicator":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.CycleIndicator)
                                                                                                 : dataQuery.OrderBy(p => p.CycleIndicator);
                        break;

                    case "DesignCapacity":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.DesignCapacity)
                                                                                                 : dataQuery.OrderBy(p => p.DesignCapacity);
                        break;
                    case "OperatingCapacity":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.OperatingCapacity)
                                                                                                 : dataQuery.OrderBy(p => p.OperatingCapacity);
                        break;

                    case "TotalScheduleQty":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.TotalScheduleQty)
                                                                                                     : dataQuery.OrderBy(p => p.TotalScheduleQty);
                        break;
                    case "OperationallyAvailableQty":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.OperationallyAvailableQty)
                                                                                                     : dataQuery.OrderBy(p => p.OperationallyAvailableQty);
                        break;

                    case "AvailablePercentage":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.AvailablePercentage)
                                                                                                        : dataQuery.OrderBy(p => p.AvailablePercentage);
                        break;

                    case "EffectiveGasDay":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.EffectiveGasDay)
                                                                                                 : dataQuery.OrderBy(p => p.EffectiveGasDay);
                        break;
                    case "PostingDate":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.PostingDate)
                                                                                                 : dataQuery.OrderBy(p => p.PostingDate);
                        break;
                    case "FlowIndicator":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.FlowIndicator)
                                                                                                 : dataQuery.OrderBy(p => p.FlowIndicator);
                        break;
                    case "LocQTIDesc":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.LocQTIDesc)
                                                                                                 : dataQuery.OrderBy(p => p.LocQTIDesc);
                        break;

                    case "MeasurementBasis":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.MeasurementBasis)
                                                                                                   : dataQuery.OrderBy(p => p.MeasurementBasis);
                        break;
                    case "ITIndicator":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.ITIndicator)
                                                                                                 : dataQuery.OrderBy(p => p.ITIndicator);
                        break;
                    case "AllQtyAvailableIndicator":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.AllQtyAvailableIndicator)
                                                                                                  : dataQuery.OrderBy(p => p.AllQtyAvailableIndicator);
                        break;
                    default:
                        dataQuery = dataQuery.OrderByDescending(p => p.CreatedDate);

                        break;

                }
            }
            else
            {
                return dataQuery;
            }

            return dataQuery;
        }

        #endregion


        public IQueryable<OACYPerTransaction> GetDataByFilterCriteria(string PipelineDuns, string keyword, DateTime? postStartDate, DateTime? effectiveGasDate, string cycle)
        {
            return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == PipelineDuns
                                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                                             && DbFunctions.TruncateTime(a.PostingDateTime) == postStartDate
                                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == effectiveGasDate
                                             && (((a.CycleIndicator ?? "").ToLower().Contains(cycle.ToLower()))));
        }

        public DateTime? GetRecentPostDateUsngDuns(string pipelineDuns)
        {
            return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns)
                .Select(a => a.PostingDateTime)
                .OrderByDescending(a => a).FirstOrDefault();
        }

        public IQueryable<OACYPerTransaction> GetByPipeline(string pipeDuns)
        {            
            return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns);
        }

        public IQueryable<OACYPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocationIdentifier)
        {
            return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier);
        }


        public IQueryable<OACYPerTransaction> GetByPipelineLoc(string pipeDuns, string LocationIdentifier)
        {
            DateTime? recentdate = GetMostRecentOacyPostDate();
            if (recentdate != null)
            {
                return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier
                       && DbFunctions.TruncateTime(a.PostingDateTime) == DbFunctions.TruncateTime(recentdate)
                       );
            }
            else
            {
                return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier);
            }
        }

        public DateTime? GetMostRecentOacyPostDate()
        {
            var recentDate = dbcontext.OACYPerTransactions.Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
            return recentDate;
        }

        public int GetOACYListTotalCount(string pipelineDuns)
        {
            return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
        }

        public List<OACYPerTransactionDTO> GetByPostDateEffDate(string pipelineDuns, DateTime? postdate, DateTime? effdate, string keyword, string Cycle)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var pTime = postdate.GetValueOrDefault().TimeOfDay;
            var eDate = effdate.GetValueOrDefault().Date;
            UprdDbEntities1 db = new UprdDbEntities1();
            var query = db.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Loc.Contains(keyword)|| a.LocName.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(Cycle))
            {
                query = query.Where(a => a.CycleIndicator == Cycle);
            }
            if (!string.IsNullOrEmpty(pDate.ToString()) && pDate != DateTime.MinValue && TimeSpan.MinValue != pTime)
            {
                query = query.Where(a => DbFunctions.TruncateTime(a.PostingDateTime) == pDate && DbFunctions.CreateTime(a.PostingDateTime.Value.Hour, a.PostingDateTime.Value.Minute, a.PostingDateTime.Value.Second) >= pTime);
            }
            if (!string.IsNullOrEmpty(eDate.ToString()) && eDate!=DateTime.MinValue)
            {
                query = query.Where(a => DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate);
            }
            return query.Select(o=>new OACYPerTransactionDTO
            {
                OACYID = o.OACYID,
                TransactionID = o.TransactionID,
                ReceiceFileID = o.ReceiceFileID,
                CreatedDate = o.CreatedDate,
                TransactionServiceProviderPropCode = o.TransactionServiceProviderPropCode,
                TransactionServiceProvider = o.TransactionServiceProvider,
                PostingDate = o.PostingDateTime,
                EffectiveGasDay = o.EffectiveGasDayTime,
                Loc = o.Loc,
                LocName = o.LocName,
                LocZn = o.LocZn,
                FlowIndicator = o.FlowIndicator,
                LocPropDesc = o.LocPropDesc,
                LocQTIDesc = o.LocQTIDesc,
                MeasurementBasis = o.MeasurementBasis,
                ITIndicator = o.ITIndicator,
                AllQtyAvailableIndicator = o.AllQtyAvailableIndicator,
                DesignCapacity = o.DesignCapacity,
                OperatingCapacity = o.OperatingCapacity,
                TotalScheduleQty = o.TotalScheduleQty,
                OperationallyAvailableQty = o.OperationallyAvailableQty,
                PipelineID = o.PipelineID,
                CycleIndicator = o.CycleIndicator,
                AvailablePercentage = o.AvailablePercentage
            }).ToList();
            //keyword = keyword.Trim();
            //  IQueryable<OACYPerTransaction> oacylist;
            //if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    var count = dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
            //    if (count > 0)
            //    {
            //        DateTime? recentdate = GetRecentPostDateUsngDuns(pipelineDuns);
            //        return GetByPostDateEffDate(pipelineDuns, recentdate, null, string.Empty, string.Empty);
            //    }
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate);
            //}
            //else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            //}
            //else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            //}
            //else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.PostingDateTime) == pDate);
            //}
            //else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            //{
            //    return dbcontext.OACYPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
            //    && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate);
            //}

            //return new List<OACYPerTransaction>().AsQueryable();
        }
    }
}
