using Nom1Done.DTO;
using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class SQTSOPPerTransactionRepository : RepositoryBase<SQTSOPPerTransaction>, ISQTSOPPerTransactionRepository
    {
        public SQTSOPPerTransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<SQTSOPPerTransactionDTO> GetSqtsForOperator(int month, string pipeDuns, string shipperCompanyDuns, bool ByLoc, bool showZero)
        {
            List<SQTSOPPerTransactionDTO> list = new List<SQTSOPPerTransactionDTO>();
            try
            {
                if (!showZero)
                {
                    var shipperCompDunsList = shipperCompanyDuns.Split(',');
                    var year = DateTime.Now.Year;
                    var data = (from sqop in DbContext.SQTSOPPerTransaction
                                where sqop.PreparerID == pipeDuns
                                && shipperCompanyDuns.Contains(sqop.Statement_ReceipentID)
                                && sqop.EffectiveStartDate.Month == month
                                && sqop.EffectiveStartDate.Year == year
                                select new SQTSOPPerTransactionDTO
                                {
                                    ConfirmationRole = sqop.ConfirmationRole,
                                    ConfirmationSusequenceCycleIndicator = sqop.ConfirmationSusequenceCycleIndicator,
                                    ConfirmationTrackingID = sqop.ConfirmationTrackingID,
                                    ConfirmationUserData1 = sqop.ConfirmationUserData1,
                                    ConfirmationUserData2 = sqop.ConfirmationUserData2,
                                    ContractualFLowIndicator = sqop.ContractualFLowIndicator,
                                    CycleIndicator = sqop.CycleIndicator,
                                    DownPkgId = sqop.DownPkgId,
                                    DownstreamParty = sqop.DownstreamParty,
                                    DwnStreamShipperContract = sqop.DwnStreamShipperContract,
                                    EffectiveEndDate = sqop.EffectiveEndDate,
                                    EffectiveStartDate = sqop.EffectiveStartDate,
                                    Id = sqop.Id,
                                    Location = sqop.Location,
                                    LocationCapacityFlowIndicator = sqop.LocationCapacityFlowIndicator,
                                    LocationNetCapacity = sqop.LocationNetCapacity,
                                    PackageId = sqop.PackageId,
                                    PkgId = sqop.PackageId,
                                    PreparerID = sqop.PreparerID,
                                    Quantity = sqop.Quantity,
                                    ReductionQuantity = sqop.ReductionQuantity,
                                    ReductionReason = sqop.ReductionReason,
                                    SchedulingStatus = sqop.SchedulingStatus,
                                    ServiceContract = sqop.ServiceContract,
                                    ServiceIdentifierCode = sqop.ServiceIdentifierCode,
                                    ServiceRequester = sqop.ServiceRequester,
                                    ServiceRequesterContract = sqop.ServiceRequesterContract,
                                    StatementDate = sqop.StatementDate,
                                    Statement_ReceipentID = sqop.Statement_ReceipentID,
                                    TransactionId = sqop.TransactionId,
                                    UpstreamParty = sqop.UpstreamParty,
                                    UpstrmPkgId = sqop.UpstrmPkgId,
                                    UpstrmShipperContract = sqop.UpstrmShipperContract
                                }).OrderByDescending(a=>a.StatementDate).ToList();
                    if (data != null && data.Count > 0)
                        list = data;
                }
                else
                {
                    var shipperCompDunsList = shipperCompanyDuns.Split(',');
                    var year = DateTime.Now.Year;
                    var data = (from sqop in DbContext.SQTSOPPerTransaction
                                where sqop.PreparerID == pipeDuns
                                && shipperCompanyDuns.Contains(sqop.Statement_ReceipentID)
                                && sqop.EffectiveStartDate.Month == month
                                && sqop.EffectiveStartDate.Year == year
                                && sqop.Quantity != 0
                                select new SQTSOPPerTransactionDTO
                                {
                                    ConfirmationRole = sqop.ConfirmationRole,
                                    ConfirmationSusequenceCycleIndicator = sqop.ConfirmationSusequenceCycleIndicator,
                                    ConfirmationTrackingID = sqop.ConfirmationTrackingID,
                                    ConfirmationUserData1 = sqop.ConfirmationUserData1,
                                    ConfirmationUserData2 = sqop.ConfirmationUserData2,
                                    ContractualFLowIndicator = sqop.ContractualFLowIndicator,
                                    CycleIndicator = sqop.CycleIndicator,
                                    DownPkgId = sqop.DownPkgId,
                                    DownstreamParty = sqop.DownstreamParty,
                                    DwnStreamShipperContract = sqop.DwnStreamShipperContract,
                                    EffectiveEndDate = sqop.EffectiveEndDate,
                                    EffectiveStartDate = sqop.EffectiveStartDate,
                                    Id = sqop.Id,
                                    Location = sqop.Location,
                                    LocationCapacityFlowIndicator = sqop.LocationCapacityFlowIndicator,
                                    LocationNetCapacity = sqop.LocationNetCapacity,
                                    PackageId = sqop.PackageId,
                                    PkgId = sqop.PackageId,
                                    PreparerID = sqop.PreparerID,
                                    Quantity = sqop.Quantity,
                                    ReductionQuantity = sqop.ReductionQuantity,
                                    ReductionReason = sqop.ReductionReason,
                                    SchedulingStatus = sqop.SchedulingStatus,
                                    ServiceContract = sqop.ServiceContract,
                                    ServiceIdentifierCode = sqop.ServiceIdentifierCode,
                                    ServiceRequester = sqop.ServiceRequester,
                                    ServiceRequesterContract = sqop.ServiceRequesterContract,
                                    StatementDate = sqop.StatementDate,
                                    Statement_ReceipentID = sqop.Statement_ReceipentID,
                                    TransactionId = sqop.TransactionId,
                                    UpstreamParty = sqop.UpstreamParty,
                                    UpstrmPkgId = sqop.UpstrmPkgId,
                                    UpstrmShipperContract = sqop.UpstrmShipperContract
                                }).OrderByDescending(a=>a.StatementDate).ToList();
                    if (data != null && data.Count > 0)
                        list = data;
                }
                return list;
            }catch (Exception ex)
            {
                return list;
            }
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public bool SaveSqtsList(List<SQTSOPPerTransaction> sqtsList)
        {
            try
            {
                this.DbContext.SQTSOPPerTransaction.AddRange(sqtsList);
                this.DbContext.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
    public interface ISQTSOPPerTransactionRepository : IRepository<SQTSOPPerTransaction>
    {
        bool SaveSqtsList(List<SQTSOPPerTransaction> sqtsList);
        void Save();
        List<SQTSOPPerTransactionDTO> GetSqtsForOperator(int month, string pipeDuns, string shipperCompanyDuns, bool ByLoc, bool showZero);
    }
}
