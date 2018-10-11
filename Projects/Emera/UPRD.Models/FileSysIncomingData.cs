using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
    public class FileSysIncomingData
    {
        [Key]
        public long Id { get; set; }
        public string MessageId { get; set; }
        public string DataReceived { get; set; }
        public System.DateTime ReceivedAt { get; set; }
        public bool IsProcessed { get; set; }
        public string DecryptedData { get; set; }
        public string PipeDuns { get; set; }
        public string DatasetType { get; set; }
    }
}
