using Microsoft.AspNetCore.SignalR.Client;

namespace UcabGo.App.Api.Services.SignalR
{
    public interface IHubConnectionFactory
    {
        HubConnection GetHubConnection(string hubName);
    }
}
