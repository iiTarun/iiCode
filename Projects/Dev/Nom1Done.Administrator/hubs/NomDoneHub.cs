using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UPRD.Data.SQLServerNotifier;

namespace Nom1Done.Admin.hubs
{
    public class NomDoneHub : Hub
    {
        internal NotifierEntity NotifierEntity { get; private set; }

        public void DispatchToClient()
        {
            Clients.All.broadcastMessage("Refresh");
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void InitializeNomTable(String value)
        {
            NotifierEntity = NotifierEntity.FromJson(value);
            if (NotifierEntity == null)
                return;
            Action<String> dispatcher = (t) => { DispatchToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher);
        }


    }
}