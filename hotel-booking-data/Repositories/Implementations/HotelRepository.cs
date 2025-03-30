using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HotelContext _context;

        public HotelRepository(HotelContext context) : base(context)
        {
            _context = context;
        }
        
        public IQueryable<Hotel> GetAll()
        {
            return _context.Hotels
                .Include(h => h.HotelAmenities)
                    .ThenInclude(a => a.Amenity)
                .Include(h => h.RoomTypes)
                .Include(h => h.Images)
                .Include(h => h.Reviews)
                .AsNoTracking();
        }

        public IQueryable<Hotel> GetByLocation(LocationRequestDto requestDto)
        {
            var query = _context.Hotels
                .Include(h => h.Ward)
                .Include(h => h.Images)
                .Include(h => h.Reviews)
                .Include(h => h.RoomTypes)
                    .ThenInclude(rt => rt.Rooms)
                .AsQueryable();

            if (!string.IsNullOrEmpty(requestDto.Code) && !string.IsNullOrEmpty(requestDto.Type))
            {
                switch (requestDto.Type.ToLower())
                {
                    case "province":
                        query = query.Where(h => h.ProvinceCode == requestDto.Code);
                        break;
                    case "district":
                        query = query.Where(h => h.DistrictCode == requestDto.Code);
                        break;
                    case "ward":
                        query = query.Where(h => h.WardCode == requestDto.Code);
                        break;
                }
            }

            return query.AsNoTracking();
        }
        public async Task<Hotel> GetHotelByIdAsync(string id)
        {
            return await _context.Hotels
                .Include(h => h.Ward)
                .Include(h => h.Province)
                .Include(h => h.District)
                //.Include(h => h.Images)
                //.Include(h => h.Reviews)
                .Include(h => h.RoomTypes)
                    .ThenInclude(rt => rt.Rooms)
                    .AsNoTracking()
                .FirstAsync();
        }


        public async Task<Hotel> GetByIdAsync(string id)
        {
            return await _context.Hotels
                .Include(h => h.HotelAmenities)
                    .ThenInclude(a => a.Amenity)
                .Include(h => h.RoomTypes)
                .Include(h => h.Images)
                .Include(h => h.Reviews)
                .Include(h => h.Province)
                .Include(h => h.District)
                .Include(h => h.Ward)
                .AsSingleQuery()
                .FirstAsync(h => h.Id == id);
        }
        public async Task<IEnumerable<Image>> GetImages(string id)
        {
            return await _context.Images
                .Where(h => h.HotelId == id)
                .OrderBy(h => h.RoomTypeImages.Any() ? 1 : 0)
                .ToListAsync();
        }


        public async Task<bool> CheckHotelManager(string hotelId, string managerId)
        {
            return _context.Hotels.Any(h => h.Id == hotelId && h.ManagerId == managerId); 
        }

    }
}
