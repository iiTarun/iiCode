using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
   public class WatchListPipelineMapping
    {
        [Key]
        public int Id { get; set; }

     
        public int WatchListId { get; set; }

      
        public int PipelineId { get; set; }

    }
}
