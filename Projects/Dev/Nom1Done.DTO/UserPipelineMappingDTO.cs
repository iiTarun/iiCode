using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class UserPipelineDTO
    {
        public List<UserPipelineMappingDTO> userPipelineMappingDTO { get; set; }
        public int? ShipperId { get; set; }
        public string UserId { get; set; }

    }
    public class UserPipelineMappingDTO
    {
        public string UserId { get; set; }
        public int? ShipperId { get; set; }
        public string PipeDuns { get; set; }
        public string PipeName { get; set; }
        public bool IsNoms { get; set; } = false;
        public bool IsUPRD { get; set; } = false;
        public string ShipperName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }


}
