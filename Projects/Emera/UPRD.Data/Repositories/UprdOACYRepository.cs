using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using UPRD.Infrastructure;
using UPRD.Model;

namespace UPRD.Data.Repositories
{
    public class UprdOACYRepository:RepositoryBase<OACYPerTransaction>, IUprdOACYRepository
    {
        public UprdOACYRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
        

        public int GetOACYListTotalCount(int pipelineID)
        {
            return DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineID).Count();
        }

        public DateTime? GetRecentPostDateFromOacyList(int pipeline) {
            return DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipeline).OrderByDescending(a => a.PostingDateTime).FirstOrDefault().PostingDateTime;
        }


        //public List<OACYPerTransaction> GetOACYUsingPostDate(int pipelineId, DateTime? postDate)
        //{
        //    var pDate = postDate.GetValueOrDefault().Date;
        //    return DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId && DbFunctions.TruncateTime(a.PostingDateTime) == pDate).ToList();
        //}

        public bool SaveOacyList(List<OACYPerTransaction> oacyList)
        {
            try
            {
                using (DbContextTransaction dbTran = this.DbContext.Database.BeginTransaction())
                {
                    var DistinctRecords = (from g in oacyList
                                           select g.PostingDateTime).Distinct().ToList();
                    

                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.OACYPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();
                        var removeFromOacy = oacyList.FindAll(
                            x =>
                                    sd.Any(
                                                k =>
                                                k.EffectiveGasDayTime == x.EffectiveGasDayTime &&
                                                k.Loc == x.Loc &&
                                                k.PostingDateTime == x.PostingDateTime &&
                                                k.FlowIndicator == x.FlowIndicator &&
                                                k.ITIndicator == x.ITIndicator &&
                                                k.TotalScheduleQty == x.TotalScheduleQty &&
                                                k.TransactionServiceProvider == x.TransactionServiceProvider &&
                                                k.CycleIndicator == x.CycleIndicator
                                          ));
                        removeFromOacy.ForEach(a => oacyList.Remove(a));
                        //this.DbContext.OACYPerTransaction.RemoveRange(RecordsToDelete);
                        //oacyList=oacyList.RemoveAt
                        oacyList = (from c in oacyList
                                    select c).GroupBy(g => new
                                    {
                                        g.Loc,
                                        g.PostingDateTime,
                                        g.EffectiveGasDayTime,
                                        g.FlowIndicator,
                                        g.ITIndicator,
                                        g.CycleIndicator,
                                        g.TotalScheduleQty,
                                        g.TransactionServiceProvider
                                    })
                            .Select(x => x.FirstOrDefault()).Distinct().ToList();


                        this.DbContext.OACYPerTransaction.AddRange(oacyList);
                        this.DbContext.SaveChanges();
                        dbTran.Commit();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OACYPerTransaction> GetByPostDateEffDate(int pipelineId, DateTime? postdate, DateTime? effdate, string keyword, string Cycle)
        {
             var pDate = postdate.GetValueOrDefault().Date;
             var eDate = effdate.GetValueOrDefault().Date;
             List<OACYPerTransaction> oacylist = new List<OACYPerTransaction>();
            if(!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                var count = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId).Count();
                if (count > 0)
                {
                    DateTime? recentdate = GetRecentPostDateFromOacyList(pipelineId);
                    return GetByPostDateEffDate(pipelineId, recentdate, null, string.Empty, string.Empty);
                }
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate).ToList();
            }
            return oacylist;
        }

        public IQueryable<OACYPerTransaction> GetByPipeline(int pipelineId)
        {  
            return  DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId);           
        }

        public IQueryable<OACYPerTransaction> GetByPipelineLoc(int pipelineId,string LocationIdentifier)
        {
            return DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipelineId && a.Loc == LocationIdentifier);
        }

    }

public interface IUprdOACYRepository : IRepository<OACYPerTransaction>
    {
        IQueryable<OACYPerTransaction> GetByPipeline(int pipelineId);
        void Save();
        int GetOACYListTotalCount(int pipelineID);
        DateTime? GetRecentPostDateFromOacyList(int pipeline);
        bool SaveOacyList(List<OACYPerTransaction> oacyList);
        List<OACYPerTransaction> GetByPostDateEffDate(int pipelineId, DateTime? postdate, DateTime? effdate, string keyword, string Cycle);
        IQueryable<OACYPerTransaction> GetByPipelineLoc(int pipelineId, string LocationIdentifier);
    }
}
