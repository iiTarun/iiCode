using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CentralisedUprd.Api.Helpers;
using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.UPRD.DTO;
using Newtonsoft.Json;

namespace CentralisedUprd.Api.Controllers
{
    public class LocationController : ApiController
    {
        SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
        UprdLocationsRepository locRepository = new UprdLocationsRepository();
        [HttpPost]
        public IHttpActionResult GetLocationsByCriteria([FromBody] LocDataFilter criteria)
        {
            sortingPagingInfo.SortField = criteria.order;
            sortingPagingInfo.SortDirection = criteria.orderDir;
            sortingPagingInfo.PageSize = criteria.size;
            sortingPagingInfo.CurrentPageIndex = criteria.page;

            var source = locRepository.GetLocationsWithPaging(criteria.keyword, criteria.PipelineDuns,criteria.PopupFor,criteria.IsSpecialDelCase, sortingPagingInfo);
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
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Location is not found."));
            }
            return Ok(source);
        }

        [HttpPost]
        public IHttpActionResult GetTotalLocationCount([FromBody] LocDataFilter criteria)
        {
            int LocationCount = locRepository.GetTotalLocationCount(criteria.keyword, criteria.PipelineDuns, criteria.PopupFor, criteria.IsSpecialDelCase);
            return Ok(LocationCount);
        }
        [HttpPost]
        public IHttpActionResult GetLocationByIdentifier([FromBody] LocDataFilter criteria)
        {
            var result = locRepository.GetLocationByIdentifier(criteria.Identifier, criteria.PipelineDuns);
            return Ok(result);
        }
        [HttpGet] 
        public List<LocationsDTO> GetLocationList()
        {
            return locRepository.GetLocationList();
        }

        [HttpGet]
        public IHttpActionResult GetLocationListByPipeDuns(string pipelineDuns)
        {
            var result= locRepository.GetLocationListByPipelineDuns(pipelineDuns);
            return Ok(result);
        }

        [HttpPost]
     public IHttpActionResult GetPipelineLocation([FromBody]string pipelineDuns)
        {
            var LocationByPipeLine = locRepository.GetLocationByPipeline(pipelineDuns);
            return Ok(LocationByPipeLine);
        }

        [HttpPost]
        public IHttpActionResult AddLocation([FromBody] List<LocationsDTO> locList)
        {
            var source = locRepository.AddLocationByPipeline(locList);
            return Ok(source);
        }

        public IHttpActionResult EditLocation([FromBody] int id)
        {
            var source = locRepository.GetLocationById(id);
            return Ok(source);
        }
        public IHttpActionResult UpdateLocation([FromBody] LocationsDTO loc)
        {
            var source = locRepository.UpdateLocationByID(loc);
            return Ok(source);
        }
        [HttpPost]
        public IHttpActionResult DeleteLocation([FromBody] int id)
        {
            var source = locRepository.DeleteLocationByID(id);
            return Ok(source);
        }
        [HttpPost]
        public IHttpActionResult AddLocsNotInTblLoc([FromBody]string Pipelineduns)
        {
            var source = locRepository.GetLocFromOacyUnsc(Pipelineduns);
            return Ok(source);
        }
        public IHttpActionResult SaveLocsNotInTblLoc([FromBody] LocationsDTO loc)
        {            
            var source = locRepository.UpdateLocationByID(loc);
            return Ok(source);
        }
    }
    public class LocDataFilter 
    {
        public string PipelineDuns { get; set; }
        public string keyword { get; set; } = string.Empty;
        public string order { get; set; } //SortField 
        public string orderDir { get; set; }     
        public int size { get; set; } //PageSize
        public int page { get; set; } //CurrentPageIndex
        public string PopupFor { get; set; }
        public bool IsSpecialDelCase { get; set; }
        public string Identifier { get; set; }

    }
}

