using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using UPRD.Model;
using System.Web.Mvc;

namespace UPRD.DTO
{
    public class Pipeline_TransactionType_MapDTO
    {
        public int ID { get; set; }
        public int PipelineID { get; set; }
        public int TransactionTypeID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string PathType { get; set; }
        public string PipeDuns { get; set; }
        public bool IsSpecialLocs { get; set; }
        public string pipelineDuns { get; set; }
        public string Name { get; set; }
        //public string SelectedPath { get; set; }
        public List<MetaDataTransactionTypesDTO> MetaDataTransactionTypesDTO { get; set; }
        //public string Identifier { get; set; }




    }
}
