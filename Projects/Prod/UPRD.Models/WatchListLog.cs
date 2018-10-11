using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
   public class WatchListLog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
