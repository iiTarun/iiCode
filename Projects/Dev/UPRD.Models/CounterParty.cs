﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Model
{

    public class CounterParty
    {
         public int ID { get; set; }

         public string Name { get; set; }
         public string Identifier { get; set; }
         public string PropCode { get; set; }
         public int PipelineID { get; set; }
         public bool IsActive { get; set; }
         public string CreatedBy { get; set; }
         public System.DateTime CreatedDate { get; set; }
         public string ModifiedBy { get; set; }
         public System.DateTime ModifiedDate { get; set; }
         public string PipeDuns { get; set; }
        
    }
}
