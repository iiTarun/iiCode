using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class TradingPartnerWorksheet
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int PipelineID { get; set; }
        public string UsernameLive { get; set; }
        public string PasswordLive { get; set; }
        public string URLLive { get; set; }
        public string KeyLive { get; set; }
        public string UsernameTest { get; set; }
        public string PasswordTest { get; set; }
        public string URLTest { get; set; }
        public string KeyTest { get; set; }
        public string ReceiveSubSeperator { get; set; }
        public string ReceiveDataSeperator { get; set; }
        public string ReceiveSegmentSeperator { get; set; }
        public string SendSubSeperator { get; set; }
        public string SendDataSeperator { get; set; }
        public string SendSegmentSeperator { get; set; }
        public bool IsTest { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PipeDuns { get; set; }
    }
}
