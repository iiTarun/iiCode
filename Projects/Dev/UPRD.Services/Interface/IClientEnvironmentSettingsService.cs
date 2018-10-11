using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.DTO;

namespace UPRD.Services.Interface
{
    public interface IClientEnvironmentSettingsService
    {
        List<ClientEnvironmentSettingsDTO> GetShipperComapnies();
        string GetFolderPathByShipperDuns(string ShipperDuns);
    }
}
