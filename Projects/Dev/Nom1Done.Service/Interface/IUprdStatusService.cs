using Nom1Done.DTO;
using System;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IUprdStatusService
    {
        List<UPRDStatusDTO> GetUprdStatus(string pipeDuns, DateTime onDate);
               
    }
}
