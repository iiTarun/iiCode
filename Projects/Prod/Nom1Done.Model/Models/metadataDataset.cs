using System.ComponentModel.DataAnnotations;

namespace Nom1Done.Model
{
    public class metadataDataset
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public string Direction { get; set; }
        public bool IsUPRDAttribute { get; set; }
        public bool IsActive { get; set; }
    }
}
