using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;
using System;
using Nom1Done.DTO;

namespace Nom1Done.Data.Repositories
{
    public class NominationStatusRepository : RepositoryBase<NominationStatu>, INominationStatusRepository
    {
        public NominationStatusRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public NominationStatu GetByTransactionId(Guid transactionId)
        {
            return this.DbContext.NominationStatus.Where(a => a.NOM_ID == transactionId).FirstOrDefault();
        }

        public NominationStatu GetNomStatusOnReferenceNumber(string referenceNumber)
        {
            return this.DbContext.NominationStatus.Where(a => a.ReferenceNumber == referenceNumber).FirstOrDefault();
        }

        public NominationStatu GetRejectedAndErroredNomStatus(Guid transactionId)
        {
            return this.DbContext.NominationStatus.Where(a => a.NOM_ID == transactionId && (a.StatusID == (int)NomStatus.Rejected || a.StatusID == (int)NomStatus.Error)).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface INominationStatusRepository:IRepository<NominationStatu>
    {
        void Save();
        NominationStatu GetNomStatusOnReferenceNumber(string referenceNumber);
        NominationStatu GetByTransactionId(Guid transactionId);
        NominationStatu GetRejectedAndErroredNomStatus(Guid transactionId);
    }
}
