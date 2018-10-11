using System.Collections.Generic;
using UPRD.Data.Repositories;
using UPRD.DTO;
using UPRD.Services.Interface;

namespace UPRD.Services.Services
{
    public class ClientEnvironmentSettingsService: IClientEnvironmentSettingsService
    {
        private readonly IClientEnvironmentSettingsRepository _ClientEnvironmentRepo;
        readonly Service.Interface.IModalFactory ModalFactory;
        public ClientEnvironmentSettingsService(IClientEnvironmentSettingsRepository _ClientEnvironmentRepo, Service.Interface.IModalFactory ModalFactory)
        {
            this._ClientEnvironmentRepo = _ClientEnvironmentRepo;
            this.ModalFactory = ModalFactory;
        }

        public string GetFolderPathByShipperDuns(string ShipperDuns)
        {
            return _ClientEnvironmentRepo.GetFolderPathByShipperDuns(ShipperDuns);
        }

        public List<ClientEnvironmentSettingsDTO> GetShipperComapnies()
        {
            var ShipperComapnies = _ClientEnvironmentRepo.GetShipperComapnies();
            return ShipperComapnies;
        }
    }
}
