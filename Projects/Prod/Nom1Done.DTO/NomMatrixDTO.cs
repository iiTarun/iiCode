using System.Collections.Generic;

namespace Nom.ViewModel
{
    public class NomMatrixDTO
    {

        public List<RecUpStreamDTO> RecUpStreamDTOList = new List<RecUpStreamDTO>();
        public List<RecDownStreamDTO> RecDownStreamDTOList = new List<RecDownStreamDTO>();
        public List<DelUpStreamDTO> DelUpStreamDTOList = new List<DelUpStreamDTO>();
        public List<DelDownStreamDTO> DelDownStreamDTOList = new List<DelDownStreamDTO>();

        public string Loc { get; set; }
        public List<RecUpStreamDTO> ReceiptUpStreamLst { get { return RecUpStreamDTOList; } set { } }
        public List<RecDownStreamDTO> ReceiptDownStreamLst { get { return RecDownStreamDTOList; } set { } }
        public List<DelUpStreamDTO> DeliveryUpStreamLst { get { return DelUpStreamDTOList; } set { } }
        public List<DelDownStreamDTO> DeliveryDownStreamLst { get { return DelDownStreamDTOList; } set { } }

        public string RecUpType { get; set; }
        public string RecDnType { get; set; }
        public string LocType { get; set; }
        public string DelUpType { get; set; }
        public string DelDnType { get; set; }
    }

    public class RecUpStreamDTO
    {
        public string TransactionType { get; set; }
        public string Contract { get; set; }
        public string CounterParty { get; set; }
        public string Quantity { get; set; }
        public string RecQty { get; set; }
        public string DelQty { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
    }
    public class RecDownStreamDTO
    {
        public string TransactionType { get; set; }
        public string Contract { get; set; }
        public string DeliveryLocation { get; set; }
        public string Quantity { get; set; }
        public string RecQty { get; set; }
        public string DelQty { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
    }
    public class DelUpStreamDTO
    {
        public string TransactionType { get; set; }
        public string Contract { get; set; }
        public string RecLocation { get; set; }
        public string Quantity { get; set; }
        public string RecQty { get; set; }
        public string DelQty { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
    }
    public class DelDownStreamDTO
    {
        public string TransactionType { get; set; }
        public string Location { get; set; }
        public string CounterPartyDownStream { get; set; }
        public string ContractNo { get; set; }
        public string Quantity { get; set; }
        public string RecQty { get; set; }
        public string DelQty { get; set; }
        public string Loc { get; set; }
        public string LocName { get; set; }
    }

}