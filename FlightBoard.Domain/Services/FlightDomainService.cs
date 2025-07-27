namespace FlightBoard.Domain.Services
{
    public class FlightDomainService
    {

        public static string CalculateStatus(DateTime departureTime, DateTime currentTime)
        {
            TimeSpan timeToDeparture = departureTime - currentTime;

            if (timeToDeparture.TotalMinutes <= -60) 
            {
                return "Landed";
            }
            else if (timeToDeparture.TotalMinutes <= 0)
            {
                return "Departed";
            }
            else if (timeToDeparture.TotalMinutes <= 30)
            {
                return "Boarding";
            }
            else
            {
                return "Scheduled";
            }
        }


    }
}
