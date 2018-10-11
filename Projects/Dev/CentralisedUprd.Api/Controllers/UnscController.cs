using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CentralisedUprd.Api.Controllers
{
    public class UnscController : ApiController
    {
        /// <summary>
        /// Get Method with INPUT: typeof(UnscDataFilter)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns> List of UnscPerTransactions</returns>
        [HttpPost]
        public IHttpActionResult GetUnscByCriteria([FromBody]UnscDataFilter criteria)
        {
            var source = (dynamic)null;
            try
            {
                //SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
                //sortingPagingInfo.SortField = criteria.sort;
                //sortingPagingInfo.SortDirection = criteria.SortDirection;
                //sortingPagingInfo.PageSize = criteria.size;
                //sortingPagingInfo.CurrentPageIndex = criteria.page;

                //UprdUnscRepository uprdUnscRepository = new UprdUnscRepository();
                var pTime = criteria.postTime.GetValueOrDefault().TimeOfDay;
                UprdDbEntities1 db = new UprdDbEntities1();
                var query = db.UnscPerTransactions.Where(a => a.TransactionServiceProvider == criteria.PipelineDuns).AsQueryable();
                if (!string.IsNullOrEmpty(criteria.keyword))
                {
                    query = query.Where(a => a.Loc.Contains(criteria.keyword) || a.LocName.Contains(criteria.keyword));
                }

                if (!string.IsNullOrEmpty(criteria.postStartDate.ToString()) && TimeSpan.MinValue!=pTime)
                {
                    query = query.Where(a => DbFunctions.TruncateTime(a.PostingDateTime) == DbFunctions.TruncateTime(criteria.postStartDate)
                    && DbFunctions.CreateTime(a.PostingDateTime.Value.Hour, a.PostingDateTime.Value.Minute, a.PostingDateTime.Value.Second) >= pTime);
                }

                if (!string.IsNullOrEmpty(criteria.EffectiveStartDate.ToString()))
                {
                    query = query.Where(a => DbFunctions.TruncateTime(a.EffectiveGasDayTime) == DbFunctions.TruncateTime(criteria.EffectiveStartDate));
                }

                if (!string.IsNullOrEmpty(criteria.EffectiveEndDate.ToString()))
                {
                    query = query.Where(a => DbFunctions.TruncateTime(a.EndingEffectiveDay) == DbFunctions.TruncateTime(criteria.EffectiveEndDate));
                }

                var data = query.Select(a=>new UnscPerTransactionDTO {
                    TransactionID=a.TransactionID,
                    //AvailablePercentage=
                    ChangePercentage=a.ChangePercentage,
                    CreatedDate=a.CreatedDate,
                    DUNSNo=a.TransactionServiceProvider,
                    EffectiveGasDay=a.EffectiveGasDayTime,
                    //EffectiveGasDayTime=a.EffectiveGasDayTime,
                    EndingEffectiveDay=a.EndingEffectiveDay,
                    Loc=a.Loc,
                    LocName=a.LocName,
                    LocPurpDesc=a.LocPurpDesc,
                    LocQTIDesc=a.LocQTIDesc,
                    LocZn=a.LocZn,
                    MeasBasisDesc=a.MeasBasisDesc,
                    PipelineID=a.PipelineID,
                    //PipelineNameDuns=a.
                    PostingDate=a.PostingDateTime,
                    //PostingDateTime=a.PostingDateTime,
                    TotalDesignCapacity=a.TotalDesignCapacity,
                    TransactionServiceProvider=a.TransactionServiceProvider,
                    TransactionServiceProviderPropCode=a.TransactionServiceProviderPropCode,
                    UnsubscribeCapacity=a.UnsubscribeCapacity
                }).ToList();
                UnscResultDTO result = new UnscResultDTO();
                result.unscPerTransactionDTO = (data != null && data.Count > 0) ? data : new List<UnscPerTransactionDTO>();
                result.RecordCount = (data != null && data.Count > 0) ? data.Count : 0;
                source = result;
                //    if (!string.IsNullOrEmpty(criteria.PipelineDuns))
                //    {
                //        source = uprdUnscRepository.GetUnscListWithPaging(criteria.PipelineDuns, criteria.keyword, criteria.postStartDate, criteria.EffectiveStartDate, criteria.EffectiveEndDate, sortingPagingInfo);
                //    }
                //        int count = sortingPagingInfo.PageCount;
                //        int CurrentPage = sortingPagingInfo.CurrentPageIndex;
                //        int PageSize = sortingPagingInfo.PageSize;
                //        int TotalCount = count;

                //        int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                //        // if CurrentPage is greater than 1 means it has previousPage  
                //        var previousPage = CurrentPage > 1 ? "Yes" : "No";

                //        // if TotalPages is greater than CurrentPage means it has nextPage  
                //        var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                //        // Object which we are going to send in header   
                //        var paginationMetadata = new
                //        {
                //            totalCount = TotalCount,
                //            pageSize = PageSize,
                //            currentPage = CurrentPage,
                //            totalPages = TotalPages,
                //            previousPage,
                //            nextPage
                //        };

                //        // Setting Header  
                //        HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //if (source == null)
            //{
            //    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            //    {
            //        ReasonPhrase = "Records Not Found."
            //    };
            //    throw new HttpResponseException(resp);
            //}

            return Ok(source);
        }

        [HttpGet]
        public IHttpActionResult GetRecentUnscPostDate(string PipelineDuns)
        {
            UprdUnscRepository uprdUnscRepository = new UprdUnscRepository();
            DateTime? recentdate = new DateTime?();
            var count = uprdUnscRepository.GetTotalCountByPipeDuns(PipelineDuns);
            if (count > 0)
            {
                recentdate = uprdUnscRepository.GetRecentPostDateUsingDuns(PipelineDuns);
            }
            else
            {
                recentdate = DateTime.Now.AddDays(-1);    // if no data found, then return Yesterday date.
            }
            return Ok(recentdate);
        }

        [HttpPost]
        public IHttpActionResult GetTotalRecordsCountUNSC([FromBody]UnscDataFilter criteria)
        {           
            UprdUnscRepository uprdUnscRepository = new UprdUnscRepository();
            int count=  uprdUnscRepository.GetUnscListTotalCount(criteria.PipelineDuns, criteria.keyword, criteria.postStartDate, criteria.EffectiveStartDate, criteria.EffectiveEndDate);
            return Ok(count);
        }


    }
    public class UnscDataFilter
    {
        public string PipelineDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public DateTime? postStartDate { get; set; }
        public DateTime? postTime { get; set; }
        public DateTime? EffectiveStartDate { get; set; } //EffectiveGasDate
        public DateTime? EffectiveEndDate { get; set; } //EndingEffectiveDay
        public string sort { get; set; } //SortField
        public string SortDirection { get; set; } //SortDirection
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex


    }
}
