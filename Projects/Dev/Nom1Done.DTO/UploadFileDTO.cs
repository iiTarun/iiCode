using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nom.ViewModel
{
    public class UploadFilesListDTO
    {
       public List<UploadFileDTO> list = new List<UploadFileDTO>();
      public  List<UploadFileDTO> UploadedFilesList { get { return list; } set { list = value; } }

    }



    public class UploadFileDTO
    {
        public long ID { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public string AddedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string pipelineDuns { get; set; }
    }
}