using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nom.ViewModel;
using Nom1Done.CustomSerialization;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using RestSharp;
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
using System.Web.Routing;
using System.Xml;

namespace Nom1Done.Controllers
{
    public class BaseController : Controller
    {
        private readonly IPipelineService pipelineService;

        static string apiBaseUrl = ConfigurationManager.AppSettings.Get("BaseUrlOfUprdApi");
        private readonly RestClient pipelines = new RestClient(apiBaseUrl + "/api/Pipeline/");

        public BaseController(IPipelineService pipelineService)
        {
            this.pipelineService = pipelineService;


        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public bool isPermission { get; set; }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public List<PipelineDTO> GetPipelines()
        {
            PipelineByUserDTO pipeByuser = new PipelineByUserDTO()
            {
                UserID = GetLoggedInUserId(),
                ShipperID = GetCurrentCompanyID()
            };
            var request = new RestRequest(string.Format("GetPipelinesByUser"), Method.POST) { RequestFormat = DataFormat.Json };
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(pipeByuser);
            var response = pipelines.Execute<List<PipelineDTO>>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Data != null)
            {
                return response.Data;
            }
            else {
                return new List<PipelineDTO>();
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<PipelineDTO> list = new List<PipelineDTO>();
            list = GetPipelines();

            var FilterResponse = list.Where(x => x.IsNoms || x.IsUPRD);
            if (FilterResponse == null) {
                FilterResponse = new List<PipelineDTO>();                     
              }

            if (FilterResponse != null)
            {
                var PipelineDuns = Request["pipelineDuns"] == null ? Convert.ToString(FilterResponse.Select(a => a.DUNSNo).FirstOrDefault()) : Request["pipelineDuns"];

                string Absoluteurl = HttpContext.Request.Url.AbsoluteUri;
                var parpipelineDuns = HttpUtility.ParseQueryString(Absoluteurl.Substring(
                                     new[] { 0, Absoluteurl.IndexOf('?') }.Max()
                             )).Get("pipelineDuns");
                PipelineDTO objPipeline = FilterResponse.Where(y => y.DUNSNo == parpipelineDuns).FirstOrDefault();

                if (objPipeline == null)
                {
                    objPipeline = FilterResponse.FirstOrDefault();
                }

                if(Absoluteurl.Contains("MOperationalCapacity") || Absoluteurl.Contains("MUnsubscribedCapacity") ||
                    Absoluteurl.Contains("Notices"))
                {
                    
                        isPermission = (objPipeline.IsUPRD == true ? objPipeline.IsUPRD : false);
                    
                }
                else if(Absoluteurl.Contains("PathedNomination") || Absoluteurl.Contains("NonPathed") || Absoluteurl.Contains("PNTNominations") ||
                        Absoluteurl.Contains("SQTSSummary") || Absoluteurl.Contains("Batch"))
                    {
                       
                           isPermission = (objPipeline.IsNoms == true ? objPipeline.IsNoms : false);
                        
                    }
                    ViewBag.IsPermission = isPermission;

                    ViewBag.objPipeline = objPipeline;

                    base.OnActionExecuting(filterContext);
                }

        }

        public PartialViewResult TopNavBarPartail()
        {
            PipelineByUserDTO pipeByuser = new PipelineByUserDTO();                
                pipeByuser.UserID = GetLoggedInUserId();
                pipeByuser.ShipperID = GetCurrentCompanyID();

            //var list = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).OrderBy(a => a.Name).ToList();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ViewBag.ShipperDetails = identity.Claims.Where(c => c.Type == "ShipperDetails")
                               .Select(c => c.Value).SingleOrDefault()+" (" + identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault()+")";
            ViewBag.UserName = identity.Claims.Where(c => c.Type == "UserName")
                               .Select(c => c.Value).SingleOrDefault();
            
            ViewBag.UserRole = identity.Claims.Where(r => r.Type == "Roles").Select(r => r.Value).ElementAt(0); //"Admin";
            
            //if (User.IsInRole("Admin"))
            //    ViewBag.UserRole = "Admin";//identity.Claims.Where(r => r.Type == "Roles").Select(r => r.Value); //"Admin";
            //else
            //    ViewBag.UserRole = "SuperAdmin";
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                            .Select(c => c.Value).SingleOrDefault();
            IList<string> roleNames = UserManager.GetRoles(UserId);
            var request = new RestRequest(string.Format("GetPipelinesByUser"), Method.POST) { RequestFormat = DataFormat.Json };
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(pipeByuser);
            var response = pipelines.Execute<List<PipelineDTO>>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Data != null)
            {
                var PipelineDuns = Request["pipelineDuns"] == null ? Convert.ToString(response.Data.Select(a=>a.DUNSNo).FirstOrDefault()) : Request["pipelineDuns"];
                var PipelineType = response.Data.Where(a => a.DUNSNo == PipelineDuns).Select(a => a.TempItem).FirstOrDefault();
                ViewBag.PipelineDropdown = new SelectList(response.Data, "TempItem", "Name", PipelineType);
            }
            else
            {
                ViewBag.PipelineDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            }
                  
            if(!string.IsNullOrEmpty(GetLoggedInUserId()))
            {
                ViewBag.IsTwoFactorEnabled = UserManager.GetTwoFactorEnabled(GetLoggedInUserId());
            }
            else
            {
                ViewBag.IsTwoFactorEnabled = false;
            }
            return PartialView("_TopNavBarPartial");

        }
     
