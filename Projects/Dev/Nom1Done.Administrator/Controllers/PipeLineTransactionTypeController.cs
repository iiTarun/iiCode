using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPRD.DTO;
using UPRD.Services.Interface;

namespace Nom1Done.Admin.Controllers
{
    public class PipeLineTransactionTypeController : Controller
    {
        private IUprdPipeTransTypeMapService _IPipeTransTypeMapService;
        IUprdPipelineService _pipelineService;
        private IUprdTransactionTypeService _transtypeService;


        public PipeLineTransactionTypeController(IUprdPipeTransTypeMapService IPipeTransTypeMapService, IUprdPipelineService pipelineService, IUprdTransactionTypeService transtypeService)
        {
            this._IPipeTransTypeMapService = IPipeTransTypeMapService;
            this._pipelineService = pipelineService;
            this._transtypeService = transtypeService;

        }

        public ActionResult Index(string pipelineDuns)
        {
            Pipeline_TransactionType_MapDTO model = new Pipeline_TransactionType_MapDTO();
            if (Request["pipelineDuns"] == null || string.IsNullOrEmpty(pipelineDuns))
            {
                var pipes = _pipelineService.GetAllActivePipeline();
                pipelineDuns = pipes.ToList().Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
            }
            else
            {
                pipelineDuns = Request["pipelineDuns"] != null ? Request["pipelineDuns"].ToString() : pipelineDuns;
            }
            model.pipelineDuns = pipelineDuns;
            return View(model);
        }
        [HttpPost]
        public ActionResult PostData(string pipelineDuns)
        {
            List<Pipeline_TransactionType_MapDTO> detail = new List<Pipeline_TransactionType_MapDTO>();
            if (string.IsNullOrEmpty(pipelineDuns))
            {
                var pipes = _pipelineService.GetAllActivePipeline();
                pipelineDuns = pipes.ToList().Count > 0 ? pipes.FirstOrDefault().DUNSNo : string.Empty;
            }

            detail = _IPipeTransTypeMapService.GetTransactions(pipelineDuns).ToList();
            return Json(new { data = detail }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddorEdit(int id = 0)
        {

            Pipeline_TransactionType_MapDTO ptm = new Pipeline_TransactionType_MapDTO();

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Contract", Value = "C" });
            items.Add(new SelectListItem { Text = "Market", Value = "M" });
            items.Add(new SelectListItem { Text = "Non-Pathed", Value = "NP" });
            items.Add(new SelectListItem { Text = "Pathed", Value = "P" });
            items.Add(new SelectListItem { Text = "Supply", Value = "S" });


            if (id == 0)
            {

                ViewBag.PathType = new SelectList(items, "Value", "Text");
                //ViewBag.Transtype = new SelectList(_transtypeService.GetTransactions(), "ID", "Name");
                ptm.MetaDataTransactionTypesDTO = _transtypeService.GetTransactions().ToList();
                return View(ptm);

            }
            else
            {

                ptm = _IPipeTransTypeMapService.GetTransactionByid(id);


                ViewBag.PathType = new SelectList(items, "Value", "Text", ptm.PathType);
                ptm.MetaDataTransactionTypesDTO = _transtypeService.GetTransactions().ToList();
                return View(ptm);
            }



        }
        [HttpPost]
        public ActionResult AddorEdit(Pipeline_TransactionType_MapDTO transaction, FormCollection fc)
        {
            //Pipeline_TransactionType_MapDTO Pipeline_TransactionTypeDTO = new Pipeline_TransactionType_MapDTO();
            //string pipelineDuns = fc["pipelineDuns"];
            //Pipeline_TransactionTypeDTO.PathType = fc["PathType"];
            //Pipeline_TransactionTypeDTO.MetaDataTransactionTypesDTO = Convert.ToInt32(fc["TransType"]);
            //List<SelectListItem> selectedItems = Pipeline_TransactionTypeDTO.MetaDataTransactionTypesDTO.Where(p =>  p.ID.Contains(int.Parse(transaction.))).ToList();




            //Pipeline_TransactionTypeDTO.PipeDuns = pipelineDuns;
            if (ModelState.IsValid)
            {
                string Message = _IPipeTransTypeMapService.UpdateTransactionByID(transaction);
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
            _IPipeTransTypeMapService.DeleteTransactionByID(id);
            return Json(new { success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Activate(int id)
        {
            _IPipeTransTypeMapService.ActivateTrasaction(id);
            return Json(new { success = true, message = "Activate Successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SyncData()
        {
            _IPipeTransTypeMapService.ClientEnvironmentsetting();
            return Json(new { success = true, message = "Sync Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}