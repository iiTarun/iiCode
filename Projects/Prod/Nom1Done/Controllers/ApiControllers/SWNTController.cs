using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace Nom1Done.Controllers.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/swnt")]
    public class SWNTController : BaseApiController
    {
        INoticesService _INoticeService;
        public SWNTController(INoticesService _INoticeService)
        {
            this._INoticeService = _INoticeService;
        }

        /// <summary>
        /// Get Method for Notices with INPUT: typeof(NoticeSearchCriteria)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>List of SwntPerTransactionDTO </returns>
        [Route("")]
        public JsonResult<List<SwntPerTransactionDTO>> Get([FromUri]BONoticeSearchCriteria criteria)
        {
            List<SwntPerTransactionDTO> noticelist = new List<SwntPerTransactionDTO>();
            noticelist = _INoticeService.GetNoticesBySearch(criteria);
            return Json(noticelist);
        }

        /// <summary>
        /// Get Notice By ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Notice detail</returns>
        /// 
        [Route("{id:int}")]
        [ResponseType(typeof(SwntPerTransactionDTO))]
        public IHttpActionResult GetById(int id)
        {
            SwntPerTransactionDTO item = _INoticeService.GetNoticeById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);

        }

    }
}