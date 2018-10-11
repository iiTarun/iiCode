using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Model;
using System.Xml;
using UPRD.Service.Interface;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
    public class BaseController : Controller

    {
        private readonly IUprdPipelineService pipelineService;
        IClientEnvironmentSettingsService _ClientSettingsService;
        IDashNominationStatusService _dashNominationStatusService;


        public BaseController(IUprdPipelineService pipelineService, IClientEnvironmentSettingsService ClientSettingsService, IDashNominationStatusService dashNominationStatusService)
        {
            this.pipelineService = pipelineService;
            this._ClientSettingsService = ClientSettingsService;
            this._dashNominationStatusService = dashNominationStatusService;

        }



        public PartialViewResult TopNavBarPartail()
        {
            PipelineByUserDTO pipeByuser = new PipelineByUserDTO();



            pipeByuser.UserID = GetLoggedInUserId();
            //pipeByuser.ShipperID = GetCurrentCompanyID();

            //var list = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).OrderBy(a => a.Name).ToList();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //ViewBag.ShipperDetails = identity.Claims.Where(c => c.Type == "ShipperDetails")
            //                   .Select(c => c.Value).SingleOrDefault() + " (" + identity.Claims.Where(c => c.Type == "ShipperDuns")
            //                   .Select(c => c.Value).SingleOrDefault() + ")";
            ViewBag.UserName = identity.Claims.Where(c => c.Type == "UserName")
                               .Select(c => c.Value).SingleOrDefault();


            var data = pipelineService.GetAllActivePipeline();

            if (data != null)
            {
                var PipelineDuns = (Request.QueryString["pipelineDuns"]) == null ? Convert.ToString(data.Select(a => a.DUNSNo).FirstOrDefault()) : Request["pipelineDuns"];
                var PipelineType = data.Where(a => a.DUNSNo == PipelineDuns).Select(a => a.TempItem).FirstOrDefault();

                ViewBag.PipelineDropdown = new SelectList(data, "TempItem", "Name", PipelineType);
            }
            else
            {
                ViewBag.PipelineDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            string Absoluteurl = HttpContext.Request.Url.AbsoluteUri;
            var shipperDuns = HttpUtility.ParseQueryString(Absoluteurl.Substring(
                                 new[] { 0, Absoluteurl.IndexOf('?') }.Max()
                         )).Get("shipperDuns");

            var ShipperCompanies = _ClientSettingsService.GetShipperComapnies();
            ViewBag.ShipperDropdown = new SelectList(ShipperCompanies, "ShipperDuns", "ShipperNameWithDuns", shipperDuns);
            if (!string.IsNullOrEmpty(shipperDuns))
            {
                var EngineStatus = _dashNominationStatusService.GetEngineStatusbyShipperDuns(shipperDuns);
                ViewBag.EngineStatus = Convert.ToBoolean(EngineStatus);
            }
            else
            {
                ViewBag.EngineStatus = false;
            }
            return PartialView("_TopNavBarPartial");

        }



        public string GetLoggedInUserId()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                            .Select(c => c.Value).SingleOrDefault();

            return UserId;
        }


        public ShipperReturnByIdentity GetValueFromIdentity()
        {
            ShipperReturnByIdentity value = new ShipperReturnByIdentity();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            value.CompanyId = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();
            value.ShipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                              .Select(c => c.Value).SingleOrDefault();
            value.UserId = identity.Claims.Where(c => c.Type == "UserId")
                            .Select(c => c.Value).SingleOrDefault();
            value.UserName = identity.Claims.Where(c => c.Type == "UserName")
                            .Select(c => c.Value).SingleOrDefault();
            value.ShipperName = identity.Claims.Where(c => c.Type == "ShipperDetails")
                            .Select(c => c.Value).SingleOrDefault();
            value.FirstSelectedPipeIdByUser = Convert.ToInt16(identity.Claims.Where(a => a.Type == "FirstSelectedPipeIdByUser")
                .Select(a => a.Value).SingleOrDefault());
            return value;

        }



        #region get file is in use or not
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        #endregion

    }
}
