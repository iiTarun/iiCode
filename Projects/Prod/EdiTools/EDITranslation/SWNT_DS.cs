using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using Nom1Done.DTO;

namespace EDITranslation.CapacityRelease
{
    public class SWNT_DS : EDIWrapperBase
    {
        string _fileName;
        //DataTable _swntTable;
        List<SwntPerTransactionDTO> _swntList;

        public SWNT_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator, string fileName)
             : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            _swntList = new List<SwntPerTransactionDTO>();
        }
        public List<SwntPerTransactionDTO> ReadSwntFile()
        {
            PopulateSWNTDataList();
            return _swntList;
        }
        private void PopulateSWNTDataList()
        {
            foreach (string[] stBlock in _stEnvelopes)
            {
                var noticeIdQuery = from item in stBlock
                                    where item.StartsWith("BMG") & item.Contains("00")
                                    select item;

                var noticeDateQuery = from item in stBlock
                                      where item.StartsWith("DTM") && item.Contains("007")
                                      select item;//

                var postingDateQuery = from item in stBlock
                                       where item.StartsWith("DTM") && item.Contains("145")
                                       select item;//

                var responseDateQuery = from item in stBlock
                                        where item.StartsWith("DTM") && item.Contains("501")
                                        select item;//

                var tspQuery = from item in stBlock
                               where item.StartsWith("N1") && item.Contains("SJ")
                               select item;

                var criticalNoticeIdQuery = from item in stBlock
                                            where item.StartsWith("REF") && item.Contains("QV")
                                            select item;

                var priorNoticeIdQuery = from item in stBlock
                                         where item.StartsWith("REF") && item.Contains("F8")
                                         select item;

                var responseRequiredQuery = from item in stBlock
                                            where item.StartsWith("REF") && item.Contains("ME")
                                            select item;

                var mitInnerIndexes = Enumerable.Range(0, stBlock.Count())
                                 .Where(i => stBlock[i].StartsWith("MIT"))
                                 .ToList();

                List<string[]> mitInnerSegments = new List<string[]>();
                if (mitInnerIndexes.Count() == 1)
                {
                    string[] mitInnerBlock = new string[stBlock.Count() - mitInnerIndexes[0]];
                    Array.Copy(stBlock, mitInnerIndexes[0], mitInnerBlock, 0, (stBlock.Count() - mitInnerIndexes[0]));
                    mitInnerSegments.Add(mitInnerBlock);
                }
                else
                {
                    for (int i = 0; i < mitInnerIndexes.Count(); i++)
                    {
                        if ((i + 1) == mitInnerIndexes.Count())
                        {
                            string[] mitLastBlock = new string[stBlock.Count() - mitInnerIndexes[i]];
                            Array.Copy(stBlock, mitInnerIndexes[i], mitLastBlock, 0, (stBlock.Count() - mitInnerIndexes[i]));
                            mitInnerSegments.Add(mitLastBlock);
                            break;
                        }
                        string[] mitInnerBlock = new string[mitInnerIndexes[i + 1] - mitInnerIndexes[i]];
                        Array.Copy(stBlock, mitInnerIndexes[i], mitInnerBlock, 0, (mitInnerIndexes[i + 1] - mitInnerIndexes[i]));
                        mitInnerSegments.Add(mitInnerBlock);
                    }
                }

                string[] noticeDateItems = (noticeDateQuery.FirstOrDefault() == null) ? null : noticeDateQuery.FirstOrDefault().Split(_dataSeparator);
                string[] postingDateItems = (postingDateQuery.FirstOrDefault() == null) ? null : postingDateQuery.FirstOrDefault().Split(_dataSeparator);
                string[] responseDateItems = (responseDateQuery.FirstOrDefault() == null) ? null : responseDateQuery.FirstOrDefault().Split(_dataSeparator);
                string[] tspItems = (tspQuery.FirstOrDefault() == null) ? null : tspQuery.FirstOrDefault().Split(_dataSeparator);
                string[] criticalNoticeIdItems = (criticalNoticeIdQuery.FirstOrDefault() == null) ? null : criticalNoticeIdQuery.FirstOrDefault().Split(_dataSeparator);
                string[] noticeIdItems = (noticeIdQuery.FirstOrDefault() == null) ? null : noticeIdQuery.FirstOrDefault().Split(_dataSeparator);
                string[] priorNoticeItems = (priorNoticeIdQuery.FirstOrDefault() == null) ? null : priorNoticeIdQuery.FirstOrDefault().Split(_dataSeparator);
                string[] responseRequiredItems = (responseRequiredQuery.FirstOrDefault() == null) ? null : responseRequiredQuery.FirstOrDefault().Split(_dataSeparator);

                string noticeDateFormat = noticeDateItems==null?"":noticeDateItems[6];
                string postingDateFormat = postingDateItems==null?"":postingDateItems[6];
                string responseDateFromats = responseDateItems==null?"":responseDateItems[6];
                string tspCodeType = tspItems==null?"":tspItems[3];
                string criticalNoticeId = criticalNoticeIdItems==null?"":criticalNoticeIdItems[2];
                string noticeId = noticeIdItems==null?"":noticeIdItems[2];
                string priorNoticeId = (priorNoticeItems == null) ? "" : priorNoticeItems[2];
                string responseRequiredCode = responseRequiredItems==null?"":responseRequiredItems[2];

                DateTime noticeEffectiveStartDateTime = new DateTime();
                DateTime noticeEffectiveEndDateTime = new DateTime();

                DateTime postingStartDateTime = new DateTime();
                DateTime postingEndDateTime = new DateTime();

                DateTime responseDateTime = new DateTime();
                

                string tspDUNS = "";
                string tspPropCode = "";
                string responseRequired = "";

                switch (tspCodeType)
                {
                    case "1":
                        tspDUNS = tspItems==null?"":tspItems[4];
                        break;
                    case "SV":
                        tspPropCode = tspItems == null ? "" : tspItems[4];
                        break;
                }

                switch (responseRequiredCode)
                {
                    case "1":
                        responseRequired = "Immediate response of an operational nature by affected parties may be required.";
                        break;
                    case "2":
                        responseRequired = "Response of an operational nature by affected parties is required within 12 hours.";
                        break;
                    case "3":
                        responseRequired = "Response of an operational nature by affected parties is required within 24 hours.";
                        break;
                    case "4":
                        responseRequired = "Affected parties must respond by specified date and time (Response Date and Response Time).";
                        break;
                    case "5":
                        responseRequired = "No response required.";
                        break;

                }

                switch (noticeDateFormat)
                {
                    case "DTS":
                        noticeEffectiveStartDateTime = DateTime.ParseExact(noticeDateItems[7], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        noticeEffectiveEndDateTime = DateTime.ParseExact(noticeDateItems[7], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        break;
                    case "RTS":
                        string[] dates = noticeDateItems[7].Split('-');
                        noticeEffectiveStartDateTime = DateTime.ParseExact(dates[0], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        noticeEffectiveEndDateTime = DateTime.ParseExact(dates[1], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        break;
                }

                switch (postingDateFormat)
                {
                    case "DTS":
                        postingStartDateTime = DateTime.ParseExact(postingDateItems[7], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        postingEndDateTime = DateTime.ParseExact(postingDateItems[7], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        break;
                    case "RTS":
                        string[] dates = postingDateItems[7].Split('-');
                        postingStartDateTime = DateTime.ParseExact(dates[0], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        postingEndDateTime = DateTime.ParseExact(dates[1], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        break;
                }
                switch (responseDateFromats)
                {
                    case "DTS":
                        responseDateTime= DateTime.ParseExact(responseDateItems[7], "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("tr-TR"));
                        break;
                    case "RTS":

                        break;
                }

                foreach (string[] mitInnerSegment in mitInnerSegments)
                {
                    var noticeStatusQuery = from item in mitInnerSegment
                                            where item.StartsWith("MIT")
                                            select item;

                    var noticeTextQuery = from item in mitInnerSegment
                                          where item.StartsWith("MSG")
                                          select item;

                    string[] noticeStatusItems = (noticeStatusQuery.FirstOrDefault() == null) ? null : noticeStatusQuery.FirstOrDefault().Split(_dataSeparator);
                    string[] noticeTextItems = (noticeTextQuery.ToArray<string>() == null) ? null : noticeTextQuery.ToArray<string>();

                    string noticeText = string.Concat(noticeTextItems);
                    noticeText = noticeText.Replace("MSG", "");
                    noticeText = noticeText.Replace(_dataSeparator[0].ToString(), "");
                    noticeText = noticeText.Replace(_segmentSeparator[0].ToString(), "");

                    string subject = (noticeStatusItems.Length == 3) ? noticeStatusItems[2] : string.Empty;

                    string noticeStatusCode = noticeStatusItems == null ? "" : noticeStatusItems[1];
                    string noticeStatusDescription = "";

                    switch (noticeStatusCode)
                    {
                        case "1":
                            noticeStatusDescription = "Initiate";
                            break;
                        case "2":
                            noticeStatusDescription = "Supercede";
                            break;
                        case "3":
                            noticeStatusDescription = "Terminate";
                            break;
                    }

                    //DataRow row = _swntTable.NewRow();
                    SwntPerTransactionDTO swnt = new SwntPerTransactionDTO();
                    swnt.TransactionId = Guid.Parse(_fileName);
                    swnt.ReceiveFileId = Guid.Parse(_fileName);
                    swnt.PipelineId = -1;
                    swnt.TransactionIdentifierCode = "";
                    swnt.TransactionControlNumber = "";
                    swnt.TransactionSetPurposeCode = "";
                    swnt.Description = "";
                    swnt.NoticeEffectiveDateTime = noticeEffectiveStartDateTime;
                 
                    swnt.NoticeEndDateTime = noticeEffectiveEndDateTime;
                 
                    swnt.PostingDateTime = postingStartDateTime;
                   
                    swnt.ResponseDateTime = responseDateTime;
                  
                    swnt.TransportationserviceProvider = tspDUNS;
                    swnt.TransportationServiceProviderPropCode = tspPropCode;
                    swnt.CriticalNoticeIndicator = criticalNoticeId;
                    swnt.FreeFormMessageText = "";
                    swnt.CreatedDate = DateTime.Now;
                    swnt.IsActive = true;
                    swnt.Message = noticeText;
                    swnt.NoticeId = noticeId;
                    swnt.NoticeTypeDesc1 = noticeTextItems[0].Replace("MSG" + _dataSeparator[0], "").ToString();//.Replace("MSG*", "").Replace("MSG|", "").Replace("MSG~","").ToString();
                    swnt.NoticeTypeDesc2 = "";
                    swnt.ReqrdResp = responseRequired;
                    swnt.Subject = subject;
                    swnt.PriorNotice = priorNoticeId;
                    swnt.NoticeStatusDesc = noticeStatusDescription;
                    swnt.PipeDuns = tspDUNS;
                    swnt.PipeDunsAndNoticeIdCombination = tspDUNS + noticeId;

                    _swntList.Add(swnt);
                }
            }
        }
    }
}
