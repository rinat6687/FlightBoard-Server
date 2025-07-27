
using FlightBoard.Domain.Services;

namespace FlightBoard.Tests.Application.Services
{
    public class FlightStatusCalculatorTests
    {
        [Fact]
        public void CalculateStatus_FutureDepartureTime_ReturnsScheduled()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime.AddMinutes(31);
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Scheduled", result);
        }

        [Fact]
        public void CalculateStatus_30MinutesBeforeDeparture_ReturnsBoarding()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime.AddMinutes(30);
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Boarding", result);
        }

        [Fact]
        public void CalculateStatus_1MinuteBeforeDeparture_ReturnsBoarding()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime.AddMinutes(1);
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Boarding", result);
        }

        [Fact]
        public void CalculateStatus_AtDepartureTime_ReturnsDeparted()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime;
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Departed", result);
        }

        [Fact]
        public void CalculateStatus_30MinutesAfterDeparture_ReturnsDeparted()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime.AddMinutes(-30);
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Departed", result);
        }

        [Fact]
        public void CalculateStatus_61MinutesAfterDeparture_ReturnsLanded()
        {
            var currentTime = DateTime.UtcNow;
            var departureTime = currentTime.AddMinutes(-61);
            var result = FlightDomainService.CalculateStatus(departureTime, currentTime);

            Assert.Equal("Landed", result);
        }

    }
}
