using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{
    public class UserPipelineMapping
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int shipperId { get; set; }
        public int pipelineId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }

        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }

    }
}
