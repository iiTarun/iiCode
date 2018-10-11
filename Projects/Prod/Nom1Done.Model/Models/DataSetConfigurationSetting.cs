using Nom1Done.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model.Models
{
    public class DataSetConfigurationSetting
    {
        public int Id { get; set; }
        public EnercrossDataSets DataSet { get; set; }
        public int? Frequency { get; set; }//null for daily and value for hours
        public TimeSpan SchedularStartTime { get; set; } 
        public bool IsTestMode { get; set; }
        public string EmailId { get; set; }
        public string SchedularCronJobTime { get; set; }
    }
}
