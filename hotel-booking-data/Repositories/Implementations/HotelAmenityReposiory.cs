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
    public class HotelAmenityReposiory : GenericRepository<HotelAmenity>, IHotelAmenityRepository
    {
        private readonly HotelContext _context;

        public HotelAmenityReposiory(HotelContext context) : base(context)
        {

            _context = context;
        }

        public IQueryable<HotelAmenity> GetHotelAmenities()
        {
            return _context.HotelAmenities.Include(h => h.Amenity).AsNoTracking();
        }
       

        public async Task<HotelAmenity> CheckHotelAmenityAsync(string hotelId, string amenityId)
        {
            return await _context.HotelAmenities.Where(h => h.HotelId == hotelId && h.AmenityId == amenityId).FirstOrDefaultAsync();
        }
    }
}

