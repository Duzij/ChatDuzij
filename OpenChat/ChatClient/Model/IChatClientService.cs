using Microsoft.AspNet.SignalR.Client;

namespace OpenChatClient
{
    public interface IChatClientService
    {
        IHubProxy chatProxy { get; set; }
        HubConnection connection { get; set; }
        string Server { get; set; }

        void Dispose();
    }
}