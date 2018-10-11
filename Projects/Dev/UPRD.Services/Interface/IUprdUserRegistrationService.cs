using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.DTO;
using UPRD.Model;

namespace UPRD.Services.Interface
{
    public interface IUprdUserRegistrationService
    {
        IEnumerable<UserRegistrationDTO> GetUserList();
        IEnumerable<ShipperCompany> GetShipperList();
        UserRegistrationDTO GetUsersListById(string id);
        bool DeleteUserById(string id);
    }
}
