using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;

namespace Nom1Done.Controllers
{
    public class BaseController : Controller
    {
        private readonly IPipelineService pipelineService;

        public BaseController(IPipelineService pipelineService)
        {
            this.pipelineService = pipelineService;
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
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

        public PartialViewResult TopNavBarPartail()
        {
            var list = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).OrderBy(a => a.Name).ToList();
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ViewBag.ShipperDetails = identity.Claims.Where(c => c.Type == "ShipperDetails")
                               .Select(c => c.Value).SingleOrDefault()+" (" + identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault()+")";
            ViewBag.UserName = identity.Claims.Where(c => c.Type == "UserName")
                               .Select(c => c.Value).SingleOrDefault();
            ViewBag.UserRole = "Admin";
            var UserId = identity.Claims.Where(c => c.Type == "UserId")
                            .Select(c => c.Value).SingleOrDefault();
            var Pipe = Request["pipelineId"] == null ? pipelineService.GetFirstPipelineByUser(UserId, GetCurrentCompanyID()).ID.ToString() : Request["pipelineId"];
            int PipeId=Convert.ToInt32(Pipe);
            var PipelineType = list.Where(a => a.ID == PipeId).Select(a => a.TempItem).FirstOrDefault();            
            ViewBag.PipelineDropdown = new SelectList(list, "TempItem", "Name",PipelineType);
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
            var list = pipelineService.GetAllPipelineList(GetCurrentCompanyID(),GetLoggedInUserId()).ToList();
            ViewBag.PipelineDropdown = new SelectList(list, "TempItem", "Name");
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
                            try
                            {
                                string UpName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            string DwName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DownID)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpName = !string.IsNullOrEmpty(UpName) ? UpName : "Not Available" ;
                            item.DownName = !string.IsNullOrEmpty(DwName) ? DwName : "Not Available";

                           
                                string UpProp = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpID)
                                 .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                string DwProp = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DownID)
                                .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                item.UpIDProp = !string.IsNullOrEmpty(UpProp) ? UpProp : "Not Available";
                                item.DownIDProp = !string.IsNullOrEmpty(DwProp) ? DwProp : "Not Available";
                            } catch (Exception ex) {

                            }

                        }
                        if (locExist)
                        {
                            try
                            {
                                string recLoc = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.RecLocID)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            string delLoc = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DelLocID)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.RecLocation = !string.IsNullOrEmpty(recLoc) ? recLoc : "Not Available";
                            item.DelLoc = !string.IsNullOrEmpty(delLoc) ? delLoc : "Not Available";


                          
                                string recLocProp = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.RecLocID)
                                .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                string delLocProp = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DelLocID)
                                    .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                item.RecLocProp = !string.IsNullOrEmpty(recLocProp) ? recLocProp : "Not Available";
                                item.DelLocProp = !string.IsNullOrEmpty(delLocProp) ? delLocProp : "Not Available";
                            } catch (Exception ex) {

                            }

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
                            item.RecLocationName = !string.IsNullOrEmpty(recLoc) ? recLoc : item.RecLocationName;
                            item.DelLocationName = !string.IsNullOrEmpty(delLoc) ? delLoc : item.DelLocationName;
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
                            item.UpstreamIDName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpstreamIDName;
                        }
                        if (locExist)
                        {
                            string recLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.Location)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.LocationName = !string.IsNullOrEmpty(recLoc) ? recLoc : item.LocationName;
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
                            item.DownstreamIDName = !string.IsNullOrEmpty(DwName) ? DwName : item.DownstreamIDName;
                        }
                        if (locExist)
                        {
                            string delLoc = nodeListLoc.Cast<XmlNode>().Where(node => node.SelectSingleNode("Identifier").InnerText == item.Location)
                                .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.LocationName = !string.IsNullOrEmpty(delLoc) ? delLoc : item.LocationName;
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
                            try
                            {
                                string DwName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DnstreamId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DnstreamName = !string.IsNullOrEmpty(DwName) ? DwName : item.DnstreamId;

                          
                                string DwProp = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DnstreamId)
                                .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                item.DnstreamProp = !string.IsNullOrEmpty(DwProp) ? DwProp : item.DnstreamId;

                            }
                            catch (Exception ex) {

                            }
                        }
                        if (locExist)
                        {
                            try
                            {
                                string DlocName = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DeliveryLocId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.DeliveryLocName = !string.IsNullOrEmpty(DlocName) ? DlocName : item.DeliveryLocId;

                           
                                string DlocProp = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.DeliveryLocId)
                                                      .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                item.DeliveryLocProp = !string.IsNullOrEmpty(DlocProp) ? DlocProp : item.DeliveryLocId;
                            }
                            catch (Exception ex) {

                            }
                         }
                        newlistD.Add(item);
                    }
                    List<NonPathedRecieptNom> newlistR = new List<NonPathedRecieptNom>();
                    foreach (var item in model.ReceiptNoms)
                    {
                        if (cpExist)
                        {
                            try
                            {
                                string UpName = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpstreamId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();
                            item.UpstreamName = !string.IsNullOrEmpty(UpName) ? UpName : item.UpstreamId;

                           
                                string UpProp = nodesCP.Where(node => node.SelectSingleNode("Identifier").InnerText == item.UpstreamId)
                                .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();
                                item.UpstreamProp = !string.IsNullOrEmpty(UpProp) ? UpProp : item.UpstreamId;

                            }
                            catch (Exception ex) { }
                       }
                        if (locExist)
                        {
                            try
                            {
                                string RlocName = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.ReceiptLocId)
                            .Select(n => n.SelectSingleNode("Name").InnerText).FirstOrDefault();

                            item.ReceiptLocName = !string.IsNullOrEmpty(RlocName) ? RlocName : item.ReceiptLocId;

                            
                                string RlocPropCode = nodesLoc.Where(node => node.SelectSingleNode("Identifier").InnerText == item.ReceiptLocId)
                                  .Select(n => n.SelectSingleNode("PropCode").InnerText).FirstOrDefault();

                                item.ReceiptLocProp = !string.IsNullOrEmpty(RlocPropCode) ? RlocPropCode : item.ReceiptLocId;

                            }
                            catch (Exception ex) { }
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