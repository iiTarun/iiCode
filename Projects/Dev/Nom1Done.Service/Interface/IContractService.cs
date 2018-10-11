using Nom.ViewModel;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
   public interface IContractService
    {
        IEnumerable<Contract> GetAll();

        ContractsDTO GetContractOnId(int conId);
        IEnumerable<ContractsDTO> GetContracts(string pipeDuns, int companyId);

        bool AddContract(ContractsDTO contract);

        bool IsContractUsed(string contractNo);
        bool UpdateContract(ContractsDTO contract);
        bool RemoveContract(int contractID);
        Contract GetContractByContractNo(string SvcCnttNo, string pipelineDuns);
        void Dispose(bool disposing);

    }
}
