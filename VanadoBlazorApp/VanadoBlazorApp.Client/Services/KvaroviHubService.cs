using Microsoft.AspNetCore.SignalR.Client;
using VanadoShared.Model;

namespace VanadoBlazor.Services
{
    public class KvaroviHubService
    {
        private HubConnection _hubConnection;

        public event Action<Kvar>? OnKvarDodan;

        public async Task ConnectAsync()
        {
            if (_hubConnection != null)
                return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7076/kvaroviHub") 
                .WithAutomaticReconnect()
            .Build();

            _hubConnection.On<Kvar>("KvarDodan", (kvar) =>
            {
                Console.WriteLine("Primljen kvar: " + kvar?.Opis);
                OnKvarDodan?.Invoke(kvar);
            });

            await _hubConnection.StartAsync();
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
                await _hubConnection.StopAsync();
        }
    }
}
