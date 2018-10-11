using System.Linq;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Data.Entity;
using Nom1Done.DTO;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;

namespace Nom1Done.Data.Repositories
{
    public class BatchRepository : RepositoryBase<V4_Batch>, IBatchRepository
    {
        public BatchRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public V4_Batch GetByTransactionID(Guid transactionId)
        {
            return (from a in DbContext.V4_Batch
                    where a.TransactionID == transactionId
                    select a).FirstOrDefault();
        }

        public int GetStatusOnTransactionId(Guid tranId)
        {
            return DbContext.V4_Batch.Where(a => a.TransactionID == tranId).FirstOrDefault().StatusID;
        }

        public void matchNonPathed()
        {
            try
            {
                var submittedBatches = (from b in this.DbContext.V4_Batch
                                        where (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                                        && b.NomTypeID == (int)NomType.NonPathed
                                        && (DbFunctions.TruncateTime(b.FlowStartDate) >= DbFunctions.TruncateTime(DateTime.Now)
                                        && b.PipelineID == b.PipelineID)
                                        select b).ToList();
                if (submittedBatches != null && submittedBatches.Count >= 2)
                {
                    var batchs = submittedBatches;
                    foreach (var sbatch in submittedBatches.ToList())
                    {
                        var batch = batchs.Where(a => a.TransactionID == sbatch.TransactionID).FirstOrDefault();
                        batchs.Remove(batch);
                        var matchbatch = batchs.Find(a => a.FlowStartDate.Day == batch.FlowStartDate.Day
                        && a.FlowStartDate.Month == batch.FlowStartDate.Month
                        && a.FlowStartDate.Year == batch.FlowStartDate.Year
                        && a.FlowEndDate.Day == batch.FlowEndDate.Day
                        && a.FlowEndDate.Month == batch.FlowEndDate.Month
                        && a.FlowEndDate.Year == batch.FlowEndDate.Year && a.TransactionID != batch.TransactionID);
                        if (matchbatch != null)
                        {
                            var Nom = this.DbContext.V4_Nomination.Where(a => a.TransactionID == batch.TransactionID).FirstOrDefault();
                            var matchNom = this.DbContext.V4_Nomination.Where(a => a.TransactionID == matchbatch.TransactionID).FirstOrDefault();
                            if (Nom.TransactionType == matchNom.TransactionType
                                && Nom.ContractNumber == matchNom.ContractNumber
                                && Nom.ReceiptLocationIdentifier == matchNom.ReceiptLocationIdentifier
                                && Nom.UpstreamIdentifier == matchNom.UpstreamIdentifier
                                && Nom.UpstreamContractIdentifier == matchNom.UpstreamContractIdentifier
                                && Nom.DeliveryLocationIdentifer == matchNom.DeliveryLocationIdentifer
                                && Nom.DownstreamIdentifier == matchNom.DownstreamIdentifier
                                && Nom.DownstreamContractIdentifier == matchNom.DownstreamContractIdentifier
                                && Nom.PackageId == matchNom.PackageId
                                && Nom.PathType == matchNom.PathType)
                            {
                                batchs.Remove(matchbatch);
                                if (batch.CreatedDate.Date <= matchbatch.CreatedDate.Date)
                                {
                                    batch.StatusID = (int)statusBatch.Replaced;
                                    this.DbContext.Entry(batch).State = EntityState.Modified;
                                    batchs.Add(matchbatch);
                                }
                                else
                                {
                                    matchbatch.StatusID = (int)statusBatch.Replaced;
                                    this.DbContext.Entry(matchbatch).State = EntityState.Modified;
                                    batchs.Add(batch);
                                }
                                this.DbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void matchPathedNomination()
        {
            try
            {
                var FindDuplicates = (from b in this.DbContext.V4_Batch
                                      join n in this.DbContext.V4_Nomination on b.TransactionID equals n.TransactionID
                                      where (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                                        && b.NomTypeID == (int)NomType.Pathed
                                        && DbFunctions.TruncateTime(b.FlowStartDate) >= DbFunctions.TruncateTime(DateTime.Now)
                                        //&& DbFunctions.TruncateTime(b.FlowStartDate) == DbFunctions.TruncateTime(b.FlowStartDate)
                                        

                                      group new { b, n } by new
                                      {
                                          //b.TransactionID,
                                          n.TransactionType,
                                          n.ContractNumber,
                                          n.ReceiptLocationIdentifier,
                                          n.UpstreamIdentifier,
                                          n.UpstreamContractIdentifier,
                                          n.DeliveryLocationIdentifer,
                                          n.DownstreamIdentifier,
                                          n.DownstreamContractIdentifier,
                                          n.PackageId,
                                          b.PipelineID,
                                          FlowStartDate = DbFunctions.TruncateTime(b.FlowStartDate),
                                          FlowEndDate = DbFunctions.TruncateTime(b.FlowEndDate)

                                      } into grouped
                                      select new TransGuids
                                      {
                                          transid = grouped.OrderByDescending(a=>a.b.SubmitDate).Select(z => z.b.TransactionID).ToList()
                                      }).ToList();
                //var list = (from b in this.DbContext.V4_Batch
                //            join n in this.DbContext.V4_Nomination on b.TransactionID equals n.TransactionID
                //            where (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                //              && b.NomTypeID == (int)NomType.Pathed
                //              && DbFunctions.TruncateTime(b.FlowStartDate) >= DbFunctions.TruncateTime(DateTime.Now)
                //            //&& DbFunctions.TruncateTime(b.FlowStartDate) == DbFunctions.TruncateTime(b.FlowStartDate)


                //            group new { b, n } by new
                //            {
                //                //b.TransactionID,
                //                n.TransactionType,
                //                n.ContractNumber,
                //                n.ReceiptLocationIdentifier,
                //                n.UpstreamIdentifier,
                //                n.UpstreamContractIdentifier,
                //                n.DeliveryLocationIdentifer,
                //                n.DownstreamIdentifier,
                //                n.DownstreamContractIdentifier,
                //                n.PackageId,
                //                b.PipelineID,
                //                FlowStartDate = DbFunctions.TruncateTime(b.FlowStartDate),
                //                FlowEndDate = DbFunctions.TruncateTime(b.FlowEndDate)

                //            } into grouped select grouped).ToList();
                           //select new TransGuids
                           //{
                           //    transid = grouped.OrderByDescending(a => a.b.SubmitDate).Select(z => z.b.TransactionID).ToList()
                           //}).ToList();



                if (FindDuplicates != null)
                {
                    List<Guid> ItemToBeReplaced = new List<Guid>();
                    //foreach (var item in FindDuplicates)
                    //{
                    //    var res=item.transid.Select(a => a).Skip(1).ToList();
                    //    ItemToBeReplaced.AddRange(res);
                    //}
                    FindDuplicates.ForEach(a => ItemToBeReplaced.AddRange(a.transid.Select(b => b).Skip(1).ToList()));
                    //var ItemToBeReplaced = FindDuplicates.FirstOrDefault().transid.Select(a => a).Skip(1).ToList();
                    if (ItemToBeReplaced.Count > 0)
                    {
                        var Batch = this.DbContext.V4_Batch.Where(a => ItemToBeReplaced.Contains(a.TransactionID)).ToList();
                        foreach (var item in Batch)
                        {
                            item.StatusID = (int)statusBatch.Replaced;
                        }
                    }
                    this.DbContext.SaveChanges();
                }


                //        lst.GroupBy(x => x)
                //.Where(g => g.Count() > 1)
                //.Select(g => g.Key)
                //.ToList();



                // #region pathed nominations
                //var submittedBatches = (from b in this.DbContext.V4_Batch
                //                        where (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                //                        && b.NomTypeID == (int)NomType.Pathed
                //                        && (DbFunctions.TruncateTime(b.FlowStartDate) >= DbFunctions.TruncateTime(DateTime.Now)
                //                        && b.PipelineID == b.PipelineID)
                //                        select b).ToList();
                //if (submittedBatches != null && submittedBatches.Count >= 2)
                //{
                //    var batchs = submittedBatches;
                //    foreach (var sbatch in submittedBatches.ToList())
                //    {
                //        var batch = batchs.Where(a => a.TransactionID == sbatch.TransactionID).FirstOrDefault();
                //        batchs.Remove(batch);
                //        var matchbatch = batchs.Find(a => a.FlowStartDate.Day == batch.FlowStartDate.Day
                //        && a.FlowStartDate.Month == batch.FlowStartDate.Month
                //        && a.FlowStartDate.Year == batch.FlowStartDate.Year
                //        && a.FlowEndDate.Day == batch.FlowEndDate.Day
                //        && a.FlowEndDate.Month == batch.FlowEndDate.Month
                //        && a.FlowEndDate.Year == batch.FlowEndDate.Year && a.TransactionID != batch.TransactionID);
                //        if (matchbatch != null)
                //        {

                //            var Nom = this.DbContext.V4_Nomination.Where(a => a.TransactionID == batch.TransactionID).FirstOrDefault();
                //            var matchNom = this.DbContext.V4_Nomination.Where(a => a.TransactionID == matchbatch.TransactionID).FirstOrDefault();
                //            if (Nom.TransactionType == matchNom.TransactionType
                //                && Nom.ContractNumber == matchNom.ContractNumber
                //                && Nom.ReceiptLocationIdentifier == matchNom.ReceiptLocationIdentifier
                //                && Nom.UpstreamIdentifier == matchNom.UpstreamIdentifier
                //                && Nom.UpstreamContractIdentifier == matchNom.UpstreamContractIdentifier
                //                && Nom.DeliveryLocationIdentifer == matchNom.DeliveryLocationIdentifer
                //                && Nom.DownstreamIdentifier == matchNom.DownstreamIdentifier
                //                && Nom.DownstreamContractIdentifier == matchNom.DownstreamContractIdentifier
                //                && Nom.PackageId == matchNom.PackageId)
                //            {
                //                batchs.Remove(matchbatch);
                //                if (batch.CreatedDate.Date <= matchbatch.CreatedDate.Date)
                //                {
                //                    batch.StatusID = (int)statusBatch.Replaced;
                //                    this.DbContext.Entry(batch).State = EntityState.Modified;
                //                    batchs.Add(matchbatch);
                //                }
                //                else
                //                {
                //                    matchbatch.StatusID = (int)statusBatch.Replaced;
                //                    this.DbContext.Entry(matchbatch).State = EntityState.Modified;
                //                    batchs.Add(batch);
                //                }
                //                this.DbContext.SaveChanges();
                //            }
                //        }
                //    }
                //}
                ///#endregion
            }
            catch (Exception ex)
            {

            }

        }

        public void matchPNTNomination()
        {
            try
            {
                var submittedBatches = (from b in this.DbContext.V4_Batch
                                        where (b.StatusID == (int)statusBatch.Success_Gisb || b.StatusID == (int)statusBatch.Success_NMQR)
                                        && b.NomTypeID == (int)NomType.PNT
                                        && (DbFunctions.TruncateTime(b.FlowStartDate) >= DbFunctions.TruncateTime(DateTime.Now)
                                        && b.PipelineID == b.PipelineID)
                                        select b).ToList();
                if (submittedBatches != null && submittedBatches.Count >= 2)
                {
                    var batchs = submittedBatches;
                    foreach (var sbatch in submittedBatches.ToList())
                    {
                        var batch = batchs.Where(a => a.TransactionID == sbatch.TransactionID).FirstOrDefault();
                        batchs.Remove(batch);
                        var matchbatch = batchs.Find(a => a.FlowStartDate.Day == batch.FlowStartDate.Day
                        && a.FlowStartDate.Month == batch.FlowStartDate.Month
                        && a.FlowStartDate.Year == batch.FlowStartDate.Year
                        && a.FlowEndDate.Day == batch.FlowEndDate.Day
                        && a.FlowEndDate.Month == batch.FlowEndDate.Month
                        && a.FlowEndDate.Year == batch.FlowEndDate.Year && a.TransactionID != batch.TransactionID);
                        if (matchbatch != null)
                        {
                            #region get nomination in each batch
                            var matchBatchNomList = this.DbContext.V4_Nomination.Where(a => a.TransactionID == matchbatch.TransactionID).ToList();
                            var batchNomList = this.DbContext.V4_Nomination.Where(a => a.TransactionID == batch.TransactionID).ToList();
                            if (matchBatchNomList != null && batchNomList != null
                                && matchBatchNomList.Count() == batchNomList.Count()
                                && matchBatchNomList.Where(a => a.PathType == "S").Count() == batchNomList.Where(a => a.PathType == "S").Count()
                                && matchBatchNomList.Where(a => a.PathType == "M").Count() == batchNomList.Where(a => a.PathType == "M").Count()
                                && matchBatchNomList.Where(a => a.PathType == "T").Count() == batchNomList.Where(a => a.PathType == "T").Count())
                            {
                                bool supplyMatch = false;
                                bool marketMatch = false;
                                bool transportMatch = false;
                                #region match supply rows
                                foreach (var item in batchNomList.Where(a => a.PathType == "S"))
                                {
                                    supplyMatch = matchBatchNomList.Any(a => a.ReceiptLocationIdentifier == item.ReceiptLocationIdentifier
                                      && a.UpstreamIdentifier == item.UpstreamIdentifier
                                      && a.ContractNumber == item.ContractNumber
                                      && a.PackageId == item.PackageId
                                      && a.UpstreamContractIdentifier == item.UpstreamContractIdentifier);
                                    if (!supplyMatch)
                                        break;
                                }
                                #endregion
                                #region match transport rows
                                if (supplyMatch)
                                    foreach (var item in batchNomList.Where(a => a.PathType == "T"))
                                    {
                                        transportMatch = matchBatchNomList.Any(a => a.ReceiptLocationIdentifier == item.ReceiptLocationIdentifier
                                          && a.DeliveryLocationIdentifer == item.DeliveryLocationIdentifer
                                          && a.ContractNumber == item.ContractNumber
                                          && a.PackageId == item.PackageId);
                                        if (!transportMatch)
                                            break;
                                    }
                                #endregion
                                #region match market rows
                                if (supplyMatch && transportMatch)
                                    foreach (var item in batchNomList.Where(a => a.PathType == "M"))
                                    {
                                        marketMatch = matchBatchNomList.Any(a => a.DeliveryLocationIdentifer == item.DeliveryLocationIdentifer
                                          && a.DownstreamIdentifier == item.DownstreamIdentifier
                                          && a.ContractNumber == item.ContractNumber
                                          && a.PackageId == item.PackageId
                                          && a.DownstreamContractIdentifier == item.DownstreamContractIdentifier);
                                        if (!marketMatch)
                                            break;
                                    }
                                #endregion
                                if (supplyMatch && marketMatch && transportMatch)
                                {
                                    batchs.Remove(matchbatch);
                                    if (batch.CreatedDate.Date <= matchbatch.CreatedDate.Date)
                                    {
                                        batch.StatusID = (int)statusBatch.Replaced;
                                        this.DbContext.Entry(batch).State = EntityState.Modified;
                                        batchs.Add(matchbatch);
                                    }
                                    else
                                    {
                                        matchbatch.StatusID = (int)statusBatch.Replaced;
                                        this.DbContext.Entry(matchbatch).State = EntityState.Modified;
                                        batchs.Add(batch);
                                    }
                                    this.DbContext.SaveChanges();
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }

        public void AddRange(List<V4_Batch> batches)
        {
            this.DbContext.V4_Batch.AddRange(batches);
            this.DbContext.SaveChanges();
        }

        public List<string> GetKochIds() {            
           return DbContext.V4_Nomination.Select(a => a.KochId).ToList(); 
        }

    }

    public interface IBatchRepository : IRepository<V4_Batch>
    {
        void SaveChages();
        V4_Batch GetByTransactionID(Guid transactionId);
        int GetStatusOnTransactionId(Guid tranId);
        void matchPathedNomination();
        void matchPNTNomination();
        void matchNonPathed();
        void AddRange(List<V4_Batch> batches);

        List<string> GetKochIds();


    }

    public class TransGuids
    {
        public List<Guid> transid { get; set; }
    }
}
