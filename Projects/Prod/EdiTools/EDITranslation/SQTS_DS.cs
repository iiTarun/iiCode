using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using Nom1Done.DTO;
using System.Xml;

namespace EDITranslation.Nominations
{
    public class SQTS_DS :EDIWrapperBase
    {
        string _fileName;
        List<SQTSPerTransactionDTO> _sqtsTable;
        IEnumerable<XmlNode> _redReasonList = null;
        public SQTS_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator, string fileName, IEnumerable<XmlNode> redReasonList)
             : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            _sqtsTable = new List<SQTSPerTransactionDTO>();
            _redReasonList = redReasonList;
        }
        public List<SQTSPerTransactionDTO> ReadSQTSFile()
        {
            PopulateSQTSData();
            return _sqtsTable;
        }
        private void PopulateSQTSData()
        {
            foreach (string[] stBlock in _stEnvelopes)
            {
                var statementDateQuery = from item in stBlock
                                         where item.StartsWith("DTM") && (item[4] == '1' && item[5] == '0' && item[6] == '2')//item.Contains("102")
                                         select item;

                var tspCodeQuery = from item in stBlock
                                   where item.StartsWith("N1") && (item[3]=='S' && item[4]=='J')//item.Contains("SJ")
                                   select item;

                var requestorCodeQuery = from item in stBlock
                                         where item.StartsWith("N1") && (item[3]=='7' && item[4]=='8')//item.Contains("78")
                                         select item;

                var dtmInnerIndexes = Enumerable.Range(0, stBlock.Count())
                                 .Where(i => stBlock[i].StartsWith("DTM") && stBlock[i][4] == '0' && stBlock[i][5] == '0' && stBlock[i][6] == '7')
                                 .ToList();

                List<string[]> dtmInnerSegments = new List<string[]>();

                if (dtmInnerIndexes.Count() == 1)
                {
                    string[] dtmInnerBlock = new string[stBlock.Count() - dtmInnerIndexes[0]];
                    Array.Copy(stBlock, dtmInnerIndexes[0], dtmInnerBlock, 0, (stBlock.Count() - dtmInnerIndexes[0]));
                    dtmInnerSegments.Add(dtmInnerBlock);
                }
                else
                {
                    for (int i = 0; i < dtmInnerIndexes.Count(); i++)
                    {
                        if ((i + 1) == dtmInnerIndexes.Count())
                        {
                            string[] dtmLastBlock = new string[stBlock.Count() - dtmInnerIndexes[i]];
                            Array.Copy(stBlock, dtmInnerIndexes[i], dtmLastBlock, 0, (stBlock.Count() - dtmInnerIndexes[i]));
                            dtmInnerSegments.Add(dtmLastBlock);
                            break;
                        }
                        string[] dtmInnerBlock = new string[dtmInnerIndexes[i + 1] - dtmInnerIndexes[i]];
                        Array.Copy(stBlock, dtmInnerIndexes[i], dtmInnerBlock, 0, (dtmInnerIndexes[i + 1] - dtmInnerIndexes[i]));
                        dtmInnerSegments.Add(dtmInnerBlock);
                    }
                }

                foreach (var dtmInnerBlock in dtmInnerSegments)
                {

                    var beginingEndDateQuery = from item in dtmInnerBlock
                                               where item.StartsWith("DTM") && item.Contains("007")
                                               select item;

                    var cycleIndicatorQuery = from item in dtmInnerBlock
                                              where item.StartsWith("N9") && item.Contains("CYI")
                                              select item;

                    var contractQuery = from item in dtmInnerBlock
                                        where item.StartsWith("CS")
                                        select item;

                    var slnInnerIndexes = Enumerable.Range(0, dtmInnerBlock.Count())
                                     .Where(i => dtmInnerBlock[i].StartsWith("SLN"))
                                     .ToList();

                    List<string[]> slnInnerSegments = new List<string[]>();
                    if (slnInnerIndexes.Count() == 1)
                    {
                        string[] slnInnerBlock = new string[dtmInnerBlock.Count() - slnInnerIndexes[0]];
                        Array.Copy(dtmInnerBlock, slnInnerIndexes[0], slnInnerBlock, 0, (dtmInnerBlock.Count() - slnInnerIndexes[0]));
                        slnInnerSegments.Add(slnInnerBlock);
                    }
                    else
                    {
                        for (int i = 0; i < slnInnerIndexes.Count(); i++)
                        {
                            if ((i + 1) == slnInnerIndexes.Count())
                            {
                                string[] slnLastBlock = new string[dtmInnerBlock.Count() - slnInnerIndexes[i]];
                                Array.Copy(dtmInnerBlock, slnInnerIndexes[i], slnLastBlock, 0, (dtmInnerBlock.Count() - slnInnerIndexes[i]));
                                slnInnerSegments.Add(slnLastBlock);
                                break;
                            }
                            string[] slnInnerBlock = new string[slnInnerIndexes[i + 1] - slnInnerIndexes[i]];
                            Array.Copy(dtmInnerBlock, slnInnerIndexes[i], slnInnerBlock, 0, (slnInnerIndexes[i + 1] - slnInnerIndexes[i]));
                            slnInnerSegments.Add(slnInnerBlock);
                        }
                    }

                    foreach (string[] slnInnerSegment in slnInnerSegments)
                    {
                        var nomTrackingIdQuery = from item in slnInnerSegment
                                                 where item.StartsWith("SLN")
                                                 select item;

                        var transactionTypeQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LQ") && item.Contains("TT")
                                                   select item;

                        var reductionReasonQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LQ") && item.Contains("RED")
                                                   select item;

                        var capacityTypeIndicatorQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LQ") && item.Contains("CT")
                                                   select item;

                        var exportDeclarationQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LQ") && item.Contains("XD")
                                                   select item;

                        var nominationSubsequentCycleIndicatorQuery = from item in slnInnerSegment
                                                                      where item.StartsWith("LQ") && item.Contains("SCI")
                                                                      select item;

                        var processingRightsIndicatorQuery = from item in slnInnerSegment
                                                             where item.StartsWith("LQ") && item.Contains("PRI")
                                                             select item;

                        var receiptRankQuery = from item in slnInnerSegment
                                               where item.StartsWith("LQ") && item.Contains("R2")
                                               select item;

                        var deliveryRankQuery = from item in slnInnerSegment
                                                where item.StartsWith("LQ") && item.Contains("R3")
                                                select item;

                        var routeQuery = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("RU")
                                             select item;

                        var packageIdQuery = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("PKG")
                                             select item;

                        var dealTypeQuery = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("PD")
                                             select item;

                        var associatedContractQuery = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("KAS")
                                             select item;

                        var ServiceProviderActivityCodeQuery = from item in slnInnerSegment
                                                               where item.StartsWith("N9") && item.Contains("BE")
                                                               select item;

                        var nomUserData1Query = from item in slnInnerSegment
                                                where item.StartsWith("N9") && item.Contains("JD")
                                                select item;

                        var nomUserData2Query = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("Y8")
                                             select item;

                        var upstreamIdQuery = from item in slnInnerSegment
                                              where item.StartsWith("N1") && (item.Contains("US"))
                                              select item;

                        var dwnstreamIdQuery = from item in slnInnerSegment
                                              where item.StartsWith("N1") && (item.Contains("DW"))
                                              select item;

                        var receiptLocationQuery = from item in slnInnerSegment
                                               where item.StartsWith("LCD") && (item.Contains("M2"))
                                               select item;

                        var deliveryLocationQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LCD") && (item.Contains("MQ"))
                                                   select item;

                        var downstreamContractIdentifierLocationQuery = from item in slnInnerSegment
                                                   where item.StartsWith("N9") && (item.Contains("DT"))
                                                   select item;

                        var downstreamPkgIdQuery = from item in slnInnerSegment
                                                   where item.StartsWith("N9") && (item.Contains("PGD"))
                                                   select item;

                        var upstreamContractIdentifierLocationQuery = from item in slnInnerSegment
                                                                      where item.StartsWith("N9") && (item.Contains("UP"))
                                                                      select item;

                        var upstreamPkgIdQuery = from item in slnInnerSegment
                                                   where item.StartsWith("N9") && (item.Contains("PKU"))
                                                   select item;

                        var receiptPointQuantityQuery= from item in slnInnerSegment
                                                       where item.StartsWith("QTY") && (item.Contains("87"))
                                                       select item;

                        var deliveryPointQuantityQuery = from item in slnInnerSegment
                                                        where item.StartsWith("QTY") && (item.Contains("QD"))
                                                        select item;


                        string[] statementDateItems = (statementDateQuery.FirstOrDefault() == null) ? null : statementDateQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] tspCodeItems = (tspCodeQuery.FirstOrDefault() == null) ? null : tspCodeQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] requestorCodeItems = (requestorCodeQuery.FirstOrDefault() == null) ? null : requestorCodeQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] beginingEndDateItems = (beginingEndDateQuery.FirstOrDefault() == null) ? null : beginingEndDateQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] cycleIndicatorItems = (cycleIndicatorQuery.FirstOrDefault() == null) ? null : cycleIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] contractItems = (contractQuery.FirstOrDefault() == null) ? null : contractQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] slnItems = (nomTrackingIdQuery.FirstOrDefault() == null) ? null : nomTrackingIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] transTypeItems = (transactionTypeQuery.FirstOrDefault() == null) ? null : transactionTypeQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] reductionReasonItems = (reductionReasonQuery.FirstOrDefault() == null) ? null : reductionReasonQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] packageIdItems = (packageIdQuery.FirstOrDefault() == null) ? null : packageIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] upstreamIdItems = (upstreamIdQuery.FirstOrDefault() == null) ? null : upstreamIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] capacityTypeIndicator = (capacityTypeIndicatorQuery.FirstOrDefault() == null) ? null : capacityTypeIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] exportDeclaration = (exportDeclarationQuery.FirstOrDefault() == null) ? null : exportDeclarationQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] nominationSubsequentCycleIndicator = (nominationSubsequentCycleIndicatorQuery.FirstOrDefault() == null) ? null : nominationSubsequentCycleIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] processingRightsIndicator = (processingRightsIndicatorQuery.FirstOrDefault() == null) ? null : processingRightsIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] routeIndicator = (routeQuery.FirstOrDefault() == null) ? null : routeQuery.FirstOrDefault().Split(_dataSeparator);
                        //string[] packageId = (packageIdQuery.FirstOrDefault() == null) ? null : packageIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] dealTypeItems = (dealTypeQuery.FirstOrDefault() == null) ? null : dealTypeQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] associatedContractItems = (associatedContractQuery.FirstOrDefault() == null) ? null : associatedContractQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] ServiceProviderActivityCodeItems = (ServiceProviderActivityCodeQuery.FirstOrDefault() == null) ? null : ServiceProviderActivityCodeQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] nomUserData1Item = (nomUserData1Query.FirstOrDefault() == null) ? null : nomUserData1Query.FirstOrDefault().Split(_dataSeparator);
                        string[] nomUserData2Item = (nomUserData2Query.FirstOrDefault() == null) ? null : nomUserData2Query.FirstOrDefault().Split(_dataSeparator);
                        //string[] upstreamId = (upstreamIdQuery.FirstOrDefault() == null) ? null : upstreamIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] dwnstreamIdItems = (dwnstreamIdQuery.FirstOrDefault() == null) ? null : dwnstreamIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] receiptLocationItems = (receiptLocationQuery.FirstOrDefault() == null) ? null : receiptLocationQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] deliveryLocationItems = (deliveryLocationQuery.FirstOrDefault() == null) ? null : deliveryLocationQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] downstreamContractIdentifierLocationItems = (downstreamContractIdentifierLocationQuery.FirstOrDefault() == null) ? null : downstreamContractIdentifierLocationQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] downstreamPkgIdItems = (downstreamPkgIdQuery.FirstOrDefault() == null) ? null : downstreamPkgIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] upstreamContractIdentifierLocationItems = (upstreamContractIdentifierLocationQuery.FirstOrDefault() == null) ? null : upstreamContractIdentifierLocationQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] upstreamPkgIdItems = (upstreamPkgIdQuery.FirstOrDefault() == null) ? null : upstreamPkgIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] receiptPointQuantityItems = (receiptPointQuantityQuery.FirstOrDefault() == null) ? null : receiptPointQuantityQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] deliveryPointQuantityItems = (deliveryPointQuantityQuery.FirstOrDefault() == null) ? null : deliveryPointQuantityQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] receiptRankItems = (receiptRankQuery.FirstOrDefault() == null) ? null : receiptRankQuery.FirstOrDefault().Split(_dataSeparator);
                        string[] delievryRankItems = (deliveryRankQuery.FirstOrDefault() == null) ? null : deliveryRankQuery.FirstOrDefault().Split(_dataSeparator);

                        DateTime statementDateTime = DateTime.ParseExact(statementDateItems[6], "yyyyMMddHHmm", CultureInfo.GetCultureInfo("tr-TR"));
                        string tspCode = (tspCodeItems == null) ? "" : tspCodeItems[4];
                        string requestorCode = (requestorCodeItems == null) ? "" : requestorCodeItems[4];
                        string[] dates = beginingEndDateItems[6].Split('-');
                        DateTime beginingDateTime = DateTime.ParseExact(dates[0].Substring(0,8), "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));
                        DateTime endDateTime = DateTime.ParseExact(dates[1].Substring(0, 8), "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));
                        string cycleIndicator = (cycleIndicatorItems == null) ? "" : cycleIndicatorItems[2];
                        string contractNumber = (contractItems == null) ? "" : contractItems[1];
                        string modelType = (contractItems == null) ? "" : contractItems[5];
                        string nomTrackingId = (slnItems == null) ? "" : slnItems[1];
                        //string bidTransportationRae = (slnItems == null) ? "" : slnItems[];
                        int fuelQuantity = 0;//(slnItems == null) ? 0 : int.Parse(slnItems[4]);
                        string transactionType = (transTypeItems == null) ? "" : transTypeItems[2];
                        string reductionReason = (reductionReasonItems == null) ? "" : reductionReasonItems[2];

                        string captypeIndicator = (capacityTypeIndicator == null) ? "" : capacityTypeIndicator[2];
                        string expDeclaration = (exportDeclaration == null) ? "" : exportDeclaration[2];
                        string nomSubseqCycleIndicator = (nominationSubsequentCycleIndicator == null) ? "" : nominationSubsequentCycleIndicator[2];
                        string processingRightsInd = (processingRightsIndicator == null) ? "" : processingRightsIndicator[2];

                        string route = (routeIndicator == null) ? "" : routeIndicator[2];
                        string packageId = (packageIdItems == null) ? "" : packageIdItems[2];
                        string dealType = (dealTypeItems == null) ? "" : dealTypeItems[2];
                        string associatedContract = (associatedContractItems == null) ? "" : associatedContractItems[2];
                        string ServiceProviderActivityCode = (ServiceProviderActivityCodeItems == null) ? "" : ServiceProviderActivityCodeItems[2];
                        string nomUserData1 = (nomUserData1Item == null) ? "" : nomUserData1Item[2];
                        string nomUserData2 = (nomUserData2Item == null) ? "" : nomUserData2Item[2];

                        string dwnstreamId = (dwnstreamIdItems == null) ? "" : dwnstreamIdItems[4];
                        string upstreamId = (upstreamIdItems == null) ? "" : upstreamIdItems[4];

                        string receiptLocation = (receiptLocationItems == null) ? "" : receiptLocationItems[6];
                        string deliveryLocation = (deliveryLocationItems == null) ? "" : deliveryLocationItems[6];

                        string downstreamContractIdentifierLocation = (downstreamContractIdentifierLocationItems == null) ? "" : downstreamContractIdentifierLocationItems[2];//p
                        string upstreamContractIdentifierLocation = (upstreamContractIdentifierLocationItems == null) ? "" : upstreamContractIdentifierLocationItems[2];//p

                        string downstreamPkgId = (downstreamPkgIdItems == null) ? "" : downstreamPkgIdItems[2];//p
                        string upstreamPkgId = (upstreamPkgIdItems == null) ? "" : upstreamPkgIdItems[2];//p

                        int receiptPointQuantity = (receiptPointQuantityItems == null) ? 0 : int.Parse(receiptPointQuantityItems[2]);
                        int deliveryPointQuantity = (deliveryPointQuantityItems == null) ? 0 : int.Parse(deliveryPointQuantityItems[2]);

                        string receiptRank = (receiptRankItems == null) ? "" : receiptRankItems[2];
                        string deliveryRank = (delievryRankItems == null) ? "" : delievryRankItems[2];

                        var redReasonDesc = string.Empty;
                        if(_redReasonList!=null && _redReasonList.Count() > 0)
                        {
                            redReasonDesc = _redReasonList.Where(a => a.SelectSingleNode("Code").InnerText == reductionReason)
                                .Select(n => n.SelectSingleNode("Value").InnerText).FirstOrDefault();
                        }

                        SQTSPerTransactionDTO sqts = new SQTSPerTransactionDTO();
                        sqts.StatementDate = statementDateTime;
                        sqts.TSPCode = tspCode;
                        sqts.ServiceRequestor = requestorCode;
                        sqts.BeginingDateTime = beginingDateTime;
                        sqts.EndingDateTime = endDateTime;
                        sqts.CycleIndicator = cycleIndicator;
                        sqts.ServiceRequestorContract = contractNumber;
                        sqts.ModelType = modelType;
                        sqts.NomTrackingId = nomTrackingId;
                        sqts.BidTransportationRate = "";
                        sqts.FuelQuantity = fuelQuantity.ToString();
                        sqts.TransactionType = transactionType;
                        sqts.ReductionReason = reductionReason;
                        sqts.PackageId = packageId;
                        sqts.UpstreamId = upstreamId;
                        sqts.ReceiptLocation = receiptLocation;
                        sqts.ReceiptRank = receiptRank;
                        sqts.ReceiptQuantity = receiptPointQuantity;
                        sqts.DownstreamID = dwnstreamId;
                        sqts.DeliveryLocation = deliveryLocation;
                        sqts.DeliveryQuantity = deliveryPointQuantity;
                        sqts.DeliveryRank = deliveryRank;
                        sqts.CapacityTypeIndicator = captypeIndicator;
                        sqts.ExportDecleration = expDeclaration;
                        sqts.NomSubsequentCycleIndicator = nomSubseqCycleIndicator;
                        sqts.ProcessingRightsIndicator = processingRightsInd;
                        sqts.Route = route;
                        sqts.DealType = dealType;
                        sqts.AssociatedContract = associatedContract;
                        sqts.ServiceProviderActivityCode = ServiceProviderActivityCode;
                        sqts.NominationUserData1 = nomUserData1;
                        sqts.NominationUserData2 = nomUserData2;
                        sqts.DownstreamContractIdentifier = downstreamContractIdentifierLocation;
                        sqts.UpstreamContractIdentifier = upstreamContractIdentifierLocation;
                        sqts.DownstreamPackageId = downstreamPkgId;
                        sqts.UpstreamPackageId = upstreamPkgId;
                        sqts.TranasactionId = Guid.Parse(_fileName);
                        sqts.ReductionReasonDescription = redReasonDesc;
                        _sqtsTable.Add(sqts);
                    }
                }
            }
        }
    }
}
