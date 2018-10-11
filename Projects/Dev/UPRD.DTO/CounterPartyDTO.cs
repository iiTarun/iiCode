using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.DTO
{

    public class CounterPartyDTO
    {
        [Required(ErrorMessage = "Required Field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string PropCode { get; set; }
        public int ID { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public int PipelineID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CounterStatusListDTO
    {
        public IEnumerable<CounterPartyDTO> CounterPartiesDTO { get; set; }
    }

}
