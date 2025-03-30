using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly HotelContext _context;

        public RoomTypeRepository(HotelContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RoomType>> SearchAvailableRooms(SearchHotelDto searchDto)
        {
            var availableRoomTypes = await _context.RoomTypes
                .Where(rt => rt.HotelId == searchDto.Code &&
                             rt.RoomCount > _context.Bookings
                                .Where(b =>
                                    b.Room.RoomTypeId == rt.Id &&
                                    ((searchDto.CheckInDate >= b.CheckInDate && searchDto.CheckInDate < b.CheckOutDate) ||
                                     (searchDto.CheckOutDate > b.CheckInDate && searchDto.CheckOutDate <= b.CheckOutDate) ||
                                     (searchDto.CheckInDate <= b.CheckInDate && searchDto.CheckOutDate >= b.CheckOutDate))
                                )
                                .Sum(b => b.QuantityRoom)
                )
                .Include(rt => rt.RoomTypeImages)
                    .ThenInclude(v => v.Image)
                .Include(rt => rt.RoomTypeBedTypes)
                    .ThenInclude(v => v.BedType)
                .Include(rt => rt.Rooms)
                    .ThenInclude(r => r.CancellationPolicies)
                .AsNoTracking()
                .ToListAsync();

            return availableRoomTypes.Where(rt =>
                rt.MaxAdults >= searchDto.Adults &&
                rt.MaxChildren >= searchDto.Children
            );
        }


        public IQueryable<RoomType> GetAllHotelRoomType(string hotelId, SearchRequestDto request)
        {
            var query = _context.RoomTypes
                .Include(rt => rt.RoomTypeBedTypes)
                    .ThenInclude(v => v.BedType)
                .Include(rt => rt.RoomTypeImages)
                    .ThenInclude(v => v.Image)
                .Where(r => r.HotelId == hotelId);
            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields != null && request.Fields.Any())
            {
                var predicate = PredicateBuilder.New<RoomType>(true);

                if (request.Fields.Contains("name", StringComparer.OrdinalIgnoreCase))
                {
                    predicate = predicate.Or(a => a.Name.Contains(request.SearchQuery));
                }

                query = query.Where(predicate);
            }
            return query;
        }

        public IQueryable<RoomType> GetAllRoomTypeRooms(string hotelId)
        {
            return _context.RoomTypes
                .Include(rt => rt.Rooms)
                    .ThenInclude(r => r.CancellationPolicies)
                .Where(rt => rt.HotelId == hotelId);
                    ;
        }

        public async Task<RoomType> GetRoomTypeById(string id)
        {
            return await _context.RoomTypes
                .Include(rt => rt.RoomTypeAmenities)
                .Include(rt=> rt.Hotel)
                    .ThenInclude(h => h.Images)
                .Include(rt => rt.RoomTypeBedTypes)
                    .ThenInclude(v => v.BedType)
                .Include(rt => rt.RoomTypeImages)
                    .ThenInclude(v => v.Image)
                .FirstAsync(r => r.Id == id);
        }
        public IQueryable<RoomType> Search(SearchRequestDto request)
        {
            var query = _context.RoomTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields?.Any() == true)
            {
                var predicate = PredicateBuilder.New<RoomType>(true);

                if (request.Fields.Contains("name", StringComparer.OrdinalIgnoreCase))
                {
                    predicate = predicate.Or(a => a.Name.Contains(request.SearchQuery));
                }

                //if (request.Fields.Contains("createdat", StringComparer.OrdinalIgnoreCase) &&
                //    DateTime.TryParse(request.SearchQuery, out DateTime dateValue))
                //{
                //    predicate = predicate.Or(a => a.CreatedAt.Date == dateValue.Date);
                //}

                query = query.Where(predicate);
            }


            return query;
        }

    }
}
