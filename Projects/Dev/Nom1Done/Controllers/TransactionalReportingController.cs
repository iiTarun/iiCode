using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    public class TransactionalReportingController : Controller
    {
        // GET: TransactionalReporting
        private ITransactionalReportingService transReportService;
        IPipelineService pipelineService;
        public TransactionalReportingController(IPipelineService pipelineService,ITransactionalReportingService transReportService)
        {
            this.transReportService = transReportService;
            this.pipelineService = pipelineService;
        }

        public ActionResult Index(int pipelineId)
        {
            TransactionalReportDTOs model = new TransactionalReportDTOs();
            var pipelineDuns = pipelineService.GetDunsByPipelineID(pipelineId);
            model.PipeLineDuns = pipelineDuns;
            model.PipelineId = pipelineId;
            model.TransactionalReportDTOList = transReportService.GetAllTransactionalReport(pipelineDuns);
            return View(model);
        }
    }
}