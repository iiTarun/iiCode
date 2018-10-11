using System.Collections.Generic;
using UPRD.Model;
using UPRD.Data.Repositories;
using UPRD.Services.Interface;
using UPRD.DTO;
using System.Linq;
using UPRD.Service.Interface;
using System;

namespace UPRD.Services.Services
{

    public class DashNominationStatusService : IDashNominationStatusService
    {
        private readonly IUprdDashNominationStatus _dashNominationStatus;
        IModalFactory _modalFactory;
        public  DashNominationStatusService(IUprdDashNominationStatus dashNominationStatus, IModalFactory modalFactory)
       {
            this._dashNominationStatus = dashNominationStatus;
            this._modalFactory = modalFactory;
       }

        public IEnumerable<DashNominationStatusDTO> GetDashNominationStatus(string shipperDuns)
        {
            var dashNomStatus =  _dashNominationStatus.GetDashNomStatus(shipperDuns).ToList(); 
            return MapDashNominationStatusDTO(dashNomStatus);
        }


        private List<DashNominationStatusDTO> MapDashNominationStatusDTO(List<DashNominationStatus> list)
        {
            List<DashNominationStatusDTO> DashNomStatusList = new List<DashNominationStatusDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    DashNomStatusList.Add(_modalFactory.Parse(item));
                }
            return DashNomStatusList;
        }
       
        public bool UpdateNomStatusIsTriggered(String ShipperDuns, String pipeDuns, int StatusID)
        {
            try
            {
                var IsTriggered = _dashNominationStatus.UpdatetblTrigger(ShipperDuns, pipeDuns , StatusID);
                return true;
            }
            catch (Exception ex)
            { }

            return false;

        }

        public bool SwitchEngineStatus(String ShipperDuns, bool EngineStatus)
        {
            return _dashNominationStatus.UpdateEngineStatus(ShipperDuns, EngineStatus);
        }

        public string GetEngineStatusbyShipperDuns(String ShipperDuns)
        {
           return _dashNominationStatus.GetEngineStatus(ShipperDuns);
        }


       
    }
}
