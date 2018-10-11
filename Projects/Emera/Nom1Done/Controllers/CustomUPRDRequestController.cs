using Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nom1Done.Service.Interface;
using Nom1Done.DTO;

namespace Nom1Done.Controllers
{
    public class CustomUPRDRequestController : BaseController
    {
        private readonly IPipelineService pipelineService;
        private readonly IPipelineEDISettingService pipelineEDISettingService;
        private readonly IUprdStatusService uprdStatusService;
        private readonly INotifierEntityService _notifierEntityService;
        public CustomUPRDRequestController(IPipelineService pipelineService, IPipelineEDISettingService pipelineEDISettingService, IUprdStatusService uprdStatusService, INotifierEntityService notifierEntityService) : base(pipelineService)
        {
            this.pipelineService = pipelineService;
            this.pipelineEDISettingService = pipelineEDISettingService;
            this.uprdStatusService = uprdStatusService;
            this._notifierEntityService = notifierEntityService;
        }

        // GET: CustomUPRDRequest
        public ActionResult Index(int? pipelineId)
        {
            CustomUPRDReqDTO dto = new CustomUPRDReqDTO();
            PipelineDTO pipe = null;
            if (pipelineId.HasValue)
                 pipe = pipelineService.GetPipeline(pipelineId.Value);
            if (pipe != null)
            {
                ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
                ViewBag.NotifierEntity = _notifierEntityService.GetNotifierEntityOfUprdStatus();
                var pipelineEDISetting = pipelineEDISettingService.GetPipelineSetting((int)DataSet.Upload_of_Requests_for_Download_of_Posted_Datasets, pipe.DUNSNo, currentIdentityValues.ShipperDuns);
                if(pipelineEDISetting!=null)
                {
                    dto.EndDate = pipelineEDISetting.EndDate;
                    dto.StartDate = pipelineEDISetting.StartDate;
                    dto.pipelineId = pipe.ID;
                    dto.pipeDuns = pipe.DUNSNo;
                    dto.shipperDuns = currentIdentityValues.ShipperDuns;
                    if(pipelineEDISetting.ForOacy)
                        dto.RequestFor = 1;
                    if(pipelineEDISetting.ForUnsc)
                        dto.RequestFor = 2;
                    if(pipelineEDISetting.ForSwnt)
                        dto.RequestFor = 3;
                }
            }
            return View(dto);
        }
        [HttpPost]
        public ActionResult Index(CustomUPRDReqDTO custUPRDReqDTO)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            var pipelineEDISetting = pipelineEDISettingService.GetPipelineSetting(11, custUPRDReqDTO.pipeDuns, currentIdentityValues.ShipperDuns);
            if (pipelineEDISetting != null)
            {
                pipelineEDISetting.StartDate = custUPRDReqDTO.StartDate;
                pipelineEDISetting.EndDate = custUPRDReqDTO.EndDate;
                pipelineEDISetting.SendManually = true;

                switch (custUPRDReqDTO.RequestFor)
                {
                    case 1:
                        pipelineEDISetting.ForOacy = true;
                        pipelineEDISetting.ForUnsc = false;
                        pipelineEDISetting.ForSwnt = false;
                        break;
                    case 2:
                        pipelineEDISetting.ForOacy = false;
                        pipelineEDISetting.ForUnsc = true;
                        pipelineEDISetting.ForSwnt = false;
                        break;
                    case 3:
                        pipelineEDISetting.ForOacy = false;
                        pipelineEDISetting.ForUnsc = false;
                        pipelineEDISetting.ForSwnt = true;
                        break;
                    default:
                        pipelineEDISetting.ForOacy = false;
                        pipelineEDISetting.ForUnsc = false;
                        pipelineEDISetting.ForSwnt = false;
                        pipelineEDISetting.SendManually = false;
                        break;
                }
                if (pipelineEDISettingService.UpdatePipelineEDISetting(pipelineEDISetting))
                {
                   
                }
            }
            return View(custUPRDReqDTO);
        }

        //[HttpGet]
        //public ActionResult GetUpdatedUprdList(DateTime date)
        //{
        //    List<UPRDStatusDTO> uprdList = new List<UPRDStatusDTO>();
        //    uprdList = uprdStatusService.GetUprdStatusOnDate(date);
        //    return PartialView("_uprdStatusList", uprdList);
        //}
    }
}