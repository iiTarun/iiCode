using System;

namespace Nom1Done.DTO
{
    public class CustomUPRDReqDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RequestFor { get; set; }
        public int pipelineId { get; set; }
        public string pipeDuns { get; set; }
        public int datasetID { get; set; }
        public string shipperDuns { get; set; }
    }
    public class UprdStatusDTO
    {
        public string pipelneName { get; set; }
        public string GISB { get; set; }
        public string RURDStatus { get; set; }
    }
}
