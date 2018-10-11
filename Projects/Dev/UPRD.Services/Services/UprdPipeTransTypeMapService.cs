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


namespace UPRD.Services.Services
{
    public class UprdPipeTransTypeMapService : IUprdPipeTransTypeMapService
    {


        private readonly IUprdPipelineTransactionTypeMapRepository _UPRDPipeTransService;

        IModalFactory modalFactory;
        public UprdPipeTransTypeMapService(IUprdPipelineTransactionTypeMapRepository IUPRDPipeTransRepository, IModalFactory modalFactory)
        {
            _UPRDPipeTransService = IUPRDPipeTransRepository;
            this.modalFactory = modalFactory;
        }

        public bool ActivateTrasaction(int id)
        {
            _UPRDPipeTransService.ActivateTrasaction(id);

            return true;
        }

        public bool DeleteTransactionByID(int id)
        {
            _UPRDPipeTransService.DeleteTransactionByID(id);

            return true;
        }

        public Pipeline_TransactionType_MapDTO GetTransactionByid(int id)
        {
            var items = _UPRDPipeTransService.GetTransactionByid(id);

            return modalFactory.Parse(items);
        }

        public IEnumerable<Pipeline_TransactionType_MapDTO> GetTransactions(string pipelineDuns)
        {
            var data = _UPRDPipeTransService.GetTransactions(pipelineDuns).ToList();

            return data;
            //return PipeLineTransactionStatusDTO(counter);
        }


        private List<Pipeline_TransactionType_MapDTO> PipeLineTransactionStatusDTO(List<Pipeline_TransactionType_Map> list)
        {
            List<Pipeline_TransactionType_MapDTO> metadataStatusList = new List<Pipeline_TransactionType_MapDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    metadataStatusList.Add(modalFactory.Parse(item));
                }
            return metadataStatusList;
        }



        public string UpdateTransactionByID(Pipeline_TransactionType_MapDTO Transaction)
        {
            string Notification = _UPRDPipeTransService.UpdateTransactionByID(modalFactory.Create(Transaction));

            return Notification;
        }

        public bool ClientEnvironmentsetting()
        {
            _UPRDPipeTransService.ClientEnvironmentsetting();

            return true;
        }
    }
}
