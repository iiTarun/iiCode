using System.Collections.Generic;
using UPRD.Infrastructure;
using UPRD.Model;

using UPRD.Service;
using UPRD.Service.Interface;
using UPRD.Services.Interface;
using UPRD.DTO;
using UPRD.Services;
using UPRD.Data.Repositories;
using System.Linq;
namespace UPRD.Services.Interface
{
    public interface IUprdPipeTransTypeMapService
    {
        Pipeline_TransactionType_MapDTO GetTransactionByid(int id);
        IEnumerable<Pipeline_TransactionType_MapDTO> GetTransactions(string pipelineDuns);

        bool ActivateTrasaction(int id);

        bool DeleteTransactionByID(int id);
        string UpdateTransactionByID(Pipeline_TransactionType_MapDTO Transaction);
        bool ClientEnvironmentsetting();

    }
}
