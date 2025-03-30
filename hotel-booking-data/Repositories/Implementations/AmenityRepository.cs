using Azure.Core;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly HotelContext _context;
        public AmenityRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Amenity> GetByIdAsync(string id)
        {
            var amenity = await _context.Amenities.FirstOrDefaultAsync(a => a.Id == id);
            return amenity;
        }

        public IQueryable<Amenity> GetAll(SearchRequestDto request)
        {
            var query = _context.Amenities.AsNoTracking();
            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields != null && request.Fields.Any())
            {
                var predicate = PredicateBuilder.New<Amenity>(true);

                if (request.Fields.Contains("name", StringComparer.OrdinalIgnoreCase))
                {
                    predicate = predicate.Or(a => a.Name.Contains(request.SearchQuery));
                }

                if (request.Fields.Contains("createdat", StringComparer.OrdinalIgnoreCase) &&
                    DateTime.TryParse(request.SearchQuery, out DateTime dateValue))
                {
                    predicate = predicate.Or(a => a.CreatedAt.Date == dateValue.Date);
                }

                query = query.Where(predicate);
            }
            var parentIds = query.Where(a => a.ParentId != null).Select(a => a.ParentId);
            var parentAmenities = _context.Amenities.Where(a => parentIds.Any(pid => pid == a.Id));
            return query.Union(parentAmenities);
        }
        public IQueryable<Amenity> GetAllHotelAmenities(string hotelId, SearchRequestDto request)
        {
            var query = _context.Amenities
                .Where(a => a.HotelAmenities.Any(ha => ha.HotelId == hotelId));

            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields?.Any() == true)
            {
                var predicate = PredicateBuilder.New<Amenity>(true);

                if (request.Fields.Contains("name", StringComparer.OrdinalIgnoreCase))
                {
                    predicate = predicate.Or(a => a.Name.Contains(request.SearchQuery));
                }

                if (request.Fields.Contains("createdat", StringComparer.OrdinalIgnoreCase) &&
                    DateTime.TryParse(request.SearchQuery, out DateTime dateValue))
                {
                    predicate = predicate.Or(a => a.CreatedAt.Date == dateValue.Date);
                }

                query = query.Where(predicate);
            }

            var parentIds = query.Where(a => a.ParentId != null).Select(a => a.ParentId);
            var parentAmenities = _context.Amenities.Where(a => parentIds.Any(pid => pid == a.Id));

            return query.Union(parentAmenities).AsNoTracking();
        }


        public IQueryable<Amenity> GetAllRoomTypeAmenities(string roomTypeId)
        {
            var query = _context.Amenities
                .Include(a => a.RoomTypeAmenities)  
                .Where(a => a.RoomTypeAmenities.Any(ha => ha.RoomTypeId == roomTypeId))
                .AsNoTracking();
            var parentIds = query.Where(a => a.ParentId != null).Select(a => a.ParentId).Distinct();
            var parentAmenities = _context.Amenities.Include(a => a.RoomTypeAmenities).Where(a => parentIds.Contains(a.Id));

            return query.Union(parentAmenities).AsNoTracking();
        }
        public IQueryable<Amenity> Search(SearchRequestDto request)
        {
            var query = _context.Amenities.AsNoTracking();

            // Kiểm tra và áp dụng tìm kiếm trên các trường
            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields != null && request.Fields.Any())
            {
                // Tạo predicate ban đầu là true (để OR hoạt động đúng)
                var predicate = PredicateBuilder.New<Amenity>(true);

                foreach (var field in request.Fields.Select(f => f.ToLower()))
                {
                    switch (field)
                    {
                        case "name":
                            predicate = predicate.Or(a => a.Name.Contains(request.SearchQuery));
                            break;
                        case "createdat":
                            if (DateTime.TryParse(request.SearchQuery, out DateTime dateValue))
                            {
                                predicate = predicate.Or(a => a.CreatedAt.Date == dateValue.Date);
                            }
                            break;
                        default:
                            break;
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }
    }
}

