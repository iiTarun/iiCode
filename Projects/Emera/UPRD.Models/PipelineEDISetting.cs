using System;

namespace UPRD.Model
{
    public class PipelineEDISetting
    {
        public int id { get; set; }
        public string PipeDuns { get; set; }
        public string ISA08_segment { get; set; }
        public string ISA06_Segment { get; set; }
        public string ISA11_Segment { get; set; }
        public string ISA12_Segment { get; set; }
        public string ISA16_Segment { get; set; }
        public string GS01_Segment { get; set; }
        public string GS02_Segment { get; set; }
        public string GS03_Segment { get; set; }
        public string GS07_Segment { get; set; }
        public string GS08_Segment { get; set; }
        public string ST01_Segment { get; set; }
        public string DataSeparator { get; set; }
        public string SegmentSeperator { get; set; }
        public int DatasetId { get; set; }
        public string ShipperCompDuns { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool SendManually { get; set; }
        public bool ForOacy { get; set; }
        public bool ForUnsc { get; set; }
        public bool ForSwnt { get; set; }
    }
}
