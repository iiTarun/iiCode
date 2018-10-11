using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Service;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly IPipelineService pipelineService;
        private readonly IPNTNominationService pntNominationService;
        public DashboardController(IPNTNominationService pntNominationService,IDashboardService dashboardService, IPipelineService pipelineService) : base(pipelineService)
        {
            this.dashboardService = dashboardService;
            this.pipelineService = pipelineService;
            this.pntNominationService = pntNominationService;
        }
       // [Authorize]
        public ActionResult Index()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string companyId = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            
            var id = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            int companyID = String.IsNullOrEmpty(companyId) ? 0:int.Parse(companyId);

            //var sd = pipelineService.GetAllPipelineList().OrderBy(a=>a.Name).ToList();
            DashboardModel Dashboard = new DashboardModel();
            NominationSearchCriteria criteria = new NominationSearchCriteria();
            // var pipelineDuns = Request["pipelineDuns"] == null ? dashboardService.GetFirstPipelineByUser(id,GetCurrentCompanyID()).DUNSNo : Request["pipelineDuns"];
            var pipelineDuns = "";
            if (Request["pipelineDuns"] == null)
            {
              var pipes =  GetPipelines();
                pipelineDuns = pipes.Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty ;
            }
            else {
                pipelineDuns = Request["pipelineDuns"];
            } 
            ViewBag.pipelineDuns = pipelineDuns;
            Dashboard.RejectedNomList = dashboardService.GetRejectedNomination(pipelineDuns, id).ToList().OrderByDescending(a => a.FlowDate).Take(10).ToList();

            string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
            RestClient client = new RestClient(apiBaseUrl + "/api/Swnt/");
             var request = new RestRequest(string.Format("FilterNotices"), Method.GET);
             request.AddQueryParameter("PipelineDuns", pipelineDuns);
             request.AddQueryParameter("isCritical", "true");
             request.AddQueryParameter("startDate", DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy"));
             request.AddQueryParameter("endDate", DateTime.Now.AddHours(24).ToString("MM/dd/yyyy"));
             var response = client.Execute<List<BONotice>>(request);            
             Dashboard.BONoticeCriteriaList = response.Data!=null ? response.Data : (new List<BONotice>()) ; // noticesService.FilterNotices(pipelineDuns, true, DateTime.Now.AddDays(-30), DateTime.Now.AddHours(24));


            var request1 = new RestRequest(string.Format("FilterNotices"), Method.GET);
            request1.AddQueryParameter("PipelineDuns", pipelineDuns);
            request1.AddQueryParameter("isCritical", "false");
            request1.AddQueryParameter("startDate", DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy"));
            request1.AddQueryParameter("endDate", DateTime.Now.AddHours(24).ToString("MM/dd/yyyy"));
            var response1 = client.Execute<List<BONotice>>(request1);        
            Dashboard.BONonNoticeCriteriaList = response1.Data!=null ? response1.Data : (new List<BONotice>()) ;   // noticesService.FilterNotices(pipelineDuns, false, DateTime.Now.AddDays(-30), DateTime.Now.AddHours(24));
                                          

            return View(Dashboard);
        }

        public PartialViewResult NotimationsPartials(string NMQRid)
        {
            NominationPartialDTO model = new NominationPartialDTO();        

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();
            int companyID = String.IsNullOrEmpty(company) ? 0 : int.Parse(company);
            string partialView = string.Empty;
            model.StatusReason = dashboardService.GetRejectionReason(NMQRid);
            partialView = "~/Views/PNTNominations/_StatusReasonPopUp.cshtml";
            return PartialView(partialView, model);
        }

    }
}