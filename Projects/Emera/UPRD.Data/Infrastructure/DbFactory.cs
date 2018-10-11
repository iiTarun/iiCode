using UPRD.Data;

namespace UPRD.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        UPRDEntities dbContext;

        public UPRDEntities Init()
        {
            return dbContext ?? (dbContext = new UPRDEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
