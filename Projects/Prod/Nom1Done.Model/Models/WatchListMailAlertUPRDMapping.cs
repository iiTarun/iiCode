using Nom1Done.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model.Models
{
    public class WatchListMailAlertUPRDMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int WatchListId { get; set; }
        public EnercrossDataSets DataSetId { get; set; }
        public long UprdID { get; set; }
        public DateTime? EmailSentDateTime { get; set; }

    }
}
