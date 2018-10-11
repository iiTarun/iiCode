using Nom.ViewModel;
using Nom1Done.CustomSerialization;
using Nom1Done.Data;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UserPipelineMappingController : BaseController
    {
        private IPipelineService pipelineService;
        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private readonly RestClient pipelines = new RestClient(apiBaseUrl + "/api/UserPipelineMapping/");
        public UserPipelineMappingController(IPipelineService pipelineService) : base(pipelineService)
        {
            this.pipelineService = pipelineService;
        }

        // GET: UserPipelineMapping
        public ActionResult Index(String userID = "", string Search = "",string PipeName="")
        {
          
            UserPipelineDTO modal = new UserPipelineDTO();
            var context = new NomEntities();

            var allUsers = context.Users.ToList();       
            if (string.IsNullOrEmpty(userID))
            {             
                modal.userPipelineMappingDTO = new List<UserPipelineMappingDTO>();
                ViewBag.UsersDropdown = new SelectList(allUsers, "Id", "UserName");
        
                
                if (modal.ShipperId == null)
                {
                    modal.ShipperId = GetCurrentCompanyID();
                }
                if (Search != "")
                {
                    modal.userPipelineMappingDTO = modal.userPipelineMappingDTO.Where(t => t.PipeName.ToLower().Contains(PipeName) || t.PipeName.ToUpper().Contains(PipeName)).ToList();
                }

                    return View(modal);
            }
            else
            {
                if (!string.IsNullOrEmpty(userID))
                {
                   
                        var request = new RestRequest(string.Format("GetAllPipelineMappingsByUser?userID=" + userID), Method.GET) { RequestFormat = DataFormat.Json };
                        request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                        var response = pipelines.Execute<List<UserPipelineMappingDTO>>(request);
                        ViewBag.UsersDropdown = new SelectList(allUsers, "Id", "UserName", userID);
                        modal.ShipperId = GetCurrentCompanyID();
                        modal.UserId = userID;
                        modal.userPipelineMappingDTO = response.Data;
                 
                    if (Search != "")                 
                    {
                        PipeName= PipeName.ToLower();
                        modal.userPipelineMappingDTO = modal.userPipelineMappingDTO.Where(t => t.PipeName.ToLower().Contains(PipeName)).ToList();
                    }
                } 
                else
                {
                    modal.userPipelineMappingDTO = new List<UserPipelineMappingDTO>();
                }


                return View(modal);

            }
        }


        [HttpPost]
        public ActionResult GetPermissionByUserId(string userID)
        {
            UserPipelineDTO modal = new UserPipelineDTO();
            if (!string.IsNullOrEmpty(userID))
            {             
                var request = new RestRequest(string.Format("GetAllPipelineMappingsByUser?userID=" + userID), Method.GET) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                var response = pipelines.Execute<List<UserPipelineMappingDTO>>(request);
                modal.ShipperId = GetCurrentCompanyID();
                modal.userPipelineMappingDTO = response.Data;
              
            }
            else {
                modal.userPipelineMappingDTO = new List<UserPipelineMappingDTO>();
            }
            return PartialView("_tblUserMapping", modal);
        }


        [HttpPost]
        public ActionResult SaveUserPermissions(UserPipelineDTO userPipelineDTO, string btnSearch,string SearchText)
        {
            if(btnSearch != null)
            {
            
                return RedirectToAction("Index", new { userID = userPipelineDTO.UserId,Search="True", PipeName=SearchText });
            }
            else
            {
                List<UserPipelineMappingDTO> lstUserPipelineMappingDTO = userPipelineDTO.userPipelineMappingDTO.ToList();
                if (lstUserPipelineMappingDTO != null)
            {
                var request = new RestRequest(string.Format("SavePermissions"), Method.POST) { RequestFormat = DataFormat.Json };
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                request.AddJsonBody(userPipelineDTO);
                var response = pipelines.Execute(request);
                UserPipelineDTO userPipeline = new UserPipelineDTO();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return RedirectToAction("Index", new { userID = userPipelineDTO.UserId });
            }
         
                return RedirectToAction("Index", new { userID = userPipelineDTO.UserId });
          
            }
        }
    }
}