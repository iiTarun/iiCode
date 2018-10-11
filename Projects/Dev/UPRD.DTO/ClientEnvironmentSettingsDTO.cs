using System;

namespace UPRD.DTO
{
    public class ClientEnvironmentSettingsDTO
    {
        public string ShipperDuns { get; set; }
        public string ShipperName { get; set; }
        public string ShipperNameWithDuns { get; set; }
        public string Environment { get; set; }
        public string FolderPath { get; set; }
        public string ConnectionString { get; set; }
        public bool Enginestatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
