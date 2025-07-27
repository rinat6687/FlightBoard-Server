using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Interfaces;
using FlightBoard.Domain.Services;
using FlightBoard.Application.DTOs;
using FlightBoard.Application.Commands;
using FlightBoard.Application.Events;
using AutoMapper;
using System.Linq.Expressions;



namespace FlightBoard.Application.Services
{
    public class FlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly IFlightEventPublisher _eventPublisher;

        public FlightService(IFlightRepository flightRepository, IMapper mapper, IFlightEventPublisher eventPublisher)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }
    

        public async Task<FlightDto> AddFlightAsync(AddFlightCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.FlightNumber))
                    throw new ArgumentException("Flight Number is required.");
                if (string.IsNullOrWhiteSpace(command.Destination))
                    throw new ArgumentException("Destination is required.");
                if (string.IsNullOrWhiteSpace(command.Gate))
                    throw new ArgumentException("Gate is required.");
                if (command.DepartureTime <= DateTime.UtcNow)
                    throw new ArgumentException("Departure Time must be in the future.");


                bool flightNumberExists = await _flightRepository.ExistsAsync(f => f.FlightNumber == command.FlightNumber);
                if (flightNumberExists)
                    throw new ArgumentException($"Flight Number '{command.FlightNumber}' already exists.");

                var newFlight = _mapper.Map<Flight>(command);
                newFlight.Id = Guid.NewGuid();

                await _flightRepository.AddAsync(newFlight);
                await _flightRepository.SaveChangesAsync();

                var flightDto = _mapper.Map<FlightDto>(newFlight);
                await _eventPublisher.NotifyFlightAddedAsync(flightDto);

                var count = await _flightRepository.CountAsync();
                Console.WriteLine($"Total flights in DB: {count}");

                flightDto.Status = FlightDomainService.CalculateStatus(newFlight.DepartureTime, DateTime.UtcNow);
                return flightDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in AddFlightAsync: {ex.Message}");
                throw;
            }
        }


        public async Task<List<FlightDto>> SearchFlightsAsync(string? status, string? destination, string? flightNumber)
        {
            Expression<Func<Flight, bool>> predicate = f =>
                (string.IsNullOrEmpty(destination) || f.Destination == destination) &&
                (string.IsNullOrEmpty(flightNumber) || f.FlightNumber == flightNumber);

            var flights = await _flightRepository.FindAllAsync(predicate);

            var flightDtos = _mapper.Map<List<FlightDto>>(flights);

            foreach (var dto in flightDtos)
            {
                dto.Status = FlightDomainService.CalculateStatus(dto.DepartureTime, DateTime.UtcNow);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                flightDtos = flightDtos
                    .Where(f => f.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return flightDtos;
        }


        public async Task DeleteFlightAsync(Guid id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);

            if (flight == null)
                throw new KeyNotFoundException($"Flight with ID {id} not found.");

            await _flightRepository.DeleteAsync(id);
            await _flightRepository.SaveChangesAsync();
        }
    }
}