using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nom.ViewModel
{

    public class LocationListDTO {
       public List<LocationsDTO> location = new List<LocationsDTO>();

        public List<LocationsDTO> LocationsList { get; set; }

        public int? PipelineID { get; set; }
        public string DunsNo { get; set; }
        public string PipelineDetails { get; set; }
        public int CurrentLocationRow { get; set; }

    }
   


    public class LocationsDTO
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string RDB { get; set; }
        public int ID { get;  set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Identifier { get;  set; }
        public string PropCode { get;  set; }
        public int RDUsageID { get;  set; }
        public int PipelineID { get; set; }
        public bool IsActive { get;  set; }
        public string CreatedBy { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public string ModifiedBy { get;  set; }
        public DateTime ModifiedDate { get;  set; }
        public Guid TransactionId { get; set; }
        public string PipelineDuns { get; set; }
      
    }
}