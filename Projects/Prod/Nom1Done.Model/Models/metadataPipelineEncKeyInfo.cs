using System;
using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class metadataPipelineEncKeyInfo
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> PipelineId { get; set; }
        public string KeyName { get; set; }
        public string PipeDuns { get; set; }
        public string PgpKey { get; set; }
    }
}
