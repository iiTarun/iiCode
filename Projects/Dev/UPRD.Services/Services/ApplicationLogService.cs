using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Data.Repositories;
using UPRD.Services.Interface;

namespace UPRD.Services.Services
{
    public class ApplicationLogService : IApplicationLogManagerService
    {
        IUPRDApplicationLogRepository _UPRDApplicationLogRepository;
        public ApplicationLogService(IUPRDApplicationLogRepository UPRDApplicationLogRepository)
        {
            this._UPRDApplicationLogRepository = UPRDApplicationLogRepository;
        }

       public void SaveAppLogManager(string source, string type, string errMsg)
        {
            _UPRDApplicationLogRepository.AppLogManager(source, type, errMsg);
        }
    }
}
