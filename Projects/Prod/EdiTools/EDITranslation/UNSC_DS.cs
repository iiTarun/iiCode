using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace EDITranslation.AdditionalStandards
{
    public class UNSC_DS : EDIWrapperBase
    {
        //DataTable _unscDataTable;
        string _fileName;
        List<EDIUnscWrapperDTO> _unscDataList;

        public UNSC_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator, string fileName)
             : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            _unscDataList = new List<EDIUnscWrapperDTO>();
        }
        public List<EDIUnscWrapperDTO> ReadUnscFile()
        {
            PopulateUNSCModelList();
            return _unscDataList;
        }
        private void PopulateUNSCModelList()
        {
            //Get the ST envelopes
            foreach (string[] stBlock in _stEnvelopes)
            {
                DateTime createdDate = DateTime.Now;
                var dtmOuterQuery = from item in stBlock
                                    where item.StartsWith("DTM")
                                    && item.Contains("809")
                                    select item;

                var n1OuterQuery = from item in stBlock
                                   where item.StartsWith("N1")
                                   select item;

                var dtmInnerIndexes = Enumerable.Range(0, stBlock.Count())
                                 .Where(i => stBlock[i].StartsWith("DTM") && stBlock[i][4]=='0' && stBlock[i][5] == '0' && stBlock[i][6] == '7')//.Contains("007"))
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

                string TSPPropCode = n1Segments[4];
                string TSPCode = n1Segments[4];
                DateTime postingDate = DateTime.ParseExact(dtmSegments[6], "yyyyMMddHHmm", CultureInfo.GetCultureInfo("tr-TR"));
                DateTime postingTime = postingDate;

                foreach (var dtmInnerBlock in dtmInnerSegments)
                {
                    var dtmQuery = from item in dtmInnerBlock
                                   where item.StartsWith("DTM")
                                   && item.Contains("007")
                                   select item;

                    var locQuery = from item in dtmInnerBlock
                                   where item.StartsWith("N1")
                                   select item;

                    var qtyDescQuery = from item in dtmInnerBlock
                                       where item.StartsWith("LQ") && item.Contains("LQT")
                                       select item;

                    var locZoneQuery = from item in dtmInnerBlock
                                       where item.StartsWith("LCD") && item.Contains("ZN")
                                       select item;

                    var qtyUnsubscribedQuery = from item in dtmInnerBlock
                                               where item.StartsWith("QTY") && item.Contains("V8")
                                               select item;

                    string[] dtmSublineItem = (dtmQuery.FirstOrDefault() == null) ? null : dtmQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] locItems = (locQuery.FirstOrDefault() == null) ? null : locQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyDescItems = (qtyDescQuery.FirstOrDefault() == null) ? null : qtyDescQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] qtyUnsubscribedItems = (qtyUnsubscribedQuery.FirstOrDefault() == null) ? null : qtyUnsubscribedQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] locZoneItems = (locZoneQuery.FirstOrDefault() == null) ? null : locZoneQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] EffectiveGasDates = (dtmSublineItem == null) ? null : dtmSublineItem[6].Trim().Split('-');

                    DateTime EffectiveGasStartDate = (EffectiveGasDates == null) ? DateTime.Now : DateTime.ParseExact(EffectiveGasDates[0], "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));
                    DateTime EffectiveGasEndDate = (EffectiveGasDates == null) ? DateTime.Now : DateTime.ParseExact(EffectiveGasDates[1], "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"));

                    string locCode = (locItems == null) ? "" : locItems[4];
                    string locName = (locItems == null) ? "" : locItems[2];
                    string qtyDesc = (qtyDescItems == null) ? "" : qtyDescItems[2];
                    string unsubQty = (qtyUnsubscribedItems == null) ? "0" : qtyUnsubscribedItems[2];
                    string measurement = (qtyUnsubscribedItems == null) ? "" : qtyUnsubscribedItems[3];
                    string locZone = (locZoneItems == null) ? "" : locZoneItems[6];

                    EDIUnscWrapperDTO unsc = new EDIUnscWrapperDTO();
                    unsc.TransactionID = Guid.Parse(_fileName);
                    unsc.ReceiveFileID = Guid.Parse(_fileName);
                    unsc.CreatedDate = createdDate;
                    unsc.PipelineID = -1;
                    unsc.TransactionServiceProviderPropCode = TSPPropCode;
                    unsc.TransactionServiceProvider = TSPCode;
                    unsc.Loc = locCode;
                    unsc.LocName = locName;
                    unsc.LocZn = locZone;
                    unsc.LocPurpDesc = qtyDesc;
                    unsc.LocQTIDesc = qtyDesc;
                    unsc.MeasBasisDesc = measurement;
                    unsc.TotalDesignCapacity = 0; //"";
                    unsc.UnsubscribeCapacity = Convert.ToInt64(Convert.ToDecimal(string.IsNullOrEmpty(unsubQty) ? "0" : unsubQty));
                    unsc.PostingDateTime = postingDate;                  
                    unsc.EffectiveGasDayTime = EffectiveGasStartDate;
                    unsc.EndingEffectiveDay = EffectiveGasEndDate;
                    _unscDataList.Add(unsc);
                }
            }
        }
    }
}
