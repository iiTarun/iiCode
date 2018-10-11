using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;
using Nom1Done.Data.Repositories;
using Nom1Done.Model.Models;

namespace Nom1Done.Service
{

    public class TransactionalReportingService : ITransactionalReportingService
    {
        ITransactionalReportingRepository transactionalReportingRepo;
        IModalFactory modelfactory;

        public TransactionalReportingService(IModalFactory modelfactory, ITransactionalReportingRepository transactionalReportingRepo)
        {
            this.transactionalReportingRepo = transactionalReportingRepo;
            this.modelfactory = modelfactory;
        }

        public bool AddReport(TransactionalReportDTO Item)
        {
            TransactionalReport model = modelfactory.Create(Item);
            transactionalReportingRepo.Add(model);
            transactionalReportingRepo.SaveChages();       
            return true;
        }

        public bool DeleteAll()
        {
            var list = transactionalReportingRepo.GetAll().ToList();

            foreach (var item in list) {
                transactionalReportingRepo.Delete(item);               
            }
            return true;
        }


        public bool Update(TransactionalReportDTO Item)
        {


            return true;
        }

        public List<TransactionalReportDTO> GetAllTransactionalReport(string PipelineDuns)
        {
            List<TransactionalReportDTO> list = new List<TransactionalReportDTO>();
            list = transactionalReportingRepo.GetAll().ToList().Where(a => a.PipeLineDuns == PipelineDuns).Select(a=>modelfactory.Parse(a)).ToList();
            return list;
        }
    }
}
