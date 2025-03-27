using HouseReservation.Core.Models.Interfaces;
using HouseReservation.Core.Utilities;
using System.Linq.Expressions;

namespace HouseReservation.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T item);
        Task<PagedResult<T>> GetPaginatedAsync(int page, int pageSize, IQueryable<T>? query = null);
    }
}
