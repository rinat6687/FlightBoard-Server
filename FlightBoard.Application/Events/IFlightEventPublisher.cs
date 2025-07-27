using FlightBoard.Application.DTOs;

namespace FlightBoard.Application.Events
{
    public interface IFlightEventPublisher
    {
        Task NotifyFlightAddedAsync(FlightDto flight);
    }
}
