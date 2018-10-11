using System;
using System.Collections.Generic;
using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class SWNTPerTransactionRepository : RepositoryBase<SwntPerTransaction>, ISWNTPerTransactionRepository
    {
        public SWNTPerTransactionRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void RemovePreviousNotices(int pipeId)
        {
            var data = this.DbContext.SwntPerTransaction.Where(a => a.CreatedDate.Value.Day == DateTime.Now.Day && a.CreatedDate.Value.Month == DateTime.Now.Month && a.CreatedDate.Value.Year == DateTime.Now.Year && a.PipelineId == pipeId);
            this.DbContext.SwntPerTransaction.RemoveRange(data);
            this.DbContext.SaveChanges();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public bool SaveSwntList(List<SwntPerTransaction> swntList, int pipeId)
        {
            try
            {
                if (swntList.Count > 0)
                {
                    swntList.ForEach(a => 
                    this.DbContext.SwntPerTransaction
                    .RemoveRange(this.DbContext.SwntPerTransaction.Where(b => 
                    b.PipelineId == pipeId 
                    && b.NoticeId == a.NoticeId)));
                    this.DbContext.SwntPerTransaction.AddRange(swntList.Select(a => { a.PipelineId = pipeId; return a; }));
                    this.DbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public interface ISWNTPerTransactionRepository : IRepository<SwntPerTransaction>
    {
        void Save();
        bool SaveSwntList(List<SwntPerTransaction> swntList, int pipeId);
        void RemovePreviousNotices(int pipeId);
    }
}
