using System;
using System.Collections.Generic;

namespace Nom.ViewModel
{

    public class UsersDTO {
        public List<UserDTO> userList = new List<UserDTO>();
        public List<UserDTO> Users { get { return userList; } set { userList = value; } }
        public int ShipperCompanyId { get; set; }

    }


    public class UserDTO
    {
       public Guid Id { get; set; } 

       public int ShipperCompanyId { get; set; }
        public string Company { get; set; }
        public string Duns { get; set; }
       public string Name { get; set; }
       public string LastName { get; set; }
       public string Email { get; set; }
       public string PhoneNo { get; set; }
       public string Username { get; set; }
       public string Role { get; set; }

    }
}