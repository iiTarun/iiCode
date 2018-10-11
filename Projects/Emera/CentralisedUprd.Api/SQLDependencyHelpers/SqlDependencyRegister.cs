using CentralisedUprd.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);

    public class SqlDependencyRegister
    {
        public event SqlNotificationEventHandler SqlNotification;
        ApplicationLogRepository applogs = new ApplicationLogRepository();
        readonly NotifierEntity notificationEntity;

        private bool IsNomTable { get; set; }

        internal SqlDependencyRegister(NotifierEntity notificationEntity, bool isNomTable)
        {
            IsNomTable = isNomTable;
            this.notificationEntity = notificationEntity;
            RegisterForNotifications(isNomTable);
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        void RegisterForNotifications(bool isNomTable)
        {
            try {
            using (var sqlConnection = new SqlConnection(notificationEntity.SqlConnectionString))
            using (var sqlCommand = new SqlCommand(notificationEntity.SqlQuery, sqlConnection))
            {
                if (isNomTable)
                {
                    sqlCommand.Parameters.Add(notificationEntity.SqlParam, SqlDbType.NVarChar);//("@p__linq__0", SqlDbType.NVarChar);
                    sqlCommand.Parameters[notificationEntity.SqlParam].Value = notificationEntity.SqlParamVal;
                }
                else
                {
                    foreach (var sqlParameter in notificationEntity.SqlParameters)
                        sqlCommand.Parameters.Add(sqlParameter);
                }
                sqlCommand.Notification = null;
                sqlCommand.CommandTimeout = 0;
                var sqlDependency = new SqlDependency(sqlCommand);
                sqlDependency.OnChange += OnSqlDependencyChange;
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();
            }
            }
            catch (Exception ex) {
                applogs.AppLogManager("SqlDependencyRegister", "WatchListAlertJob", "ErrorRegisterForNotifications: "+ex.Message);
            }
        }




        void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            applogs.AppLogManager("SqlDependencyRegister", "WatchListAlertJob", "OnSqlDependencyChange working. i.e. Notification fired by SQL.");
            if (SqlNotification != null)
                SqlNotification(sender, e);
            RegisterForNotifications(IsNomTable);
        }
    }
}
