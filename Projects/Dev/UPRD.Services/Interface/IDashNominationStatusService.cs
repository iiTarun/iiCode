
using System;
using System.Collections.Generic;
using UPRD.DTO;

namespace UPRD.Services.Interface
{
    public interface IDashNominationStatusService
    {
        IEnumerable<DashNominationStatusDTO> GetDashNominationStatus(string shipperDuns);
        bool UpdateNomStatusIsTriggered(String ShipperDuns, String pipeDuns , int StatusID);
        bool SwitchEngineStatus(String ShipperDuns, bool EngineStatus);
        string GetEngineStatusbyShipperDuns(String ShipperDuns);
    }
}
