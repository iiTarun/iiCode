using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Nom1Done.Data.Repositories
{
    public class SwntProcessingDTO
    {
        public string NoticeId { get; set; }
        public DateTime? PostingDateTime { get; set; }
        public DateTime? NoticeEffectiveDateTime { get; set; }

    }


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

        public bool SaveSwntList(List<SwntPerTransaction> swntList)
        {
            using (DbContextTransaction dbTran = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    //if (swntList.Count > 0)
                    //{

                    var DistinctRecords = (from c in swntList
                                           group c by new
                                           {
                                               c.NoticeId,
                                               c.PostingDateTime,
                                               c.NoticeEffectiveDateTime,
                                               c.TransportationserviceProvider
                                           } into grouped
                                           select grouped.Key.PostingDateTime).Distinct().ToList();


                    //                 select c).GroupBy(g => new { g.NoticeId, g.PostingDateTime, g.NoticeEffectiveDateTime })
                    //.Select(a => new SwntProcessingDTO
                    //{
                    //    NoticeId = a.Key.NoticeId,
                    //    PostingDateTime = a.Key.PostingDateTime,
                    //    NoticeEffectiveDateTime = a.Key.NoticeEffectiveDateTime
                    //}).Distinct();

                    //var distinctPostingDates = (from a in DistinctRecords
                    //                            select a.PostingDateTime).Distinct();

                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.SwntPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();



                        //var RecordsToDelete = sd.FindAll(
                        //             x =>
                        //             swntList.Any(
                        //                         k =>
                        //                         k.NoticeEffectiveDateTime == x.NoticeEffectiveDateTime &&
                        //                         k.NoticeId == x.NoticeId &&
                        //                         k.PostingDateTime == x.PostingDateTime &&
                        //                         k.TransportationserviceProvider == x.TransportationserviceProvider
                        //                   ));

                        //this.DbContext.SwntPerTransaction.RemoveRange(RecordsToDelete);
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
                                    select c).GroupBy(g => new { g.NoticeId, g.PostingDateTime, g.NoticeEffectiveDateTime,g.TransportationserviceProvider })
                        .Select(x => x.FirstOrDefault()).Distinct().ToList();


                        this.DbContext.SwntPerTransaction.AddRange(swntList);
                        this.DbContext.SaveChanges();
                        dbTran.Commit();
                        return true;
                        //   }
                        //   return true;
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

    public interface ISWNTPerTransactionRepository : IRepository<SwntPerTransaction>
    {
        void Save();
        bool SaveSwntList(List<SwntPerTransaction> swntList);
        void RemovePreviousNotices(int pipeId);
    }
}
