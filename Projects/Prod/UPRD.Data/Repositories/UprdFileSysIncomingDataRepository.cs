using UPRD.Infrastructure;
using UPRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Data.Repositories
{
    public class UprdFileSysIncomingDataRepository : RepositoryBase<FileSysIncomingData>, IUprdFileSysIncomingDataRepository
    {
        public UprdFileSysIncomingDataRepository(IDbFactory dbFactory) : base(dbFactory)
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
    public interface IUprdFileSysIncomingDataRepository : IRepository<FileSysIncomingData>
    {
        void save();
        FileSysIncomingData GetByTransactionId(string transactionId);
        bool AddListOfFileSysIncomingData(List<FileSysIncomingData> listData);
    }
}
