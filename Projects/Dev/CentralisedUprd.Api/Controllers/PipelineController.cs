using CentralisedUprd.Api.UPRD.DTO;
using System.Linq;
using System.Web.Http;
using Nom1Done.Data.Repositories;
using System;
using CentralisedUprd.Api.Repositories;

namespace CentralisedUprd.Api.Controllers
{
    public class PipelineController : ApiController
    {
        readonly ApplicationLogRepository Logger = new ApplicationLogRepository();

        [HttpPost]
        public IHttpActionResult GetPipelinesByUser([FromBody] PipelineByUserDTO pipelineByUser)
        {
            try
            {
                PipelineRepository repo = new PipelineRepository();
                var result = repo.GetPipelinesByUser(pipelineByUser).ToList();
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(System.Net.HttpStatusCode.NoContent);
                }
            }
            catch(Exception ex)
            {
                Logger.AppLogManager(ex.Source, "PipelineController", ex.Message);
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }
            
        }
    }
}