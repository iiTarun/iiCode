using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom1Done.DTO
{
    public class TabsModel
    {

    }

    public class TestNomMarket
    {
        public string Select { get; set; }
        public string LocProp { get; set; }
        public string LocName { get; set; }
        public string Loc { get; set; }
        public string TT_Desc { get; set; }
        public string TT { get; set; }
        public string SVCRe { get; set; }
        public string SVCTyp { get; set; }
        public string DnIDProp { get; set; }
        public string DnName { get; set; }
        public string DnID { get; set; }
        public string Default_Ind { get; set; }
        public string RecQty { get; set; }
        public string FuelPerc { get; set; }
        public string FuelQty { get; set; }
        public string DelQty { get; set; }
        public string DnRank { get; set; }
        public string PkgID { get; set; }
        public string DnCntrctID { get; set; }
        public string DnPkgID { get; set; }
    }
    public class TestNomTransport
    {
        public string Select { get; set; }
        public string TT_Desc { get; set; }
        public string TT { get; set; }
        public string RecLocProp { get; set; }
        public string RecLocName { get; set; }
        public string RecLoc { get; set; }
        public string ReRank { get; set; }
        public string RecZone { get; set; }
        public string DelLocProp { get; set; }
        public string DelLocName { get; set; }
        public string DelLoc { get; set; }
        public string DelRank { get; set; }
        public string DelZone { get; set; }
        public string RecDTH { get; set; }
        public string FuelPerc { get; set; }
        public string FuelDTH { get; set; }
        public string DelDTH { get; set; }
        public string PkgID { get; set; }
        public string PathRank { get; set; }
        public string Contract { get; set; }
    }
    public class TestNomSupply
    {
        public string DelQty { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
        public string LocProp { get; set; }
        public string RecQty { get; set; }
        public string SVCRe { get; set; }
        public string TT { get; set; }
        public string TT_Desc { get; set; }
        public string UpID { get; set; }
        public string UpIDP { get; set; }
        public string UpName { get; set; }
        public string UpRank { get; set; }
        public string Default_Ind { get; set; }
        public string Fuel { get; set; }
        public string FuelQty { get; set; }
        public string PkgID { get; set; }
        public string SVCTyp { get; set; }
        public string Select { get; set; }
        public string UpCntrct { get; set; }
        public string UpPkgID { get; set; }
        //public string DownStreamIDProp { get; set; }
        //public string DownStreamIDName { get; set; }
        //public string DownStreamId { get; set; }
        //public string downstreamRank { get; set; }
    }

    public class BatchTabModel
    {
        public string LocProp { get; set; }
        public string Location { get; set; }
        public int RecQty { get; set; }
        public int DelQty { get; set; }
        public int Variance { get; set; }
        public int NominatiedQty { get; set; }
        public List<BatchTabModel> Lst = new List<BatchTabModel>();
    }
}