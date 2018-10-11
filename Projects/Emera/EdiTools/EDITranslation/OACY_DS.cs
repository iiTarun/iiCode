using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using Nom1Done.DTO;

namespace EDITranslation.AdditionalStandards
{
    public class OACY_DS : EDIWrapperBase
    {
        List<EDIOacyWrapperDTO> _oacyDataList;
        //DataTable _oacyDataTable;
        string _fileName;

        public OACY_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator, string fileName)
            : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            _oacyDataList = new List<EDIOacyWrapperDTO>();
        }

        public List<EDIOacyWrapperDTO> ReadOacyFile()
        {
            PopulateOACYList();
            return _oacyDataList;
        }
        
        private void PopulateOACYList()
        {
            //Get the ST envelopes
            foreach (string[] stBlock in _stEnvelopes)
            {
                //Set variables for common elements
                DateTime createdDate = DateTime.Now;
                var dtmOuterQuery = from item in stBlock
                                    where item.StartsWith("DTM")
                                    && item.Contains("809")
                                    select item;

                var n1OuterQuery = from item in stBlock
                                   where item.StartsWith("N1")
                                   select item;

                var dtmInnerIndexes = Enumerable.Range(0, stBlock.Count())
                                 .Where(i => stBlock[i].StartsWith("DTM") && stBlock[i][4] == '0' && stBlock[i][5] == '0' && stBlock[i][6] == '7')//.Contains("007"))
                                 .ToList();

                string[] n1Segments = (n1OuterQuery.FirstOrDefault() == null) ? null : n1OuterQuery.FirstOrDefault().Split(_dataSeparator);
                string[] dtmSegments = (dtmOuterQuery.FirstOrDefault() == null) ? null : dtmOuterQuery.FirstOrDefault().Split(_dataSeparator);
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

                //need to check for DUNS or prop code
                string TSPPropCode = n1Segments[4];
                string TSPCode = n1Segments[4];
                DateTime postingDate = DateTime.ParseExact(dtmSegments[6], "yyyyMMddHHmm", CultureInfo.GetCultureInfo("tr-TR"));
                DateTime postingTime = postingDate;

                //Set variables for detail
                foreach (var dtmInnerBlock in dtmInnerSegments)
                {
                    var dtmQuery = from item in dtmInnerBlock
                                   where item.StartsWith("DTM")
                                   && item.Contains("007")
                                   select item;

                    var locQuery = from item in dtmInnerBlock
                                   where item.StartsWith("N1")
                                   select item;

                    var flowQuery = from item in dtmInnerBlock
                                    where item.StartsWith("LQ") && item.Contains("DOF")
                                    select item;

                    var qtyDescQuery = from item in dtmInnerBlock
                                       where item.StartsWith("LQ") && item.Contains("LQT")
                                       select item;

                    var itIndicatorQuery = from item in dtmInnerBlock
                                           where item.StartsWith("LQ") && item.Contains("ITI")
                                           select item;


                    var qtyAvailableQuery = from item in dtmInnerBlock
                                            where item.StartsWith("QTY") && item.Contains("H6")
                                            select item;

                    var qtyOperatingQuery = from item in dtmInnerBlock
                                            where item.StartsWith("QTY") && item.Contains("H8")
                                            select item;

                    var qtyTotalScheduledQuery = from item in dtmInnerBlock
                                                 where item.StartsWith("QTY") && item.Contains("TQ")
                                                 select item;

                    var qtyDesignQuery = from item in dtmInnerBlock
                                         where item.StartsWith("QTY") && item.Contains("DV")
                                         select item;

                    var cycleIndicatorQuery = from item in dtmInnerBlock
                                              where item.StartsWith("N9") && item.Contains("CYI")
                                              select item;

                    var allQuantityAvailQuery = from item in dtmInnerBlock
                                                where item.StartsWith("LQ") && item.Contains("AQA")
                                                select item;


                    var locZoneQuery = from item in dtmInnerBlock
                                       where item.StartsWith("LCD") && item.Contains("ZN")
                                       select item;

                    string[] dtmSublineItem = (dtmQuery.FirstOrDefault() == null) ? null : dtmQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] locItems = (locQuery.FirstOrDefault() == null) ? null : locQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] flowItems = (flowQuery.FirstOrDefault() == null) ? null : flowQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyItems = (qtyDescQuery.FirstOrDefault() == null) ? null : qtyDescQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyAvailableItems = (qtyAvailableQuery.FirstOrDefault() == null) ? null : qtyAvailableQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyOperatingItems = (qtyOperatingQuery.FirstOrDefault() == null) ? null : qtyOperatingQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyTotalScheduledItems = (qtyTotalScheduledQuery.FirstOrDefault() == null) ? null : qtyTotalScheduledQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyDesignItems = (qtyDesignQuery.FirstOrDefault() == null) ? null : qtyDesignQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] itIndicatorItems = (itIndicatorQuery.FirstOrDefault() == null) ? null : itIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] cycleIndicatorItems = (cycleIndicatorQuery.FirstOrDefault() == null) ? null : cycleIndicatorQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] locZoneItems = (locZoneQuery.FirstOrDefault() == null) ? null : locZoneQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] allQuantityAvailItems = (allQuantityAvailQuery.FirstOrDefault() == null) ? null : allQuantityAvailQuery.FirstOrDefault().Split(_dataSeparator);

                    DateTime EffectiveGasDate = (dtmSublineItem == null) ? DateTime.Now : DateTime.ParseExact(dtmSublineItem[6], "yyyyMMddHHmm", CultureInfo.GetCultureInfo("tr-TR"));
                    DateTime EffectiveTime = EffectiveGasDate;

                    string locCode = (locItems == null) ? null : locItems[4];
                    string locName = (locItems == null) ? null : locItems[2];
                    string flowDirection = (flowItems == null) ? null : flowItems[2];
                    string qtyDesc = (qtyItems == null) ? null : qtyItems[2];
                    string measurementBasis = (qtyTotalScheduledItems == null) ? null : qtyTotalScheduledItems[3];
                    string availableQuantity = (qtyAvailableItems == null) ? "0" : qtyAvailableItems[2];
                    string operatingQuantity = (qtyOperatingItems == null) ? "0" : qtyOperatingItems[2];
                    string totalScheduled = (qtyTotalScheduledItems == null) ? "0" : qtyTotalScheduledItems[2];
                    string totalDesign = (qtyDesignItems == null) ? "0" : qtyDesignItems[2];
                    string itIndicator = (itIndicatorItems == null) ? "" : itIndicatorItems[2];
                    string cycleIndicator = (cycleIndicatorItems == null) ? "" : cycleIndicatorItems[2];
                    string locZone = (locZoneItems == null) ? "" : (locZoneItems.Count() == 7 ? locZoneItems[6] : "");
                    string allQtyAvail = (allQuantityAvailItems == null) ? "" : allQuantityAvailItems[2];

                    EDIOacyWrapperDTO oacy = new EDIOacyWrapperDTO();
                    oacy.TransactionID = Guid.Parse(_fileName);
                    oacy.ReceiceFileID = Guid.Parse(_fileName);
                    oacy.CreatedDate = createdDate;
                    oacy.TransactionServiceProviderPropCode = TSPPropCode;
                    oacy.TransactionServiceProvider = TSPCode;
                    oacy.PostingDateTime = postingDate;                   
                    oacy.EffectiveGasDayTime = EffectiveGasDate;                                      
                    oacy.Loc = locCode;
                    oacy.LocName = locName;
                    oacy.LocZn = locZone;
                    oacy.FlowIndicator = flowDirection;
                    oacy.LocPropDesc = qtyDesc;
                    oacy.MeasurementBasis = measurementBasis;
                    oacy.ITIndicator = itIndicator;
                    oacy.AllQtyAvailableIndicator = allQtyAvail;
                    oacy.DesignCapacity =Convert.ToInt64(string.IsNullOrEmpty(totalDesign)?"0":totalDesign);
                    oacy.OperatingCapacity = Convert.ToInt64(string.IsNullOrEmpty(operatingQuantity) ? "0" : operatingQuantity);
                    oacy.TotalScheduleQty =  Convert.ToInt64(string.IsNullOrEmpty(totalScheduled) ? "0" : totalScheduled); 
                    oacy.OperationallyAvailableQty = Convert.ToInt64(string.IsNullOrEmpty(availableQuantity) ? "0" : availableQuantity);
                    oacy.PipelineID = -1;
                    oacy.CycleIndicator = cycleIndicator;
                    oacy.LocQTIDesc = qtyDesc;
                    oacy.AvailablePercentage = (oacy.OperatingCapacity != 0) && (oacy.OperationallyAvailableQty != 0) ? ((oacy.OperationallyAvailableQty * 100) / oacy.OperatingCapacity) : 0;
                    _oacyDataList.Add(oacy);
                }
            }

        }
    }
}
