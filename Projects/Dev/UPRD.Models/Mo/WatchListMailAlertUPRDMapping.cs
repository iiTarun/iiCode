using Nom1Done.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Model.Enums;

namespace UPRD.Model
{
    public class WatchListMailAlertUPRDMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int WatchListId { get; set; }
        public UprdDataSet DataSetId { get; set; }
        public long UprdID { get; set; }
        public DateTime? EmailSentDateTime { get; set; }

        //uprd models

    }
}
