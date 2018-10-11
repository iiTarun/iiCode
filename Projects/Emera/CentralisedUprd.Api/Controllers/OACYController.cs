using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Repositories;
using System;
using System.Web.Http;

namespace CentralisedUprd.Api.Controllers
{
    public class OACYController : ApiController
    {
        /// <summary>
        /// Get Method with INPUT: typeof(OacyDataFilter)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns> List of OACYPerTransaction </returns>
        [HttpPost]
        public IHttpActionResult GetOACYByCriteria([FromBody]OacyDataFilter criteria)
        {
            SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
            sortingPagingInfo.SortField = criteria.sort;
            sortingPagingInfo.SortDirection = criteria.SortDirection;
            sortingPagingInfo.PageSize = criteria.size;
            sortingPagingInfo.CurrentPageIndex = criteria.page;
            UprdOACYRepository uprdOACYRepository = new UprdOACYRepository();

              var source = uprdOACYRepository.GetOACYListWithPaging(criteria.PipelineDuns, criteria.keyword, criteria.postStartDate, criteria.EffectiveStartDate, criteria.Cycle,sortingPagingInfo);

                //int count = sortingPagingInfo.PageCount;
                //int CurrentPage = sortingPagingInfo.CurrentPageIndex;
                //int PageSize = sortingPagingInfo.PageSize;
                //int TotalCount = count;

                //int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                //// if CurrentPage is greater than 1 means it has previousPage  
                //var previousPage = CurrentPage > 1 ? "Yes" : "No";

                //// if TotalPages is greater than CurrentPage means it has nextPage  
                //var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                //// Object which we are going to send in header   
                //var paginationMetadata = new
                //{
                //    totalCount = TotalCount,
                //    pageSize = PageSize,
                //    currentPage = CurrentPage,
                //    totalPages = TotalPages,
                //    previousPage,
                //    nextPage
                //};

                // Setting Header  
                //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
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
        public IHttpActionResult GetRecentOacyPostDate([FromUri]string PipelineDuns)
        {
            UprdOACYRepository uprdOACYRepository = new UprdOACYRepository();
            DateTime? recentdate = new DateTime?();
            var count = uprdOACYRepository.GetOACYListTotalCount(PipelineDuns);
            if (count > 0)
            {
                recentdate = uprdOACYRepository.GetRecentPostDateUsngDuns(PipelineDuns);
            }
            else
            {
                recentdate = DateTime.Now.AddDays(-1);    // if no data found, then return Yesterday date.
            }
            return Ok(recentdate);
        }

        [HttpPost]
        public IHttpActionResult GetTotalCountOacy([FromBody]OacyDataFilter criteria)
        {
            UprdOACYRepository uprdOACYRepository = new UprdOACYRepository();
            int totalRecords= uprdOACYRepository.GetTotalCountOACYList(criteria.PipelineDuns, criteria.keyword, criteria.postStartDate, criteria.EffectiveStartDate, criteria.Cycle);
            return Ok(totalRecords);
        }


    }

    public class OacyDataFilter
    {
        public string PipelineDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public DateTime? postStartDate { get; set; }
        public DateTime? EffectiveStartDate { get; set; } //EffectiveGasDate
        public string Cycle { get; set; }
        public string sort { get; set; } //SortField
        public string SortDirection { get; set; }
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex
      
    }


}

