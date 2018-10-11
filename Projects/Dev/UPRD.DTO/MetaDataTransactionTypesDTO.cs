using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPRD.DTO
{
    public class MetaDataTransactionTypesDTO
    {
        [Required(ErrorMessage = "Required Field")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Name { get; set; }
        //public string PathType { get; set; }
        //public string pipelineDuns { get; set; }
        public int ID { get; set; }



    }


}