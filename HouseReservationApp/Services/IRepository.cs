using HouseReservationApp.Models.DB.Entities;
using System.Linq.Expressions;

namespace HouseReservationApp.Services
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T item);
    }
}
