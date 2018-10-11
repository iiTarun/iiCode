using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class SQTSPerTransactionRepository : RepositoryBase<SQTSPerTransaction>, ISQTSPerTransactionRepository
    {
        public SQTSPerTransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public bool SaveSqtsList(List<SQTSPerTransaction> sqtsList)
        {
            try
            {
                this.DbContext.SQTSPerTransaction.AddRange(sqtsList);
                this.DbContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public void Sqts2ndScenerio()
        {
            try
            {
                DateTime dateRange = DateTime.Now.AddDays(-60);
                var SQTSItems = this.DbContext.SQTSPerTransaction.Where(a => a.NomTrackingId == "N/A").ToList();
                foreach (var item in SQTSItems)
                {
                    switch (item.ModelType)
                    {
                        case "P": //pathed
                            var Noms = (from noms in this.DbContext.V4_Nomination
                                        join batch in this.DbContext.V4_Batch on noms.TransactionID equals batch.TransactionID
                                        join pipeline in this.DbContext.Pipeline on batch.PipelineDuns equals pipeline.DUNSNo
                                        where noms.PathType == "P"
                                        && (DbFunctions.TruncateTime(batch.FlowStartDate) <= DbFunctions.TruncateTime(item.BeginingDateTime)
                                        && DbFunctions.TruncateTime(batch.FlowEndDate) >= DbFunctions.TruncateTime(item.BeginingDateTime))
                                        && pipeline.DUNSNo == item.TSPCode
                                        && noms.TransactionType == item.TransactionType
                                        && noms.PackageId == item.PackageId
                                        && noms.DeliveryLocationIdentifer == item.DeliveryLocation
                                        && noms.ReceiptLocationIdentifier == item.ReceiptLocation
                                        && noms.ContractNumber == item.ServiceRequestorContract
                                        && batch.StatusID == 7
                                        && batch.SubmitDate >= dateRange
                                        && noms.UpstreamIdentifier == item.UpstreamId
                                        && noms.DownstreamIdentifier == item.DownstreamID
                                        && noms.UpstreamContractIdentifier == item.UpstreamContractIdentifier
                                        && noms.DownstreamContractIdentifier == item.DownstreamContractIdentifier
                                        orderby batch.SubmitDate
                                        select noms.NominatorTrackingId).FirstOrDefault();
                            item.NomTrackingId = Noms != null ? Noms : " N/A ";
                            break;

                        case "T": // threaded
                            var threadedNoms = (from noms in this.DbContext.V4_Nomination
                                                join batch in this.DbContext.V4_Batch on noms.TransactionID equals batch.TransactionID
                                                join pipeline in this.DbContext.Pipeline on batch.PipelineDuns equals pipeline.DUNSNo
                                                where noms.PathType == "T"
                                                && (DbFunctions.TruncateTime(batch.FlowStartDate) <= DbFunctions.TruncateTime(item.BeginingDateTime)
                                                && DbFunctions.TruncateTime(batch.FlowEndDate) >= DbFunctions.TruncateTime(item.BeginingDateTime))
                                                && pipeline.DUNSNo == item.TSPCode
                                                && noms.ContractNumber == item.ServiceRequestorContract
                                                && noms.TransactionType == item.TransactionType
                                                && noms.PackageId == item.PackageId
                                                && noms.DeliveryLocationIdentifer == item.DeliveryLocation
                                                && noms.ReceiptLocationIdentifier == item.ReceiptLocation
                                                && batch.StatusID == 7
                                                && batch.SubmitDate >= dateRange
                                                orderby batch.SubmitDate
                                                select noms.NominatorTrackingId).FirstOrDefault();
                            item.NomTrackingId = threadedNoms != null ? threadedNoms : " N/A ";
                            break;

                        case "U"://un-threaded
                                 //rec
                            if (!string.IsNullOrEmpty(item.ReceiptLocation) && !string.IsNullOrEmpty(item.UpstreamId))
                            {
                                var recNoms = (from noms in this.DbContext.V4_Nomination
                                               join batch in this.DbContext.V4_Batch on noms.TransactionID equals batch.TransactionID
                                               join pipeline in this.DbContext.Pipeline on batch.PipelineDuns equals pipeline.DUNSNo
                                               where noms.PathType == "S"
                                               && (DbFunctions.TruncateTime(batch.FlowStartDate) <= DbFunctions.TruncateTime(item.BeginingDateTime)
                                               && DbFunctions.TruncateTime(batch.FlowEndDate) >= DbFunctions.TruncateTime(item.BeginingDateTime))
                                               && pipeline.DUNSNo == item.TSPCode
                                               && noms.ContractNumber == item.ServiceRequestorContract
                                               && noms.TransactionType == item.TransactionType
                                               && noms.PackageId == item.PackageId
                                               && noms.ReceiptLocationIdentifier == item.ReceiptLocation
                                               && batch.StatusID == 7
                                               && batch.SubmitDate >= dateRange
                                               && noms.UpstreamIdentifier == item.UpstreamId
                                               && noms.UpstreamContractIdentifier == item.UpstreamContractIdentifier
                                               orderby batch.SubmitDate
                                               select noms.NominatorTrackingId).FirstOrDefault();
                                item.NomTrackingId = recNoms != null ? recNoms : " N/A ";

                            }
                            //del
                            else if (!string.IsNullOrEmpty(item.ReceiptLocation) && !string.IsNullOrEmpty(item.UpstreamId))
                            {
                                var delNoms = (from noms in this.DbContext.V4_Nomination
                                               join batch in this.DbContext.V4_Batch on noms.TransactionID equals batch.TransactionID
                                               join pipeline in this.DbContext.Pipeline on batch.PipelineDuns equals pipeline.DUNSNo
                                               where noms.PathType == "M"
                                               && (DbFunctions.TruncateTime(batch.FlowStartDate) <= DbFunctions.TruncateTime(item.BeginingDateTime)
                                               && DbFunctions.TruncateTime(batch.FlowEndDate) >= DbFunctions.TruncateTime(item.BeginingDateTime))
                                               && pipeline.DUNSNo == item.TSPCode
                                               && noms.ContractNumber == item.ServiceRequestorContract
                                               && noms.TransactionType == item.TransactionType
                                               && noms.PackageId == item.PackageId
                                               && noms.DeliveryLocationIdentifer == item.DeliveryLocation
                                               && batch.StatusID == 7
                                               && batch.SubmitDate >= dateRange
                                               && noms.DownstreamIdentifier == item.DownstreamID
                                               && noms.DownstreamContractIdentifier == item.DownstreamContractIdentifier
                                               orderby batch.SubmitDate
                                               select noms.NominatorTrackingId).FirstOrDefault();
                                item.NomTrackingId = delNoms != null ? delNoms : " N/A ";
                            }
                            break;
                        case "N": //
                            break;
                        default:
                            break;
                    }
                }
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            
        }
    }
    public interface ISQTSPerTransactionRepository : IRepository<SQTSPerTransaction>
    {
        bool SaveSqtsList(List<SQTSPerTransaction> sqtsList);
        void Sqts2ndScenerio();
        void Save();
    }
}
