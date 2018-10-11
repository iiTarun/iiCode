using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api
{
    public static class ObjectQueryExtensions
    {
        public static String SqlString(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            return objectQuery.ToTraceString();
        }

        public static String SqlConnectionString(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var dbConnection = objectQuery.Context.Connection;
            return ((EntityConnection)dbConnection).StoreConnection.ConnectionString;
        }

        public static ICollection<SqlParameter> SqlParameters(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var collection = new List<SqlParameter>();
            foreach (ObjectParameter parameter in objectQuery.Parameters)
                collection.Add(new SqlParameter(parameter.Name, parameter.Value));
            return collection;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlCommand SqlCommand(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var sqlCommand = new SqlCommand(objectQuery.SqlConnectionString(), new SqlConnection(objectQuery.SqlConnectionString()));
            foreach (ObjectParameter parameter in objectQuery.Parameters)
                sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value);

            return sqlCommand;
        }
    }
}