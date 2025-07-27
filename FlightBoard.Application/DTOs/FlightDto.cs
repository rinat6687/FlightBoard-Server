namespace FlightBoard.Application.DTOs
{
    public class FlightDto
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public string Gate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
