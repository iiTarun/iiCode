using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace Nom1Done.Controllers.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/oacy")]
    public class OacyController : BaseApiController
    {
        private IOACYService _IOACYService;       

        // public OacyController() { }

        public OacyController( IOACYService _IOACYService)
        {
            this._IOACYService = _IOACYService;           
        }

        /// <summary>
        /// Get Method with INPUT: typeof(Search)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns> List of OACYPerTransactionDTO </returns>
        [Route("")]
        public JsonResult<List<OACYPerTransactionDTO>> Get([FromUri]Search criteria)
        {
            List<OACYPerTransactionDTO> list = new List<OACYPerTransactionDTO>();
           
            list = _IOACYService.GetAllOacyOnPipelineId(criteria);

            // Apply pagination.
            if (criteria.page > 0 && criteria.size > 0) { 
                 list = list.Skip((criteria.page - 1) * criteria.size).Take(criteria.size).ToList();
            }

            return Json(list);
        }
       
    }
}
