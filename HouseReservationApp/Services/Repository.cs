
using HouseReservationApp.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace HouseReservationApp.Services
{
    public class Repository<T>(HouseReservationContext context) : IRepository<T> where T : class
    {
        private readonly HouseReservationContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> Delete(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item is null) return false;

            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T?> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return _dbSet;
        }

        public async Task Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
