using Microsoft.AspNetCore.SignalR.Client;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.Api.Services.SignalR
{
    public class HubConnectionFactory : IHubConnectionFactory
    {
        private readonly Dictionary<string, HubConnection> _connections = new();
        private readonly ISettingsService settings;
        public HubConnectionFactory(ISettingsService settings)
        {
            this.settings = settings;
        }

        public HubConnection GetHubConnection(string hubName)
        {
            var apiUrl = EnviromentVariables.GetValue("ApiUrl");

            if (!_connections.ContainsKey(hubName))
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl(apiUrl, options => 
                    {
                        options.AccessTokenProvider = () => Task.FromResult(settings.AccessToken);
                        options.Headers.Add("HubName", hubName);
                    })
                    .WithAutomaticReconnect()
                    .Build();

                _connections.Add(hubName, connection);
            }

            return _connections[hubName];
        }
    }
}
