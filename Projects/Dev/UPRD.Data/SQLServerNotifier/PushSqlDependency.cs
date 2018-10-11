using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UPRD.Data.SQLServerNotifier
{
    public class PushSqlDependency
    {
        static Dictionary<string,PushSqlDependency> instance = new Dictionary<string, PushSqlDependency>();
        readonly SqlDependencyRegister sqlDependencyNotifier;
        readonly Action<String> dispatcher;

        public static PushSqlDependency Instance(NotifierEntity notifierEntity, Action<String> dispatcher)
        {
       
                return new PushSqlDependency(notifierEntity, dispatcher);       
        }

        private PushSqlDependency(NotifierEntity notifierEntity, Action<String> dispatcher)
        {
            this.dispatcher = dispatcher;
            sqlDependencyNotifier = new SqlDependencyRegister(notifierEntity);
            sqlDependencyNotifier.SqlNotification += OnSqlDependencyNotifierResultChanged;
        }

        internal void OnSqlDependencyNotifierResultChanged(object sender, SqlNotificationEventArgs e)
        {
            dispatcher("Refresh");
        }
    }
}
