using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Infrastructure;
using UPRD.Model;
using UPRD.DTO;

namespace UPRD.Data.Repositories
{
    public class UPRDUserRegistrationRepository : RepositoryBase<UserRegistrationDTO>, IUPRDUserRegistrationRepository
    {
        public UPRDUserRegistrationRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }
        public IEnumerable<UserRegistrationDTO> GetUserList()
        {
            IEnumerable<UserRegistrationDTO> list = (from user in DbContext.Users
                                                     join shipper in DbContext.ShipperCompany on user.ShipperDuns equals shipper.DUNS
                                                     where !string.IsNullOrEmpty(user.UserType)
                                                     select new
                                                     {
                                                         UserId = user.Id,
                                                         Username = user.UserName,
                                                         Email = user.Email,
                                                         Shipper = shipper.Name,
                                                         ShipperDuns = user.ShipperDuns,
                                                         IsEnabled = user.IsEnabled
                                                     }).ToList().Select(p => new UserRegistrationDTO()
                                                     {
                                                         Id =p.UserId,
                                                         UserName = p.Username,
                                                         Email = p.Email,
                                                         Shipper = p.Shipper,
                                                         ShipperDuns = p.ShipperDuns,
                                                         IsActive = p.IsEnabled,
                                                     });
            
            return list;
        }
        public IEnumerable<ShipperCompany> GetShipperList()
        {
            return DbContext.ShipperCompany.Where(a => a.IsActive == true).ToList();
        }
        public UserRegistrationDTO GetUserById(string id)
        {
            var data = (from user in DbContext.Users
                         where !string.IsNullOrEmpty(user.UserType) && user.Id == id
                         select new UserRegistrationDTO
                         {
                             Id = user.Id,
                             Email = user.Email,
                             Password =user.PasswordHash,
                             ShipperDuns = user.ShipperDuns
                         }).FirstOrDefault();
            return data;
        }
        public bool DeleteUser(string userid)
        {
            try
            {
                using (var db = new IdentityDbContext())
                {
                    var user = db.Users.Find(userid);
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
    public interface IUPRDUserRegistrationRepository : IRepository<UserRegistrationDTO>
    {
        IEnumerable<UserRegistrationDTO> GetUserList();
        IEnumerable<ShipperCompany> GetShipperList();
        UserRegistrationDTO GetUserById(string id);
        bool DeleteUser(string userid);
    }
}
