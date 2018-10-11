using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using Nom.ViewModel;

namespace Nom1Done.Service
{
   public class ContractService : IContractService
    {
        IContractRepository _IContractRepository;
        ILocationRepository _ILocationRepository;
        ImetadataRequestTypeRepository _ImetadataRequestTypeRepository;
        INominationsRepository _INominationsRepository;
        IModalFactory modalFactory;
        IPipelineRepository IpipelineRepository;

        public ContractService(IPipelineRepository IpipelineRepository,INominationsRepository INominationsRepository, IContractRepository IContractRepository, ILocationRepository ILocationRepository, ImetadataRequestTypeRepository ImetadataRequestTypeRepository, IModalFactory modalFactory) {
            _INominationsRepository=INominationsRepository;
            _IContractRepository = IContractRepository;
            _ILocationRepository = ILocationRepository;
            _ImetadataRequestTypeRepository = ImetadataRequestTypeRepository;
            this.IpipelineRepository = IpipelineRepository;
            this.modalFactory = modalFactory;
        }

        public bool AddContract(ContractsDTO contract)
        {
            try
            {                
                Contract contractModel = modalFactory.Create(contract);
                _IContractRepository.Add(contractModel);
                _IContractRepository.SaveChages();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        private Contract MapDTOToContract(ContractsDTO conDTO)
        {
            Contract con = new Contract();
            if (conDTO != null)
            {
                con.CreatedBy = conDTO.CreatedBy;
                con.CreatedDate = conDTO.CreatedDate;
                con.DeliveryZone = conDTO.DeliveryZone;
                con.FuelPercentage = conDTO.FuelPercentage;
                con.ID = conDTO.ID;
                con.IsActive = conDTO.IsActive;
                con.LocationFromID = conDTO.LocationFromID;
                con.LocationToID = conDTO.LocationToID;
                con.MDQ = conDTO.MDQ;
                con.ModifiedBy = conDTO.ModifiedBy;
                con.ModifiedDate = conDTO.ModifiedDate;
                //con.PipelineID = conDTO.PipelineID;
                con.ReceiptZone = conDTO.ReceiptZone;
                con.RequestNo = conDTO.RequestNo;
                con.RequestTypeID = conDTO.RequestTypeID != null ? conDTO.RequestTypeID.Value : 0;
                con.ShipperID = conDTO.ShipperID;
                con.ValidUpto = conDTO.ValidUpto;
                con.PipeDuns = conDTO.PipeDuns;
            }
            return con;
        }


        public IEnumerable<Contract> GetAll()
        {
            return _IContractRepository.GetAll();
        }

        public IEnumerable<ContractsDTO> GetContracts(string pipeDuns, int companyId)
        {
            var allContracts = _IContractRepository.GetByPipeNShipper(pipeDuns, companyId).ToList();
            return  MapContractsToDTO(allContracts);
        }


        private List<ContractsDTO> MapContractsToDTO(List<Contract> list)
        {
            List<ContractsDTO> conList = new List<ContractsDTO>();
            if (list != null && list.Count > 0)
                foreach (var item in list)
                {                    
                    conList.Add(modalFactory.Parse(item));
                }
            return conList;
        }

        public bool IsContractUsed(string contractNo)
        {
            return _INominationsRepository.IsContractExist(contractNo);
            //bool isContractUsed = false;
            //var model = _INominationsRepository.IsContractExist(contractNo);
            //if (model != null)
            //{
            //    isContractUsed = true;
            //}
            //return isContractUsed;           
        }

        public ContractsDTO GetContractOnId(int conId)
        {
            Contract contract= _IContractRepository.GetById(conId);
            return modalFactory.Parse(contract);
        }

        public bool UpdateContract(ContractsDTO contract)
        {
            try
            {   
                Contract cntrct = modalFactory.Create(contract);
                _IContractRepository.Update(cntrct);
                _IContractRepository.SaveChages();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        public Contract GetContractByContractNo(string SvcCnttNo, string pipelineDuns)
        {
            var contract = _IContractRepository.GetContractByContractNo(SvcCnttNo, pipelineDuns);
            return contract;
        }
        public bool RemoveContract(int contractID)
        {
            try
            {
                Contract cntrct = _IContractRepository.GetById(contractID);
                _IContractRepository.Delete(cntrct);
                _IContractRepository.SaveChages();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _IContractRepository.Dispose();
            }            
        }
    }
}
