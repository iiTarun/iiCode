using Nom.ViewModel;
using Nom1Done.Data;
using Nom1Done.Data.Repositories;
using Nom1Done.DTO;
using Nom1Done.Enums;
using Nom1Done.Service.Interface;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;

namespace Nom1Done.Controllers
{
    [Authorize]
    public class SQTSSummaryController : BaseController
    {
        ISQTSRepository _sqtsRepo;
        INominationsRepository _nomRepo;
        IPipelineService _pipelineService;
        ISQTSOPPerTransactionRepository _sqtsOpRepo;
        IShipperCompSubShipperCompRepository _shipperSubCompRepo;
        public SQTSSummaryController(ISQTSRepository _sqtsRepo, INominationsRepository _nomRepo, IPipelineService pipelineService, ISQTSOPPerTransactionRepository sqtsOpRepo, IShipperCompSubShipperCompRepository shipperSubCompRepo) : base(pipelineService)
        {
            this._sqtsRepo = _sqtsRepo;
            this._nomRepo = _nomRepo;
            this._sqtsOpRepo = sqtsOpRepo;
            this._pipelineService = pipelineService;
            this._shipperSubCompRepo = shipperSubCompRepo;
        }
        public ActionResult Index(string val, int? pipelineId)
        {
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            if (pipelineId == null)
                pipelineId = currentIdentityValues.FirstSelectedPipeIdByUser;

            var value = val == null ? "loc" : val;
            ViewBag.SqtsType = value;
            if (value == "loc")
            {
                ViewBag.IsSqtsForLoc = true;
            }
            else
            {
                ViewBag.IsSqtsForLoc = false;
            }
            if (pipelineId != null)
            {
                NomType modelType = _pipelineService.GetPathTypeByPipelineDuns(_pipelineService.GetDunsByPipelineID(pipelineId.Value));
                ViewBag.PipelineNomType = modelType;
            }
            return View();
        }

