namespace FlightBoard.Application.Commands
{
    public class AddFlightCommand
    {

        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public string Gate { get; set; }
        public DateTime DepartureTime { get; set; }


    }
}
