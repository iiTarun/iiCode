using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model
{
    public class EDIFileSettings
    {
        public int Id { get; set; }
        public string PipeDuns { get; set; }
        public int DatasetId { get; set; }
        public string GS01_FunctionalIdentifierCode { get; set; }
        public string GS02_SendersCode { get; set; }
        public string GS03_ReceiversCode { get; set; }
        public string GS04_DateFormat { get; set; }
        public string GS05_TimeFormat { get; set; }
        public string GS07_ResponsibleAgencyCode { get; set; }
        public string GS08_Version { get; set; }
    }
}
