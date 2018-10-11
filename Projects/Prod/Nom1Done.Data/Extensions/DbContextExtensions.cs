using Nom1Done.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Nom1Done.Data
{
    public static class DbContextExtensions
    {
        public static ObjectContext UnderlyingContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }

        public static NotifierEntity GetNotifierEntity<TEntity>(this DbContext dbContext, IQueryable iQueryable) where TEntity : EntityBase
        {          
            var objectQuery = dbContext.GetObjectQuery<TEntity>(iQueryable);
            var notifier= new NotifierEntity()
            {
                SqlQuery = objectQuery.ToTraceString(),
                SqlConnectionString = objectQuery.SqlConnectionString(),
                SqlParameters = objectQuery.SqlParameters()
            };
            return notifier;
        }


        public static NotifierEntity GetNotifierEntityForNoms(this DbContext dbContext, IQueryable<V4_Batch> iQueryable,string userId) 
        {
            IQueryable<V4_Batch> iQueryable1 = AddUserIdInQuery(iQueryable, userId);
            var objectQuery = GetObjectQueryForNom(dbContext,iQueryable1);
            var notifier = new NotifierEntity()
            {
                SqlQuery = objectQuery.ToTraceString(),
                SqlConnectionString = objectQuery.SqlConnectionString(),
                SqlParamVal = objectQuery.SqlParameters().FirstOrDefault().Value.ToString(),
                SqlParam= objectQuery.SqlParameters().FirstOrDefault().ParameterName,
                // SqlParameters = objectQuery.SqlParameters()
            };
            return notifier;
        }


        public static ObjectQuery GetObjectQueryForNom(this DbContext dbContext, IQueryable<V4_Batch> query) 
        {
            if (query is ObjectQuery)
                return query as ObjectQuery;

            if (dbContext == null)
                throw new ArgumentException("dbContext cannot be null");

            var objectSet = dbContext.UnderlyingContext().CreateObjectSet<V4_Batch>();
            var iQueryProvider = ((IQueryable)objectSet).Provider;

            // Use the provider and expression to create the ObjectQuery.
            return (ObjectQuery)iQueryProvider.CreateQuery(query.Expression);
        }

        public static IQueryable<V4_Batch> AddUserIdInQuery(IQueryable<V4_Batch> query, string userId) 
        {
            var query1 = query.Where(x => x.CreatedBy == userId);
            return query1;         
            
        }

        public static ObjectQuery GetObjectQuery<TEntity>(this DbContext dbContext, IQueryable query) where TEntity : EntityBase
        {
            if (query is ObjectQuery)
                return query as ObjectQuery;

            if (dbContext == null)
                throw new ArgumentException("dbContext cannot be null");

            var objectSet = dbContext.UnderlyingContext().CreateObjectSet<TEntity>();
            var iQueryProvider = ((IQueryable)objectSet).Provider;

            // Use the provider and expression to create the ObjectQuery.
            return (ObjectQuery)iQueryProvider.CreateQuery(query.Expression);
        }
    }
}