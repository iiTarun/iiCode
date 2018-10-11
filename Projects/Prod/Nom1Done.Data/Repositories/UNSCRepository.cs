using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class UNSCRepository : RepositoryBase<UnscPerTransaction>, IUNSCRepository
    {
        public UNSCRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public bool SaveUnscList(List<UnscPerTransaction> unscList)
        {
            using (DbContextTransaction dbTran = this.DbContext.Database.BeginTransaction())
            {
                try
                {



                    var DistinctRecords = (from c in unscList
                                           group c by
                        new
                        {
                            c.Loc,
                            c.PostingDateTime,
                            c.EffectiveGasDayTime,
                            c.TransactionServiceProvider,
                            c.LocQTIDesc
                        } into grouped
                                           select grouped.Key.PostingDateTime).Distinct().ToList();



                    //(from c in unscList
                    // select c).GroupBy(g => new { g.Loc, g.PostingDateTime, g.EffectiveGasDayTime, g.TransactionServiceProvider, g.LocQTIDesc })
                    //      .Select(a => new UNSCDTO
                    //      {
                    //          Loc = a.Key.Loc,
                    //          PostingDateTime = a.Key.PostingDateTime,
                    //          EffectiveGasDayTime = a.Key.EffectiveGasDayTime,
                    //          TransactionServiceProvider = a.Key.TransactionServiceProvider,
                    //          LocQTIDesc = a.Key.LocQTIDesc
                    //      }).Distinct();

                    //var distinctPostingDates = (from a in DistinctRecords
                    //                            select a.PostingDateTime).Distinct();

                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.UnscPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();
                        //.Select(a => new UNSCDTO
                        //{
                        //    Loc = a.Loc,
                        //    PostingDateTime = a.PostingDateTime,
                        //    EffectiveGasDayTime = a.EffectiveGasDayTime,
                        //    TransactionServiceProvider = a.TransactionServiceProvider,
                        //    LocQTIDesc = a.LocQTIDesc

                        //}

                        //var RecordsToDelete = sd.FindAll(
                        //            x =>
                        //            unscList.Any(
                        //                        k =>
                        //                        k.EffectiveGasDayTime == x.EffectiveGasDayTime &&
                        //                        k.Loc == x.Loc &&
                        //                        k.LocQTIDesc == x.LocQTIDesc &&
                        //                        k.PostingDateTime == x.PostingDateTime &&
                        //                        k.TransactionServiceProvider == x.TransactionServiceProvider
                        //                  ));

                        //this.DbContext.UnscPerTransaction.RemoveRange(RecordsToDelete);
                        var RemoveFromUnscList = unscList.FindAll(
                                    x =>
                                    sd.Any(
                                                k =>
                                                k.EffectiveGasDayTime == x.EffectiveGasDayTime &&
                                                k.Loc == x.Loc &&
                                                k.LocQTIDesc == x.LocQTIDesc &&
                                                k.PostingDateTime == x.PostingDateTime &&
                                                k.TransactionServiceProvider == x.TransactionServiceProvider
                                          ));

                        RemoveFromUnscList.ForEach(a => unscList.Remove(a));

                        unscList = (from c in unscList
                                    select c).GroupBy(g => new { g.Loc, g.PostingDateTime, g.EffectiveGasDayTime, g.TransactionServiceProvider, g.LocQTIDesc })
                            .Select(x => x.FirstOrDefault()).Distinct().ToList();


                        this.DbContext.UnscPerTransaction.AddRange(unscList);
                        this.DbContext.SaveChanges();
                        dbTran.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    return false;
                }
            }

        }

        public List<UnscPerTransaction> GetRecentUnscByPostDate(string pipelineDuns, DateTime? postDate)
        {
            var pDate = postDate.GetValueOrDefault().Date;
            return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns && DbFunctions.TruncateTime(a.PostingDateTime) == pDate).ToList();
        }


        public int GetTotalCountBypipelineId(int pipelineId)
        {
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId).Count();
        }

        public int GetTotalCountByPipeDuns(string pipelineDuns)
        {
            return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
        }

        public DateTime? GetRecentPostDateFromUNSCTable(int pipelineId)
        {
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId).Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault().GetValueOrDefault();
        }

        public DateTime? GetRecentPostDateUsingDuns(string pipelineDuns)
        {
            return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns).Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }

        public List<UnscPerTransaction> GetByPostDateStartEffEndEffDate(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == PipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateStartEffEndEffDateKeyword(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == PipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByStartEffDateEndEffDate(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate)
        {
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByStartEffDateEndEffDateKeyword(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateEndEffDate(string pipelineDuns, DateTime? postdate, DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateEndEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateStartEffDate(string pipelineDuns, DateTime? postdate, DateTime? starteffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDateStartEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? starteffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDate(string pipelineDuns, DateTime? postdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var pDate = postdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                             ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDateKeyword(string pipelineDuns, DateTime? postdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var pDate = postdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                              ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByEndEffDate(string pipelineDuns, DateTime? endeffdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = endeffdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                            ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByEndEffDateKeyword(string pipelineDuns, DateTime? endeffdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = endeffdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                             ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByStartEffDate(string pipelineDuns, DateTime? starteffdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = starteffdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                            ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByStartEffDateKeyword(string pipelineDuns, DateTime? starteffdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = starteffdate.GetValueOrDefault().Date;
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                             ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByKeyword(string pipelineDuns, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                              ).ToList();
            return unsclist;
        }

        public IQueryable<UnscPerTransaction> GetByPipeline(string pipelineDuns)
        {
            return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns);
        }


        public IQueryable<UnscPerTransaction> GetByPipelineLoc(string pipeDuns, string LocIdentifier)
        {
            DateTime? recentDate = GetRecentPostingDate();
            if (recentDate != null)
            {
                return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier
                && DbFunctions.TruncateTime(a.PostingDateTime) == DbFunctions.TruncateTime(recentDate)
                );
            }
            else
            {
                return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier);
            }
        }
        public IQueryable<UnscPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocIdentifier)
        {
            return DbContext.UnscPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocIdentifier);
        }

        public DateTime? GetRecentPostingDate()
        {
            return DbContext.UnscPerTransaction.Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
        }


        public IQueryable<UnscPerTransaction> QueryPostDate(IQueryable<UnscPerTransaction> query, DateTime postdate)
        {
            query = query.Where(p => p.PostingDateTime == postdate);
            return query;
        }

        public IQueryable<UnscPerTransaction> QueryStartEffGasDate(IQueryable<UnscPerTransaction> query, DateTime starteffGasdate)
        {
            query = query.Where(p => p.EffectiveGasDayTime == starteffGasdate);
            return query;
        }

        public IQueryable<UnscPerTransaction> QueryEndEffDate(IQueryable<UnscPerTransaction> query, DateTime endeffGasdate)
        {
            query = query.Where(p => p.EndingEffectiveDay == endeffGasdate);
            return query;
        }

        public IQueryable<UnscPerTransaction> QueryKeyword(IQueryable<UnscPerTransaction> query, string keyword)
        {
            query = query.Where(a => (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower()))));
            return query;
        }

        public UnscPerTransaction GetUnscByPipelIdLocForAvailPercent(int pipelineId, DateTime date, string location)
        {
            var data = this.DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
              && DbFunctions.TruncateTime(a.PostingDateTime) == date.Date
              && a.Loc == location).FirstOrDefault();
            return data;
        }
    }
    public interface IUNSCRepository : IRepository<UnscPerTransaction>
    {
        void Save();
        IQueryable<UnscPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocIdentifier);
        DateTime? GetRecentPostDateUsingDuns(string pipelineDuns);

        DateTime? GetRecentPostingDate();
        IQueryable<UnscPerTransaction> GetByPipelineLoc(string pipeDuns, string LocIdentifier);

        IQueryable<UnscPerTransaction> GetByPipeline(string pipelineDuns);

        bool SaveUnscList(List<UnscPerTransaction> unscList);

        int GetTotalCountBypipelineId(int pipelineId);
        int GetTotalCountByPipeDuns(string pipelineDuns);

        DateTime? GetRecentPostDateFromUNSCTable(int pipelineId);
        List<UnscPerTransaction> GetRecentUnscByPostDate(string pipelineDuns, DateTime? postDate);

        List<UnscPerTransaction> GetByKeyword(string pipelineDuns, string keyword);

        List<UnscPerTransaction> GetByPostDateStartEffEndEffDate(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate);

        List<UnscPerTransaction> GetByPostDateStartEffEndEffDateKeyword(string PipelineDuns, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByStartEffDateEndEffDate(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate);

        List<UnscPerTransaction> GetByStartEffDateEndEffDateKeyword(string pipelineDuns, DateTime? starteffdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByPostDateEndEffDate(string pipelineDuns, DateTime? postdate, DateTime? endeffdate);
        List<UnscPerTransaction> GetByPostDateEndEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByPostDateStartEffDate(string pipelineDuns, DateTime? postdate, DateTime? starteffdate);
        List<UnscPerTransaction> GetByPostDateStartEffDateKeyword(string pipelineDuns, DateTime? postdate, DateTime? starteffdate, string keyword);
        List<UnscPerTransaction> GetByPostDate(string pipelineDuns, DateTime? postdate);
        List<UnscPerTransaction> GetByPostDateKeyword(string pipelineDuns, DateTime? postdate, string keyword);

        List<UnscPerTransaction> GetByEndEffDate(string pipelineDuns, DateTime? endeffdate);
        List<UnscPerTransaction> GetByEndEffDateKeyword(string pipelineDuns, DateTime? endeffdate, string keyword);

        List<UnscPerTransaction> GetByStartEffDate(string pipelineDuns, DateTime? starteffdate);
        List<UnscPerTransaction> GetByStartEffDateKeyword(string pipelineDuns, DateTime? starteffdate, string keyword);
        UnscPerTransaction GetUnscByPipelIdLocForAvailPercent(int pipelineId, DateTime date, string location);
    }
}
