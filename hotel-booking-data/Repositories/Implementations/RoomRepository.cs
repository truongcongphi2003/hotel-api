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
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly HotelContext _context;

        public RoomRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Room> GetAllByHotelId(string hotelId)
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.CancellationPolicies)
                .Where(r => r.RoomType.HotelId == hotelId);
        }

        public async Task<Room> GetById(string id)
        {
            return await _context.Rooms.Include(r => r.CancellationPolicies).Where(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}
