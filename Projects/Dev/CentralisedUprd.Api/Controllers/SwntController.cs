using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.UPRD.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static CentralisedUprd.Api.WebApiApplication;

namespace CentralisedUprd.Api.Controllers
{
   
    public class SwntController : ApiController
    {
        /// <summary>
        /// Get Method with INPUT: typeof(SwntDataFilter)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns> List of SwntPerTransactions</returns>
        [HttpPost]
        public IHttpActionResult GetSwntByCriteria([FromBody]SwntDataFilter criteria)
        {
            var source = (dynamic)null;           
            try
            {
                SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
                sortingPagingInfo.SortField = criteria.SortField;
                sortingPagingInfo.SortDirection = criteria.SortDirection;
                sortingPagingInfo.PageSize = criteria.PageSize;
                sortingPagingInfo.CurrentPageIndex = criteria.PageNo;
                UprdSwntRepository uprdSwntRepository = new UprdSwntRepository();

                if (!string.IsNullOrEmpty(criteria.PipelineDuns))
                {
                    source = uprdSwntRepository.GetSwntListWithPaging(criteria.PipelineDuns, criteria.IsCritical,criteria.Keyword,criteria.postStartDate, criteria.postEndDate, criteria.EffectiveStartDate, criteria.EffectiveEndDate, sortingPagingInfo);
                    int count = sortingPagingInfo.PageCount;
                    int CurrentPage = sortingPagingInfo.CurrentPageIndex;
                    int PageSize = sortingPagingInfo.PageSize;
                    int TotalCount = count;

                    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                    // if CurrentPage is greater than 1 means it has previousPage  
                    var previousPage = CurrentPage > 1 ? "Yes" : "No";

                    // if TotalPages is greater than CurrentPage means it has nextPage  
                    var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                    // Object which we are going to send in header   
                    var paginationMetadata = new
                    {
                        totalCount = TotalCount,
                        pageSize = PageSize,
                        currentPage = CurrentPage,
                        totalPages = TotalPages,
                        previousPage,
                        nextPage
                    };

                    // Setting Header  
                    HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                  
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in notice controller in API.", ex);
            }          
            if (source == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    //Content = new StringContent(string.Format("No product with ID = {0}", id)),
                    ReasonPhrase = "Records Not Found."
                };
                throw new HttpResponseException(resp);
            }

            return Ok(source);
        }

        [HttpGet]
        public IHttpActionResult GetNoticeOnId(int id)
        {
            UprdSwntRepository uprdSwntRepository = new UprdSwntRepository();
            var notice = uprdSwntRepository.GetNoticeById(id);
            if (notice != null) {
                return Ok(notice);
            }
            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult FilterNotices(string PipelineDuns, bool isCritical, DateTime startDate, DateTime endDate)
        {
            UprdSwntRepository uprdSwntRepository = new UprdSwntRepository();
            var result = uprdSwntRepository.GetByCreatedDateRange(PipelineDuns, isCritical, startDate, endDate);
           
            List<BONotice> noticeList = new List<BONotice>();
            foreach (var item in result)
            {
                BONotice notice = uprdSwntRepository.MappingNotice(item);
                noticeList.Add(notice);
            }
            return Json(noticeList);
        }


        [HttpPost]
        public IHttpActionResult GetSwntTotalRecords([FromBody]SwntDataFilter criteria)
        {
            UprdSwntRepository uprdSwntRepository = new UprdSwntRepository();
            int records= uprdSwntRepository.GetSwntListTotalCount(criteria.PipelineDuns, criteria.IsCritical, criteria.Keyword, criteria.postStartDate, criteria.postEndDate, criteria.EffectiveStartDate, criteria.EffectiveEndDate);
            return Ok(records);
        }

    }
    public class SwntDataFilter
    {
        public string PipelineDuns { get; set; }
        public bool IsCritical { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public DateTime? postStartDate { get; set; } 
        public DateTime? postEndDate { get; set; } 
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; } //CurrentPageIndex

    }
}
