using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Services.Interface;


namespace Nom1Done.Admin.Controllers
{
    public class TransactiontypesController : Controller
    {
        // GET: CounterParty
        public TransactiontypesController()
        {

        }
        //IUprdPipelineService _pipelineService;
        private IUprdTransactionTypeService locService;
        public TransactiontypesController(IUprdTransactionTypeService locRepository, IUprdPipelineService pipelineService)
        {
            this.locService = locRepository;
            //this._pipelineService = pipelineService;
        }
        public ActionResult Index()
        {


            //MetaDataTransactionTypesDTO model = new MetaDataTransactionTypesDTO();
            //var model = locService.GetTransactions();
            //if (Request["pipelineDuns"] == null || string.IsNullOrEmpty(pipelineDuns))
            //{
            //    var pipes = _pipelineService.GetAllActivePipeline();
            //    pipelineDuns = pipes.ToList().Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
            //}
            //else
            //{
            //    pipelineDuns = Request["pipelineDuns"] != null ? Request["pipelineDuns"].ToString() : pipelineDuns;
            //}
            //model.pipelineDuns = pipelineDuns;
            return View();
        }
        [HttpPost]
        public ActionResult PostData()
        {
            //if (string.IsNullOrEmpty(pipelineDuns))
            //{
            //    var pipes = _pipelineService.GetAllActivePipeline();
            //    pipelineDuns = pipes.ToList().Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
            //}
            var detail = locService.GetTransactions();
            return Json(new { data = detail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransactions()
        {
            var data = locService.GetTransactions();
            return View(data);
        }


        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {

            if (id == 0)
            {
                return View(new MetaDataTransactionTypesDTO());
            }
            else
            {
                return View(locService.GetTransactionByid(id));
            }



        }
        [HttpPost]
        public ActionResult AddorEdit(MetaDataTransactionTypesDTO transaction)
        {
            if (ModelState.IsValid)
            {

                string Message = locService.UpdateTransactionByID(transaction);
                locService.Save();

                return Json(new { success = true, message = Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("AddorEdit");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            locService.DeleteTransactionByID(id);
            return Json(new { success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Activate(int id)
        {
            locService.ActivateTrasaction(id);
            return Json(new { success = true, message = "Activate Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SyncData()
        {
            locService.ClientEnvironmentsetting();
            return Json(new { success = true, message = "Sync Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}