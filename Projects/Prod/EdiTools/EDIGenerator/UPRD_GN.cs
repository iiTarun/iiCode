using Nom1Done.DTO;
using System;
using System.IO;
using System.Linq;

namespace EdiTools.EDIGenerator
{
    public class UPRD_GN
    {
        public EdiDocument GenerateEDIUPRD(bool IsOacy, bool IsUnsc, bool IsSwnt, bool IsTest, string pipeDuns, PipelineEDISettingDTO pipelineEdiSetting, bool sendManu)
        {
            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            var ediDocument = new EdiDocument();
            var isa = new EdiSegment("ISA");
            isa[01] = "00";
            isa[02] = "".PadRight(10);
            isa[03] = "00";
            isa[04] = "".PadRight(10);
            isa[05] = "01";
            isa[06] = pipelineEdiSetting.ISA06_Segment.Trim().PadRight(15);// service requester duns
            isa[07] = pipelineEdiSetting.ISA08_segment.Trim().All(char.IsDigit) == true ? "01" : "14";
            isa[08] = pipelineEdiSetting.ISA08_segment.Trim().PadRight(15);
            isa[09] = EdiValue.Date(6, DateTime.Now);
            isa[10] = EdiValue.Time(4, DateTime.Now);
            isa[11] = pipelineEdiSetting.ISA11_Segment.Trim();//"U";// UPRD setting ISA11_seg
            isa[12] = pipelineEdiSetting.ISA12_Segment.Trim();//"00304";//Release/version*******PipelineUprdSetting_ISA12_Seg
            isa[13] = number;
            isa[14] = "0";
            isa[15] = IsTest ? "T" : "P";//Convert.ToBoolean(ConfigurationManager.AppSettings["EDIForTest"]) ? "T" : "P";//"P";//configuration setting for Test and Prod
            isa[16] = pipelineEdiSetting.ISA16_Segment.Trim();//">";//uprd setting ISA16
            ediDocument.Segments.Add(isa);

            var gs = new EdiSegment("GS");
            gs[01] = pipelineEdiSetting.GS01_Segment.Trim();//"IB"; //GS01_segment in edisetting
            gs[02] = pipelineEdiSetting.GS02_Segment.Trim();//"078711334";//service requester duns
            gs[03] = pipelineEdiSetting.GS03_Segment.Trim();//"RECEIVER";//GS03_Segment UPRD setting
            gs[04] = (pipeDuns.Trim() == "006958581"
                || pipeDuns.Trim() == "046077343"
                || pipeDuns.Trim() == "007933021"
                || pipeDuns.Trim() == "116025180"
                || pipeDuns.Trim() == "094992187"
                || pipeDuns.Trim() == "784158214"
                || pipeDuns.Trim() == "876833500"
                || pipeDuns.Trim() == "808168645"
                || pipeDuns.Trim() == "006912885"
                || pipeDuns.Trim() == "021632583"
                || pipeDuns.Trim() == "030353283"
                || pipeDuns.Trim() == "961777229"
                || pipeDuns.Trim() == "013081810"
                || pipeDuns.Trim() == "017738746") ?
                EdiValue.Date(6, DateTime.Now) : EdiValue.Date(8, DateTime.Now);
            gs[05] = EdiValue.Time(4, DateTime.Now);
            gs[06] = (number.Substring(number.Length - 4, 4) != "0000") ? number.Substring(number.Length - 4, 4) : number.Substring(0, 4);
            gs[07] = pipelineEdiSetting.GS07_Segment.Trim();//"X"; //GS07 segmnet in EDI setting
            gs[08] = pipelineEdiSetting.GS08_Segment.Trim();//"004010"; //GS08 segment in EDI setting
            ediDocument.Segments.Add(gs);

            var st = new EdiSegment("ST");
            st[01] = pipelineEdiSetting.ST01_Segment.Trim();//"846"; //ST01 Segment in EDI setting
            st[02] = gs[06];
            ediDocument.Segments.Add(st);

            var bia = new EdiSegment("BIA");
            bia[01] = "00";
            bia[02] = "PS";
            bia[03] = path.ToString();
            bia[04] = EdiValue.Date(6, DateTime.Now);
            ediDocument.Segments.Add(bia);


            //var searchCri = DateTime.Parse("9/9/2017");
            var dtm = new EdiSegment("DTM");
            dtm[01] = "007";

            if (pipeDuns.Trim() == "116025180" || pipeDuns.Trim() == "876833500")
            {
                dtm[06] = "RD8";
                if (sendManu)
                    dtm[07] = EdiValue.Date(8, pipelineEdiSetting.StartDate) + "-" + EdiValue.Date(8, pipelineEdiSetting.EndDate);
                else
                    dtm[07] = EdiValue.Date(8, DateTime.Now) + "-" + EdiValue.Date(8, DateTime.Now);
            }
            else
            {
                dtm[06] = "DTS";
                if (sendManu)
                    dtm[07] = (pipeDuns.Trim() == "094992187") ? EdiValue.Date(14, pipelineEdiSetting.StartDate.Date) : EdiValue.Date(14, pipelineEdiSetting.StartDate.AddSeconds(1));//  + "-" + EdiValue.Date(14, pipelineEdiSetting.EndDate);//EdiValue.Date(8, searchCri) + "-" + EdiValue.Date(8, searchCri.AddDays(1));
                else
                    dtm[07] = (pipeDuns.Trim() == "094992187") ? EdiValue.Date(14, DateTime.Now.Date) : EdiValue.Date(14, DateTime.Now.Date.AddSeconds(1));// + "-" + EdiValue.Date(14, DateTime.Now);//EdiValue.Date(8, searchCri) + "-" + EdiValue.Date(8, searchCri.AddDays(1));
            }


            ediDocument.Segments.Add(dtm);

            var n1SvcPvdr = new EdiSegment("N1");
            n1SvcPvdr[01] = "SJ";
            n1SvcPvdr[03] = "1";
            n1SvcPvdr[04] = pipeDuns.Trim();//"Receiver";
            ediDocument.Segments.Add(n1SvcPvdr);

            var n1SvcRq = new EdiSegment("N1");
            n1SvcRq[01] = "41";
            n1SvcRq[03] = "1";
            n1SvcRq[04] = "078711334";//Sender
            ediDocument.Segments.Add(n1SvcRq);

            if (IsOacy)
            {
                var lin = new EdiSegment("LIN");
                lin[01] = "1";
                lin[02] = "OA";
                lin[03] = "8";//uprd_DataRequestCode.RequestCode.Trim();//"8";//oacy(8),UNSC(9),SWNT(6);
                ediDocument.Segments.Add(lin);
            }

            if (IsUnsc)
            {
                var lin = new EdiSegment("LIN");
                lin[01] = "1";
                lin[02] = "OA";
                lin[03] = "9";//uprd_DataRequestCode.RequestCode.Trim();//"8";//oacy(8),UNSC(9),SWNT(6);
                ediDocument.Segments.Add(lin);
            }

            if (IsSwnt)
            {
                var lin = new EdiSegment("LIN");
                lin[01] = "1";
                lin[02] = "OA";
                lin[03] = "6";//uprd_DataRequestCode.RequestCode.Trim();//"8";//oacy(8),UNSC(9),SWNT(6);
                ediDocument.Segments.Add(lin);
            }

            var ctt = new EdiSegment("CTT");
            ctt[01] = "1";
            ediDocument.Segments.Add(ctt);

            var se = new EdiSegment("SE");
            se[01] = "8";
            se[02] = st[02];
            ediDocument.Segments.Add(se);

            var ge = new EdiSegment("GE");
            ge[01] = "1";
            ge[02] = gs[06];
            ediDocument.Segments.Add(ge);

            var iea = new EdiSegment("IEA");
            iea[01] = "1";
            iea[02] = number;
            ediDocument.Segments.Add(iea);

            // more segments...

            // ediDocument.Options.SegmentTerminator = pipeDuns.Trim()== "094992187" ? Convert.ToChar("\n"): Convert.ToChar(pipelineEdiSetting.SegmentSeperator);//Segment Separator from edi setting
            ediDocument.Options.SegmentTerminator = !string.IsNullOrEmpty(pipelineEdiSetting.SegmentSeperator) ? Convert.ToChar(pipelineEdiSetting.SegmentSeperator) : EdiOptions.DefaultSegmentTerminator;
            ediDocument.Options.ElementSeparator = Convert.ToChar(pipelineEdiSetting.DataSeparator);//'*';// data separator from edi setting

            return ediDocument;

        }
    }
}
