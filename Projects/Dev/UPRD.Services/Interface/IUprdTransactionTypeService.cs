using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.DTO;

namespace UPRD.Services.Interface
{
    public interface IUprdTransactionTypeService
    {
        bool ActivateTrasaction(int id);
        MetaDataTransactionTypesDTO GetTransactionByid(int id);
        IEnumerable<MetaDataTransactionTypesDTO> GetTransactions();

        void Save();
        bool DeleteTransactionByID(int id);
        string UpdateTransactionByID(MetaDataTransactionTypesDTO Transactions);
        bool ClientEnvironmentsetting();
    }
}
