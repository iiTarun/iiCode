using Microsoft.AspNet.Identity;
using Nom.ViewModel;
using Nom1Done.Model;
using Nom1Done.Service.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class ContractsController : BaseController
    {
        IContractService _IContractService;
        //IPipelineService _IPipelineService;
        ILocationService _ILocationService;
        ImetadataRequestTypeService _ImetadataRequestTypeService;
      
        public ContractsController(IContractService IContractService, IPipelineService IPipelineService, ILocationService ILocationService, ImetadataRequestTypeService ImetadataRequestTypeService):base(IPipelineService)
        {
            _IContractService = IContractService;
            //_IPipelineService = IPipelineService;
            _ILocationService = ILocationService;
            _ImetadataRequestTypeService = ImetadataRequestTypeService;
        }


        public ActionResult Index(string pipelineDuns)
        {
            if (TempData["status"] != null)
            {
                ViewBag.Status = TempData["status"] + "";
            }
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ContractListDTO _contractListModel = new ContractListDTO();

            //var pipelineData = _IPipelineService.GetPipeline(pipelineDuns);           
            _contractListModel.PipelineDuns = pipelineDuns;

            string companyId = identity.Claims.Where(c => c.Type == "CompanyId")
                              .Select(c => c.Value).SingleOrDefault();
            
            int companyID = String.IsNullOrEmpty(companyId) ? 0 : int.Parse(companyId);
            _contractListModel.ContractList= _IContractService.GetContracts(_contractListModel.PipelineDuns, companyID).ToList();
           // _contractListModel.ContractList =  UpdateContractLoactionName(_contractListModel.ContractList);
            var pipes = GetPipelines();
            var pipe = pipes.Where(a => a.DUNSNo == pipelineDuns).FirstOrDefault();
            _contractListModel.PipelineDetails = pipe.Name;
             return View(_contractListModel);           
        }

      

        // GET: Contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractsDTO contract = _IContractService.GetContractOnId(id.Value);        
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create(string pipelineDuns)
        {
            ContractsDTO model = new ContractsDTO();

            model.PipeDuns = pipelineDuns;
            model.ValidUpto = DateTime.Now.Date;
            var reqTypeList= _ImetadataRequestTypeService.GetRequestType();
             ViewBag.RequestType = reqTypeList; 
            return View(model);
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(ContractsDTO model)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();
            var userId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);

            model.CreatedBy =userId;
            model.ModifiedBy = userId;
            model.ModifiedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ShipperID=companyID;
            model.IsActive = true;           
            bool isCreated = _IContractService.AddContract(model);
            if (isCreated)
                return RedirectToAction("Index", new { pipelineDuns = model.PipeDuns });
            var reqTypeList = _ImetadataRequestTypeService.GetRequestType();
            ViewBag.RequestType = reqTypeList;
            return View(model);
        }





        /// <summary>
        /// save contract on fly
        /// </summary>
        /// <param name="ContractNumber"></param>
        /// <param name="FuelPercentage"></param>
        /// <returns></returns>
        public bool OnflyContractSave(string ContractNumber, string FuelPercentage,string pipelineDuns)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);
            var userId = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();

            ContractsDTO model = new ContractsDTO();
            model.RequestNo = ContractNumber;
            if (string.IsNullOrEmpty(FuelPercentage))
            {
                model.FuelPercentage = 0;
            }
            else
            {
                model.FuelPercentage = Convert.ToDecimal(FuelPercentage);
            }
            model.PipeDuns = pipelineDuns;
            model.CreatedBy = userId;
            model.ModifiedBy = model.CreatedBy;
            model.ModifiedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
           
            model.ShipperID = companyID;
            model.IsActive = true;
            model.ValidUpto = DateTime.Now.AddYears(1);
            bool isCreated = _IContractService.AddContract(model);
            if (isCreated)
                return true;
            else
                return false;           
        }

        
        public ActionResult Edit(string pipelineDuns, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContractsDTO model =_IContractService.GetContractOnId(id.Value);
            model.PipeDuns = pipelineDuns;
            var reqTypeList = _ImetadataRequestTypeService.GetRequestType();
            ViewBag.RequestType = reqTypeList;
            if (model == null)
            {
                return HttpNotFound();
            }
            else {
                bool isContractUsed = _IContractService.IsContractUsed(model.RequestNo);
                if (isContractUsed)
                {
                    TempData["status"] = "Can't Edit. This is a Used-Contract.";
                    return RedirectToAction("Index", new { pipelineDuns = model.PipeDuns });
                }
                else
                {
                    return View(model);
                }
            }           
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ContractsDTO model)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;           
            model.CreatedBy = identity.Claims.Where(c => c.Type == "UserId")
                               .Select(c => c.Value).SingleOrDefault();
            model.ModifiedBy = model.CreatedBy;
            model.ModifiedDate = DateTime.Now;
           
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);

            model.ShipperID = companyID;
            model.IsActive = true;
            if (ModelState.IsValid)
            {
                bool isCreated = _IContractService.UpdateContract(model);
                if (isCreated)
                    return RedirectToAction("Index", new { pipelineDuns = model.PipeDuns });
            }
            var reqTypeList = _ImetadataRequestTypeService.GetRequestType();
            ViewBag.RequestType = reqTypeList;
            return View(model);
        }

        // GET: Contracts/Delete/5
        public ActionResult Delete(string pipelineDuns, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractsDTO model =_IContractService.GetContractOnId(id.Value);  
            
            
            if (model == null)
            {
                return HttpNotFound();
            }
            else {
                bool isContractUsed =_IContractService.IsContractUsed(model.RequestNo);
                if (isContractUsed)
                {
                    TempData["status"] = "Can't delete. This is a Used-Contract.";
                    return RedirectToAction("Index", new { pipelineDuns });
                }
                else {
                    return View(model);
                }
            }
           
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContractsDTO model = _IContractService.GetContractOnId(id);                   
            bool isDeleted = _IContractService.RemoveContract(id);
            if (isDeleted)
            {
                return RedirectToAction("Index", new { pipelineDuns = model.PipeDuns });
            }
            return View(model);
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //       _IContractService.Dispose(disposing);
        //    }
        //    base.Dispose(disposing);
        //}


        public PartialViewResult NotimationsPartials(string popUpFor, string pipelineDuns)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyID = String.IsNullOrEmpty(company) ? 0:int.Parse(company);

            List<LocationsDTO> locationList = new List<LocationsDTO>();
            NominationPartialDTO model = new NominationPartialDTO();
            //int companyId = Convert.ToInt32(Session["ShipperCompanyId"]);
            string partialView = string.Empty;           
            model.PopUpFor = popUpFor;
            model.Locations = new List<LocationsDTO>();
            string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
            RestClient clientLocation = new RestClient(apiBaseUrl + "/api/Location/");
            var request = new RestRequest(string.Format("GetLocationListByPipeDuns"), Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("pipelineDuns", pipelineDuns);
            var response = clientLocation.Execute<List<LocationsDTO>>(request);

            locationList = response.Data != null ? response.Data : new List<LocationsDTO>();   // _ILocationService.GetLocations(pipelineDuns).ToList();
            if (popUpFor == "RecLoc")
            {
                model.Locations = locationList.Where(a => (a.RDB == "R" || a.RDB=="B")).ToList();
            }
            else {
                model.Locations = locationList.Where(a => (a.RDB == "D" || a.RDB == "B")).ToList();
            }
            partialView = "~/Views/Contracts/_LocationPopUp.cshtml";
            return PartialView(partialView, model);
        }
    }
}
