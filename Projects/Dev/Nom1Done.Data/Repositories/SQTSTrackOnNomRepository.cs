using System;
using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class SQTSTrackOnNomRepository:RepositoryBase<SQTSTrackOnNom>, ISQTSTrackOnNomRepository
    {
        public SQTSTrackOnNomRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public SQTSTrackOnNom GetSqtsOnNomTrackId(Guid? transactionId, string NomTrackId)
        {
            if(transactionId!=null)
                return this.DbContext.SQTSTrackOnNoms.Where(a => a.NomTransactionID == transactionId && a.NomTrackingId == NomTrackId).FirstOrDefault();
            else
                return this.DbContext.SQTSTrackOnNoms.Where(a => a.NomTrackingId == NomTrackId).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface ISQTSTrackOnNomRepository : IRepository<SQTSTrackOnNom>
    {
        void Save();
        SQTSTrackOnNom GetSqtsOnNomTrackId(Guid? transactionId, string NomTrackId);
    }
}
