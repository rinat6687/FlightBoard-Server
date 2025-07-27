using FlightBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightBoard.Infrastructure.Data
{
    public class FlightDbContext : DbContext
    {

        public DbSet<Flight> Flights => Set<Flight>();

        public FlightDbContext(DbContextOptions<FlightDbContext> options)
            : base(options) { }
    }
}
