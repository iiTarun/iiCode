using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDITranslation.AdditionalStandards
{
    public class UPRD_DS
    {
        private const string _ediFileTemplate =
            "ISA*00*          *00*          *01*"+RQ_DUNS+"      *01*"+PL_DUNS+"      *"+CDF+"*1535*U*00304*000001777*0*"+ENV+"*>"+
            "~GS*IB*"+RQC_DUNS+"*"+PLC_DUNS+"*"+CD+"*1535*1777*X*003040~"+
            "ST*846*1775~BIA*00*PS*"+RID+"*"+CDF+"~"+
            "DTM*007*****RD8*"+START_DATE+"-"+END_DATE+"~"+
            "N1*SJ**1*"+PL_DUNS+"~"+
            "N1*41**1*"+RQ_DUNS+"~"+
            DSREQ_OACY+
            DSREQ_UNSC+
            DSREQ_SWNT+
            "CTT*1~"+
            "SE*"+C+"*1775~"+
            "GE*1*1777~"+
            "IEA*1*000001777~";

        private const string RQ_DUNS = "[RQ_DUNS]";
        private const string PL_DUNS = "[PL_DUNS]";
        private const string CDF = "[CDF]";
        private const string ENV = "[ENV]";
        private const string RQC_DUNS = "[RQC_DUNS]";
        private const string PLC_DUNS = "[PLC_DUNS]";
        private const string CD = "[CD]";
        private const string RID = "[RID]";
        private const string START_DATE = "[START_DATE]";
        private const string END_DATE = "[END_DATE]";
        private const string DSREQ_OACY = "[DSREQ_OACY]";
        private const string DSREQ_UNSC = "[DSREQ_UNSC]";
        private const string DSREQ_SWNT = "[DSREQ_SWNT]";
        private const string C = "[C]";


        private char _segmentSeparator =  '~' ;
        private char _dataSeparator = '*';
        private const string _unscLineSegment = "LIN*1*OA*9~";
        private const string _oacyLineSegment = "LIN*1*OA*8~";
        private const string _swntLineSegment = "LIN*1*OA*6~";

        private string _requestorCompanyDUNs;
        private string _destinationPipelineDUNs;
        private string _requestorCompanyDUNsC;
        private string _destinationPipelineDUNsC;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _oacyRequest;
        private bool _unscRequest;
        private bool _swntRequest;
        private bool _isProduction;

        public UPRD_DS(string requestorCompanyDUNs, string destinationPipelineDUNs, string requestorCompanyDUNsC, string destinationPipelineDUNsC
            , DateTime startDate, DateTime endDate, 
            bool oacyRequest, bool unscRequest, bool swntRequest, bool isProduction
            , char segmentSeparator, char dataSeparator)
        {
            _requestorCompanyDUNs = requestorCompanyDUNs;
            _destinationPipelineDUNs = destinationPipelineDUNs;
            _requestorCompanyDUNsC = requestorCompanyDUNsC;
            _destinationPipelineDUNsC = destinationPipelineDUNsC;
            _startDate = startDate;
            _endDate = endDate;
            _oacyRequest = oacyRequest;
            _unscRequest = unscRequest;
            _swntRequest = swntRequest;
            _isProduction = isProduction;
            _segmentSeparator = segmentSeparator;
            _dataSeparator = dataSeparator;
        }

        public string GenerateUPRDFile()
        {
            string ediFile;

            ediFile = _ediFileTemplate.Replace(RQ_DUNS, _requestorCompanyDUNs);
            ediFile = ediFile.Replace(RQC_DUNS, _requestorCompanyDUNsC);

            ediFile = ediFile.Replace(PL_DUNS, _destinationPipelineDUNs);
            ediFile = ediFile.Replace(PLC_DUNS, _destinationPipelineDUNsC);

            ediFile = ediFile.Replace(CDF, DateTime.Now.ToString("yyMMdd"));
            ediFile = ediFile.Replace(CD, DateTime.Now.ToString("yyyyMMdd"));

            ediFile = ediFile.Replace(START_DATE, _startDate.ToString("yyyyMMdd"));
            ediFile = ediFile.Replace(END_DATE, _endDate.ToString("yyyyMMdd"));

            ediFile = ediFile.Replace(ENV, (_isProduction) ? "P" : "T");

            ediFile = ediFile.Replace(RID, "REQUP_" + new Random().Next(111,11111).ToString());

            if (_oacyRequest)
                ediFile = ediFile.Replace(DSREQ_OACY, _oacyLineSegment);
            else
                ediFile = ediFile.Replace(DSREQ_OACY, "");

            if (_unscRequest)
                ediFile = ediFile.Replace(DSREQ_UNSC, _unscLineSegment);
            else
                ediFile = ediFile.Replace(DSREQ_UNSC, "");

            if (_swntRequest)
                ediFile = ediFile.Replace(DSREQ_SWNT, _swntLineSegment);
            else
                ediFile = ediFile.Replace(DSREQ_SWNT, "");

            ediFile = ediFile.Replace(C, GetSTCount(ediFile).ToString());

            ediFile = ediFile.Replace('~', _segmentSeparator);
            ediFile = ediFile.Replace('*', _dataSeparator);
            ediFile = ediFile.Trim();

            return ediFile;
        }

        private int GetSTCount(string ediFile)
        {
            string[] segments = ediFile.Trim().Split(_segmentSeparator);

            var stQuery = from item in segments
                          where item.StartsWith("ST")
                          select item;

            int stIndex = Array.IndexOf(segments, stQuery.FirstOrDefault(), 0);

            var seQuery = from item in segments
                          where item.StartsWith("SE")
                          select item;

            int seIndex = Array.IndexOf(segments, seQuery.FirstOrDefault(), 0);

            return (seIndex - (stIndex - 1));
        }
    }
}
