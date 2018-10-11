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
    [RoutePrefix("api/unsc")]
    public class UnscController : BaseApiController
    {
        private readonly IUNSCService _IUNSCService;
        private IModalFactory _IModalFactory;
        public UnscController(IModalFactory _IModalFactory, IUNSCService _IUNSCService)
        {
            this._IModalFactory = _IModalFactory;
            this._IUNSCService = _IUNSCService;
        }


        /// <summary>
        ///  Get Method INPUT Parameter: typeof Search [PipelineID, Keyword, Postingdate, Effdate]
        ///  OUTPUT: List of Datatypeof(UnscPerTransactionDTO)
        /// </summary>
        [Route("")]
        public JsonResult<List<UnscPerTransactionDTO>> Get([FromUri]Search criteria)
        {
            List<UnscPerTransactionDTO> list = new List<UnscPerTransactionDTO>();
            list = _IUNSCService.GetAllUNSCOnPipelineId(criteria);

            // Apply pagination.
            if (criteria.page > 0 && criteria.size > 0)
            {
                list = list.Skip((criteria.page - 1) * criteria.size).Take(criteria.size).ToList();
            }
            return Json(list);
        }       
               
    }
}
