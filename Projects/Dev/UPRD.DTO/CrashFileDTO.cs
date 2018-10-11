using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.DTO
{
    public class CrashFileDTO
    {
        public string FileName { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte[] FileData { get; set; }
        public string ShipperDuns { get; set; }
    }
}
