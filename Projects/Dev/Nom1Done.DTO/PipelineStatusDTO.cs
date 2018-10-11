using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{

    public class PipelineStatusListDTO
    {
        List<PipelineStatusDTO> list = new List<PipelineStatusDTO>();

       public int pipelineId { get; set; }
        public int pageSize { get; set; }
        public List<PipelineStatusDTO> PipelineStatusList { get { return list; } set { list=value;  } }
    }



    public class PipelineStatusDTO
    {
        public int PipelineId { get; set; }
        public DateTime onDate { get; set; }
        public bool IsBrowserTestSuccess { get; set; }
        public bool IsOacyReceive { get; set; }
        public bool IsUnscReceive { get; set; }
        public bool IsNoticeReceive { get; set; }
    }




}