using Microsoft.AspNetCore.SignalR;

namespace FlightBoard.API.Hubs;

public class FlightHub : Hub
{
    public async Task SendFlightAdded(string message)
    {
        await Clients.All.SendAsync("FlightAdded", message);
    }
}