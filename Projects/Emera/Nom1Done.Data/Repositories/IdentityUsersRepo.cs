using Microsoft.AspNet.Identity.EntityFramework;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
   public class  IdentityUsersRepo : IIdentityUsersRepo
    {
        IdentityDbContext context;
        public IdentityUsersRepo()
        {
            context = new IdentityDbContext("NomEntities");           
        }

        public string GetUserEmailByUserId(string userId)
        {
            return context.Users.Where(a => a.Id == userId).FirstOrDefault().Email;
        }

        public string GetUserPhoneByUserId(string userId)
        {
            return context.Users.Where(a => a.Id == userId).FirstOrDefault().PhoneNumber;
        }
    }


    public interface IIdentityUsersRepo
    {
        string GetUserEmailByUserId(string userId);
        string GetUserPhoneByUserId(string userId);
    }


}
