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
    public class UprdTransactionTypeService : IUprdTransactionTypeService
    {

        private readonly IUPRDTransactionTypeRepository _UPRDTrasRepo;

        IModalFactory modalFactory;
        public UprdTransactionTypeService(IUPRDTransactionTypeRepository IUPRDTransactionTypeRepository, IModalFactory modalFactory)
        {
            _UPRDTrasRepo = IUPRDTransactionTypeRepository;
            this.modalFactory = modalFactory;
        }

        public void Save()
        {
            this._UPRDTrasRepo.Save();
        }


        private List<MetaDataTransactionTypesDTO> metaDataStatusDTO(List<metadataTransactionType> list)
        {
            List<MetaDataTransactionTypesDTO> metadataStatusList = new List<MetaDataTransactionTypesDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {
                    metadataStatusList.Add(modalFactory.Parse(item));
                }
            return metadataStatusList;
        }




        public MetaDataTransactionTypesDTO GetTransactionByid(int id)
        {
            var items = _UPRDTrasRepo.GetTransactionByid(id);

            return modalFactory.Parse(items);
            //return modalFactory.Parse(items);
        }

        public IEnumerable<MetaDataTransactionTypesDTO> GetTransactions()
        {


            var counter = _UPRDTrasRepo.GetTransactions().ToList();


            return metaDataStatusDTO(counter);
        }

        public bool DeleteTransactionByID(int id)
        {
            _UPRDTrasRepo.DeleteTransactionByID(id);

            return true;
        }

        public string UpdateTransactionByID(MetaDataTransactionTypesDTO Transactions)
        {
            //string Notification = _UPRDTrasRepo.UpdateTransactionByID(modalFactory.Parse(Transactions));
            string Notification = _UPRDTrasRepo.UpdateTransactionByID(modalFactory.Create(Transactions));

            return Notification;
        }

        public bool ActivateTrasaction(int id)
        {
            _UPRDTrasRepo.ActivateTrasaction(id);

            return true;
        }
        public bool ClientEnvironmentsetting()
        {
            _UPRDTrasRepo.ClientEnvironmentsetting();

            return true;
        }


    }
}

