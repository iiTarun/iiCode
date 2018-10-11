using CentralisedUprd.Api.Enum;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.Services;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CentralisedUprd.Api.Controllers
{
    
    public class WatchListController : ApiController
    {
        public WatchlistService WatchlistService;
        public WatchListController()
        {
            WatchlistService = new WatchlistService();
        }

        [HttpPost]
        public IHttpActionResult SaveWatchList([FromBody]WatchListDTO watchList)
        {
            bool result = false;
            if (watchList != null)
            {
                WatchlistService.SaveWatchList(watchList);
                result = true;
            }
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult GetWatchListById(int Id)
        {
            if (Id != 0) {
               var watchList= WatchlistService.GetWatchListById(Id);
                if (watchList == null) {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Watchlist is not found."));
                }
                return Json(watchList);
            } else {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id is not provided."));
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateWatchList([FromBody]WatchListDTO watchListDto)
        {
            bool result = false;
            if (watchListDto != null) {
                WatchlistService.UpdateWatchList(watchListDto);
                result = true;
            }
            return Json(result);
        }

        [HttpPost]
        public IHttpActionResult DeleteWatchListById([FromBody]int WatchListId)
        {
            bool isDeleted = false;
            isDeleted = WatchlistService.DeleteWatchListById(WatchListId);
            return Json(isDeleted);
        }

        [HttpPost]
        public IHttpActionResult GetWatchListByUserId([FromBody]string userId)
        {
            bool result = false ;
            if (!string.IsNullOrEmpty(userId))
            {
                var list = WatchlistService.GetWatchListByUserId(userId);
                return Json(list);
            }
            else {
                return Json(result);
            }
            
        }


        [HttpGet]
        public IHttpActionResult GetPropertiesByDataSet([FromUri]UprdDataSet dataset)
        {
            var list = WatchlistService.GetPropertiesByDataSet(dataset);
            return Json(list);
        }


        [HttpGet]
        public IHttpActionResult GetLocationsFromUPRDs([FromUri]string PipelineDuns,[FromUri]string Keyword)
        {
            var list = WatchlistService.GetLocationsFromUPRDs(PipelineDuns, Keyword);
            return Json(list);
        }

        [HttpGet]
        public IHttpActionResult GetLocationsFromUPRDsUsingPaging([FromUri]string PipelineDuns, [FromUri]UprdDataSet datasetType, [FromUri] string Keyword, [FromUri] int PageNo, [FromUri] int PageSize, [FromUri] string order, [FromUri] string orderDir)
        {
            var list = WatchlistService.GetLocationsFromUPRDsUsingPaging(PipelineDuns, Keyword,PageNo,PageSize,order,orderDir);
            return Json(list);
        }


        [HttpGet]
        public IHttpActionResult GetTotalCountLocationsFromUPRD([FromUri]string PipelineDuns, [FromUri]UprdDataSet datasetType, [FromUri] string Keyword)
        {
            var list = WatchlistService.GetTotalLocationsFromUPRDs(PipelineDuns, Keyword);
            return Json(list);
        }

        [HttpGet]
        public IHttpActionResult GetPropertyById(int propertyId)
        {
            var list = WatchlistService.GetPropertyById(propertyId);
            return Json(list);

        }

        [HttpGet]
        public IHttpActionResult ExecutedResult(int Id)
        {
            WatchListAlertExecutedDataDTO model = new WatchListAlertExecutedDataDTO();
            var watchListDto = WatchlistService.GetWatchListById(Id);

            model.watchList = watchListDto;
            if (watchListDto.DatasetId == UprdDataSet.OACY)
                model.OacyDataList = WatchlistService.ExecuteWatchListOACYonScreen(watchListDto.RuleList, typeof(OACYPerTransaction));
            else if (watchListDto.DatasetId == UprdDataSet.UNSC)
            {
                model.UnscDataList = WatchlistService.ExecuteWatchListUNSConScreen(watchListDto.RuleList, typeof(UnscPerTransaction));
            }
            else if (watchListDto.DatasetId == UprdDataSet.SWNT)
            {
                model.SwntDataList = WatchlistService.ExecuteWatchListSWNTOnScreen(watchListDto.RuleList, typeof(SwntPerTransaction));
            }

            return Json(model);
        }
    }
}
