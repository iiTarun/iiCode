using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.DTO;
using UPRD.Data.Repositories;
using UPRD.Model;
using UPRD.Services.Interface;
using UPRD.Service.Interface;

namespace UPRD.Services.Services
{
    public class UprdUserRegistrationService : IUprdUserRegistrationService
    {
        IUPRDUserRegistrationRepository _IUPRDUserRegistrationRepository;
        IModalFactory modalFactory;
        public UprdUserRegistrationService(IUPRDUserRegistrationRepository IUPRDUserRegistrationRepository, IModalFactory modalFactory)
        {
            _IUPRDUserRegistrationRepository = IUPRDUserRegistrationRepository;
            this.modalFactory = modalFactory;
        }
        public IEnumerable<UserRegistrationDTO> GetUserList()
        {
            return _IUPRDUserRegistrationRepository.GetUserList();
            //return modalFactory.Parse(_IUPRDUserRegistrationRepository.GetUserList());
        }
        public IEnumerable<ShipperCompany> GetShipperList()
        {
            return _IUPRDUserRegistrationRepository.GetShipperList();
        }        
        public UserRegistrationDTO GetUsersListById(string id)
        {
            UserRegistrationDTO result = new UserRegistrationDTO();
            if (!string.IsNullOrEmpty(id))
            {
                //result = modalFactory.Parse(_IUPRDUserRegistrationRepository.GetUserById(id));
                result = _IUPRDUserRegistrationRepository.GetUserById(id);
            }
            else
            {
                result = new UserRegistrationDTO();
            }
            return result;
        }
        public bool DeleteUserById(string id)
        {
            return _IUPRDUserRegistrationRepository.DeleteUser(id);
        }
    }
}
