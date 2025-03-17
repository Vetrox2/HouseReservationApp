namespace HouseReservationApp.Services
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T?> Get(int id);
        Task Update(T item);
        Task<bool> Delete(int id);
    }
}
