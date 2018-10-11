using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
    public interface IPasswordHistoryService
    {
        DateTime GetLastModifiedDateByUser(string userID);
        bool UpdateLastModifiedDateByUser(string userID);
    }

}
