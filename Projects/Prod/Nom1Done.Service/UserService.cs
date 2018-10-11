using Nom1Done.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class UserService : IUserService
    {
        IUserRepository _userRepo;
        private readonly IShipperRepository _shipperRepo;

        public UserService(IUserRepository _userRepo, IShipperRepository _shipperRepo) {
           this._userRepo = _userRepo;
            this._shipperRepo = _shipperRepo;
        }

        public List<string> GetAllByShipperID(int ShipperCompanyId, List<string> userIds)
        {
            var query = _shipperRepo.GetAll().Where(a => userIds.Contains(a.UserId) && a.ShipperCompanyID == ShipperCompanyId).Select(a => a.UserId).ToList();
            return query;
        }

        //public IEnumerable<UserDTO> GetAll()
        //{
        //    IEnumerable<Shipper> shipperList = _userRepo.GetAll();
        //    return MappingFromShipprToUserDTO(shipperList.ToList());
        //}



        //public UserDTO GetUserOnId(int Id)
        //{
        //    List<Shipper> shipperList = new List<Shipper>();
        //    shipperList.Add(_userRepo.GetById(Id));
        //    return MappingFromShipprToUserDTO(shipperList).FirstOrDefault();            
        //}



        //public List<UserDTO> MappingFromShipprToUserDTO(List<Shipper> shipperList)
        //{
        //    List<UserDTO> userList = new List<UserDTO>();
        //    foreach (var shipper in shipperList) {
        //        UserDTO user = new UserDTO();
        //        user.Id = Guid.Parse(shipper.ID.ToString());
        //        user.Name = shipper.FirstName;
        //        user.LastName = shipper.LastName;
        //        user.ShipperCompanyId = shipper.ShipperCompanyID;

        //        //TODO: 
        //        // //using solve the Procedure.
        //        //if (dr["Company"] != DBNull.Value)
        //        //    itemObj.Company = dr["Company"].ToString();
        //        //if (dr["DUNS"] != DBNull.Value)
        //        //    itemObj.Duns = dr["DUNS"].ToString();
        //        //if (dr["Username"] != DBNull.Value)
        //        //    itemObj.Username = dr["Username"].ToString();
        //        //if (dr["Email"] != DBNull.Value)
        //        //    itemObj.Email = dr["Email"].ToString();
        //        //if (dr["PhoneNumber"] != DBNull.Value)
        //        //    itemObj.PhoneNo = dr["PhoneNumber"].ToString();
        //        //if (dr["Role"] != DBNull.Value)
        //        //    itemObj.Role = dr["Role"].ToString();
        //        userList.Add(user);
        //    }


        //    return userList;
        //}
    }
}
