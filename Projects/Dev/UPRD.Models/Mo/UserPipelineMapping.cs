using System;

namespace UPRD.Model
{
    public class UserPipelineMapping
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int shipperId { get; set; }
        public string PipeDuns { get; set; }
        public bool IsNoms { get; set; }
        public bool IsUPRD { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }

    }
}
