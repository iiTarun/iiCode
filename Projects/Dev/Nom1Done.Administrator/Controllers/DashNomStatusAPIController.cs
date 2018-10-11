using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UPRD.DTO;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
    public class DashNomStatusAPIController : ApiController
    {
        IApplicationLogManagerService _ApplicationLogManagerService;
        IDashNominationStatusService _dashNominationStatusService;
        public DashNomStatusAPIController(IDashNominationStatusService DashNominationStatusService, IApplicationLogManagerService ApplicationLogManagerService)
        {
            this._dashNominationStatusService = DashNominationStatusService;
            this._ApplicationLogManagerService = ApplicationLogManagerService;

        }
        [HttpGet]
        public IHttpActionResult GetNominationStatusData(string shipperDuns)
        {
            try
            {               
                var dns = _dashNominationStatusService.GetDashNominationStatus(shipperDuns);
                if (dns != null)
                return Ok(dns);
                else
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _ApplicationLogManagerService.SaveAppLogManager(ex.Source, "ApplicationLogRepository", ex.Message);
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }
        }
    }
}
