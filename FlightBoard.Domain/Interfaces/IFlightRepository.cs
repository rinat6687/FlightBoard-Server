using FlightBoard.Domain.Entities;
using System.Linq.Expressions;

namespace FlightBoard.Domain.Interfaces
{
    public interface IFlightRepository
    {

        Task<Flight?> GetByIdAsync(Guid id);
        Task AddAsync(Flight flight);
        Task DeleteAsync(Guid id);
        Task<List<Flight>> FindAllAsync(Expression<Func<Flight, bool>> predicate = null);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(Expression<Func<Flight, bool>> predicate);
        Task SaveChangesAsync();

    }
}
