using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDITranslation;
using System.Data;
using Nom1Done.Model;
using Nom1Done.DTO;

namespace EDITranslation.AdditionalStandards
{
    public class RURD_DS : EDIWrapperBase
    {
        private string _trackingId;
        private string _senderDUNS;
        private string _recieverDUNS;
        private bool _oacyAvailable;
        private bool _unscAvailable;
        private bool _swntAvailable;
        private bool _isAvailable;
        private List<UPRDStatusDTO> _uprdTable;
        string _fileName;

        public RURD_DS(string ediFile, char[] segmentSeparator, char[] dataSeparator, string fileName)
            : base(ediFile, segmentSeparator, dataSeparator)
        {
            _uprdTable = new List<UPRDStatusDTO>();
            _fileName = fileName;
        }

        public string TrackingID
        {
            get { return _trackingId; }
        }

        public string SenderDUNS
        {
            get { return _senderDUNS; }
        }

        public string RecieverDUNS
        {
            get { return _recieverDUNS; }
        }

        public bool OACYAvailable
        {
            get { return _oacyAvailable; }
        }

        public bool UNSCAvailable
        {
            get { return _unscAvailable; }
        }

        public bool SWNTAvailable
        {
            get { return _swntAvailable; }
        }
        public List<UPRDStatusDTO> ReadRurdFile()
        {
            PopulateRURD();
            return _uprdTable;
        }
        private void PopulateRURD()
        {
            var trackingIdQuery = from item in _segments
                           where item.StartsWith("BIA")
                           select item;

            var recieverIdQuery = from item in _segments
                                  where item.StartsWith("N1") 
                                  && item.Contains("SJ")
                                  select item;

            var senderIdQuery = from item in _segments
                                  where item.StartsWith("N1") 
                                  && item.Contains("41")
                                  select item;

            var linIndexes = Enumerable.Range(0, _segments.Count())
                 .Where(i => _segments[i].StartsWith("LIN"))
                 .ToList();

            foreach (int linIndex in linIndexes)
            {
                string[] linItems = _segments[linIndex].Split(_dataSeparator);
                string[] refItems = _segments[linIndex + 1].Split(_dataSeparator);

                switch (linItems[3])
                {
                    case "6":
                        _swntAvailable = true;
                        _isAvailable=(refItems[2].Equals("Y")) ? true : false;
                        break;
                    case "9":
                        _unscAvailable = true;
                        _isAvailable=(refItems[2].Equals("Y")) ? true : false;
                        break;
                    case "8":
                        _oacyAvailable = true;
                        _isAvailable=(refItems[2].Equals("Y")) ? true : false;
                        break; 
                }
            }

            string[] trackingIDLine = trackingIdQuery==null?null:trackingIdQuery.FirstOrDefault().Split(_dataSeparator);
            _trackingId = trackingIDLine==null?"":trackingIDLine[3];

            string[] recieverIDline = recieverIdQuery == null ? null : recieverIdQuery.FirstOrDefault().Split(_dataSeparator);
            _recieverDUNS = recieverIDline == null ? "" : recieverIDline[4];

            string[] senderIDLine = senderIdQuery == null ? null : senderIdQuery.FirstOrDefault().Split(_dataSeparator);
            _senderDUNS = senderIDLine == null ? "" : senderIDLine[4];

            UPRDStatusDTO uprdStatus = new UPRDStatusDTO();
            if (_oacyAvailable)
                uprdStatus.DatasetSummary = "OACY ";//+ (_isAvailable?"Available": "not Available");
            if (_unscAvailable)
                uprdStatus.DatasetSummary = "UNSC ";// + (_isAvailable ? "Available" : "not Available");
            if (_swntAvailable)
                uprdStatus.DatasetSummary = "SWNT ";// + (_isAvailable ? "Available" : "not Available");
            uprdStatus.IsDataSetAvailable = _isAvailable;
            uprdStatus.IsRURDReceived = true;
            uprdStatus.RURD_ID = Guid.Parse(_fileName);
            uprdStatus.RequestID = _trackingId;
            _uprdTable.Add(uprdStatus);
        }
    }
}
