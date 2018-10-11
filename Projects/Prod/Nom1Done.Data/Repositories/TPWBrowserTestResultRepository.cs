using Nom1Done.Model;
using Nom1Done.Infrastructure;

namespace Nom1Done.Data.Repositories
{
    public class TPWBrowserTestResultRepository : RepositoryBase<TPWBrowserTestResult>, ITPWBrowserTestResultRepository
    {
        public TPWBrowserTestResultRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface ITPWBrowserTestResultRepository:IRepository<TPWBrowserTestResult>
    {

    }
}
