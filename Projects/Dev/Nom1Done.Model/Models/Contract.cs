using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class Contract
    {
        [Key]
        public int ID { get; set; }
        public string RequestNo { get; set; }
        public int RequestTypeID { get; set; }
        public decimal FuelPercentage { get; set; }
        public decimal MDQ { get; set; }
        public int LocationFromID { get; set; }
        public string LocFromName { get; set; }
        public string LocFromIdentifier { get; set; }
        public int LocationToID { get; set; }
        public string LocToName { get; set; }
        public string LocToIdentifier { get; set; }
        public DateTime ValidUpto { get; set; }
        public int PipelineID { get; set; }
        public int ShipperID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ReceiptZone { get; set; }
        public string DeliveryZone { get; set; }
        public string PipeDuns { get; set; }
        public string RateSchedule { get; set; }
    }
}
