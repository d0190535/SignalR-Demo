using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalrExample.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> connections = new Dictionary<string, string>();

        public void SendWelcomeMessage()
        {
            Clients.Caller.sendWelcomeMessage();
        }

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, HttpUtility.HtmlEncode(message));
        }

        public override Task OnConnected()
        {
            var name = Context.QueryString["name"];
            var connectionId = Context.ConnectionId;

            AddNewConnection(connectionId, name);
            Clients.Others.joined(connectionId, name);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = Context.QueryString["name"];
            RemoveExistingConnection(Context.ConnectionId);
            Clients.Others.left(Context.ConnectionId, name);
            return base.OnDisconnected(stopCalled);
        }

        private void AddNewConnection(string connectionId, string connectionName)
        {
            connections.Add(connectionId, connectionName);
        }

        private void RemoveExistingConnection(string connectionId)
        {
            connections.Remove(connectionId);
        }
    }
}