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
    public class ReviewRepository : GenericRepository<Review>,IReviewRepository
    {
        private HotelContext _context;
        public ReviewRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Review> GetAllReviewsByHotel(string hotelId)
        {
            return _context.Reviews.AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.Hotel)
                .Include(r => r.Customer)
                .OrderBy(r => r.CreatedAt);
        } 

        public async Task<Review> GetById(string reviewId)
        {
            return await _context.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId);
        }
        public async Task<double> GetAverageRatingByHotelId(string hotelId)
        {
            return await _context.Reviews.AverageAsync(r => r.Rating);
        }
    }
}
