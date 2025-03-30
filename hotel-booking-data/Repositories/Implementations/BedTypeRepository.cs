using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
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
    public class BedTypeRepository : GenericRepository<BedType> , IBedTypeRepository
    {
        private readonly HotelContext _context;
        public BedTypeRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<BedType> GetAllByHotelId(string hotelId)
        {
            return _context.BedTypes.Where(b => b.HotelId == hotelId);
        }
        
        public async Task<BedType> GetById(string id)
        {
            return await _context.BedTypes.FirstOrDefaultAsync(b => b.Id == id);
        }
        public IQueryable<BedType> Search(SearchRequestDto request)
        {
            var query = _context.BedTypes.AsNoTracking();

            // Kiểm tra và áp dụng tìm kiếm trên các trường
            if (!string.IsNullOrEmpty(request.SearchQuery) && request.Fields != null && request.Fields.Any())
            {
                // Tạo predicate ban đầu là true (để OR hoạt động đúng)
                var predicate = PredicateBuilder.New<BedType>(true);

                foreach (var field in request.Fields.Select(f => f.ToLower()))
                {
                    switch (field)
                    {
                        case "bedName":
                            predicate = predicate.Or(a => a.BedName.Contains(request.SearchQuery));
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
