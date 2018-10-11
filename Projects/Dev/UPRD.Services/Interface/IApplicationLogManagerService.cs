using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Services.Interface
{
   public interface IApplicationLogManagerService
    {
       void  SaveAppLogManager(string source, string type, string errMsg);
    }
}
