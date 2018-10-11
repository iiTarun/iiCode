using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdSWNTPerTransactionRepository : RepositoryBase<SwntPerTransaction>, IUprdSWNTPerTransactionRepository
    {
        public UprdSWNTPerTransactionRepository(IDbFactory dbfactory) : base(dbfactory)
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

        public bool SaveSwntList(List<SwntPerTransaction> swntList)
        {
            using (DbContextTransaction dbTran = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var DistinctRecords = (from c in swntList
                                           select c.PostingDateTime).Distinct().ToList();
                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.SwntPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();
                        var RemoveFromSwntList = swntList.FindAll(
                                     x =>
                                     sd.Any(
                                                 k =>
                                                 k.NoticeEffectiveDateTime == x.NoticeEffectiveDateTime &&
                                                 k.NoticeId == x.NoticeId &&
                                                 k.PostingDateTime == x.PostingDateTime &&
                                                 k.TransportationserviceProvider == x.TransportationserviceProvider
                                           ));
                        RemoveFromSwntList.ForEach(a => swntList.Remove(a));
                        swntList = (from c in swntList
                                    select c).GroupBy(g => new { g.NoticeId, g.PostingDateTime, g.NoticeEffectiveDateTime, g.TransportationserviceProvider })
                        .Select(x => x.FirstOrDefault()).Distinct().ToList();
                        this.DbContext.SwntPerTransaction.AddRange(swntList);
                        this.DbContext.SaveChanges();
                        dbTran.Commit();
                        return true;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    return false;
                }
            }

        }
    }

    public interface IUprdSWNTPerTransactionRepository : IRepository<SwntPerTransaction>
    {
        void Save();
        bool SaveSwntList(List<SwntPerTransaction> swntList);
        void RemovePreviousNotices(int pipeId);
    }
}
