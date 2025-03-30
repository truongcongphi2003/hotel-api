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
    public class RoomTypeImageRepository : GenericRepository<RoomTypeImage>, IRoomTypeImageRepository
    {
        private readonly HotelContext _context;

        public RoomTypeImageRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<RoomTypeImage> GetAllByRoomTypeId(string roomTypeId)
        {
            return _context.RoomTypeImages
                .Include(r => r.Image)
                .Include(r => r.RoomType)
                .Where(r => r.RoomTypeId == roomTypeId)
                .AsNoTracking();
        } 
    }
}
