using Microsoft.AspNet.Identity;
using Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class PipelineController : BaseController
    {
        // GET: Pipeline
        IPipelineService pipelinesService;

        public PipelineController(IPipelineService _pipelinesService):base(_pipelinesService)
        {
            pipelinesService = _pipelinesService;
        }
        public ActionResult Index()
        {
            PipelineListDTO model = new PipelineListDTO();
            model.PipelineList = new List<PipelineDTO>();
            model.PipelineList = pipelinesService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).ToList();
            return this.View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
            {
                var pipeline = pipelinesService.GetPipeline(id.Value);
                if (pipeline == null)
                    return HttpNotFound();
                return View(pipeline);
            }
        }
        [HttpPost]
        public ActionResult Edit(PipelineDTO pipe)
        {
            if (ModelState.IsValid)
            {
                if (pipelinesService.UpdatePipeline(pipe))
                    return RedirectToAction("Index");
            }
            return View(pipe);
        }
    }
}