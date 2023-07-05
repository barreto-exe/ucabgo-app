using Microsoft.AspNetCore.SignalR.Client;
using UcabGo.App.Utils;

namespace UcabGo.App.Api.Services.SignalR
{
    public class HubConnectionFactory : IHubConnectionFactory
    {
        private readonly Dictionary<string, HubConnection> _connections = new();

        public HubConnection GetHubConnection(string hubName)
        {
            var api = EnviromentVariables.GetValue("ApiUrl");

            if (!_connections.ContainsKey(hubName))
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl($"{api}hubs/{hubName}")
                    .WithAutomaticReconnect()
                    .Build();

                _connections.Add(hubName, connection);
            }

            return _connections[hubName];
        }
    }
}
