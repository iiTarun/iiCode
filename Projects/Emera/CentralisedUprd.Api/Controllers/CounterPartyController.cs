using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.UPRD.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CentralisedUprd.Api.Controllers
{
    public class CounterPartyController : ApiController
    {
        UprdCounterPartyRepository uprdCounterPartyRepository = new UprdCounterPartyRepository();
        SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();

        /// <summary>
        /// Get Method with INPUT: typeof(CounterPartyFilter)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns> List of CounterParties </returns>
        [HttpPost]
        public IHttpActionResult GetCounterPartyByCriteria([FromBody] CounterPartyFilter criteria)
        {          
            sortingPagingInfo.SortField = criteria.order;
            sortingPagingInfo.SortDirection = criteria.orderDir;
            sortingPagingInfo.PageSize = criteria.size;
            sortingPagingInfo.CurrentPageIndex = criteria.page;
           
            var source = uprdCounterPartyRepository.GetCounterPartiesUsingPaging(criteria.keyword, criteria.PipeDuns, sortingPagingInfo);

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
            if (source == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "CounterParties is not found."));
            }
            return Ok(source);
        }

        [HttpPost]
        public IHttpActionResult GetTotalCounterParties([FromBody] CounterPartyFilter criteria)
        {
            int CounterParties = uprdCounterPartyRepository.GetTotalCounterParties(criteria.keyword, criteria.PipeDuns);
            return Ok(CounterParties);
        }

        [HttpGet]
        public List<CounterPartiesDTO> GetCounterPartyList()
        {
            return uprdCounterPartyRepository.GetCounterParty();
        }
    }
   public class CounterPartyFilter
    {
        public string PipeDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public string order { get; set; } //SortField
        public string orderDir { get; set; }
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex

    }
}
