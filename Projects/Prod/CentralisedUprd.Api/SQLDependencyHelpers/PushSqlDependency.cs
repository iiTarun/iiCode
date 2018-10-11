using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api
{
    public class PushSqlDependency
    {
        static Dictionary<string, PushSqlDependency> instance = new Dictionary<string, PushSqlDependency>();
        readonly SqlDependencyRegister sqlDependencyNotifier;
        readonly Action<String> dispatcher;

        public static PushSqlDependency Instance(NotifierEntity notifierEntity, Action<String> dispatcher, bool isNomTable)
        {
            if (isNomTable)
            {
                var userId = notifierEntity.SqlParamVal;
                if (instance.ContainsKey(userId))
                {
                    PushSqlDependency sqlInstancne;
                    if (instance.TryGetValue(userId, out sqlInstancne))
                    {
                        return sqlInstancne;
                    }
                    else
                    {
                        instance.Remove(userId);
                        var newSqlInstance1 = new PushSqlDependency(notifierEntity, dispatcher, isNomTable);
                        instance.Add(userId, newSqlInstance1);
                        return newSqlInstance1;
                    }
                }
                else
                {
                    var newSqlInstance = new PushSqlDependency(notifierEntity, dispatcher, isNomTable);
                    instance.Add(userId, newSqlInstance);
                    return newSqlInstance;
                }
            }
            else
            {
                return new PushSqlDependency(notifierEntity, dispatcher, isNomTable);
            }
        }

        private PushSqlDependency(NotifierEntity notifierEntity, Action<String> dispatcher, bool isNomTable)
        {
            this.dispatcher = dispatcher;
            sqlDependencyNotifier = new SqlDependencyRegister(notifierEntity, isNomTable);
            sqlDependencyNotifier.SqlNotification += OnSqlDependencyNotifierResultChanged;
        }

        internal void OnSqlDependencyNotifierResultChanged(object sender, SqlNotificationEventArgs e)
        {
            dispatcher("Refresh");
        }
    }

}
