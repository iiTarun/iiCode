using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NomsApi.DTO
{
    public class PathedNonPathedHybridDTO
    {
        public List<PathedNomDTO> PathedNomList { get; set; } = new List<PathedNomDTO>();
        public List<NonPathedBatch> NonPathedNomList { get; set; } = new List<NonPathedBatch>();
    }
}