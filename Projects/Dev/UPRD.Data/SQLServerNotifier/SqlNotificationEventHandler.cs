using System.Data.SqlClient;

namespace UPRD.Data.SQLServerNotifier
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}
