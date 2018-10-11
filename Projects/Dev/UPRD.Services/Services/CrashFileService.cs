using System.Collections.Generic;
using System.Configuration;
using UPRD.Data.Repositories;
using UPRD.DTO;
using UPRD.Services.Interface;

namespace UPRD.Services.Services
{
    public class CrashFileService : ICrashFileService
    {
        private readonly ICrashFileRepository _iCrashFileRepository;
        private readonly IClientEnvironmentSettingsService _ClientEnvironmentSettingsService;
        Service.Interface.IModalFactory ModalFactory;
        private string FilePath;
        public CrashFileService(ICrashFileRepository _iCrashFileRepository, 
            Service.Interface.IModalFactory ModalFactory, 
            IClientEnvironmentSettingsService _ClientEnvironmentSettingsService)
        {
            this._iCrashFileRepository = _iCrashFileRepository;
            this.ModalFactory = ModalFactory;
            this._ClientEnvironmentSettingsService = _ClientEnvironmentSettingsService;
            
        }

        public byte[] GetFileData(string FileName, string ShipperDuns)
        {
            FilePath = _ClientEnvironmentSettingsService.GetFolderPathByShipperDuns(ShipperDuns);
            var data= _iCrashFileRepository.GetFileData(FileName, FilePath);
            return data;
        }

        public List<CrashFileDTO> GetFiles(string ShipperDuns)
        {
            FilePath = _ClientEnvironmentSettingsService.GetFolderPathByShipperDuns(ShipperDuns);
            var files = _iCrashFileRepository.GetFiles(ShipperDuns, FilePath);
            return files;
        }

    }
}
