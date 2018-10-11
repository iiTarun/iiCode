using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class OACYRepository : RepositoryBase<OACYPerTransaction>, IOACYRepository
    {
        public OACYRepository(IDbFactory dbfactory) : base(dbfactory)
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

        public int GetOACYListTotalCount(string pipelineDuns)
        {
            return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
        }


        public DateTime? GetRecentPostDateFromOacyList(int pipeline)
        {
            return DbContext.OACYPerTransaction.Where(a => a.PipelineID == pipeline).OrderByDescending(a => a.PostingDateTime).FirstOrDefault().PostingDateTime;
        }

        public DateTime? GetRecentPostDateUsngDuns(string pipelineDuns)
        {
            return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns)
                .Select(a => a.PostingDateTime)
                .OrderByDescending(a => a).FirstOrDefault();
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
                                           group g by
                        new
                        {
                            g.Loc,
                            g.PostingDateTime,
                            g.EffectiveGasDayTime,
                            g.FlowIndicator,
                            g.ITIndicator,
                            g.CycleIndicator,
                            g.TotalScheduleQty,
                            g.TransactionServiceProvider
                        } into grouped
                                           select grouped.Key.PostingDateTime).Distinct().ToList();



                    //  select c).GroupBy(g => new
                    //  {
                    //      g.Loc,
                    //      g.PostingDateTime,
                    //      g.EffectiveGasDayTime,
                    //      g.FlowIndicator,
                    //      g.ITIndicator,
                    //      g.CycleIndicator,
                    //      g.TotalScheduleQty,
                    //      g.TransactionServiceProvider
                    //  })
                    //.Select(a => new OACYDTO
                    //{
                    //    //Loc = a.Key.Loc,
                    //    //CycleIndicator = a.Key.CycleIndicator,
                    //    //EffectiveGasDayTime = a.Key.EffectiveGasDayTime,
                    //    //FlowIndicator = a.Key.FlowIndicator,
                    //    //ITIndicator = a.Key.ITIndicator,
                    //    PostingDateTime = a.Key.PostingDateTime
                    //    //TotalScheduleQty = a.Key.TotalScheduleQty,
                    //    //TransactionServiceProvider = a.Key.TransactionServiceProvider
                    //}).ToList();

                    //var distinctPostingDates = (from a in DistinctRecords
                    //                            select a.PostingDateTime).Distinct();

                    if (DistinctRecords.Count() > 0)
                    {

                        var sd = this.DbContext.OACYPerTransaction.Where(c => DistinctRecords.Contains(c.PostingDateTime)).ToList();
                        //.Select(a => new OACYDTO
                        //{
                        //    Loc = a.Loc,
                        //    CycleIndicator = a.CycleIndicator,
                        //    EffectiveGasDayTime = a.EffectiveGasDayTime,
                        //    FlowIndicator = a.FlowIndicator,
                        //    ITIndicator = a.ITIndicator,
                        //    PostingDateTime = a.PostingDateTime,
                        //    TotalScheduleQty = a.TotalScheduleQty,
                        //    TransactionServiceProvider = a.TransactionServiceProvider

                        //}).ToList();

                        //var RecordsToDelete = sd.FindAll(
                        //            x =>
                        //            oacyList.Any(
                        //                        k =>
                        //                        k.EffectiveGasDayTime == x.EffectiveGasDayTime &&
                        //                        k.Loc == x.Loc &&
                        //                        k.PostingDateTime == x.PostingDateTime &&
                        //                        k.FlowIndicator == x.FlowIndicator &&
                        //                        k.ITIndicator == x.ITIndicator &&
                        //                        k.TotalScheduleQty == x.TotalScheduleQty &&
                        //                        k.TransactionServiceProvider == x.TransactionServiceProvider &&
                        //                        k.CycleIndicator == x.CycleIndicator
                        //                  ));

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

        public List<OACYPerTransaction> GetByPostDateEffDate(string pipelineDuns, DateTime? postdate, DateTime? effdate, string keyword, string Cycle)
        {
            var pDate = postdate.GetValueOrDefault().Date;
            var eDate = effdate.GetValueOrDefault().Date;
            List<OACYPerTransaction> oacylist = new List<OACYPerTransaction>();
            if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                var count = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns).Count();
                if (count > 0)
                {
                    DateTime? recentdate = GetRecentPostDateUsngDuns(pipelineDuns);
                    return GetByPostDateEffDate(pipelineDuns, recentdate, null, string.Empty, string.Empty);
                }
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (!string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && (((a.CycleIndicator ?? "").ToLower().Contains(Cycle.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && !string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && (((a.LocName ?? "").ToLower().Contains(keyword.ToLower())) || ((a.Loc ?? "").ToLower().Contains(keyword.ToLower())))).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate != DateTime.MinValue && eDate == DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.PostingDateTime) == pDate).ToList();
            }
            else if (string.IsNullOrEmpty(Cycle) && string.IsNullOrEmpty(keyword) && pDate == DateTime.MinValue && eDate != DateTime.MinValue)
            {
                oacylist = DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipelineDuns
                && DbFunctions.TruncateTime(a.EffectiveGasDayTime) == eDate).ToList();
            }
            return oacylist;
        }





        public IQueryable<OACYPerTransaction> GetByPipeline(string pipeDuns)
        {
            return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns);
        }

        public IQueryable<OACYPerTransaction> GetByPipelineLoc(string pipeDuns, string LocationIdentifier)
        {
            DateTime? recentdate = GetMostRecentOacyPostDate();
            if (recentdate != null)
            {
                return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier
                       && DbFunctions.TruncateTime(a.PostingDateTime) == DbFunctions.TruncateTime(recentdate)
                       );
            }
            else
            {
                return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier);
            }
        }


        public IQueryable<OACYPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocationIdentifier)
        {
            return DbContext.OACYPerTransaction.Where(a => a.TransactionServiceProvider == pipeDuns && a.Loc == LocationIdentifier);
        }

        public DateTime? GetMostRecentOacyPostDate()
        {
            var recentDate = DbContext.OACYPerTransaction.Select(a => a.PostingDateTime).OrderByDescending(a => a).FirstOrDefault();
            return recentDate;
        }


    }

    public interface IOACYRepository : IRepository<OACYPerTransaction>
    {
        IQueryable<OACYPerTransaction> GetAllByPipelineLoc(string pipeDuns, string LocationIdentifier);
        DateTime? GetRecentPostDateUsngDuns(string pipelineDuns);
        DateTime? GetMostRecentOacyPostDate();
        IQueryable<OACYPerTransaction> GetByPipeline(string pipeDuns);
        void Save();
        int GetOACYListTotalCount(int pipelineID);
        int GetOACYListTotalCount(string pipelineDuns);

        DateTime? GetRecentPostDateFromOacyList(int pipeline);
        bool SaveOacyList(List<OACYPerTransaction> oacyList);
        List<OACYPerTransaction> GetByPostDateEffDate(string pipelineDuns, DateTime? postdate, DateTime? effdate, string keyword, string Cycle);
        IQueryable<OACYPerTransaction> GetByPipelineLoc(string pipeDuns, string LocationIdentifier);
    }
}
