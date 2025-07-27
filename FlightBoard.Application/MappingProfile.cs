using AutoMapper;
using FlightBoard.Application.Commands;
using FlightBoard.Application.DTOs;
using FlightBoard.Domain.Entities;

namespace FlightBoard.Application
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<FlightDto, Flight>();
            CreateMap<AddFlightCommand, Flight>();
            CreateMap<Flight, FlightDto>();
        }
    }
}
