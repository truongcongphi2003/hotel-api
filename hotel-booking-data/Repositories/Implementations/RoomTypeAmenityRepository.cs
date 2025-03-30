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
    public class RoomTypeAmenityRepository : GenericRepository<RoomTypeAmenity>, IRoomTypeAmenityRepository
    {
        private readonly HotelContext _context;
        public RoomTypeAmenityRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<RoomTypeAmenity> GetHotelAmenities()
        {
            return _context.RoomTypeAmenities.Include(h => h.Amenity).AsNoTracking();
        }

        public async Task<RoomTypeAmenity> CheckRoomTypeAmenityAsync(string roomTypeId, string amenityId)
        {
            return await _context.RoomTypeAmenities.Where(h => h.RoomTypeId == roomTypeId && h.AmenityId == amenityId).FirstOrDefaultAsync();
        }
    }
}
