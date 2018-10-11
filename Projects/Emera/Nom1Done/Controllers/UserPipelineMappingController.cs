using Nom1Done.Data;
using Nom1Done.DTO;
using Nom1Done.Model.Models;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    public class UserPipelineMappingController : BaseController
    {
        private IPipelineService pipelineService;
        public UserPipelineMappingController(IPipelineService pipelineService) : base(pipelineService)
        {
            this.pipelineService = pipelineService;
        }

        // GET: UserPipelineMapping
        public ActionResult Index()
        {
            var context = new NomEntities();
            var allUsers = context.Users.ToList();
            ViewBag.UsersDropdown = new SelectList(allUsers, "Id", "UserName");

            UserPipelineMappingDTO model = new UserPipelineMappingDTO();
            model.pipelines = context.Pipeline.Select(a => new PipelineListDTO
            {
                DunsNo = a.DUNSNo,
                Name = a.Name,
                pipelineId = a.ID
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserPipelineMappingDTO model)
        {
            var context = new NomEntities();

            //delete all exixting entries if any 
            var exixtingEntries = context.UserPipelineMapping.Where(a => a.userId == model.userId).ToList();
            context.UserPipelineMapping.RemoveRange(exixtingEntries);
            context.SaveChanges();


            var pipelines = model.pipelines.Where(a => a.IsSelected == true);
            List<UserPipelineMapping> list = new List<UserPipelineMapping>();
            foreach (var item in pipelines)
            {
                UserPipelineMapping obj = new UserPipelineMapping();
                obj.userId = model.userId;
                obj.pipelineId = item.pipelineId;
                obj.shipperId = GetCurrentCompanyID();
                list.Add(obj);
            }
            context.UserPipelineMapping.AddRange(list);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}