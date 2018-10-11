using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdUNSCRepository : RepositoryBase<UnscPerTransaction>, IUprdUNSCRepository
    {
        public UprdUNSCRepository(IDbFactory dbfactory) : base(dbfactory)
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
                                           select c.PostingDateTime).Distinct().ToList();
                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.UnscPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();
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

        public List<UnscPerTransaction> GetRecentUnscByPostDate(int pipelineID,DateTime? postDate )
        {
            var pDate = postDate.GetValueOrDefault().Date;        
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineID && DbFunctions.TruncateTime(a.PostingDateTime) == pDate).ToList();
        }


        public int GetTotalCountBypipelineId(int pipelineId)
        {
            return  DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId).Count();
        }

        public DateTime? GetRecentPostDateFromUNSCTable(int pipelineId)
        {
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId).Select(a => a.PostingDateTime).OrderByDescending(a=>a).FirstOrDefault().GetValueOrDefault();
        }

        public List<UnscPerTransaction> GetByPostDateStartEffEndEffDate(int pipelineId, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();            
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateStartEffEndEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();  
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();           
            return unsclist;
        }
        public List<UnscPerTransaction> GetByStartEffDateEndEffDate(int pipelineId,  DateTime? starteffdate, DateTime? endeffdate)
        {           
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>(); 
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId                   
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();            
            return unsclist;
        }
        public List<UnscPerTransaction> GetByStartEffDateEndEffDateKeyword(int pipelineId, DateTime? starteffdate, DateTime? endeffdate, string keyword)
        {            
            var seDate = starteffdate.GetValueOrDefault().Date;
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId                    
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();            
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateEndEffDate(int pipelineId, DateTime? postdate,  DateTime? endeffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;          
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();           
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate                  
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     ).ToList();           
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateEndEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? endeffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;           
            var eeDate = endeffdate.GetValueOrDefault().Date;
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>(); 
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate                    
                     && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eeDate
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();           
            return unsclist;
        }
        public List<UnscPerTransaction> GetByPostDateStartEffDate(int pipelineId, DateTime? postdate, DateTime? starteffdate)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;            
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();           
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate                   
                     ).ToList();            
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDateStartEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? starteffdate, string keyword)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var seDate = starteffdate.GetValueOrDefault().Date;           
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();            
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                     && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                     && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == seDate                   
                     && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                     ).ToList();           
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDate(int pipelineId, DateTime? postdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var pDate = postdate.GetValueOrDefault().Date;           
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                             ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByPostDateKeyword(int pipelineId, DateTime? postdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var pDate = postdate.GetValueOrDefault().Date;          
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                              && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                              ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByEndEffDate(int pipelineId, DateTime? endeffdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = endeffdate.GetValueOrDefault().Date;          
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                            ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByEndEffDateKeyword(int pipelineId, DateTime? endeffdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = endeffdate.GetValueOrDefault().Date;            
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                             && DbFunctions.TruncateTime(a.EndingEffectiveDay) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                             ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByStartEffDate(int pipelineId, DateTime? starteffdate)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = starteffdate.GetValueOrDefault().Date;          
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                            ).ToList();
            return unsclist;
        }

        public List<UnscPerTransaction> GetByStartEffDateKeyword(int pipelineId, DateTime? starteffdate, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            var eDate = starteffdate.GetValueOrDefault().Date;           
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                             && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                             && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                             ).ToList();    
            return unsclist;
        }

        public List<UnscPerTransaction> GetByKeyword(int pipelineId, string keyword)
        {
            List<UnscPerTransaction> unsclist = new List<UnscPerTransaction>();
            unsclist = DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId
                              && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                              ).ToList();
            return unsclist;
        }

        public IQueryable<UnscPerTransaction> GetByPipeline(int pipelineId)
        {
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId);
        }


        public IQueryable<UnscPerTransaction> GetByPipelineLoc(int pipelineId, string LocIdentifier)
        {
            return DbContext.UnscPerTransaction.Where(a => a.PipelineID == pipelineId && a.Loc==LocIdentifier);
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
    public interface IUprdUNSCRepository : IRepository<UnscPerTransaction>
    {
        void Save();
        IQueryable<UnscPerTransaction> GetByPipelineLoc(int pipelineId, string LocIdentifier);

        IQueryable<UnscPerTransaction> GetByPipeline(int pipelineId);

        bool SaveUnscList(List<UnscPerTransaction> unscList);

        int GetTotalCountBypipelineId(int pipelineId);
        DateTime? GetRecentPostDateFromUNSCTable(int pipelineId);
        List<UnscPerTransaction> GetRecentUnscByPostDate(int pipelineID, DateTime? postDate);  

        List<UnscPerTransaction> GetByKeyword(int pipelineId, string keyword);

        List<UnscPerTransaction> GetByPostDateStartEffEndEffDate(int pipelineId, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate);

        List<UnscPerTransaction> GetByPostDateStartEffEndEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? starteffdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByStartEffDateEndEffDate(int pipelineId, DateTime? starteffdate, DateTime? endeffdate);

        List<UnscPerTransaction> GetByStartEffDateEndEffDateKeyword(int pipelineId, DateTime? starteffdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByPostDateEndEffDate(int pipelineId, DateTime? postdate, DateTime? endeffdate);
        List<UnscPerTransaction> GetByPostDateEndEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? endeffdate, string keyword);
        List<UnscPerTransaction> GetByPostDateStartEffDate(int pipelineId, DateTime? postdate, DateTime? starteffdate);
        List<UnscPerTransaction> GetByPostDateStartEffDateKeyword(int pipelineId, DateTime? postdate, DateTime? starteffdate, string keyword);
        List<UnscPerTransaction> GetByPostDate(int pipelineId, DateTime? postdate);
        List<UnscPerTransaction> GetByPostDateKeyword(int pipelineId, DateTime? postdate, string keyword);

        List<UnscPerTransaction> GetByEndEffDate(int pipelineId, DateTime? endeffdate);
        List<UnscPerTransaction> GetByEndEffDateKeyword(int pipelineId, DateTime? endeffdate, string keyword);

        List<UnscPerTransaction> GetByStartEffDate(int pipelineId, DateTime? starteffdate);
        List<UnscPerTransaction> GetByStartEffDateKeyword(int pipelineId, DateTime? starteffdate, string keyword);
        UnscPerTransaction GetUnscByPipelIdLocForAvailPercent(int pipelineId, DateTime date, string location);
    }
}
