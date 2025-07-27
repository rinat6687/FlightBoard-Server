using Microsoft.AspNetCore.SignalR;
using FlightBoard.API.Hubs;
using FlightBoard.Application.DTOs;
using FlightBoard.Application.Events;


namespace FlightBoard.API.SignalR
{

    public class FlightEventPublisher : IFlightEventPublisher
    {
        private readonly IHubContext<FlightHub> _hubContext;

        public FlightEventPublisher(IHubContext<FlightHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyFlightAddedAsync(FlightDto flight)
        {
            return _hubContext.Clients.All.SendAsync("FlightAdded", flight);
        }
    }

}
