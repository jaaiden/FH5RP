using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using FH5RP.Data;

namespace FH5RP.Hubs
{
    public class TelemetryDataHub : Hub
    {
        public async Task SendUpdate (TelemetryData Data)
        {
            Console.WriteLine($"[TelemetryData] Sending data update: {Data}");
            await Clients.All.SendAsync("ReceiveUpdate", Data);
        }
    }
}
