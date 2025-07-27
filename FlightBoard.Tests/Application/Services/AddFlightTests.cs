
using Moq;
using AutoMapper;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Interfaces;
using FlightBoard.Domain.Entities;
using FlightBoard.Application.DTOs;
using FlightBoard.Application.Commands;
using FlightBoard.Application.Events;
using System.Linq.Expressions;

namespace FlightBoard.Tests
{
    public class AddFlightTests
    {
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFlightEventPublisher> _eventPublisherMock;
        private readonly FlightService _flightService;

        public AddFlightTests()
        {
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _mapperMock = new Mock<IMapper>();
            _eventPublisherMock = new Mock<IFlightEventPublisher>();

            _flightService = new FlightService(
                _flightRepositoryMock.Object,
                _mapperMock.Object,
                _eventPublisherMock.Object);
        }

        [Fact]
        public async Task AddFlightAsync_ValidCommand_AddsFlightSuccessfully()
        {

            var command = new AddFlightCommand
            {
                FlightNumber = "AA123",
                Destination = "Tel Aviv",
                Gate = "B12",
                DepartureTime = DateTime.UtcNow.AddHours(1)
            };

            var flightEntity = new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = command.FlightNumber,
                Destination = command.Destination,
                Gate = command.Gate,
                DepartureTime = command.DepartureTime
            };

            var flightDto = new FlightDto
            {
                Id = flightEntity.Id,
                FlightNumber = flightEntity.FlightNumber,
                Destination = flightEntity.Destination,
                Gate = flightEntity.Gate,
                DepartureTime = flightEntity.DepartureTime,
                Status = "Scheduled"
            };

            _flightRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Flight, bool>>>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Flight>(command))
                .Returns(flightEntity);

            _mapperMock.Setup(m => m.Map<FlightDto>(It.IsAny<Flight>()))
                .Returns(flightDto);

            _flightRepositoryMock.Setup(r => r.AddAsync(flightEntity))
                .Returns(Task.CompletedTask);

            _flightRepositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _eventPublisherMock.Setup(e => e.NotifyFlightAddedAsync(flightDto))
                .Returns(Task.CompletedTask);

            _flightRepositoryMock.Setup(r => r.CountAsync())
                .ReturnsAsync(1);


            var result = await _flightService.AddFlightAsync(command);


            Assert.NotNull(result);
            Assert.Equal("Scheduled", result.Status);
            Assert.Equal(command.FlightNumber, result.FlightNumber);

            _flightRepositoryMock.Verify(r => r.AddAsync(flightEntity), Times.Once);
            _flightRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _eventPublisherMock.Verify(e => e.NotifyFlightAddedAsync(flightDto), Times.Once);
        }

        [Fact]
        public async Task AddFlightAsync_MissingFlightNumber_ThrowsArgumentException()
        {

            var command = new AddFlightCommand
            {
                FlightNumber = "",  // Missing flight number
                Destination = "Tel Aviv",
                Gate = "B12",
                DepartureTime = DateTime.UtcNow.AddHours(1)
            };


            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _flightService.AddFlightAsync(command));
            Assert.Equal("Flight Number is required.", ex.Message);
        }

        [Fact]
        public async Task AddFlightAsync_FlightNumberAlreadyExists_ThrowsArgumentException()
        {

            var command = new AddFlightCommand
            {
                FlightNumber = "AA123",
                Destination = "Tel Aviv",
                Gate = "B12",
                DepartureTime = DateTime.UtcNow.AddHours(1)
            };

            _flightRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Flight, bool>>>()))
                .ReturnsAsync(true); // Flight number already exists


            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _flightService.AddFlightAsync(command));
            Assert.Equal($"Flight Number '{command.FlightNumber}' already exists.", ex.Message);
        }
    }
}
