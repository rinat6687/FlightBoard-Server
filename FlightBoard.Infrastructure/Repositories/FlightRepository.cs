using FlightBoard.Domain.Entities;
using FlightBoard.Domain.Interfaces;
using FlightBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightBoard.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightDbContext _context;

        public FlightRepository(FlightDbContext context)
        {
            _context = context;
        }


        public async Task<Flight?> GetByIdAsync(Guid id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task AddAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var flight = await GetByIdAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Flights.CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Flight, bool>> predicate)
        {
            return await _context.Flights.AnyAsync(predicate);
        }

        public async Task<List<Flight>> FindAllAsync(Expression<Func<Flight, bool>> predicate = null)
        {
            if (predicate == null)
                return await _context.Flights.ToListAsync();

            return await _context.Flights.Where(predicate).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();


        }
    }
}
