using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Data.Repositories
{
    public class IncomingDataRepository : RepositoryBase<IncomingData>, IIncomingDataRepository
    {
        public IncomingDataRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public List<IncomingData> Get50UnprocessedFiles()
        {
            var InFileList = this.DbContext.IncomingDatas.Where(a => a.IsProcessed == false).Take(50).ToList();
            if (InFileList != null && InFileList.Count > 0)
                foreach (var item in InFileList)
                {
                    item.IsProcessed = true;
                }
            this.DbContext.SaveChanges();
            return InFileList;
        }

        public IncomingData GetByTransactionId(string transactionId)
        {
            return this.DbContext.IncomingDatas.Where(a => a.MessageId == transactionId).FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }
    }

    public interface IIncomingDataRepository : IRepository<IncomingData>
    {
        void Save();
        IncomingData GetByTransactionId(string transactionId);
        List<IncomingData> Get50UnprocessedFiles();
    }
}
