using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDITranslation.Nominations
{
    public class SQTSOP_DS : EDIWrapperBase
    {
        string _fileName;
        List<SQTSOPPerTransactionDTO> _sqtsopTable;
        public SQTSOP_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator,string fileName) 
            : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            _sqtsopTable = new List<SQTSOPPerTransactionDTO>();
        }
        public List<SQTSOPPerTransactionDTO> ReadSqtsopFile()
        {
            PopulateSQTSOPData();
            return _sqtsopTable;
        }

        private void PopulateSQTSOPData()
        {
            foreach(string[] stBlock in _stEnvelopes)
            {
                var statementDateQuery = from item in stBlock
                                         where item.StartsWith("DTM") && item.Contains("102")
                                         select item;
                string[] statementDateItems = (statementDateQuery.FirstOrDefault() == null) ? null : statementDateQuery.FirstOrDefault().Split(_dataSeparator);
                DateTime statementDate= DateTime.ParseExact(statementDateItems[6], "yyyyMMddHHmm", CultureInfo.GetCultureInfo("tr-TR"));

                var tspCodeQuery = from item in stBlock
                                   where item.StartsWith("N1") && item.Contains("41")
                                   select item;
                string[] tspCodeItem = (tspCodeQuery.FirstOrDefault() == null) ? null : tspCodeQuery.FirstOrDefault().Split(_dataSeparator);
                string tspCode = (tspCodeItem!=null && tspCodeItem.Count() >= 4) ? tspCodeItem[4] : string.Empty;

                var receiverCodeQuery = from item in stBlock
                                         where item.StartsWith("N1") && item.Contains("40")
                                         select item;
                string[] receiverCodeItem = (receiverCodeQuery.FirstOrDefault() == null) ? null : receiverCodeQuery.FirstOrDefault().Split(_dataSeparator);
                string receiverCode = (receiverCodeItem!=null && receiverCodeItem.Count() >= 4) ? receiverCodeItem[4] : string.Empty;


                var dtmInnerIndexes = Enumerable.Range(0, stBlock.Count())
                                 .Where(i => stBlock[i].StartsWith("DTM") && stBlock[i].Contains("007"))
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

                    string[] begEndDateItem = (beginingEndDateQuery.FirstOrDefault() == null) ? null : beginingEndDateQuery.FirstOrDefault().Split(_dataSeparator);

                    string[] begEndDates = begEndDateItem[6].Split('-');
                    DateTime effectiveStartDate = DateTime.ParseExact(begEndDates[0].Substring(0, 8), "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));
                    DateTime effectiveEndDate = DateTime.ParseExact(begEndDates[1].Substring(0, 8), "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));

                    var cycleIndicatorQuery = from item in dtmInnerBlock
                                              where item.StartsWith("N9") && item.Contains("CYI")
                                              select item;
                    string[] cycleIndItem = (cycleIndicatorQuery.FirstOrDefault() == null) ? null : cycleIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                    string cycleIndicator = (cycleIndItem != null && cycleIndItem.Count() >= 2) ? cycleIndItem[2] : string.Empty;


                    var LocationaCapacityFlowIndicatorQuery = from item in dtmInnerBlock
                                                              where item.StartsWith("LQ") && item.Contains("COR")
                                                              select item;
                    string[] locationCapacityFlowIndicatorItem = (LocationaCapacityFlowIndicatorQuery.FirstOrDefault() == null) ? null : LocationaCapacityFlowIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                    string locationCapacityFlowIndicator = (locationCapacityFlowIndicatorItem!=null && locationCapacityFlowIndicatorItem.Count() >=2)? locationCapacityFlowIndicatorItem[2] : string.Empty;

                    var ConfirmationRoleQuery = from item in dtmInnerBlock
                                                where item.StartsWith("LQ") && item.Contains("LCF")
                                                select item;
                    string[] confirmationRoleItem = (ConfirmationRoleQuery.FirstOrDefault() == null) ? null : ConfirmationRoleQuery.FirstOrDefault().Split(_dataSeparator);
                    string confirmationRole= (confirmationRoleItem!=null && confirmationRoleItem.Count() >= 2) ? confirmationRoleItem[2] : string.Empty;

                    var LocationQuery = from item in dtmInnerBlock
                                        where item.StartsWith("LCD") && item.Contains("LCN")
                                        select item;

                    string[] GasNominatinLocationItem = (LocationQuery.FirstOrDefault() == null) ? null : LocationQuery.FirstOrDefault().Split(_dataSeparator);
                    string Location = (GasNominatinLocationItem !=null && GasNominatinLocationItem.Count() >= 6) ? GasNominatinLocationItem[6] : string.Empty;

                    var contractQuery = from item in dtmInnerBlock
                                        where item.StartsWith("CS")
                                        select item;
                    string[] contractItem = (contractQuery.FirstOrDefault() == null) ? null : contractQuery.FirstOrDefault().Split(_dataSeparator);
                    string confirmationServiceContract = (contractItem != null && contractItem.Count() >= 1) ? contractItem[1] : string.Empty;

                    var LocationNetQuantityQuery = from item in dtmInnerBlock
                                                   where item.StartsWith("QTY") && item.Contains("L8")
                                                   select item;
                    string[] locationNetQuantityItem = (LocationNetQuantityQuery.FirstOrDefault() == null) ? null : LocationNetQuantityQuery.FirstOrDefault().Split(_dataSeparator);
                    long locationNetQuantity = (locationNetQuantityItem!=null && locationNetQuantityItem.Count() >= 2) ? Convert.ToInt64(locationNetQuantityItem[2]) : 0;


                    var confirmationServiceIdentifierQuery = from item in dtmInnerBlock
                                        where item.StartsWith("N1") && item.Contains("CNS")
                                        select item;

                    string[] ServiceIdentifierCodeItem = (confirmationServiceIdentifierQuery.FirstOrDefault() == null) ? null : confirmationServiceIdentifierQuery.FirstOrDefault().Split(_dataSeparator);
                    string ServiceIdentifierCode = (ServiceIdentifierCodeItem != null && ServiceIdentifierCodeItem.Count() >= 4) ? ServiceIdentifierCodeItem[4] : string.Empty;


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
                        var SulineItemDetailQuery = from item in slnInnerSegment
                                                 where item.StartsWith("SLN")
                                                 select item;

                        string[] slnItems = (SulineItemDetailQuery.FirstOrDefault() == null) ? null : SulineItemDetailQuery.FirstOrDefault().Split(_dataSeparator);

                        string confirmationTrackingId = (slnItems == null) ? "" : slnItems[1];
                        long quantity = (slnItems == null) ? 0 : Convert.ToInt64(slnItems[4]);


                        var contractualFlowIndicatorQuery = from item in slnInnerSegment
                                                            where item.StartsWith("LQ") && item.Contains("CFI")
                                                            select item;
                        string[] contractualFlowIndicatorItem = (contractualFlowIndicatorQuery.FirstOrDefault() == null) ? null : contractualFlowIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string contractualFlowIndicator = (contractualFlowIndicatorItem != null && contractualFlowIndicatorItem.Count() >= 2) ? contractualFlowIndicatorItem[2] : string.Empty;

                        var ReductionReasonQuery= from item in slnInnerSegment
                                                  where item.StartsWith("LQ") && item.Contains("RED")
                                                  select item;

                        string[] ReductionReasonItem = (ReductionReasonQuery.FirstOrDefault() == null) ? null : ReductionReasonQuery.FirstOrDefault().Split(_dataSeparator);
                        string ReductionReason = (ReductionReasonItem != null && ReductionReasonItem.Count() >= 2) ? ReductionReasonItem[2] : string.Empty;

                        var ConfirmationSusequenceCycleIndicatorQuery = from item in slnInnerSegment
                                                   where item.StartsWith("LQ") && item.Contains("SCI")
                                                   select item;

                        string[] ConfirmationSusequenceCycleIndicatorItem = (ConfirmationSusequenceCycleIndicatorQuery.FirstOrDefault() == null) ? null : ConfirmationSusequenceCycleIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                        string ConfirmationSusequenceCycleIndicator = (ConfirmationSusequenceCycleIndicatorItem != null && ConfirmationSusequenceCycleIndicatorItem.Count() >= 2) ? ConfirmationSusequenceCycleIndicatorItem[2] : string.Empty;

                        var SchedulingStatusQuery = from item in slnInnerSegment
                                                                        where item.StartsWith("LQ") && item.Contains("SLS")
                                                                        select item;

                        string[] SchedulingStatusItem = (SchedulingStatusQuery.FirstOrDefault() == null) ? null : SchedulingStatusQuery.FirstOrDefault().Split(_dataSeparator);
                        string SchedulingStatus = (SchedulingStatusItem != null && SchedulingStatusItem.Count() >= 2) ? SchedulingStatusItem[2] : string.Empty;

                        var PackageIdQuery = from item in slnInnerSegment
                                             where item.StartsWith("N9") && item.Contains("PKG")
                                             select item;

                        var ConfirmationUserData1Query= from item in slnInnerSegment
                                                   where item.StartsWith("N9") && item.Contains("JD")
                                                   select item;

                        var ConfirmationUserData2Query = from item in slnInnerSegment
                                                    where item.StartsWith("N9") && item.Contains("Y8")
                                                    select item;

                        var ReductionQuantityQuery = from item in slnInnerSegment
                                                         where item.StartsWith("QTY") && item.Contains("Z9")
                                                         select item;

                        var ServiceRequesterQuery = from item in slnInnerSegment
                                                     where item.StartsWith("N1") && item.Contains("78")
                                                     select item;

                        var DownstreamPartyQuery = from item in slnInnerSegment
                                                     where item.StartsWith("N1") && item.Contains("DW")
                                                     select item;

                        var UpstreamPartyQuery = from item in slnInnerSegment
                                                 where item.StartsWith("N1") && item.Contains("US")
                                                 select item;


                        var DwnStreamShipperContractQuery = from item in slnInnerSegment
                                                            where item.StartsWith("N9") && item.Contains("DT")
                                                            select item;

                        var ServiceRequesterContractQuery = from item in slnInnerSegment
                                                 where item.StartsWith("N9") && item.Contains("KSR")
                                                 select item;

                        var DownPkgIdQuery = from item in slnInnerSegment
                                                            where item.StartsWith("N9") && item.Contains("PGD")
                                                            select item;

                        var UpstrmPkgIdQuery = from item in slnInnerSegment
                                                            where item.StartsWith("N9") && item.Contains("PGU")
                                                            select item;

                        var UpstrmShipperContractQuery = from item in slnInnerSegment
                                                            where item.StartsWith("N9") && item.Contains("UP")
                                                            select item;


                        string[] PackageIdItem = (PackageIdQuery.FirstOrDefault() == null) ? null : PackageIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string PackageId = (PackageIdItem != null && PackageIdItem.Count() >= 2) ? PackageIdItem[2] : string.Empty;
                        string[] ConfirmationUserData1Item = (ConfirmationUserData1Query.FirstOrDefault() == null) ? null : ConfirmationUserData1Query.FirstOrDefault().Split(_dataSeparator);
                        string ConfirmationUserData1 = (ConfirmationUserData1Item != null && ConfirmationUserData1Item.Count() >= 2) ? ConfirmationUserData1Item[2] : string.Empty;
                        string[] ConfirmationUserData2Item = (ConfirmationUserData2Query.FirstOrDefault() == null) ? null : ConfirmationUserData2Query.FirstOrDefault().Split(_dataSeparator);
                        string ConfirmationUserData2 = (ConfirmationUserData2Item != null && ConfirmationUserData2Item.Count() >= 2) ? ConfirmationUserData2Item[2] : string.Empty;
                        string[] ReductionQuantityItem = (ReductionQuantityQuery.FirstOrDefault() == null) ? null : ReductionQuantityQuery.FirstOrDefault().Split(_dataSeparator);
                        long ReductionQuantity = (ReductionQuantityItem != null && ReductionQuantityItem.Count() >= 2) ? Convert.ToInt64(ReductionQuantityItem[2]) : 0;
                        string[] ServiceRequesterItem = (ServiceRequesterQuery.FirstOrDefault() == null) ? null : ServiceRequesterQuery.FirstOrDefault().Split(_dataSeparator);
                        string ServiceRequester = (ServiceRequesterItem != null && ServiceRequesterItem.Count() >= 4) ? ServiceRequesterItem[4] : string.Empty;
                        string[] DownstreamPartyItem = (DownstreamPartyQuery.FirstOrDefault() == null) ? null : DownstreamPartyQuery.FirstOrDefault().Split(_dataSeparator);
                        string DownstreamParty = (DownstreamPartyItem != null && DownstreamPartyItem.Count() >= 4) ? DownstreamPartyItem[4] : string.Empty;
                        string[] UpstreamPartyItem = (UpstreamPartyQuery.FirstOrDefault() == null) ? null : UpstreamPartyQuery.FirstOrDefault().Split(_dataSeparator);
                        string UpstreamParty = (UpstreamPartyItem != null && UpstreamPartyItem.Count() >= 4) ? UpstreamPartyItem[4] : string.Empty;

                        string[] DwnStreamShipperContractItem = (DwnStreamShipperContractQuery.FirstOrDefault() == null) ? null : DwnStreamShipperContractQuery.FirstOrDefault().Split(_dataSeparator);
                        string DwnStreamShipperContract = (DwnStreamShipperContractItem != null && DwnStreamShipperContractItem.Count() >= 2) ? DwnStreamShipperContractItem[2] : string.Empty;

                        string[] ServiceRequesterContractItem = (ServiceRequesterContractQuery.FirstOrDefault() == null) ? null : ServiceRequesterContractQuery.FirstOrDefault().Split(_dataSeparator);
                        string ServiceRequesterContract = (ServiceRequesterContractItem != null && ServiceRequesterContractItem.Count() >= 2) ? ServiceRequesterContractItem[2] : string.Empty;

                        string[] DownPkgIdItem = (DownPkgIdQuery.FirstOrDefault() == null) ? null : DownPkgIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string DownPkgId = (DownPkgIdItem != null && DownPkgIdItem.Count() >= 2) ? DownPkgIdItem[2] : string.Empty;

                        string[] UpstrmPkgIdItem = (UpstrmPkgIdQuery.FirstOrDefault() == null) ? null : UpstrmPkgIdQuery.FirstOrDefault().Split(_dataSeparator);
                        string UpstrmPkgId = (UpstrmPkgIdItem != null && UpstrmPkgIdItem.Count() >= 2) ? UpstrmPkgIdItem[2] : string.Empty;

                        string[] UpstrmShipperContractItem = (UpstrmShipperContractQuery.FirstOrDefault() == null) ? null : UpstrmShipperContractQuery.FirstOrDefault().Split(_dataSeparator);
                        string UpstrmShipperContract = (UpstrmShipperContractItem != null && UpstrmShipperContractItem.Count() >= 2) ? UpstrmShipperContractItem[2] : string.Empty;


                        SQTSOPPerTransactionDTO sqtsop = new SQTSOPPerTransactionDTO();
                        sqtsop.ConfirmationTrackingID = confirmationTrackingId;
                        sqtsop.Quantity = quantity;
                        sqtsop.ConfirmationRole = confirmationRole;
                        sqtsop.ConfirmationSusequenceCycleIndicator = ConfirmationSusequenceCycleIndicator;
                        sqtsop.ContractualFLowIndicator = contractualFlowIndicator;
                        sqtsop.CycleIndicator = cycleIndicator;
                        sqtsop.ConfirmationUserData1 = ConfirmationUserData1;
                        sqtsop.ConfirmationUserData2 = ConfirmationUserData2;
                        sqtsop.DownPkgId = DownPkgId;
                        sqtsop.DownstreamParty = DownstreamParty;
                        sqtsop.DwnStreamShipperContract = DwnStreamShipperContract;
                        sqtsop.EffectiveEndDate = effectiveEndDate;
                        sqtsop.EffectiveStartDate = effectiveStartDate;
                        sqtsop.Location = Location;
                        sqtsop.LocationCapacityFlowIndicator = locationCapacityFlowIndicator;
                        sqtsop.LocationNetCapacity = locationNetQuantity;
                        sqtsop.PreparerID = tspCode;
                        sqtsop.PackageId = PackageId;
                        sqtsop.ReductionQuantity = ReductionQuantity;
                        sqtsop.ReductionReason = ReductionReason;
                        sqtsop.SchedulingStatus = SchedulingStatus;
                        sqtsop.ServiceContract = confirmationServiceContract;
                        sqtsop.ServiceIdentifierCode = ServiceIdentifierCode;
                        sqtsop.ServiceRequester = ServiceRequester;
                        sqtsop.ServiceRequesterContract = ServiceRequesterContract;
                        sqtsop.StatementDate = statementDate;
                        sqtsop.Statement_ReceipentID = receiverCode;
                        sqtsop.UpstreamParty = UpstreamParty;
                        sqtsop.UpstrmPkgId = UpstrmPkgId;
                        sqtsop.UpstrmShipperContract = UpstrmShipperContract;
                        sqtsop.TransactionId= Guid.Parse(_fileName);
                        _sqtsopTable.Add(sqtsop);

                    }
                }
            }
        }
    }
}
