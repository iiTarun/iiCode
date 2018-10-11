using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class UserPipelineMappingDTO
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int shipperId { get; set; }
        public int pipelineId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }

        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }

        public List<PipelineListDTO> pipelines { get; set; }
    }

    public class PipelineListDTO
    {
        public string DunsNo { get; set; }
        public int pipelineId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
