using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IUserService
    {
      //  IEnumerable<UserDTO> GetAll();
        List<string> GetAllByShipperID(int ShipperCompanyId, List<string> userIds);
       // UserDTO GetUserOnId(int Id);
    }
}
