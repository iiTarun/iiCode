using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Model;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IUploadNominationService
    {
        List<UploadFileDTO> GetUploadedFiles(int Pipeline, string UserID);
        UploadedFile DownloadFile(int FileID);
        void SaveUploadedFile(UploadedFile UploadFile);
        void SavePNTBulkUpload(BatchDetailDTO batch, bool IsSave);
        void SavePathedBulkUpload(PathedDTO pathed, bool IsSave);
        void AddBatches(List<V4_Batch> batches);
    }
}
