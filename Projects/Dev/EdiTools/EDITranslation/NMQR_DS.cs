using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EDITranslation.Nominations
{
    public class NMQR_DS : EDIWrapperBase
    {
        //DataTable _nmqrTable;
        string _fileName;
        List<NMQRPerTransactionDTO> _nmqrList;
        public NMQR_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator,string fileName) 
            : base(ediFile, segmentSeparator, dataSeparator)
        {
            _fileName = fileName;
            //CreateNmqrTable();
            _nmqrList = new List<NMQRPerTransactionDTO>();
            //ReadNMQRFile();
        }
        //public DataTable ReadNMQRFile()
        //{
        //    PopulateNMQRTable();
        //    return _nmqrTable;
        //}
        public List<NMQRPerTransactionDTO> ReadNMQRFileData()
        {
            PopulateNMQRModelList();
            return _nmqrList;
        }
        private void PopulateNMQRModelList()
        {
            foreach (string[] stBlock in _stEnvelopes)
            {
                DateTime createdDate = DateTime.Now;
                var bgnOuterQuery = from item in stBlock
                                    where item.StartsWith("BGN")
                                    select item;

                var serviceRequesterQuery = from item in stBlock
                                            where item.StartsWith("N1")
                                            && item.Contains("SJ")
                                            select item;

                var serviceProviderQuery = from item in stBlock
                                           where item.StartsWith("N1")
                                           && item.Contains("78")
                                           select item;
                
                
                string[] bgnSegment = (bgnOuterQuery.FirstOrDefault() == null) ? null : bgnOuterQuery.FirstOrDefault().Split(_dataSeparator);
                string[] serviceRequesterSegment = (serviceRequesterQuery.FirstOrDefault() == null) ? null : serviceRequesterQuery.FirstOrDefault().Split(_dataSeparator);
                string[] serviceProviderSegment = (serviceProviderQuery.FirstOrDefault() == null) ? null : serviceProviderQuery.FirstOrDefault().Split(_dataSeparator);

                string statusCode = (bgnSegment) == null ? null : bgnSegment[8];
                string refenenceNumber = (bgnSegment) == null ? null : bgnSegment[2];
                switch (statusCode)
                {
                    case "EZ":
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
                            var contractQuery = from item in dtmInnerBlock
                                                where item.StartsWith("CS")
                                                select item;

                            

                            var slnIndexes = Enumerable.Range(0, dtmInnerBlock.Count())
                                                             .Where(i => dtmInnerBlock[i].StartsWith("SLN"))
                                                             .ToList();

                            if (slnIndexes.Count == 0)
                            {
                                var informationQueryListDTM = from item in dtmInnerBlock
                                                              where item.StartsWith("III") && (!item.Contains("SLN"))
                                                              select item;
                                foreach (var informationQuery in informationQueryListDTM)
                                {
                                    string[] informationSegment = (informationQuery == null) ? null : informationQuery.Split(_dataSeparator);
                                    string validationCode = (informationSegment != null && informationSegment.Count() >= 2) ? informationSegment[2] : "";
                                    string ValidationMassage = (informationSegment != null && informationSegment.Count() >= 4) ? informationSegment[4] : "";

                                    NMQRPerTransactionDTO Nmqr = new NMQRPerTransactionDTO();
                                    Nmqr.Transactionid = Guid.Parse(_fileName);
                                    Nmqr.NominationTrackingId = string.Empty;//DTM segment information section doesn't contain tracking Id 
                                    Nmqr.ValidationCode = validationCode;
                                    Nmqr.ValidationMessage = ValidationMassage;
                                    Nmqr.CreatedDate = createdDate;
                                    Nmqr.ReferenceNumber = refenenceNumber;
                                    Nmqr.StatusCode = statusCode;
                                    _nmqrList.Add(Nmqr);
                                }
                            }

                            List<string[]> sublineSegments = new List<string[]>();

                            if (slnIndexes.Count() == 1)
                            {
                                string[] slnInnerBlock = new string[dtmInnerBlock.Count() - slnIndexes[0]];
                                Array.Copy(dtmInnerBlock, slnIndexes[0], slnInnerBlock, 0, (dtmInnerBlock.Count() - slnIndexes[0]));
                                sublineSegments.Add(slnInnerBlock);
                            }
                            else
                            {
                                for (int i = 0; i < slnIndexes.Count(); i++)
                                {
                                    if ((i + 1) == slnIndexes.Count())
                                    {
                                        string[] slnInnerLastBlock = new string[dtmInnerBlock.Count() - slnIndexes[i]];
                                        Array.Copy(dtmInnerBlock, slnIndexes[i], slnInnerLastBlock, 0, (dtmInnerBlock.Count() - slnIndexes[i]));
                                        sublineSegments.Add(slnInnerLastBlock);
                                        break;
                                    }
                                    string[] slnInnerBlock = new string[slnIndexes[i + 1] - slnIndexes[i]];
                                    Array.Copy(dtmInnerBlock, slnIndexes[i], slnInnerBlock, 0, (slnIndexes[i + 1] - slnIndexes[i]));
                                    sublineSegments.Add(slnInnerBlock);
                                }
                            }
                            foreach (var sublineBlock in sublineSegments)
                            {
                                var sublineQuery = from item in sublineBlock
                                                   where item.StartsWith("SLN")
                                                   select item;

                                var informationQueryList = from item in sublineBlock
                                                           where item.StartsWith("III")
                                                           select item;

                                string[] SLNSegment = (sublineQuery.FirstOrDefault() == null) ? null : sublineQuery.FirstOrDefault().Split(_dataSeparator);
                                string nomTrackingId = (SLNSegment) == null ? string.Empty : SLNSegment[1];
                                foreach (var informationQuery in informationQueryList)
                                {
                                    string[] informationSegment = (informationQuery == null) ? null : informationQuery.Split(_dataSeparator);
                                    string validationCode = (informationSegment != null && informationSegment.Count() >= 2) ? informationSegment[2] : "";
                                    string ValidationMassage = (informationSegment != null && informationSegment.Count() >= 4) ? informationSegment[4] : "";

                                    NMQRPerTransactionDTO Nmqr = new NMQRPerTransactionDTO();
                                    Nmqr.Transactionid = Guid.Parse(_fileName);
                                    Nmqr.NominationTrackingId = nomTrackingId;
                                    Nmqr.ValidationCode = validationCode;
                                    Nmqr.ValidationMessage = ValidationMassage;
                                    Nmqr.CreatedDate = createdDate;
                                    Nmqr.ReferenceNumber = refenenceNumber;
                                    Nmqr.StatusCode = statusCode;
                                    _nmqrList.Add(Nmqr);
                                }
                            }
                        }
                        break;
                    case "RZ":
                        var InformationQueryList = from item in stBlock
                                               where item.StartsWith("III")
                                               select item;
                        foreach(var informationQuery in InformationQueryList)
                        {
                            string[] informationSegment = (informationQuery == null) ? null : informationQuery.Split(_dataSeparator);
                            string validationCode = (informationSegment != null && informationSegment.Count() >= 2) ? informationSegment[2] : "";
                            string ValidationMassage = (informationSegment != null && informationSegment.Count() >= 4) ? informationSegment[4] : "";

                            NMQRPerTransactionDTO NMQR = new NMQRPerTransactionDTO();
                            NMQR.Transactionid = Guid.Parse(_fileName);
                            NMQR.NominationTrackingId = string.Empty;//when Nomination is Rejected(RZ) from header level error then we will not extract nomTrackingId
                            NMQR.ValidationCode = validationCode;
                            NMQR.ValidationMessage = ValidationMassage;
                            NMQR.CreatedDate = createdDate;
                            NMQR.ReferenceNumber = refenenceNumber;
                            NMQR.StatusCode = statusCode;
                            _nmqrList.Add(NMQR);
                        }
                        //string[] infoList = (InformationQuery.FirstOrDefault() == null) ? null : InformationQuery.FirstOrDefault().Split(_dataSeparator);
                        break;
                    case "WQ":
                        NMQRPerTransactionDTO nmqr = new NMQRPerTransactionDTO();
                        nmqr.Transactionid = Guid.Parse(_fileName);
                        nmqr.NominationTrackingId = string.Empty;//we will not get it if nomination is accepted
                        nmqr.ValidationCode = "";//we will not get it if nomination is accepted
                        nmqr.ValidationMessage = "";//we will not get it if nomination is accepted
                        nmqr.CreatedDate = createdDate;
                        nmqr.ReferenceNumber = refenenceNumber;
                        nmqr.StatusCode = statusCode;
                        _nmqrList.Add(nmqr);
                        break;
                }
            }
        }
        //private void PopulateNMQRTable()
        //{
        //    foreach(string[] stBlock in _stEnvelopes)
        //    {
        //        DateTime createdDate = DateTime.Now;
        //        var bgnOuterQuery = from item in stBlock
        //                         where item.StartsWith("BGN")
        //                         select item;
        //        var serviceRequesterQuery = from item in stBlock
        //                                    where item.StartsWith("N1")
        //                                    && item.Contains("SJ")
        //                                  select item;
        //        var serviceProviderQuery = from item in stBlock
        //                                   where item.StartsWith("N1")
        //                                   && item.Contains("78")
        //                                   select item;
        //        var informationQueryList = from item in stBlock
        //                               where item.StartsWith("III")
        //                               select item;

        //        //var sublineQuery = from item in stBlock
        //        //                   where item.StartsWith("SLN")
        //        //                   select item;

        //        string[] bgnSegment = (bgnOuterQuery.FirstOrDefault() == null) ? null : bgnOuterQuery.FirstOrDefault().Split(_dataSeparator);
        //        string[] serviceRequesterSegment = (serviceRequesterQuery.FirstOrDefault() == null) ? null : serviceRequesterQuery.FirstOrDefault().Split(_dataSeparator);
        //        string[] serviceProviderSegment = (serviceProviderQuery.FirstOrDefault() == null) ? null : serviceProviderQuery.FirstOrDefault().Split(_dataSeparator);


        //        string statusCode = (bgnSegment) == null ? null : bgnSegment[8];
        //        string refenenceNumber = (bgnSegment) == null ? null : bgnSegment[2];
        //        if (statusCode == "WQ")
        //        {
        //            DataRow row = _nmqrTable.NewRow();
        //            row["Transactionid"] = _fileName;
        //            row["NominationTrackingId"] = "";
        //            row["ValidationCode"] = "";
        //            row["ValidationMessage"] = "";
        //            row["CreatedDate"] = createdDate;
        //            row["RefenenceNumber"] = refenenceNumber;
        //            row["StatusCode"] = statusCode;
        //            _nmqrTable.Rows.Add(row);
        //        }
        //        foreach (var informationQuery in informationQueryList)
        //        {
        //            string[] informationSegment = (informationQuery==null)?null:informationQuery.Split(_dataSeparator);
        //            string validationCode = (informationSegment != null && informationSegment.Count() >= 2) ? informationSegment[2] : "";
        //            string ValidationMassage = (informationSegment != null && informationSegment.Count() >= 4) ? informationSegment[4] : "";

        //            DataRow row = _nmqrTable.NewRow();
        //            row["Transactionid"] = _fileName;
        //            row["NominationTrackingId"] = string.Empty;
        //            row["ValidationCode"] = validationCode;
        //            row["ValidationMessage"] = ValidationMassage;
        //            row["CreatedDate"] = createdDate;
        //            row["RefenenceNumber"] = refenenceNumber;
        //            row["StatusCode"] = statusCode;
        //            _nmqrTable.Rows.Add(row);
        //        }
        //    }
        //}
        //private void CreateNmqrTable()
        //{
        //    _nmqrTable = new DataTable("NMQRPerTransactions");

        //    DataColumn nmqrIdColumn = new DataColumn("ID", typeof(int));
        //    nmqrIdColumn.AutoIncrement = true;
        //    nmqrIdColumn.AutoIncrementSeed = 1;
        //    nmqrIdColumn.Unique = true;

        //    DataColumn TransactionColumn = new DataColumn("Transactionid", typeof(Guid));
        //    DataColumn NominationTrackingId = new DataColumn("NominationTrackingId", typeof(string));
        //    DataColumn ValidationCode = new DataColumn("ValidationCode", typeof(string));
        //    DataColumn ValidationMessageColumn = new DataColumn("ValidationMessage", typeof(string));
        //    DataColumn CreatedDateColumn = new DataColumn("CreatedDate", typeof(DateTime));
        //    DataColumn ReferenceNumberColumn = new DataColumn("RefenenceNumber", typeof(string));
        //    DataColumn StatusCodeColumn = new DataColumn("StatusCode", typeof(string));

        //    _nmqrTable.Columns.Add(nmqrIdColumn);
        //    _nmqrTable.Columns.Add(TransactionColumn);
        //    _nmqrTable.Columns.Add(NominationTrackingId);
        //    _nmqrTable.Columns.Add(ValidationCode);
        //    _nmqrTable.Columns.Add(ValidationMessageColumn);
        //    _nmqrTable.Columns.Add(CreatedDateColumn);
        //    _nmqrTable.Columns.Add(ReferenceNumberColumn);
        //    _nmqrTable.Columns.Add(StatusCodeColumn);
        //}
    }
}
