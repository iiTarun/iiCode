using CentralisedUprd.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CentralisedUprd.Api.Controllers
{
    public class UserPipelineMappingController : ApiController
    {
        readonly ApplicationLogRepository Logger = new ApplicationLogRepository();
        UserPipelineMappingRepository UserPipeRepo = new UserPipelineMappingRepository();

        [HttpGet]
        public IHttpActionResult GetAllPipelineMappingsByUser([FromUri]string userID)
        {
            try
            {

                var AllPermissions = UserPipeRepo.GetAllPermissionsByUser(userID);
                if (AllPermissions != null)
                    return Ok(AllPermissions);
                else
                    return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch(Exception ex)
            {
                Logger.AppLogManager(ex.Source, "ApplicationLogRepository", ex.Message);
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }

        }

        [HttpPost]
        public IHttpActionResult SavePermissions([FromBody]UPRD.DTO.UserPipelineDTO userPipeDTO)
        {
            try
            {
                if(userPipeDTO.userPipelineMappingDTO.Count > 0)
                {
                    var IsSaved = UserPipeRepo.SaveUserPermissions(userPipeDTO);
                    if (IsSaved)
                        return Ok();
                    else
                        return StatusCode(System.Net.HttpStatusCode.InternalServerError);
                }
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                Logger.AppLogManager(ex.Source, "ApplicationLogRepository", ex.Message);
                return StatusCode(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult HasPipelines([FromUri]string userID,[FromUri] int ShipperID)
        {
            try
            {

                var IsPipeAssigned = UserPipeRepo.HasPipeline(userID, ShipperID);
                if (IsPipeAssigned)
                    return Ok(true);
                else
                    return Ok(false);
            }
            catch (Exception ex)
            {
                Logger.AppLogManager(ex.Source, "UserPipelineMappingController", ex.Message);
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }

        }
    }
}