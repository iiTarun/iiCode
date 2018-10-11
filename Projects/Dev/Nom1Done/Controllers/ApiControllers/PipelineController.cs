using Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Nom1Done.Controllers.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/pipeline")]
    public class PipelineController : BaseApiController
    {
        IPipelineService _IpipelineService;
        public PipelineController(IPipelineService _IpipelineService)
        {
            this._IpipelineService = _IpipelineService;
        }

        // GET api/<controller>
        [Route("")]
        public JsonResult<List<PipelineDTO>> Get()
        {
            var list = _IpipelineService.GetAllPipelineList().OrderBy(a => a.Name).ToList();
            return Json(list);
        }
       
    }
}