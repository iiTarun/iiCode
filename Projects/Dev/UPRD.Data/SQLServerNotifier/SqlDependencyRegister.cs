using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.Data.SQLServerNotifier
{
    public class SqlDependencyRegister
    {
        public event SqlNotificationEventHandler SqlNotification;        

        readonly NotifierEntity notificationEntity;

        private bool IsNomTable { get; set; }

        internal SqlDependencyRegister(NotifierEntity notificationEntity)
        {
            this.notificationEntity = notificationEntity;
            RegisterForNotifications();
        }
               


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        void RegisterForNotifications()
        {
           
            using (var sqlConnection = new SqlConnection(notificationEntity.SqlConnectionString))
            using (var sqlCommand = new SqlCommand(notificationEntity.SqlQuery, sqlConnection))
            {
                    foreach (var sqlParameter in notificationEntity.SqlParameters)
                        sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.Notification = null;
                sqlCommand.CommandTimeout = 0;             
                var sqlDependency = new SqlDependency(sqlCommand);
                sqlDependency.OnChange += OnSqlDependencyChange;
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
               
                sqlCommand.ExecuteNonQuery();
            }
        }




        void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (SqlNotification != null)
                SqlNotification(sender, e);
            RegisterForNotifications();
        }
    }
}