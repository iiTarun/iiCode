using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
  public  class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        IDbFactory _dbfactory;
        public ContractRepository(IDbFactory dbfactory) : base(dbfactory)
        {
            _dbfactory = dbfactory;
        }

        public void Dispose()
        {
            _dbfactory.Dispose();
        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }

        public Contract GetContractByContractNo(string SvcCnttNo, string pipelineDuns)
        {
            var contract = this.DbContext.Contract.Where(a => a.RequestNo == SvcCnttNo && a.PipeDuns == pipelineDuns).FirstOrDefault();
            return contract;
        }


        public IQueryable<Contract> GetByPipeNShipper(string PipelineDuns , int ShipperId)
        {
            return DbContext.Contract.Where(a=>a.PipeDuns== PipelineDuns && a.IsActive && a.ShipperID==ShipperId);
        }
    }

    public interface IContractRepository : IRepository<Contract>
    {
        void Dispose();
        void SaveChages();

        Contract GetContractByContractNo(string SvcCnttNo, string pipelineDuns);

        IQueryable<Contract> GetByPipeNShipper(string PipelineDuns, int ShipperId);
    }

}
