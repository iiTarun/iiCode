using Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class PipelineStatusController : Controller
    {


        IPipelineStatusService pipelineStatusService;

        public PipelineStatusController(IPipelineStatusService _pipelineStatusService)
        {
            pipelineStatusService = _pipelineStatusService;
        }


        public ActionResult Index()
        {
            PipelineStatusListDTO pipelineStatusList=new PipelineStatusListDTO();
            
            List<PipelineStatusDTO> pipelineList = new List<PipelineStatusDTO>();
            int pipelineID = 0;

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);

            //int companyID = Session["CompanyId"] != null ? int.Parse(Session["CompanyId"].ToString()) : 0;
            if (Request["pipelineId"] != null)
            {
                pipelineID = int.Parse(Request["pipelineId"]);
            }
            else
            {
                //TODO:
                //var PipeLine = BLLPipeline.SearchMappedPipeline("", companyID, 1, 15).FirstOrDefault();
                //if (PipeLine != null)
                //{
                //    pipelineID = PipeLine.ID;
                //}
            }
            pipelineStatusList.pipelineId = pipelineID;
            var list = pipelineStatusService.GetPipelineStatus(pipelineID);
            pipelineStatusList.PipelineStatusList = list.ToList();
            return View(pipelineStatusList); 
        }

        [HttpPost]
        public ActionResult Index(PipelineStatusListDTO model)
        {
            List<PipelineStatusDTO> pipelineList = new List<PipelineStatusDTO>();
            model.PipelineStatusList = new List<PipelineStatusDTO>();
            return View(model);
        }

   }
}