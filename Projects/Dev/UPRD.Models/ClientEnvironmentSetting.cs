using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
    public class ClientEnvironmentSetting
    {
             [Key]
        public int ID { get; set; }

        public string ShipperDuns  { get; set; }
        public string ShipperName { get; set; }
        public string Environment { get; set; }
        public string FolderPath { get; set; }
        public string  ConnectionString { get; set; }

        public bool Enginestatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }


    }
}
