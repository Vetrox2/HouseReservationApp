
using HouseReservationApp.Models;
using HouseReservationApp.Models.DB;
using HouseReservationApp.Models.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HouseReservationApp.Services
{
    public class Repository<T>(HouseReservationContext context) : IRepository<T> where T : class, IEntity
    {
        private readonly HouseReservationContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> DeleteAsync(int id) => await _dbSet.Where(e => e.Id == id).ExecuteDeleteAsync() > 0;

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
       => await _dbSet.AnyAsync(predicate);

        public async Task UpdateAsync(T item)
        {
            _dbSet.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(T item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<T>> GetPaginatedAsync(int page, int pageSize, IQueryable<T>? query = null)
        {
            query ??= _dbSet;

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }
    }
}
