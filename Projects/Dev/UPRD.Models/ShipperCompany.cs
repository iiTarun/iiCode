namespace UPRD.Model
{
    using System;
    public class ShipperCompany
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DUNS { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? SubscriptionID { get; set; }
        public string ShipperAddress { get; set; }
    }
}
