namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
