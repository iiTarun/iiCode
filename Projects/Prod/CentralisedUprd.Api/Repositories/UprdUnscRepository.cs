using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Repositories
{
    public class UprdUnscRepository
    {
        UprdDbEntities1 dbcontext = new UprdDbEntities1();
        ModalFactory modalFactory = new ModalFactory();

        public UnscResultDTO GetUnscListWithPaging(string PipelineDuns, string keyword, DateTime? postStartDate, DateTime? effectiveStartDate, DateTime? effectiveEndDate, SortingPagingInfo sortingPagingInfo)
        {
            UnscResultDTO unscResult = new UnscResultDTO();
            //Get unsc list using above parameters
            IQueryable<UnscPerTransaction> result;
            IQueryable<UnscPerTransactionDTO> UnscDTO = null;
            try
            {

                result = GetAllUNSCOnPipelineId(PipelineDuns, keyword, postStartDate, effectiveStartDate, effectiveEndDate);
                if (result.Count() > 0)
                {
                    UnscDTO = GetUnscData(result).AsQueryable();
                }
                var dataQueryWithOrder = SortByColumnWithOrder(UnscDTO, sortingPagingInfo);

                unscResult.RecordCount = dataQueryWithOrder.Count();

                var resultData = dataQueryWithOrder.Skip((sortingPagingInfo.CurrentPageIndex - 1) * sortingPagingInfo.PageSize).Take(sortingPagingInfo.PageSize).ToList();


                if (resultData.Count() > 0)
                {
                    unscResult.unscPerTransactionDTO = resultData.Select(a => modalFactory.ParseDTO(a)).ToList();
                    return unscResult;
                }
                return new UnscResultDTO();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<UnscPerTransactionDTO> GetUnscData(IQueryable<UnscPerTransaction> result)
        {

            var FinalResult= (from u in result
            join l in dbcontext.Locations on u.TransactionServiceProvider equals l.PipeDuns into unsc
            from unscResult in unsc.Where(x => x.PropCode == u.Loc).DefaultIfEmpty()

                    select new UnscPerTransactionDTO
                    {
                           UnscID= u.UnscID,
                           TransactionID= u.TransactionID,
                           ReceiveFileID = u.ReceiveFileID,
                           CreatedDate = u.CreatedDate,
                           PipelineID =u.PipelineID,
                           TransactionServiceProvider =u.TransactionServiceProvider, 
                           TransactionServiceProviderPropCode =u.TransactionServiceProviderPropCode,
                           Loc =u.Loc,
                           LocName = (u.LocName == "" ? unscResult.Name : u.LocName),
                           LocZn =u.LocZn,
                           LocPurpDesc =u.LocPurpDesc,
                           LocQTIDesc=u.LocQTIDesc,
                           MeasBasisDesc=u.MeasBasisDesc,
                           TotalDesignCapacity =u.TotalDesignCapacity, 
                           UnsubscribeCapacity =u.UnsubscribeCapacity ,
                           PostingDate =u.PostingDateTime,
                           EffectiveGasDay=u.EffectiveGasDayTime,
                           EndingEffectiveDay=u.EndingEffectiveDay,
                           ChangePercentage = u.ChangePercentage

                    });
            return FinalResult;

        }
        public int GetUnscListTotalCount(string PipelineDuns, string keyword, DateTime? postStartDate, DateTime? effectiveStartDate, DateTime? effectiveEndDate)
        {

            //Get unsc list using above parameters
          //  List<UnscPerTransaction> Unscdatalist = new List<UnscPerTransaction>();
            try
            {

              var Unscdatalist = GetAllUNSCOnPipelineId(PipelineDuns, keyword, postStartDate, effectiveStartDate, effectiveEndDate);
                return Unscdatalist.Count();
               
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
        /// 
        //ToDo-add hard coded var
        public IQueryable<UnscPerTransactionDTO> SortByColumnWithOrder(IQueryable<UnscPerTransactionDTO> dataQuery, Helpers.SortingPagingInfo sortingPagingInfo)
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

                    case "LocQTIDesc":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.LocQTIDesc)
                                                                                                 : dataQuery.OrderBy(p => p.LocQTIDesc);
                        break;
                    case "PostingDate":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.PostingDate)
                                                                                                 : dataQuery.OrderBy(p => p.PostingDate);
                        break;

                    case "EffectiveGasDay":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.EffectiveGasDay)
                                                                                                 : dataQuery.OrderBy(p => p.EffectiveGasDay);
                        break;

                    case "EndingEffectiveDay":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.EndingEffectiveDay)
                                                                                                 : dataQuery.OrderBy(p => p.EndingEffectiveDay);
                        break;

                    case "MeasBasisDesc":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.MeasBasisDesc)
                                                                                                   : dataQuery.OrderBy(p => p.MeasBasisDesc);
                        break;

                    case "LocZn":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.LocZn)
                                                                                                   : dataQuery.OrderBy(p => p.LocZn);
                        break;

                    case "UnsubscribeCapacity":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.UnsubscribeCapacity)
                                                                                                 : dataQuery.OrderBy(p => p.UnsubscribeCapacity);
                        break;

                    case "ChangePercentage":
                        dataQuery = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? dataQuery.OrderByDescending(p => p.ChangePercentage)
                                                                                                 : dataQuery.OrderBy(p => p.ChangePercentage);
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

        public DateTime? GetRecentPostDateUsingDuns(string pipelineDuns)
        {
            return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns).Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }

        public IQueryable<UnscPerTransaction> GetByPipeline(string pipelineDuns)
        {
            return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns);
        }

        public IQueryable<UnscPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocIdentifier)
        {
            return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier);
        }

        public IQueryable<UnscPerTransaction> GetByPipelineLoc(string pipeDuns, string LocIdentifier)
        {
            DateTime? recentDate = GetRecentPostingDate();
            if (recentDate != null)
            {
                return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier
                && DbFunctions.TruncateTime(a.PostingDateTime) == DbFunctions.TruncateTime(recentDate)
                );
            }
            else
            {
                return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier);
            }
        }

        public DateTime? GetRecentPostingDate()
        {
            return dbcontext.UnscPerTransactions.Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }
        public int GetTotalCountByPipeDuns(string pipelineDuns)
        {
            return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
        }
        public IQueryable<UnscPerTransaction> GetRecentUnscByPostDate(string pipelineDuns, DateTime? postDate)
        {
            var pDate = postDate.GetValueOrDefault().Date;
            return dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns && DbFunctions.TruncateTime(a.PostingDateTime) == pDate);
        }

        public IQueryable<UnscPerTransaction> GetRecentUnsc(string PipelineDuns)
        {
            var oacyList = (dynamic)null;
            var count = GetTotalCountByPipeDuns(PipelineDuns);
            if (count > 0)
            {
                DateTime? recentdate = GetRecentPostDateUsingDuns(PipelineDuns);
               oacyList = GetRecentUnscByPostDate(PipelineDuns, recentdate);
            }
            return oacyList;
        }

        public IQueryable<UnscPerTransaction> GetByKeyword(string pipelineDuns, string keyword)
        {        
           var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateStartEffEndEffDate(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
           
          var  unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == PipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateStartEffEndEffDateKeyword(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
         
         var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == PipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByStartEffDateEndEffDate(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate)
        {
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;          
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     );
            return unsclist;
        }

        public IQueryable<UnscPerTransaction> GetByStartEffDateEndEffDateKeyword(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                      && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                      && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                      && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                      );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateStartEffDate(string pipelineDuns, DateTime? postdate, DateTime? starteffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
          
         var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateStartEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? starteffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;        
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateEndEffDate(string pipelineDuns, DateTime? postdate, DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;         
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateEndEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;          
           var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDate(string pipelineDuns, DateTime? postdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                             );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByPostDateKeyword(string pipelineDuns, DateTime? postdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                              );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByEndEffDate(string pipelineDuns, DateTime? endeffdate)
        {
            var eDate = endeffdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                            );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByEndEffDateKeyword(string pipelineDuns, DateTime? endeffdate, string keyword)
        {
            var eDate = endeffdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                             );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByStartEffDate(string pipelineDuns, DateTime? starteffdate)
        {
            var eDate = starteffdate.GetValueOrDefault().Date;
            var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                            );
            return unsclist;
        }
        public IQueryable<UnscPerTransaction> GetByStartEffDateKeyword(string pipelineDuns, DateTime? starteffdate, string keyword)
        {
           var eDate = starteffdate.GetValueOrDefault().Date;
           var unsclist = dbcontext.UnscPerTransactions.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            return unsclist;
        }


        public IQueryable<UnscPerTransaction> GetAllUNSCOnPipelineId(string PipelineDuns, string keyword,  DateTime? postStartDate, DateTime? EffectiveStartDate, DateTime? EffectiveEndDate)
        {

            IQueryable< UnscPerTransaction> result=null;
            DateTime minDate = DateTime.MinValue;

             postStartDate = postStartDate.GetValueOrDefault().Date;
             EffectiveStartDate = EffectiveStartDate.GetValueOrDefault().Date;
             EffectiveEndDate = EffectiveEndDate.GetValueOrDefault().Date;


            if ((EffectiveStartDate == minDate) && (EffectiveEndDate == minDate) && (postStartDate == minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetRecentUnsc(PipelineDuns);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate == minDate) && (postStartDate == minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByKeyword(PipelineDuns, keyword);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate != minDate) && (postStartDate != minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateStartEffEndEffDate(PipelineDuns, postStartDate, EffectiveStartDate, EffectiveEndDate);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate != minDate) && (postStartDate != minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateStartEffEndEffDateKeyword(PipelineDuns, postStartDate, EffectiveStartDate, EffectiveEndDate, keyword);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate != minDate) && (postStartDate == minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByStartEffDateEndEffDate(PipelineDuns, EffectiveStartDate, EffectiveEndDate);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate != minDate) && (postStartDate == minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByStartEffDateEndEffDateKeyword(PipelineDuns, EffectiveStartDate, EffectiveEndDate, keyword);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate == minDate) && (postStartDate != minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateStartEffDate(PipelineDuns, postStartDate, EffectiveStartDate);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate == minDate) && (postStartDate != minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateStartEffDateKeyword(PipelineDuns, postStartDate, EffectiveStartDate, keyword);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate != minDate) && (postStartDate != minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateEndEffDate(PipelineDuns, postStartDate, EffectiveEndDate);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate != minDate) && (postStartDate != minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateEndEffDateKeyword(PipelineDuns, postStartDate, EffectiveEndDate, keyword);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate == minDate) && (postStartDate != minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDate(PipelineDuns, postStartDate);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate == minDate) && (postStartDate != minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByPostDateKeyword(PipelineDuns, postStartDate, keyword);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate != minDate) && (postStartDate == minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByEndEffDate(PipelineDuns, EffectiveEndDate);
            }
            else if ((EffectiveStartDate == minDate) && (EffectiveEndDate != minDate) && (postStartDate == minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByEndEffDateKeyword(PipelineDuns, EffectiveEndDate, keyword);

            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate == minDate) && (postStartDate == minDate) && (string.IsNullOrEmpty(keyword)))
            {
                result = GetByStartEffDate(PipelineDuns, EffectiveStartDate);
            }
            else if ((EffectiveStartDate != minDate) && (EffectiveEndDate == minDate) && (postStartDate == minDate) && (!string.IsNullOrEmpty(keyword)))
            {
                result = GetByStartEffDateKeyword(PipelineDuns, EffectiveStartDate, keyword);
            }
            return result;        
        }
    }
}