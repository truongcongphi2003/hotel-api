using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
    {
        private readonly HotelContext _context;

        public ManagerRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Manager> GetManagerStatistics(string managerId)
        {
            var manager = await _context.Managers.Where(x => x.AppUserId == managerId).FirstOrDefaultAsync();
            return manager;
        }

        public async Task<Manager> GetManagerAsync(string managerId)
        {
            return await _context.Managers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUserId == managerId);
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsForManagerAsync(string managerId)
        {
            var query = await _context.Managers.AsNoTracking()
                .Where(mg => mg.AppUserId == managerId)
                .Include(mg => mg.Hotels).ThenInclude(h => h.Reviews)
                .Include(mg => mg.Hotels).ThenInclude(h => h.Images)
                .FirstOrDefaultAsync();
            return query != null ? query.Hotels : throw new ArgumentException("Manager does not exist");
        }


        public async Task<bool> AddManagerAsync(Manager entity)
        {
            var manager = await _context.Managers.Where(x => x.AppUserId == entity.AppUserId)
                .FirstOrDefaultAsync();
            return true;
        }

        public async Task<Manager> GetAppUserByEmail(string email)
        {
            var checkDatabase = await _context.Managers
                .Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUser.Email == email);

            return checkDatabase;
        }

        public IQueryable<Manager> GetHotelManagersAsync()
        {
            var managers = _context.Managers
                .Include(x => x.AppUser)
                .Include(x => x.Hotels)
                .ThenInclude(x => x.Bookings)
                .ThenInclude(x => x.Payment);
            return managers;
        }
    }
}
