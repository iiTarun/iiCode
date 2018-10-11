using Nom1Done.Infrastructure;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class FileSysIncomingDataRepository : RepositoryBase<FileSysIncomingData>, IFileSysIncomingDataRepository
    {
        public FileSysIncomingDataRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool AddListOfFileSysIncomingData(List<FileSysIncomingData> listData)
        {
            try
            {
                this.DbContext.FileSysIncomingData.AddRange(listData);
                this.DbContext.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }

        }

        public FileSysIncomingData GetByTransactionId(string transactionId)
        {
            return this.DbContext.FileSysIncomingData.Where(a => a.MessageId == transactionId).FirstOrDefault();
        }

        public void save()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IFileSysIncomingDataRepository : IRepository<FileSysIncomingData>
    {
        void save();
        FileSysIncomingData GetByTransactionId(string transactionId);
        bool AddListOfFileSysIncomingData(List<FileSysIncomingData> listData);
    }
}