        public PartialViewResult PipelineDropdown()
        {
           // var list = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).ToList();
            PipelineByUserDTO pipeByuser = new PipelineByUserDTO()
            {
                UserID=GetLoggedInUserId(),
                ShipperID=GetCurrentCompanyID()
            };
            var request = new RestRequest(string.Format("GetPipelinesByUser"), Method.POST) { RequestFormat = DataFormat.Json };
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(pipeByuser);
            var response = pipelines.Execute<List<PipelineDTO>>(request);
            if(response.Data!=null)
            {
                ViewBag.PipelineDropdown = new SelectList(response.Data, "TempItem", "Name");
            }
            return PartialView("_PipelinesDropdown");
        }

        public int GetCurrentCompanyID()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            return String.IsNullOrEmpty(company) ? 0 : int.Parse(company);
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
        protected List<SummaryDTO> UpdateCounterPartyName(List<SummaryDTO> list)
        {
            try
            {
                List<SummaryDTO> newList = new List<SummaryDTO>();
                var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfo = new FileInfo(path);
                if (System.IO.File.Exists(path) && !IsFileLocked(fileInfo))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(path);
                    XmlNodeList nodeList = xml.GetElementsByTagName("CounterPartiesDTO");
                    foreach (var item in list)
                    {
                        string UpName = nodeList.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpIdentifier)
                        .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                        string DwName = nodeList.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.DwnIdentifier)
                        .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                        item.UpStreamName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpStreamName;
                        item.DownStreamName = !string.IsNullOrEmpty(DwName) ? DwName : item.DownStreamName;
                        newList.Add(item);
                    }
                }
                else
                {
                    newList = list;
                }
                return newList;
            }
            catch(Exception ex)
            {
                return list;
            }
        }

        protected List<PathedNomDetailsDTO> UpdatePathedCounterPartyAndLoactionName(List<PathedNomDetailsDTO> list)
        {
            try
            {
                bool cpExist = false;
                bool locExist = false;
                List<PathedNomDetailsDTO> newList = new List<PathedNomDetailsDTO>();
                var pathCP = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfoCP = new FileInfo(pathCP);
                var pathLoc = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "Location.xml");
                FileInfo fileInfoLoc = new FileInfo(pathLoc);
                if (System.IO.File.Exists(pathCP) && !IsFileLocked(fileInfoCP))
                    cpExist = true;
                if (System.IO.File.Exists(pathLoc) && !IsFileLocked(fileInfoLoc))
                    locExist = true;
                if (cpExist || locExist)
                {
                    XmlNodeList nodeListLoc = null;
                    XmlNodeList nodeListCP = null;

                    XmlDocument xmlCP = new XmlDocument();
                    XmlDocument xmlLoc = new XmlDocument();
                    List<XmlNode> nodesCP = null;
                    List<XmlNode> nodesLoc = null;
                    if (cpExist)
                    {
                        xmlCP.Load(pathCP);
                        nodeListCP = xmlCP.GetElementsByTagName("CounterPartiesDTO");
                        nodesCP = new List<XmlNode>(nodeListCP.Cast<XmlNode>());
                    }
                    if (locExist)
                    {
                        xmlLoc.Load(pathLoc);
                        nodeListLoc = xmlLoc.GetElementsByTagName("LocationsDTO");
                        nodesLoc = new List<XmlNode>(nodeListLoc.Cast<XmlNode>());
                    }
                    foreach (var item in list)
                    {
                        if (cpExist)
                        {
                            string UpName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            string DwName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DownID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpID;
                            item.DownName = !string.IsNullOrEmpty(DwName) ? DwName : item.DownID;
                        }
                        if (locExist)
                        {
                            string recLoc = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.RecLocID)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            string delLoc = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DelLocID)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.RecLocation = !string.IsNullOrEmpty(recLoc) ? recLoc : item.RecLocID;
                            item.DelLoc = !string.IsNullOrEmpty(delLoc) ? delLoc : item.DelLocID;
                        }
                        newList.Add(item);
                    }
                }
                else
                    newList = list;
                return newList;
            }
            catch(Exception ex)
            {
                return list;
            }
        }

        protected BatchDetailDTO UpdateCounterPartyNameAndLocNameInBatchDetail(BatchDetailDTO batchDetail)
        {
            try
            {
                bool cpExist = false;
                bool locExist = false;
                List<PathedNomDetailsDTO> newList = new List<PathedNomDetailsDTO>();
                var pathCP = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfoCP = new FileInfo(pathCP);
                var pathLoc = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "Location.xml");
                FileInfo fileInfoLoc = new FileInfo(pathLoc);
                if (System.IO.File.Exists(pathCP) && !IsFileLocked(fileInfoCP))
                    cpExist = true;
                if (System.IO.File.Exists(pathLoc) && !IsFileLocked(fileInfoLoc))
                    locExist = true;
                if (cpExist || locExist)
                {
                    XmlNodeList nodeListLoc = null;
                    XmlNodeList nodeListCP = null;
                    XmlDocument xmlCP = new XmlDocument();
                    XmlDocument xmlLoc = new XmlDocument();
                    if (cpExist)
                    {
                        xmlCP.Load(pathCP);
                        nodeListCP = xmlCP.GetElementsByTagName("CounterPartiesDTO");
                    }
                    if (locExist)
                    {
                        xmlLoc.Load(pathLoc);
                        nodeListLoc = xmlLoc.GetElementsByTagName("LocationsDTO");
                    }
                    List<BatchDetailContractDTO> listCon = new List<BatchDetailContractDTO>();
                    foreach (var item in batchDetail.Contract)
                    {
                        if (locExist)
                        {
                            string recLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.RecLocation)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            string delLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.DelLocation)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.RecLocationName = !string.IsNullOrEmpty(recLoc) ? recLoc : item.RecLocation;
                            item.DelLocationName = !string.IsNullOrEmpty(delLoc) ? delLoc : item.DelLocation;
                        }
                        listCon.Add(item);
                    }
                    List<BatchDetailSupplyDTO> listSup = new List<BatchDetailSupplyDTO>();
                    foreach (var item in batchDetail.SupplyList)
                    {
                        if (cpExist)
                        {
                            string UpName = nodeListCP.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpstreamID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpstreamIDName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpstreamID;
                        }
                        if (locExist)
                        {
                            string recLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.Location)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.LocationName = !string.IsNullOrEmpty(recLoc) ? recLoc : item.Location;
                        }
                        listSup.Add(item);
                    }
                    List<BatchDetailMarketDTO> listMar = new List<BatchDetailMarketDTO>();
                    foreach (var item in batchDetail.MarketList)
                    {
                        if (cpExist)
                        {
                            string DwName = nodeListCP.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.DownstreamID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DownstreamIDName = !string.IsNullOrEmpty(DwName) ? DwName : item.DownstreamID;
                        }
                        if (locExist)
                        {
                            string delLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.Location)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.LocationName = !string.IsNullOrEmpty(delLoc) ? delLoc : item.Location;
                        }
                        listMar.Add(item);
                    }
                    batchDetail.Contract = listCon;
                    batchDetail.SupplyList = listSup;
                    batchDetail.MarketList = listMar;
                }
                return batchDetail;
            }
            catch(Exception ex)
            {
                return batchDetail;
            }
            
        }

        protected NonPathedDTO UpdateCounterPartyAndLocNameInNonPathed(NonPathedDTO model)
        {
            try
            {
                bool cpExist = false;
                bool locExist = false;
                List<PathedNomDetailsDTO> newList = new List<PathedNomDetailsDTO>();
                var pathCP = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfoCP = new FileInfo(pathCP);
                var pathLoc = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "Location.xml");
                FileInfo fileInfoLoc = new FileInfo(pathLoc);
                if (System.IO.File.Exists(pathCP) && !IsFileLocked(fileInfoCP))
                    cpExist = true;
                if (System.IO.File.Exists(pathLoc) && !IsFileLocked(fileInfoLoc))
                    locExist = true;

                if (cpExist || locExist)
                {
                    XmlNodeList nodeListLoc = null;
                    XmlNodeList nodeListCP = null;
                    XmlDocument xmlCP = new XmlDocument();
                    XmlDocument xmlLoc = new XmlDocument();
                    List<XmlNode> nodesCP = null;
                    List<XmlNode> nodesLoc = null;
                    if (cpExist)
                    {
                        xmlCP.Load(pathCP);
                        nodeListCP = xmlCP.GetElementsByTagName("CounterPartiesDTO");
                        nodesCP = new List<XmlNode>(nodeListCP.Cast<XmlNode>());
                    }
                    if (locExist)
                    {
                        xmlLoc.Load(pathLoc);
                        nodeListLoc = xmlLoc.GetElementsByTagName("LocationsDTO");
                        nodesLoc = new List<XmlNode>(nodeListLoc.Cast<XmlNode>());
                    }
                    List<NonPathedDeliveryNom> newlistD = new List<NonPathedDeliveryNom>();
                    foreach (var item in model.DeliveryNoms)
                    {
                        if (cpExist)
                        {
                            string DwName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DnstreamId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DnstreamName = !string.IsNullOrEmpty(DwName) ? DwName : item.DnstreamId;
                        }
                        if (locExist)
                        {
                            string DlocName = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DeliveryLocId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DeliveryLocName = !string.IsNullOrEmpty(DlocName) ? DlocName : item.DeliveryLocId;
                        }
                        newlistD.Add(item);
                    }
                    List<NonPathedRecieptNom> newlistR = new List<NonPathedRecieptNom>();
                    foreach (var item in model.ReceiptNoms)
                    {
                        if (cpExist)
                        {
                            string UpName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpstreamId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpstreamName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpstreamId;
                        }
                        if (locExist)
                        {
                            string RlocName = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.ReceiptLocId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();

                            item.ReceiptLocName = !string.IsNullOrEmpty(RlocName) ? RlocName : item.ReceiptLocId;
                        }
                        newlistR.Add(item);
                    }
                    if (newlistD.Count > 0)
                        model.DeliveryNoms = newlistD;
                    if (newlistR.Count > 0)
                        model.ReceiptNoms = newlistR;
                }
                return model;
            }
            catch(Exception ex)
            {
                return model;
            }
            
        }

        protected List<SQTSOPPerTransactionDTO> UpdateSqopCpName(List<SQTSOPPerTransactionDTO> list)
        {
            try
            {
                List<SQTSOPPerTransactionDTO> newList = new List<SQTSOPPerTransactionDTO>();
                var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfo = new FileInfo(path);
                if (System.IO.File.Exists(path) && !IsFileLocked(fileInfo))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(path);
                    XmlNodeList nodeList = xml.GetElementsByTagName("CounterPartiesDTO");
                    List<XmlNode> nodesCP = new List<XmlNode>(nodeList.Cast<XmlNode>());
                    foreach (var item in list)
                    {
                        if (item.ContractualFLowIndicator.Trim() == "R")
                        {
                            string UpName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpstreamParty)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpstreamParty = !string.IsNullOrEmpty(UpName) ? UpName : item.UpstreamParty;
                        }
                        else if (item.ContractualFLowIndicator.Trim() == "D")
                        {
                            string DwName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DownstreamParty)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DownstreamParty = !string.IsNullOrEmpty(DwName) ? DwName : item.DownstreamParty;
                        }
                        newList.Add(item);
                    }
                }
                else
                {
                    newList = list;
                }
                return newList;
            }catch(Exception ex)
            {
                return list;
            }
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