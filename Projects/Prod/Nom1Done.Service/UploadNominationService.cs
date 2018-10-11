using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using Nom.ViewModel;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using System.Linq;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class UploadNominationService : IUploadNominationService
    {
        private readonly IUploadedNominationRepository uploadedNominationRepository;
        private readonly IModalFactory modelFactory;
        private readonly IPNTNominationService PNTNominationService;
        private readonly IPathedNominationService PathedNominationService;
        private readonly IBatchRepository Ibatchrepo;


        public UploadNominationService(IBatchRepository Ibatchrepo,IUploadedNominationRepository uploadedNominationRepository,
            IModalFactory modelFactory,
            IPNTNominationService PNTNominationService,
            IPathedNominationService PathedNominationService)
        {
            this.Ibatchrepo = Ibatchrepo;
            this.uploadedNominationRepository = uploadedNominationRepository;
            this.modelFactory = modelFactory;
            this.PathedNominationService = PathedNominationService;
            this.PNTNominationService = PNTNominationService;
        }
        public UploadedFile DownloadFile(int FileID)
        {
            return uploadedNominationRepository.GetById(FileID);
        }
        public List<UploadFileDTO> GetUploadedFiles(int Pipeline, string UserID)
        {
            return uploadedNominationRepository.GetUplodedFilesOnUserId(UserID)
                .Select(a => modelFactory.Parse(a)).OrderByDescending(a => a.CreatedDate)
                .ToList();
            //return uploadedNominationRepository.GetAll().ToList().Where(a => a.AddedBy == UserID).
            //     Select(a => modelFactory.Parse(a)).OrderByDescending(a=>a.CreatedDate).ToList();
        }

        public void SavePathedBulkUpload(PathedDTO pathed, bool IsSave)
        {
            PathedNominationService.SaveAndUpdatePathedNomination(pathed, true);
        }

        public void SavePNTBulkUpload(BatchDetailDTO batch, bool IsSave)
        {
            PNTNominationService.SaveBulkUpload(batch, true);
        }

        public void SaveUploadedFile(UploadedFile UploadFile)
        {
            uploadedNominationRepository.Add(UploadFile);
            uploadedNominationRepository.SaveChages();

        }

        public void AddBatches(List<V4_Batch> batches)
        {
            Ibatchrepo.AddRange(batches);
        }


    }
}
