using FlightBoard.Application.Commands;
using FlightBoard.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightBoard.API.Controllers
{

    [ApiController]
    [Route("api/flights")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _service;

        public FlightsController(FlightService service)
        {
            _service = service;
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchFlights([FromQuery] string? status, [FromQuery] string? destination, [FromQuery] string? flightNumber)
        {
            var flights = await _service.SearchFlightsAsync(status, destination, flightNumber);
            return Ok(flights);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFlight([FromBody] AddFlightCommand command)
        {
            var flights = await _service.AddFlightAsync(command);
            return Ok(flights);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFlight([FromRoute] Guid id)
        {
            await _service.DeleteFlightAsync(id);
            return Ok();
        }

    }
}
