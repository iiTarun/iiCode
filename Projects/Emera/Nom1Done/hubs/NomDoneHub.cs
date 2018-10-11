using Microsoft.AspNet.SignalR;
using Nom1Done.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Nom1Done.Hubs
{
    public class NomDoneHub : Hub
    {
        internal NotifierEntity NotifierEntity { get; private set; }

        public void DispatchToClient()
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext();
            // hubContext.Clients.All.refreshPage();
            // Clients.All.broadcastMessage("Refresh");
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string name = identity.Claims.Where(c => c.Type == "UserId")
                                        .Select(c => c.Value).SingleOrDefault();
            Clients.Group(name).broadcastMessage("Refresh");
        }

        public override Task OnConnected()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string name = identity.Claims.Where(c => c.Type == "UserId")
                                        .Select(c => c.Value).SingleOrDefault();            

            Groups.Add(Context.ConnectionId, name);

            return base.OnConnected();
        }

        public void Initialize(String value)
        {
            NotifierEntity = NotifierEntity.FromJson(value);
            if (NotifierEntity == null)
                return;
            Action<String> dispatcher = (t) => { DispatchToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher,true);
        }

        public void InitializeNomTable(String value)
        {
            NotifierEntity = NotifierEntity.FromJson(value);
            if (NotifierEntity == null)
                return;
            Action<String> dispatcher = (t) => { DispatchToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher,true);
        }

        public void DispatchUprdStatusToClient()
        {
            Clients.All.updateUprdStatus("Refresh");
        }
        public void InitializeUprdStatus(string value)
        {
            NotifierEntity = NotifierEntity.FromJson(value);
            if (NotifierEntity == null)
                return;
            Action<String> dispatcher = (t) => { DispatchUprdStatusToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher,false);
        }
    }
}