using System.Collections.Generic;
using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using Nom1Done.Data.Repositories;
using System.Linq;
using System;

namespace Nom1Done.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IPipelineRepository pipelinesRepository;
        private readonly INominationsRepository nominationRepository;
        private readonly IShipperCompanyRepository shipperCompanyRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INMQRPerTransactionRepository NMQRPerTransactionRepository;

        public DashboardService(INMQRPerTransactionRepository NMQRPerTransactionRepository, IPipelineRepository pipelinesRepository, INominationsRepository nominationRepository, IShipperCompanyRepository shipperCompanyRepository, IUnitOfWork unitOfWork)
        {
            this.pipelinesRepository = pipelinesRepository;
            this.nominationRepository = nominationRepository;
            this.shipperCompanyRepository = shipperCompanyRepository;
            this.unitOfWork = unitOfWork;
            this.NMQRPerTransactionRepository = NMQRPerTransactionRepository;
        }
        public IEnumerable<Pipeline> GetAllPipelines()
        {
            return pipelinesRepository.GetAll();
        }

        public BOShipperCompany GetItemByUser(string UserID)
        {
            return shipperCompanyRepository.GetShipperCompanyByUserId(UserID);
        }

        public IEnumerable<RejectedNomModel> GetRejectedNomination(string pipelineDuns,string userId)
        {
            return nominationRepository.GetAllRejectedNoms(pipelineDuns, userId);
        }

        public BOShipperCompany GetShipperByUser(string UserId)
        {
            return shipperCompanyRepository.GetShipperByUserId(UserId);
        }

        public List<RejectionReasonDTO> GetRejectionReason(string nomId)
        {
            List<RejectionReasonDTO> RejectionResonList = new List<RejectionReasonDTO>();
            List<NMQRPerTransaction> nmqrList = new List<NMQRPerTransaction>();
            Guid nomID = new Guid(nomId);
            string code = "";
            nmqrList = NMQRPerTransactionRepository.GetByTransactionId(nomID).ToList();
            foreach (var item in nmqrList)
            {
                RejectionReasonDTO reason = new RejectionReasonDTO();
                reason.ValidationCode = item.ValidationCode;
                reason.ValidationMessage = item.ValidationMessage;
                if (item.ValidationCode != code)
                {
                    RejectionResonList.Add(reason);
                    code = item.ValidationCode;
                }               
            }
            return RejectionResonList;
        }

        public Pipeline GetFirstPipelineByUser(string UserId, int companyId)
        {
            return pipelinesRepository.GetSelectedPipelineByUser(UserId,companyId);
        }
    }
}