        public PartialViewResult SQTSPartials(SqtsPartials sqts, int month, string sqtsType, int? pipelineId, bool showMine,bool showZero, int? page)
        {
            int pageSize = 20;
            page = page == null ? 1 : page;
            ViewBag.Month = month;
            if (Request["pipelineId"] != null)
            {
                var pipelineData = Request["pipelineId"] + "";
                string[] pilineVals = pipelineData.Split('-');
                pipelineId = Convert.ToInt32(pilineVals[0]);
            }
            if (sqtsType == null)
                sqtsType = "loc";
            dynamic model = null;
            PipelineDTO pipeDto = null;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            if (pipelineId == null)
                pipelineId = currentIdentityValues.FirstSelectedPipeIdByUser;
            pipeDto = _pipelineService.GetPipeline(pipelineId.Value);
            ViewBag.PipelineDuns = pipeDto.DUNSNo;
            string PartialName = string.Empty;
            ViewBag.IsSqtsForLoc = sqtsType == "loc" ? true : false;
            string UserId = showMine ? currentIdentityValues.UserId : string.Empty;

            switch (sqts)
            {
                case SqtsPartials.NomQty:
                    PartialName = "_NomQtyPartial";
                    model = _nomRepo.GetAcceptedNomsOnDate(month, pipeDto.ID, UserId, currentIdentityValues.ShipperDuns, sqtsType == "loc" ? true : false, showZero);
                    break;
                case SqtsPartials.SqtsQty:
                    ViewBag.PathType = "P";
                    ViewBag.IsSqtsQty = "true";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> list = _sqtsRepo.GetSqtsData(month, pipeDto.DUNSNo, UserId, "P", currentIdentityValues.ShipperDuns,showZero, pageSize, page.Value);
                    list = UpdateCounterPartyName(list);
                    model = list;
                    break;
                case SqtsPartials.SqtsOrphan:
                    PartialName = "_sqtsOrphanCom";
                    ViewBag.IsSqtsQty = "false";
                    ViewBag.PathType = "P";
                    List<SummaryDTO> listO = _sqtsRepo.GetSqtsOrphanData(month, pipeDto.DUNSNo, "P", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listO = UpdateCounterPartyName(listO);
                    model = listO;
                    break;
                case SqtsPartials.NomQtyDelivery:
                    PartialName = "_NomQtyDeliveryPartial";
                    model = _nomRepo.GetAcceptedNomOnDate(month, "D", pipeDto.ID, UserId, currentIdentityValues.ShipperDuns, sqtsType == "loc" ? true : false, showZero);
                    break;
                case SqtsPartials.NomQtyReceipt:
                    PartialName = "_NomQtyReceiptPartial";
                    model = _nomRepo.GetAcceptedNomOnDate(month, "R", pipeDto.ID, UserId, currentIdentityValues.ShipperDuns, sqtsType == "loc" ? true : false, showZero);
                    break;

                case SqtsPartials.SqtsOrphanDelivery:
                    ViewBag.PathType = "D";
                    ViewBag.IsSqtsQty = "false";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listDO = _sqtsRepo.GetSqtsOrphanData(month, pipeDto.DUNSNo, "D", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listDO = UpdateCounterPartyName(listDO);
                    model = listDO;
                    break;

                case SqtsPartials.SqtsOrphanReceipt:
                    ViewBag.PathType = "R";
                    ViewBag.IsSqtsQty = "false";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listRO = _sqtsRepo.GetSqtsOrphanData(month, pipeDto.DUNSNo, "R", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listRO = UpdateCounterPartyName(listRO);
                    model = listRO;
                    break;
                case SqtsPartials.SqtsQtyDelivery:
                    ViewBag.PathType = "D";
                    ViewBag.IsSqtsQty = "true";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listD = _sqtsRepo.GetSqtsData(month, pipeDto.DUNSNo, UserId, "D", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listD = UpdateCounterPartyName(listD);
                    model = listD;
                    break;
                case SqtsPartials.SqtsQtyReceipt:
                    ViewBag.PathType = "R";
                    ViewBag.IsSqtsQty = "true";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listR = _sqtsRepo.GetSqtsData(month, pipeDto.DUNSNo, UserId, "R", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listR = UpdateCounterPartyName(listR);
                    model = listR;
                    break;
                case SqtsPartials.NomQtyContractPath:
                    PartialName = "__NomQtyContractPathPartial";
                    model = _nomRepo.GetNomQtyForContractPath(month, pipeDto.ID, UserId, currentIdentityValues.ShipperDuns, sqtsType == "loc" ? true : false, showZero);
                    break;
                case SqtsPartials.SqtsOrphanContractPath:
                    ViewBag.PathType = "T";
                    ViewBag.IsSqtsQty = "false";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listCO = _sqtsRepo.GetSqtsOrphanData(month, pipeDto.DUNSNo, "T", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listCO = UpdateCounterPartyName(listCO);
                    model = listCO;
                    break;
                case SqtsPartials.SqtsQtyContractPath:
                    ViewBag.PathType = "T";
                    ViewBag.IsSqtsQty = "true";
                    PartialName = "_sqtsOrphanCom";
                    List<SummaryDTO> listC = _sqtsRepo.GetSqtsData(month, pipeDto.DUNSNo, UserId, "T", currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
                    listC = UpdateCounterPartyName(listC);
                    model = listC;
                    break;
                case SqtsPartials.OperSqts:
                    PartialName = "_OperSqts";
                    string shipperSubDuns = string.Empty;
                    var shipperAndSubShipperDuns = _shipperSubCompRepo.GetShipperAndSubShipperCompOnDuns(currentIdentityValues.ShipperDuns);
                    if (shipperAndSubShipperDuns == null)
                        shipperSubDuns = currentIdentityValues.ShipperDuns;
                    else
                        shipperSubDuns = shipperAndSubShipperDuns.SubShipperCompDuns + "," + currentIdentityValues.ShipperDuns;
                    var data = _sqtsOpRepo.GetSqtsForOperator(month, pipeDto.DUNSNo, shipperSubDuns, sqtsType == "loc" ? true : false, showZero);
                    data = UpdateSqopCpName(data);
                    return PartialView(PartialName, data);
                default:
                    break;
            }
            return PartialView(PartialName, model);
        }
       
        public PartialViewResult GetDataSqtsOrphan(int? page, string pipelineDuns, int month, string pathType, string sqtsType, string IsSqtsQty, bool showMine, bool showZero)
        {
            ViewBag.Month = month;
            ViewBag.PathType = pathType;
            ViewBag.IsSqtsQty = IsSqtsQty;
            ViewBag.IsSqtsForLoc = Convert.ToBoolean(sqtsType) ? true : false;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            string UserId = showMine ? currentIdentityValues.UserId : string.Empty;
            int pageSize = 15;
            List<SummaryDTO> model = new List<SummaryDTO>();
            if (Convert.ToBoolean(IsSqtsQty))
            {
                model = _sqtsRepo.GetSqtsData(month, pipelineDuns, UserId, pathType, currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
            }
            else
            {
                model = _sqtsRepo.GetSqtsOrphanData(month, pipelineDuns, pathType, currentIdentityValues.ShipperDuns, showZero, pageSize, page.Value);
            }
            model = UpdateCounterPartyName(model);
            return PartialView("_sqtsOrphanData", model);
        }

        public PartialViewResult _SQTSPopUpPartial(string transactionId, string nomTrackingId)
        {
            var timely = "TIM";
            var evening = "EVE";
            var intraDay1 = "ID1";
            var intraDay2 = "ID2";
            var intraDay3 = "ID3";
            List<SqtsDTO> sqtsDto = new List<SqtsDTO>();
            var dto = _sqtsRepo.GetSQTSForNom(nomTrackingId);
            //if(dto!=null&& dto.Count > 0)
            //{
            //    foreach(var item in dto)
            //    {
            //        if (item.Cycle == timely)
            //        {
            //            item.BeginingDate = new DateTime(item.BeginingDate.Year, item.BeginingDate.Month, item.BeginingDate.Day, 9, 0, 0);
            //            item.EndDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day, 9, 0, 0);
            //        }else if (item.Cycle == evening)
            //        {
            //            item.BeginingDate = new DateTime(item.BeginingDate.Year, item.BeginingDate.Month, item.BeginingDate.Day, 9, 0, 0);
            //            item.EndDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day, 9, 0, 0);
            //        }
            //        else if(item.Cycle == intraDay1)
            //        {
            //            item.BeginingDate = new DateTime(item.BeginingDate.Year, item.BeginingDate.Month, item.BeginingDate.Day, 14, 0, 0);
            //            item.EndDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day, 9, 0, 0);
            //        }else if (item.Cycle == intraDay2)
            //        {
            //            item.BeginingDate = new DateTime(item.BeginingDate.Year, item.BeginingDate.Month, item.BeginingDate.Day, 18, 0, 0);
            //            item.EndDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day, 9, 0, 0);
            //        }
            //        else if (item.Cycle == intraDay3)
            //        {
            //            item.BeginingDate = new DateTime(item.BeginingDate.Year, item.BeginingDate.Month, item.BeginingDate.Day, 22, 0, 0);
            //            item.EndDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day, 9, 0, 0);
            //        }
            //        sqtsDto.Add(item);
            //    }
            //}
            return PartialView(dto);
        }
        public string _CounterPartyName(string counteryPartyDuns)
        {
            //string partialViewName = "_CounterPartyName";
            string name = string.Empty;
            NomEntities db = new NomEntities();
            var counterPartyName = db.CounterParty.Where(a => a.Identifier == counteryPartyDuns).FirstOrDefault();
            if (counterPartyName != null)
            {
                name = counterPartyName.Name;
            }
            return name;
        }

        public ActionResult DownloadExcel(string pipelineDuns,int month, string pathType,string IsSqtsQty, bool showMine, bool showZero)
        {
            try
            {
                ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
                string UserId = showMine ? currentIdentityValues.UserId : string.Empty;
                List<SummaryDTO> list = new List<SummaryDTO>();
                if (Convert.ToBoolean(IsSqtsQty))
                {
                    list = _sqtsRepo.GetSqtsDataForExcel(month, pipelineDuns, UserId, pathType, currentIdentityValues.ShipperDuns, showZero);
                }
                else
                {
                    list = _sqtsRepo.GetSqtsOrphanDataForExcel(month, pipelineDuns, pathType, currentIdentityValues.ShipperDuns, showZero);
                }
                //list = UpdateCounterPartyName(list);
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                var headerStyle = workbook.CreateCellStyle(); 
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);
                //headerStyle.FillBackgroundColor = IndexedColors.Green.Index;
                

                ISheet sheet1 = workbook.CreateSheet("SQTS");


                IRow row1 = sheet1.CreateRow(0);
                DataTable dt = ToDataTable(list);
                int headCell = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    String columnName = dt.Columns[j].ToString();
                    if (columnName.Trim() != "SubmittedDate" && columnName.Trim() != "Username")
                    {
                        if (pathType == "R")
                        {
                            if (columnName.Trim() != "DownStreamName" && columnName.Trim() != "DelLoc" && columnName.Trim() != "DwnIdentifier" && columnName.Trim()!= "DelPointQty")
                            {
                                ICell cell = row1.CreateCell(headCell);
                                cell.SetCellValue(columnName);
                                cell.CellStyle = headerStyle;
                                headCell++;
                            }
                        }
                        else if (pathType == "D")
                        {
                            if (columnName.Trim() != "UpStreamName" && columnName.Trim() != "RecLoc" && columnName.Trim() != "UpIdentifier" && columnName.Trim() != "RecPointQty")
                            {
                                ICell cell = row1.CreateCell(headCell);
                                cell.SetCellValue(columnName);
                                cell.CellStyle = headerStyle;
                                headCell++;
                            }
                        }
                        else
                        {
                            ICell cell = row1.CreateCell(headCell);
                            cell.SetCellValue(columnName);
                            cell.CellStyle = headerStyle;
                            headCell++;
                        }
                        
                    }
                }
                //loops through data  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int rowCell = 0;
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String columnName = dt.Columns[j].ToString();
                        if (columnName.Trim() != "SubmittedDate" && columnName.Trim() != "Username")
                        {
                            string value = string.Empty;
                            if(columnName.Trim()== "nomStartDate" || columnName.Trim()== "nomEndDate")
                            {
                                value = Convert.ToDateTime(dt.Rows[i][columnName].ToString()).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                if (pathType == "R")
                                {
                                    if (columnName.Trim() != "DownStreamName" && columnName.Trim() != "DelLoc" && columnName.Trim() != "DwnIdentifier" && columnName.Trim() != "DelPointQty")
                                    {
                                        value = dt.Rows[i][columnName].ToString();
                                    }
                                }
                                else if (pathType == "D")
                                {
                                    if (columnName.Trim() != "UpStreamName" && columnName.Trim() != "RecLoc" && columnName.Trim() != "UpIdentifier" && columnName.Trim() != "RecPointQty")
                                    {
                                        value = dt.Rows[i][columnName].ToString();
                                    }
                                }
                                else
                                {
                                    value = dt.Rows[i][columnName].ToString();
                                }
                            }
                            //string date = DateTime.Now.ToString("");
                           
                            if (pathType == "R" || pathType == "D")
                            {
                                if ((columnName.Trim() == "UpStreamName" && pathType == "R" )|| (columnName.Trim() == "DownStreamName" && pathType == "D"))
                                {
                                    ICell cell = row.CreateCell(rowCell);
                                    rowCell++;
                                    string formula = "VLOOKUP(J" + (i + 2) + ",CP!A1:B5000,2,0)";
                                    cell.SetCellFormula(formula);
                                }
                                else
                                {
                                    if(pathType=="R" && (columnName.Trim() != "DownStreamName" && columnName.Trim() != "DelLoc" && columnName.Trim() != "DwnIdentifier" && columnName.Trim() != "DelPointQty"))
                                    {
                                        ICell cell = row.CreateCell(rowCell);
                                        rowCell++;
                                        cell.SetCellValue(value);
                                    }else if(pathType=="D" && (columnName.Trim() != "UpStreamName" && columnName.Trim() != "RecLoc" && columnName.Trim() != "UpIdentifier" && columnName.Trim() != "RecPointQty"))
                                    {
                                        ICell cell = row.CreateCell(rowCell);
                                        rowCell++;
                                        cell.SetCellValue(value);
                                    }
                                    
                                }
                            }
                            else
                            {
                                ICell cell = row.CreateCell(rowCell);
                                rowCell++;
                                if (columnName.Trim() == "UpStreamName")
                                {
                                    string formula = "VLOOKUP(L" + (i + 2) + ",CP!A1:B5000,2,0)";
                                    cell.SetCellFormula(formula);
                                }
                                else if (columnName.Trim() == "DownStreamName")
                                {
                                    string formula = "VLOOKUP(M" + (i + 2) + ",CP!A1:B5000,2,0)";
                                    cell.SetCellFormula(formula);
                                }
                                else
                                {
                                    cell.SetCellValue(value);
                                }
                            }
                            
                            
                        }
                    }
                }
                #region Counter Party Sheet
                var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), "CounterParty.xml");
                FileInfo fileInfo = new FileInfo(path);
                if (System.IO.File.Exists(path) && !IsFileLocked(fileInfo))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(path);
                    XmlNodeList nodeListCP = xml.GetElementsByTagName("CounterPartiesDTO");
                    List<XmlNode> NodeCP = new List<XmlNode>(nodeListCP.Cast<XmlNode>());
                    ISheet sheet2 = workbook.CreateSheet("CP");
                    IRow row2 = sheet2.CreateRow(0);
                    ICell cell1 = row2.CreateCell(0);
                    cell1.SetCellValue("Identifier");
                    cell1.CellStyle = headerStyle;
                    ICell cell2 = row2.CreateCell(1);
                    cell2.SetCellValue("Name");
                    cell2.CellStyle = headerStyle;
                    for (int i = 0; i < NodeCP.Count; i++)
                    {
                        IRow row = sheet2.CreateRow(i + 1);
                        var nodeIden = NodeCP[i].SelectSingleNode("Identifier").InnerText;
                        var nodeName = NodeCP[i].SelectSingleNode("Name").InnerText;
                        ICell cellIden = row.CreateCell(0);
                        cellIden.SetCellValue(nodeIden);
                        ICell cellName = row.CreateCell(1);
                        cellName.SetCellValue(nodeName);
                    }
                }
                #endregion
                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    return File(exportData.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "SQTS.xls");
                }
            }
            catch(Exception ex)
            {
                return File(new byte[0],
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "SQTS.xls");
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}