using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace hotel_booking_data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelContext _context;
        private readonly DbSet<T> _dbSet;
       

        public GenericRepository(HotelContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
           

        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
             _dbSet.Update(entity);
        }
    }
}
