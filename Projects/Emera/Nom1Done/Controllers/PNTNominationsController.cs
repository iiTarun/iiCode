using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Models;
using Nom1Done.Nom.ViewModel;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Nom1Done.Controllers
{
   [Authorize]
    public class PNTNominationsController : BaseController
    {
        //TODO:
        INonPathedService nonPathedService;
        IPathedNominationService _pathedNominationService;
        IPNTNominationService IPNTNominationService;
        private readonly IPipelineService pipelineService;
        ImetadataBidUpIndicatorService _metaDataBidUpIndicatorService;
        ImetadataCapacityTypeIndicatorService _metadataCapacityTypeIndicatorService;
        ImetadataQuantityTypeIndicatorService _metadataQuantityTypeIndicatorService;
        ImetadataExportDeclarationService _metadataExportDeclarationService;
        ImetadataFileStatusService _metadataFileStatusService;
        ICycleIndicator _cycleIndicator;
        INotifierEntityService _notifierEntityService;

        public PNTNominationsController(INotifierEntityService _notifierEntityService ,INonPathedService nonPathedService,IPathedNominationService _pathedNominationService,ImetadataFileStatusService metadataFileStatusService, ImetadataExportDeclarationService ImetadataExportDeclarationService, ImetadataQuantityTypeIndicatorService metadataQuantityTypeIndicatorService, ICycleIndicator ICycleIndicator, ImetadataCapacityTypeIndicatorService metadataCapacityTypeIndicatorService, ImetadataBidUpIndicatorService metaDataBidUpIndicatorService, IPipelineService pipelineService, IPNTNominationService IPNTNominationService):base(pipelineService)
        {
            this._notifierEntityService = _notifierEntityService;
            this.nonPathedService = nonPathedService;
            this._pathedNominationService = _pathedNominationService;
            this.IPNTNominationService = IPNTNominationService;
            this.pipelineService =pipelineService;
            this._metaDataBidUpIndicatorService = metaDataBidUpIndicatorService;
            this._metadataCapacityTypeIndicatorService = metadataCapacityTypeIndicatorService;
            this._metadataQuantityTypeIndicatorService = metadataQuantityTypeIndicatorService;
            this._metadataExportDeclarationService = ImetadataExportDeclarationService;
            this._metadataFileStatusService = metadataFileStatusService;
            this._cycleIndicator = ICycleIndicator;
        }

        public PartialViewResult DynamicTabs(List<TestNomSupply> SupplyRecords, List<TestNomTransport> TransportRecords, List<TestNomMarket> MarketRecords, string tab, int PipelineID)//tab = 1 for Batch, 2 for NomMatrix, 3 for Supply, 4 for Market
        {
            try
            {
                if (tab == "1")
                {
                    List<TestNomTransport> TransportRecordsRec = new List<TestNomTransport>();
                    List<TestNomTransport> TransportRecordsDel = new List<TestNomTransport>();
                    BatchTabModel modelLst = new BatchTabModel();
                    // return batch partial
                    try
                    {
                        if (SupplyRecords != null)
                            SupplyRecords = SupplyRecords
                            .GroupBy(a => a.LocProp)
                            .Select(cl => new TestNomSupply
                            {
                                LocProp = cl.First().LocProp,
                                LocName = cl.First().LocName,
                                DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                                RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString(),
                            }).ToList();
                        if (MarketRecords != null)
                            MarketRecords = MarketRecords
                                .GroupBy(a => a.LocProp)
                                .Select(cl => new TestNomMarket
                                {
                                    LocProp = cl.First().LocProp,
                                    LocName = cl.First().LocName,
                                    DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                                    RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString()
                                }).ToList();
                        if (TransportRecords != null)
                            TransportRecordsRec = TransportRecords.GroupBy(a => a.RecLocProp)
                                .Select(cl => new TestNomTransport
                                {
                                    RecLoc = cl.First().RecLoc,
                                    RecLocProp = cl.First().RecLocProp,
                                    RecLocName = cl.First().RecLocName,
                                    RecDTH = cl.Sum(c => Convert.ToDecimal(c.RecDTH)).ToString(),
                                    DelDTH = cl.Sum(c => Convert.ToDecimal(c.DelDTH)).ToString()
                                }).ToList();
                        if (TransportRecords != null)
                            TransportRecordsDel = TransportRecords.GroupBy(a => a.DelLocProp)
                                .Select(cl => new TestNomTransport
                                {
                                    DelLoc = cl.First().DelLoc,
                                    DelLocProp = cl.First().DelLocProp,
                                    DelLocName = cl.First().DelLocName,
                                    RecDTH = cl.Sum(c => Convert.ToDecimal(c.RecDTH)).ToString(),
                                    DelDTH = cl.Sum(c => Convert.ToDecimal(c.DelDTH)).ToString()
                                }).ToList();



                        if (SupplyRecords != null)
                            foreach (var item in SupplyRecords)
                            {
                                if (modelLst.Lst.Any(a => a.LocProp == item.LocProp))
                                {
                                    BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.LocProp).FirstOrDefault();
                                    modelLst.Lst.Remove(batchTab);
                                    batchTab.RecQty = batchTab.RecQty + Convert.ToInt32(item.RecQty);
                                    //batchTab.DelQty=
                                    batchTab.NominatiedQty=batchTab.NominatiedQty+ Convert.ToInt32(item.DelQty);
                                    batchTab.Variance = batchTab.NominatiedQty;
                                    modelLst.Lst.Add(batchTab);
                                }
                                else
                                {
                                    BatchTabModel model = new BatchTabModel();
                                    //model.DelQty = Convert.ToInt32(item.DelQty);
                                    model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.RecQty));
                                    model.LocProp = item.LocProp;
                                    model.Location = item.LocName;
                                    //model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.DelQty));
                                    model.NominatiedQty= Convert.ToInt32(Convert.ToDecimal(item.DelQty));
                                    model.Variance = model.NominatiedQty;
                                    modelLst.Lst.Add(model);
                                }
                                
                                
                            }

                        if (TransportRecordsRec != null)
                            foreach (var item in TransportRecordsRec)
                            {
                                if (modelLst.Lst.Any(a => a.LocProp == item.RecLocProp))
                                {
                                    BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.RecLocProp).FirstOrDefault();
                                    modelLst.Lst.Remove(batchTab);
                                    batchTab.DelQty = Convert.ToInt32(item.RecDTH);
                                    //batchTab.Variance = batchTab.RecQty - batchTab.DelQty;

                                    batchTab.Variance=batchTab.NominatiedQty- Convert.ToInt32(item.RecDTH);
                                    modelLst.Lst.Add(batchTab);
                                }
                                else
                                {
                                    BatchTabModel model = new BatchTabModel();
                                    //model.DelQty = Convert.ToInt32(item.DelQty);
                                   // model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.DelDTH));
                                    model.LocProp = item.RecLocProp;
                                    model.Location = item.RecLocName;
                                    model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.RecDTH));
                                    model.NominatiedQty = 0;
                                    model.Variance = model.NominatiedQty - model.DelQty;
                                    modelLst.Lst.Add(model);
                                }
                            }
                        if (TransportRecordsDel != null)
                            foreach (var item in TransportRecordsDel)
                            {
                                if (modelLst.Lst.Any(a => a.LocProp == item.DelLocProp))
                                {
                                    BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.DelLocProp).FirstOrDefault();
                                    modelLst.Lst.Remove(batchTab);
                                    batchTab.RecQty = batchTab.RecQty + Convert.ToInt32(item.RecDTH);
                                    batchTab.NominatiedQty= Convert.ToInt32(item.DelDTH);
                                    batchTab.Variance = batchTab.RecQty - batchTab.DelQty;
                                    modelLst.Lst.Add(batchTab);
                                }
                                else
                                {
                                    BatchTabModel model = new BatchTabModel();
                                    //model.DelQty = Convert.ToInt32(item.DelQty);
                                    model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.RecDTH));
                                    model.LocProp = item.DelLocProp;
                                    model.Location = item.DelLocName;
                                    model.NominatiedQty= Convert.ToInt32(Convert.ToDecimal(item.DelDTH));
                                    model.Variance = model.NominatiedQty;
                                    //model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.DelDTH));
                                    //model.Variance = model.RecQty - model.DelQty;
                                    modelLst.Lst.Add(model);
                                }
                            }

                        if (MarketRecords != null)
                            foreach (var item in MarketRecords)
                            {
                                if (!modelLst.Lst.Any(a => a.LocProp == item.LocProp))
                                {
                                    BatchTabModel model = new BatchTabModel();
                                    model.DelQty = Convert.ToInt32(item.RecQty);
                                    model.RecQty = Convert.ToInt32(item.RecQty);
                                    model.LocProp = item.LocProp;
                                    model.Location = item.LocName;
                                    model.NominatiedQty = 0;
                                    model.Variance = model.NominatiedQty - model.RecQty;
                                    modelLst.Lst.Add(model);
                                }
                                else
                                {
                                    BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.LocProp).FirstOrDefault();
                                    modelLst.Lst.Remove(batchTab);
                                    batchTab.DelQty = batchTab.DelQty + Convert.ToInt32(item.RecQty);
                                    //batchTab.NominatiedQty = 0;
                                    batchTab.Variance = batchTab.NominatiedQty - Convert.ToInt32(item.RecQty);
                                    modelLst.Lst.Add(batchTab);
                                }
                                
                            }
                       
                    }
                    catch (Exception ex)
                    {

                    }
                    return PartialView("_BatchTabPartial", modelLst);
                }
                else if (tab == "3")
                {
                    BatchDetailDTO batchObj = new BatchDetailDTO();
                    try
                    {
                        batchObj.PipelineId = PipelineID;
                        batchObj.CurrentSupplyRow = SupplyRecords != null ? SupplyRecords.Count : 0;
                        if (TransportRecords == null)
                            return null;
                        else
                        {
                            TransportRecords = TransportRecords
                                .GroupBy(a => a.RecLoc)
                                .Select(b => new TestNomTransport
                                {
                                    RecLoc = b.First().RecLoc,
                                    RecLocName = b.First().RecLocName,
                                    RecLocProp = b.First().RecLocProp,
                                }).ToList();
                            if (SupplyRecords != null)
                                SupplyRecords = SupplyRecords
                            .GroupBy(a => a.LocProp)
                            .Select(cl => new TestNomSupply
                            {
                                LocProp = cl.First().LocProp,
                                LocName = cl.First().LocName,
                                DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                                RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString(),
                            }).ToList();
                            else
                                SupplyRecords = new List<TestNomSupply>();
                            foreach (var item in TransportRecords)
                            {
                                if (!SupplyRecords.Any(b => item.RecLocProp == b.LocProp))
                                {
                                    BatchDetailSupplyDTO sup = new BatchDetailSupplyDTO();
                                    //sup.BatchID=item.
                                    sup.Location = item.RecLoc;
                                    sup.LocationName = item.RecLocName;
                                    sup.LocationProp = item.RecLocProp;
                                    batchObj.SupplyList.Add(sup);
                                }
                            }
                            if (batchObj.SupplyList.Count == 0)
                                return null;
                            else
                                return PartialView("_AddSupplyRow", batchObj);

                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else if (tab == "4")
                {
                    BatchDetailDTO batchObj = new BatchDetailDTO();
                    try
                    {
                        batchObj.PipelineId = PipelineID;
                        batchObj.CurrentSupplyRow = MarketRecords != null ? MarketRecords.Count : 0;
                        if (TransportRecords == null)
                            return null;
                        else
                        {
                            TransportRecords = TransportRecords
                                .GroupBy(a => a.DelLoc)
                                .Select(b => new TestNomTransport
                                {
                                    DelLoc = b.First().DelLoc,
                                    DelLocName = b.First().DelLocName,
                                    DelLocProp = b.First().DelLocProp,
                                }).ToList();
                            if (MarketRecords != null)
                                MarketRecords = MarketRecords
                                    .GroupBy(a => a.LocProp)
                                    .Select(cl => new TestNomMarket
                                    {
                                        LocProp = cl.First().LocProp,
                                        LocName = cl.First().LocName,
                                        DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                                        RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString()
                                    }).ToList();
                            else
                                MarketRecords = new List<TestNomMarket>();

                            foreach (var item in TransportRecords)
                            {
                                if (!MarketRecords.Any(b => item.DelLocProp == b.LocProp))
                                {
                                    BatchDetailMarketDTO mar = new BatchDetailMarketDTO();
                                    //sup.BatchID=item.
                                    mar.Location = item.DelLoc;
                                    mar.LocationName = item.DelLocName;
                                    mar.LocationProp = item.DelLocProp;
                                    batchObj.MarketList.Add(mar);
                                }
                            }
                            if (batchObj.MarketList.Count == 0)
                                return null;
                            else
                                return PartialView("_AddMarketRow", batchObj);

                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else
                {
                    NomMatrixDTO nomMatrixModel;
                    List<NomMatrixDTO> nomLst = new List<NomMatrixDTO>();
                    List<UniqueLocations> transportLocations = new List<UniqueLocations>();
                    List<UniqueLocations> dummySupplyLocations = new List<UniqueLocations>();
                    List<UniqueLocations> dummyMarketLocations = new List<UniqueLocations>();

                    List<RecUpStreamDTO> receiptUpStreams = new List<RecUpStreamDTO>();
                    List<RecDownStreamDTO> receiptDnStreams = new List<RecDownStreamDTO>();
                    List<DelUpStreamDTO> deliveryUpStreams = new List<DelUpStreamDTO>();
                    List<DelDownStreamDTO> deliveryDnStreams = new List<DelDownStreamDTO>();

                    if(TransportRecords!=null)
                        foreach (var item in TransportRecords)
                        {
                            UniqueLocations model = new UniqueLocations();
                            model.Loc = item.DelLoc;
                            model.Type = "D";
                            model.LocName = item.DelLocName;
                            transportLocations.Add(model);
                            model = new UniqueLocations();
                            model.Loc = item.RecLoc;
                            model.LocName = item.RecLocName;
                            model.Type = "R";
                            transportLocations.Add(model);
                        }

                    if(SupplyRecords!=null)
                        foreach (var item in SupplyRecords)
                        {
                            UniqueLocations model = new UniqueLocations();
                            model.Loc = item.Loc;
                            model.Type = "R";
                            model.LocName = item.LocName;
                            dummySupplyLocations.Add(model);
                        }

                    if(MarketRecords!=null)
                        foreach (var item in MarketRecords)
                        {
                            UniqueLocations model = new UniqueLocations();
                            model.Loc = item.Loc;
                            model.Type = "D";
                            model.LocName = item.LocName;
                            dummyMarketLocations.Add(model);
                        }
                    var combineDummyLocs = dummySupplyLocations.Concat(dummyMarketLocations).ToList();

                    List<string> aa = dummySupplyLocations.Select(a => a.Loc ).ToList();
                    List<string> bb = dummyMarketLocations.Select(a => a.Loc).ToList();
                    var intersectLocs = aa.Intersect(bb).ToList();

                    var distinctDummyLocs = new List<UniqueLocations>();

                    for (int i = 0; i < intersectLocs.Count; i++)
                    {
                        UniqueLocations mm = new UniqueLocations();
                        mm.Loc = intersectLocs[i];
                        mm.Type = "B";
                        mm.LocName = combineDummyLocs.Where(a=>a.Loc==mm.Loc).FirstOrDefault().LocName;
                        distinctDummyLocs.Add(mm);
                    }

                   // var distinctDummyLocs = aa.Intersect(bb).ToList();
                    foreach (var item in distinctDummyLocs)
                    {
                        item.Type = "B";
                    }
                    
                    var withoutDistinctRemainLocs = combineDummyLocs.Where(a => !distinctDummyLocs.Select(c => c.Loc).Contains(a.Loc)).ToList();


                    transportLocations = transportLocations.Concat(distinctDummyLocs).Concat(withoutDistinctRemainLocs).ToList();


                    //var UniqueTransportlocations = transportLocations;

                    // filter Unique Transport Locations                
                    var UniqueTransportlocations = (from a in transportLocations
                                                    group a by new
                                                    {
                                                        a.Loc,
                                                        a.LocName,
                                                        a.Type
                                                    } into b
                                                    select new UniqueLocations
                                                    {
                                                        Loc =  b.Key.Loc,
                                                        LocName = b.Key.LocName,
                                                        Type=b.Key.Type
                                                    }).ToList();

                    //.Select(a => new { Loc = a.Loc, LocName = a.LocName, Type = a.Type }).ToList();



                    //var UniqueTransportlocations = new List<TransportLocations>();

                    foreach (var item in UniqueTransportlocations) // Master For Loop for UpStream and DownStream
                    {
                        nomMatrixModel = new NomMatrixDTO();
                        nomMatrixModel.Loc = item.LocName + "-(" + item.Loc + ")";
                        //Upstream of Receipt
                        if (item.Type == "R")
                        {
                            if (SupplyRecords != null)
                            {
                                foreach (var SupplyItem in SupplyRecords.Where(a => a.Loc == item.Loc)) // Grab things from Supply
                                {
                                    RecUpStreamDTO model = new RecUpStreamDTO();
                                    model.Contract = SupplyItem.SVCRe;
                                    model.CounterParty = SupplyItem.UpID;
                                    model.Quantity = SupplyItem.RecQty;
                                    model.RecQty = SupplyItem.RecQty;
                                    model.DelQty = SupplyItem.DelQty;
                                    model.TransactionType = SupplyItem.TT;
                                    model.Loc = item.Loc;
                                    model.LocName = item.LocName;
                                    nomMatrixModel.RecUpType = "RecUpStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.ReceiptUpStreamLst.Add(model);// receiptUpStreams; 
                                }
                            }
                            if (TransportRecords != null)
                            {
                                foreach (var DeliveryItem in TransportRecords.Where(a => a.RecLoc == item.Loc)) // Grab things from Tranport
                                {
                                    RecDownStreamDTO model = new RecDownStreamDTO();
                                    model.Contract = DeliveryItem.Contract;
                                    model.DeliveryLocation = DeliveryItem.DelLocName;
                                    model.Quantity = DeliveryItem.DelDTH;
                                    model.RecQty = DeliveryItem.RecDTH;
                                    model.DelQty = DeliveryItem.DelDTH;
                                    model.TransactionType = DeliveryItem.TT;
                                    model.Loc = DeliveryItem.DelLoc;
                                    model.LocName = DeliveryItem.DelLocName;
                                    // receiptDnStreams.Add(model);
                                    nomMatrixModel.RecDnType = "RecDownStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.ReceiptDownStreamLst.Add(model);// = receiptDnStreams;
                                }
                            }
                        }
                        else if (item.Type == "D")
                        {
                            if (TransportRecords != null)
                            {
                                foreach (var TransportItem in TransportRecords.Where(a => a.DelLoc == item.Loc))// grab things from transport
                                {
                                    DelUpStreamDTO model = new DelUpStreamDTO();
                                    model.Contract = TransportItem.Contract;
                                    model.Quantity = TransportItem.RecDTH;
                                    model.RecQty = TransportItem.RecDTH;
                                    model.DelQty = TransportItem.DelDTH;
                                    model.RecLocation = TransportItem.RecLoc;
                                    model.TransactionType = TransportItem.TT;
                                    model.Loc = item.Loc;
                                    model.LocName = item.LocName;                                  
                                    nomMatrixModel.DelUpType = "DelUpStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.DeliveryUpStreamLst.Add(model);
                                }
                            }
                            if (MarketRecords != null)
                            {
                                foreach (var DeliveryItem in MarketRecords.Where(a => a.Loc == item.Loc)) // grab things from Market
                                {
                                    DelDownStreamDTO model = new DelDownStreamDTO();
                                    model.ContractNo = DeliveryItem.SVCRe;
                                    model.Location = DeliveryItem.LocName;
                                    model.Quantity = DeliveryItem.RecQty;
                                    model.RecQty = DeliveryItem.RecQty;
                                    model.DelQty = DeliveryItem.DelQty;
                                    model.TransactionType = DeliveryItem.TT;
                                    model.Loc = DeliveryItem.DnID;
                                    model.LocName = DeliveryItem.DnName;                                    
                                    nomMatrixModel.DelDnType = "DelDownStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.DeliveryDownStreamLst.Add(model);// = deliveryDnStreams;
                                }
                            }
                        }
                        else if (item.Type == "B")
                        {
                            if (SupplyRecords != null)
                            {
                                foreach (var SupplyItem in SupplyRecords.Where(a => a.Loc == item.Loc))// grab things from transport
                                {
                                    RecUpStreamDTO model = new RecUpStreamDTO();
                                    model.Contract = SupplyItem.SVCRe;
                                    model.CounterParty = SupplyItem.UpID;
                                    model.Quantity = SupplyItem.RecQty;
                                    model.RecQty = SupplyItem.RecQty;
                                    model.DelQty = SupplyItem.DelQty;
                                    model.TransactionType = SupplyItem.TT;
                                    model.Loc = item.Loc;
                                    model.LocName = item.LocName;
                                    nomMatrixModel.RecUpType = "RecUpStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.ReceiptUpStreamLst.Add(model);// receiptUpStreams;
                                }
                            }
                            if (MarketRecords != null)
                            {
                                foreach (var DeliveryItem in MarketRecords.Where(a => a.Loc == item.Loc)) // grab things from Market
                                {
                                    DelDownStreamDTO model = new DelDownStreamDTO();
                                    model.ContractNo = DeliveryItem.SVCRe;
                                    model.Location = DeliveryItem.LocName;
                                    model.Quantity = DeliveryItem.RecQty;
                                    model.RecQty = DeliveryItem.RecQty;
                                    model.DelQty = DeliveryItem.DelQty;
                                    model.TransactionType = DeliveryItem.TT;
                                    model.Loc = DeliveryItem.DnID;
                                    model.LocName = DeliveryItem.DnName;
                                    nomMatrixModel.DelDnType = "DelDownStream";
                                    nomMatrixModel.LocType = item.Type;
                                    nomMatrixModel.DeliveryDownStreamLst.Add(model);// = deliveryDnStreams;
                                }
                            }
                        }
                        nomLst.Add(nomMatrixModel);
                    }
                    return PartialView("_NomMatrixTabPartial", nomLst);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Index(Guid BatchId, int pipelineId)
        {
            ViewBag.Status = "";
            if (TempData["status"] != null)
            {
                ViewBag.Status = TempData["status"] + "";
            }
            //Fetch Nomination Details // batchid wise            

            BatchDetailDTO batchdetailModel=new BatchDetailDTO();
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();

            batchdetailModel = IPNTNominationService.GetNomDetail(BatchId, pipelineId);
            batchdetailModel = UpdateCounterPartyNameAndLocNameInBatchDetail(batchdetailModel);

            NomType modelType = pipelineService.GetPathTypeByPipelineDuns(pipelineService.GetDunsByPipelineID(pipelineId));
            batchdetailModel.PipelineModelType = modelType;

            if (modelType == NomType.HyPathedPNT && batchdetailModel.BatchNomType == 1)
            {
                DateTime todayDate = DateTime.Now.Date;

                int status = 0;
                int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(batchdetailModel.PipelineId, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));

                List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(batchdetailModel.PipelineId, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId), new SortingPagingInfo() { CurrentPageIndex = 0, PageSize = pathedListTotalCount });
                batchdetailModel.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);

                var CycleIndicator = _cycleIndicator.GetCycles();
                var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
                var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
                var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
                var Export = _metadataExportDeclarationService.GetExportDeclarations();
                var Status = _metadataFileStatusService.GetNomStatus();
                ViewBag.Cycles = CycleIndicator;
                ViewBag.QuantityType = QuantityType;
                ViewBag.CapacityType = CapacityType;
                ViewBag.BidUp = BidUp;
                ViewBag.Export = Export;
                ViewBag.StatusID = Status;
            }
           
            batchdetailModel.PipelineId = pipelineId;           
            batchdetailModel.ShiperDuns = currentIdentityValues.ShipperDuns; 
            batchdetailModel.ShiperName = currentIdentityValues.ShipperName;  
            if (batchdetailModel.ContractPath == null || batchdetailModel.ContractPath.Count <= 0)
            {
                batchdetailModel.ContractPath = new List<BatchDetailContractPathDTO>()
                {new BatchDetailContractPathDTO{ServiceRequestNo = "--Select--",ServiceRequestType = "--Select--" }
                };
            }

            if (batchdetailModel.SupplyList == null)
            {
                batchdetailModel.SupplyList = new List<BatchDetailSupplyDTO>();
            }
            if (batchdetailModel.Contract == null)
            {
                batchdetailModel.Contract = new List<BatchDetailContractDTO>();
            }
            if (batchdetailModel.MarketList == null)
            {
                batchdetailModel.MarketList = new List<BatchDetailMarketDTO>();
            }

            var notifier = _notifierEntityService.GetNotifierEntityForNoms(currentIdentityValues.UserId);//.GetNotifierEntityofBatchTable();
            ViewBag.NotifierEntity = notifier;

            return this.View(batchdetailModel);
        }

        [HttpPost]
        public ActionResult Index(BatchDetailDTO model, string send)
        {
            bool sendToTest = Convert.ToBoolean(ConfigurationManager.AppSettings["SendToTest"]);
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                              .Select(c => c.Value).SingleOrDefault();
            string shippername = identity.Claims.Where(c => c.Type == "ShipperName")
                               .Select(c => c.Value).SingleOrDefault();
            string shipperDuns = identity.Claims.Where(c => c.Type == "ShipperDuns")
                               .Select(c => c.Value).SingleOrDefault();
            var UserKey = Guid.Parse(identity.Claims.Where(c => c.Type == "UserId")
                            .Select(c => c.Value).SingleOrDefault());
            model.ShiperDuns = shipperDuns;
            model.ShiperName = shippername;
            model.CreatedBy = UserKey.ToString(); 
            if (send == null)
            {
                //TODO:

                var data = IPNTNominationService.SaveAndUpdatePNTBatchDetail(model, true);
             
                if (data != null)
                {
                    TempData["status"] = "Data saved successfully";
                    return RedirectToAction("Index", new { BatchId = model.Id, pipelineId = model.PipelineId });
                }
                else
                {
                    TempData["status"] = "Data saving failed";
                    return RedirectToAction("Index", new { BatchId = model.Id, pipelineId = model.PipelineId });
                }
            }
            else
            {    //TODO
                bool isSend = false;
                int statusId= _pathedNominationService.GetStatusOnTransactionId(model.Id);
                if (statusId == 11) {
                    isSend = IPNTNominationService.DirectSent(model, sendToTest);
                }
                if (isSend)
                {
                    TempData["status"] = "Successfully Sent";
                    return RedirectToAction("Index", "Batch", new { pipelineId = model.PipelineId });
                }
                else
                {
                    TempData["status"] = "Sending Failed";
                    return RedirectToAction("Index", new { BatchId = model.Id, pipelineId = model.PipelineId });
                }
            }

        }


        public ActionResult AddPathedHybrid(int pipelineid)
        {
            BatchDetailDTO mainmodel = new BatchDetailDTO();
            mainmodel.PipelineId = pipelineid;           
            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;

            mainmodel.PathedNomsList = new List<PathedNomDetailsDTO>();

            var item = new PathedNomDetailsDTO();
            DateTime todayDate = DateTime.Now.Date;
            item.StartDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 09, 00, 00);// todayDate;
            item.EndDate = new DateTime(todayDate.AddDays(1).Year, todayDate.AddDays(1).Month, (todayDate.AddDays(1).Day), 09, 00, 00);
            item.CycleID = 1;
            item.NomSubCycle = "Y";
            item.MaxRate = "Y";
            item.ProcessingRights = "Y";
            item.CreatedDate = DateTime.Now;
            item.QuantityType = "R";
            mainmodel.PathedNomsList.Add(item);

            return PartialView("_AddHybridPathedRow", mainmodel);
        }

        [HttpPost]
        public ActionResult LoadPNTPartial(BatchDetailDTO model)
        {
            model = IPNTNominationService.GetNomDetail(model.Id, model.PipelineId);
            model = UpdateCounterPartyNameAndLocNameInBatchDetail(model);
            return PartialView("_Pnt", model);
        }

        [HttpPost]
        public ActionResult LoadHybridPathedPartial(BatchDetailDTO model)
        {
            model.PathedNomsList = new List<PathedNomDetailsDTO>() { new PathedNomDetailsDTO() { StartDate = DateTime.Now, EndDate = DateTime.Now, CycleID = 1 } };

            DateTime todayDate = DateTime.Now.Date;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();           
            int status = -1;        
            int pathedListTotalCount = _pathedNominationService.GetPathedListTotalCount(model.PipelineId, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));
         
            List<PathedNomDetailsDTO> nomlist = _pathedNominationService.GetPathedListWithPaging(model.PipelineId, status, DateTime.MinValue, DateTime.Now.AddYears(1), currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId), new SortingPagingInfo() { CurrentPageIndex=0, PageSize = pathedListTotalCount});
            model.PathedNomsList = UpdatePathedCounterPartyAndLoactionName(nomlist);

            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status;
            return PartialView("_Pathedhybrid", model);
        }

        [HttpPost]
        public ActionResult LoadHybridNonPathedPartial(BatchDetailDTO model)
        {
            DateTime todayDate = DateTime.Now.Date;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            int status = -1;
            var  data = nonPathedService.GetNonPathedNominations(model.PipelineId, status, DateTime.MinValue, DateTime.MaxValue, currentIdentityValues.ShipperDuns, new Guid(currentIdentityValues.UserId));
            data = UpdateCounterPartyAndLocNameInNonPathed(data);
            model.ReceiptNoms = data.ReceiptNoms;
            model.DeliveryNoms = data.DeliveryNoms;
            var CycleIndicator = _cycleIndicator.GetCycles();           
            ViewBag.Cycles = CycleIndicator;           
            return PartialView("_NonPathedHybrid", model);
        }


        public PartialViewResult AddDeliveryRow(int PipelineID)
        {
           
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;                     
            model.DeliveryNoms.Add(new NonPathedDeliveryNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddDeliveryRowHybrid", model);
        }


        public PartialViewResult AddReceiptRow(int PipelineID)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;                  
            model.ReceiptNoms.Add(new NonPathedRecieptNom() { StartDateTime = DateTime.Now.Date, EndDateTime = DateTime.Now.Date.AddDays(2), CreateDateTime = DateTime.Now.Date });
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            return PartialView("_AddReceiptRowHybrid", model);
        }


        [HttpPost]
        public ActionResult SaveNonPathedHybrid(BatchDetailDTO Mainmodel)
        {
            NonPathedDTO model = new NonPathedDTO();
            model.PipelineId = Mainmodel.PipelineId;
            model.ReceiptNoms = Mainmodel.ReceiptNoms;
            model.DeliveryNoms = Mainmodel.DeliveryNoms;
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.ShipperDuns = currentIdentityValues.ShipperDuns;
            model.UserId = Guid.Parse(currentIdentityValues.UserId);
            var pipe = pipelineService.GetPipeline(model.PipelineId);
            model.PipelineDuns = pipe.DUNSNo;
            model.CompanyId = Convert.ToInt32(currentIdentityValues.CompanyId ?? "0");
            bool result = false;
            var Id = nonPathedService.SaveAllNonPathedNominations(model);
            result = (Id == Guid.Empty) ? false : true;
            ViewBag.Cycles = _cycleIndicator.GetCycles();
            ViewBag.SubmitStatus = result;
            if (result)
                TempData["status"] = "Data saved successfully";
            else
            {
                TempData["status"] = "Data saving failed";
            }
            return RedirectToAction("Index", new { BatchId = Mainmodel.Id, pipelineId = Mainmodel.PipelineId });
        }


        [HttpPost]
        public ActionResult PathedHybrid(BatchDetailDTO Mainmodel)
        {
            PathedDTO model = new PathedDTO();
            model.PathedNomsList = Mainmodel.PathedNomsList;
            model.PipelineID = Mainmodel.PipelineId;           
            ShipperReturnByIdentity currentIdentityValues = GetValueFromIdentity();
            model.DunsNo = currentIdentityValues.ShipperDuns;             
            model.companyId = String.IsNullOrEmpty(currentIdentityValues.CompanyId) ? 0 : int.Parse(currentIdentityValues.CompanyId);
            model.ShipperID = new Guid(currentIdentityValues.UserId);
            if (model.PathedNomsList == null)
            {
                TempData["status"] = "Please fill row.";
                return RedirectToAction("Index", new { BatchId = Mainmodel.Id, pipelineId = model.PipelineID });
            }

            Guid? transactionID = _pathedNominationService.SaveAndUpdatePathedNomination(model, true);
            if (transactionID != null)
                TempData["status"] = "Data saved successfully";
            else
            {
                TempData["status"] = "Data saving failed";
            }
            return RedirectToAction("Index", new { BatchId = Mainmodel.Id, pipelineId = model.PipelineID });
            
        }

        public PartialViewResult CopyHybridPathedRow(List<PathedNomDetailsDTO> PathedRecordToCopy, int pipelineid)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = pipelineid;

            var CycleIndicator = _cycleIndicator.GetCycles();
            var QuantityType = _metadataQuantityTypeIndicatorService.GetQuantityTypes();
            var CapacityType = _metadataCapacityTypeIndicatorService.GetCapacityTypes();
            var BidUp = _metaDataBidUpIndicatorService.GetBidUpIndicator();
            var Export = _metadataExportDeclarationService.GetExportDeclarations();
            var Status1 = _metadataFileStatusService.GetNomStatus();
            ViewBag.Cycles = CycleIndicator;
            ViewBag.QuantityType = QuantityType;
            ViewBag.CapacityType = CapacityType;
            ViewBag.BidUp = BidUp;
            ViewBag.Export = Export;
            ViewBag.StatusID = Status1;

            var item = new PathedNomDetailsDTO();
            var today = DateTime.Now;
            if (PathedRecordToCopy[0].StartDate < today)
            {
                var time = PathedRecordToCopy[0].StartDate.TimeOfDay;
                item.StartDate = today.Date.Add(time);
            }
            else
            {
                item.StartDate = PathedRecordToCopy[0].StartDate;
            }

            if (PathedRecordToCopy[0].EndDate < today)
            {
                var nextDay = DateTime.Now.AddDays(1);
                var time1 = PathedRecordToCopy[0].EndDate.TimeOfDay;
                item.EndDate = nextDay.Date.Add(time1);
            }
            else
            {
                item.EndDate = PathedRecordToCopy[0].EndDate;
            }
            if (item.EndDate < item.StartDate)
            {
                var nextDay = item.StartDate.AddDays(1);
                var time1 = PathedRecordToCopy[0].EndDate.TimeOfDay;
                item.EndDate = nextDay.Date.Add(time1);
            }
            // item.StartDate = PathedRecordToCopy[0].StartDate.Date < DateTime.Now.Date ? DateTime.Now.Date : PathedRecordToCopy[0].StartDate.Date;
            // item.EndDate = PathedRecordToCopy[0].EndDate.Date < DateTime.Now.Date ? DateTime.Now.AddDays(1).Date : PathedRecordToCopy[0].EndDate.Date;
            item.CycleID = PathedRecordToCopy[0].CycleID;
            item.Contract = PathedRecordToCopy[0].Contract;
            item.NomSubCycle = PathedRecordToCopy[0].NomSubCycle;
            item.TransType = PathedRecordToCopy[0].TransType;
            item.TransTypeMapId = PathedRecordToCopy[0].TransTypeMapId;
            item.RecLocation = PathedRecordToCopy[0].RecLocation;
            item.RecLocProp = PathedRecordToCopy[0].RecLocProp;
            item.RecLocID = PathedRecordToCopy[0].RecLocID;
            item.UpName = PathedRecordToCopy[0].UpName;
            item.UpIDProp = PathedRecordToCopy[0].UpIDProp;
            item.UpID = PathedRecordToCopy[0].UpID;
            item.DelLoc = PathedRecordToCopy[0].DelLoc;
            item.DelLocID = PathedRecordToCopy[0].DelLocID;
            item.DelLocProp = PathedRecordToCopy[0].DelLocProp;
            item.DownID = PathedRecordToCopy[0].DownID;
            item.DownIDProp = PathedRecordToCopy[0].DownIDProp;
            item.DownName = PathedRecordToCopy[0].DownName;
            item.QuantityType = PathedRecordToCopy[0].QuantityType;
            item.MaxRate = PathedRecordToCopy[0].MaxRate;
            item.CapacityType = PathedRecordToCopy[0].CapacityType;
            item.BidUp = PathedRecordToCopy[0].BidUp;
            item.Export = PathedRecordToCopy[0].Export;
            item.ProcessingRights = PathedRecordToCopy[0].ProcessingRights;

            item.UpKContract = PathedRecordToCopy[0].UpKContract;
            item.RecQty = PathedRecordToCopy[0].RecQty;
            item.RecRank = PathedRecordToCopy[0].RecRank;

            item.DownContract = PathedRecordToCopy[0].DownContract;
            item.DelQuantity = PathedRecordToCopy[0].DelQuantity;
            item.DelRank = PathedRecordToCopy[0].DelRank;

            item.PkgID = PathedRecordToCopy[0].PkgID;
            item.NomUserData1 = PathedRecordToCopy[0].NomUserData1;
            item.NomUserData2 = PathedRecordToCopy[0].NomUserData2;
            item.ActCode = PathedRecordToCopy[0].ActCode;
            item.BidTransportRate = PathedRecordToCopy[0].BidTransportRate;
            item.AssocContract = PathedRecordToCopy[0].AssocContract;
            item.DealType = PathedRecordToCopy[0].DealType;
            item.FuelPercentage = PathedRecordToCopy[0].FuelPercentage;

            item.UpPkgID = PathedRecordToCopy[0].UpPkgID;
            item.DownPkgID = PathedRecordToCopy[0].DownPkgID;
            item.UpRank = PathedRecordToCopy[0].UpRank;
            item.DownRank = PathedRecordToCopy[0].DownRank;

            item.CreatedDate = DateTime.Now;

            model.PathedNomsList = new List<PathedNomDetailsDTO>();
            model.PathedNomsList.Add(item);

            return PartialView("_AddHybridPathedRow", model);
        }

        public PartialViewResult NotimationsPartials(string partial, string clickedRow, string popUpFor, int PipelineID)
        {
            NominationPartialDTO model = new NominationPartialDTO();
            string partialView = string.Empty;
            model.ForRow = clickedRow;
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string company = identity.Claims.Where(c => c.Type == "CompanyId")
                               .Select(c => c.Value).SingleOrDefault();

            int companyId = String.IsNullOrEmpty(company) ? 0:int.Parse(company);
          
            model.PopUpFor = popUpFor;
            model.PipelineId = PipelineID;
            if (partial.ToLower() == "locations")
            {   
                model.Locations = new List<LocationsDTO>();               
                ViewBag.IsSpecialDelCase = false;
                ViewBag.PopUpFor = popUpFor;
                partialView = "~/Views/PNTNominations/_LocationPopUp.cshtml";
            }
            else if (partial == "TransactionType")
            {
                model.TransactionTypes = new List<TransactionTypesDTO>();
                //TODO:
                model.TransactionTypes = IPNTNominationService.GetTransactionsTypes(PipelineID, "", model.PopUpFor).ToList();
                model.TransactionTypes = model.TransactionTypes.OrderBy(a => Convert.ToInt32(a.Identifier)).ToList();
                partialView = "~/Views/PNTNominations/_TransactionTypePopUp.cshtml";
            }
            else if (partial == "CounterParties")
            {
                model.CounterParties = new List<CounterPartiesDTO>(); 
                partialView = "~/Views/PNTNominations/_CounterPartyPopUp.cshtml";
            }
            else if (partial == "Contract")
            {
                model.Contracts = new List<ContractsDTO>();
                //TODO:
                model.Contracts = IPNTNominationService.GetContracts("", companyId, PipelineID, 1, 1000).ToList();
                model.PipelineId = PipelineID;
                partialView = "~/Views/PNTNominations/_ContractPopUp.cshtml";
            }
            else if (partial == "Routes")
            {   
                model.Routes = IPNTNominationService.GetRoutes();
                model.PipelineId = PipelineID;
                partialView = "~/Views/PNTNominations/_RoutePopup.cshtml";
            }
            return PartialView(partialView, model);
        }

        public PartialViewResult AddContractPathRow(int PipelineID, string TableID)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;
             model.Duns= pipelineService.GetDunsByPipelineID(PipelineID);
            model.CurrentTableId = TableID;
            BatchDetailContractDTO contractObj = new BatchDetailContractDTO();
            model.Contract.Add(contractObj);
            return PartialView("_AddContractRow", model);
        }


        public PartialViewResult AddContractTransportPath(int PipelineID)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;
            model.Duns = pipelineService.GetDunsByPipelineID(PipelineID);
            BatchDetailContractPathDTO contract = new BatchDetailContractPathDTO();
            BatchDetailContractDTO transport = new BatchDetailContractDTO();
            model.ContractPath.Add(contract);
            model.Contract.Add(transport);
            return PartialView("_AddMultipleContractPath", model);
        }

        public PartialViewResult AddMarketRow(int PipelineID)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;
            model.Duns = pipelineService.GetDunsByPipelineID(PipelineID);
            BatchDetailMarketDTO marketObj = new BatchDetailMarketDTO();
            model.MarketList.Add(marketObj);
            return PartialView("_AddMarketRow", model);
        }

        public PartialViewResult AddSupplyRow(int PipelineID)
        {
            BatchDetailDTO model = new BatchDetailDTO();
            model.PipelineId = PipelineID;
            model.Duns = pipelineService.GetDunsByPipelineID(PipelineID);
            BatchDetailSupplyDTO supplyObj = new BatchDetailSupplyDTO();
            model.SupplyList.Add(supplyObj);
            return PartialView("_AddSupplyRow", model);
        }


        public bool ValidateVariance(List<TestNomSupply> SupplyRecords, List<TestNomTransport> TransportRecords, List<TestNomMarket> MarketRecords)
        {
            List<TestNomTransport> TransportRecordsRec = new List<TestNomTransport>();
            List<TestNomTransport> TransportRecordsDel = new List<TestNomTransport>();
            BatchTabModel modelLst = new BatchTabModel();
            // return batch partial
            try
            {
                if (SupplyRecords != null)
                    SupplyRecords = SupplyRecords
                    .GroupBy(a => a.LocProp)
                    .Select(cl => new TestNomSupply
                    {
                        LocProp = cl.First().LocProp,
                        LocName = cl.First().LocName,
                        DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                        RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString(),
                    }).ToList();
                if (MarketRecords != null)
                    MarketRecords = MarketRecords
                        .GroupBy(a => a.LocProp)
                        .Select(cl => new TestNomMarket
                        {
                            LocProp = cl.First().LocProp,
                            LocName = cl.First().LocName,
                            DelQty = cl.Sum(c => Convert.ToDecimal(c.DelQty)).ToString(),
                            RecQty = cl.Sum(c => Convert.ToDecimal(c.RecQty)).ToString()
                        }).ToList();
                if (TransportRecords != null)
                    TransportRecordsRec = TransportRecords.GroupBy(a => a.RecLocProp)
                        .Select(cl => new TestNomTransport
                        {
                            RecLoc = cl.First().RecLoc,
                            RecLocProp = cl.First().RecLocProp,
                            RecLocName = cl.First().RecLocName,
                            RecDTH = cl.Sum(c => Convert.ToDecimal(c.RecDTH)).ToString(),
                            DelDTH = cl.Sum(c => Convert.ToDecimal(c.DelDTH)).ToString()
                        }).ToList();
                if (TransportRecords != null)
                    TransportRecordsDel = TransportRecords.GroupBy(a => a.DelLocProp)
                        .Select(cl => new TestNomTransport
                        {
                            DelLoc = cl.First().DelLoc,
                            DelLocProp = cl.First().DelLocProp,
                            DelLocName = cl.First().DelLocName,
                            RecDTH = cl.Sum(c => Convert.ToDecimal(c.RecDTH)).ToString(),
                            DelDTH = cl.Sum(c => Convert.ToDecimal(c.DelDTH)).ToString()
                        }).ToList();

                if (SupplyRecords != null)
                    foreach (var item in SupplyRecords)
                    {
                        if (modelLst.Lst.Any(a => a.LocProp == item.LocProp))
                        {
                            BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.LocProp).FirstOrDefault();
                            modelLst.Lst.Remove(batchTab);
                            batchTab.RecQty = batchTab.RecQty + Convert.ToInt32(item.RecQty);
                            //batchTab.DelQty=
                            batchTab.Variance = batchTab.RecQty - batchTab.DelQty;
                            modelLst.Lst.Add(batchTab);
                        }
                        else
                        {
                            BatchTabModel model = new BatchTabModel();
                            //model.DelQty = Convert.ToInt32(item.DelQty);
                            model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.RecQty));
                            model.LocProp = item.LocProp;
                            model.Location = item.LocName;
                            //model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.DelQty));
                            model.Variance = model.RecQty - model.DelQty;
                            modelLst.Lst.Add(model);
                        }


                    }

                if (TransportRecordsRec != null)
                    foreach (var item in TransportRecordsRec)
                    {
                        if (modelLst.Lst.Any(a => a.LocProp == item.RecLocProp))
                        {
                            BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.RecLocProp).FirstOrDefault();
                            modelLst.Lst.Remove(batchTab);
                            batchTab.DelQty = Convert.ToInt32(item.RecDTH);
                            batchTab.Variance = batchTab.RecQty - batchTab.DelQty;
                            modelLst.Lst.Add(batchTab);
                        }
                        else
                        {
                            BatchTabModel model = new BatchTabModel();
                            //model.DelQty = Convert.ToInt32(item.DelQty);
                            // model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.DelDTH));
                            model.LocProp = item.RecLocProp;
                            model.Location = item.RecLocName;
                            model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.RecDTH));
                            model.Variance = model.RecQty - model.DelQty;
                            modelLst.Lst.Add(model);
                        }
                    }
                if (TransportRecordsDel != null)
                    foreach (var item in TransportRecordsDel)
                    {
                        if (modelLst.Lst.Any(a => a.LocProp == item.DelLocProp))
                        {
                            BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.DelLocProp).FirstOrDefault();
                            modelLst.Lst.Remove(batchTab);
                            batchTab.RecQty = batchTab.RecQty + Convert.ToInt32(item.RecDTH);
                            batchTab.Variance = batchTab.RecQty - batchTab.DelQty;
                            modelLst.Lst.Add(batchTab);
                        }
                        else
                        {
                            BatchTabModel model = new BatchTabModel();
                            //model.DelQty = Convert.ToInt32(item.DelQty);
                            model.RecQty = Convert.ToInt32(Convert.ToDecimal(item.RecDTH));
                            model.LocProp = item.DelLocProp;
                            model.Location = item.DelLocName;
                            //model.DelQty = Convert.ToInt32(Convert.ToDecimal(item.DelDTH));
                            model.Variance = model.RecQty - model.DelQty;
                            modelLst.Lst.Add(model);
                        }
                    }

                if (MarketRecords != null)
                    foreach (var item in MarketRecords)
                    {
                        if (!modelLst.Lst.Any(a => a.LocProp == item.LocProp))
                        {
                            BatchTabModel model = new BatchTabModel();
                            model.DelQty = Convert.ToInt32(item.RecQty);
                            //model.RecQty = Convert.ToInt32(item.RecQty);
                            model.LocProp = item.LocProp;
                            model.Location = item.LocName;
                            model.Variance = model.RecQty - model.DelQty;
                            modelLst.Lst.Add(model);
                        }
                        else
                        {
                            BatchTabModel batchTab = modelLst.Lst.Where(a => a.LocProp == item.LocProp).FirstOrDefault();
                            modelLst.Lst.Remove(batchTab);
                            batchTab.DelQty = batchTab.DelQty + Convert.ToInt32(item.RecQty);
                            batchTab.Variance = batchTab.RecQty - batchTab.DelQty;
                            modelLst.Lst.Add(batchTab);
                        }

                    }

            }
            catch (Exception ex)
            {
                return false;
            }

            var isVarianceBalanced = true;
            foreach (var item in modelLst.Lst) {
                if (item.Variance != 0)
                {
                    isVarianceBalanced = false;
                }
            }
            return isVarianceBalanced;
        }

        
    }

    public class TransportLocations
    {
        public string Loc { get; set; }
        public string LocName { get; set; }
        public string Type { get; set; }//D for deliver , R for Receipt
    }

    public class UniqueLocations
    {
        public string Loc { get; set; }
        public string LocName { get; set; }
        public string Type { get; set; }//D for deliver , R for Receipt
    }
}