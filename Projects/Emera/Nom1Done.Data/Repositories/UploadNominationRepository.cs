using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Nom1Done.Data.Repositories
{
    public class UploadNominationRepository : RepositoryBase<UploadedFile>, IUploadedNominationRepository
    {
       
        public UploadNominationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<UploadedFile> GetUplodedFilesOnUserId(string UserId)
        {
            return this.DbContext.UploadedFiles.Where(a => a.AddedBy == UserId).ToList();
        }

        public void SaveChages()
        {
            this.DbContext.SaveChanges();
        }
    }
    public interface IUploadedNominationRepository : IRepository<UploadedFile>
    {
        void SaveChages();
        List<UploadedFile> GetUplodedFilesOnUserId(string UserId);
    }
}
