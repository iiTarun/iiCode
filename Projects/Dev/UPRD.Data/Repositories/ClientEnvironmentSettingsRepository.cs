using System;
using System.Collections.Generic;
using System.Linq;
using UPRD.DTO;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class ClientEnvironmentSettingsRepository: RepositoryBase<ClientEnvironmentSetting>, IClientEnvironmentSettingsRepository
    {
        public ClientEnvironmentSettingsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
           
        }

        public string GetFolderPathByShipperDuns(string shipperDuns)
        {
            if(!string.IsNullOrEmpty(shipperDuns))
            {
                return DbContext.ClientEnvironmentSetting
                    .Where(a => a.ShipperDuns == shipperDuns)
                    .Select(a => a.FolderPath).FirstOrDefault();
            }
            return null;
            
        }

        public List<ClientEnvironmentSettingsDTO> GetShipperComapnies()
        {
            var Shippers = DbContext.ClientEnvironmentSetting
                .Select(a => new ClientEnvironmentSettingsDTO
                {
                    ShipperDuns = a.ShipperDuns,
                    ShipperName = a.ShipperName,
                    ShipperNameWithDuns = a.ShipperName + "(" + a.ShipperDuns + ")",
                    ConnectionString=a.ConnectionString,
                    FolderPath=a.FolderPath,
                    Environment=a.Environment,
                    Enginestatus=a.Enginestatus,
                    CreatedBy=a.CreatedBy,
                    CreatedDate=a.CreatedDate,
                    ModifiedBy=a.ModifiedBy,
                    ModifiedDate=a.ModifiedDate
                }).ToList();
            return Shippers;
        }
    }

    public interface IClientEnvironmentSettingsRepository: IRepository<ClientEnvironmentSetting>
    {
        List<ClientEnvironmentSettingsDTO> GetShipperComapnies();
        string GetFolderPathByShipperDuns(string shipperDuns);
    }
}
