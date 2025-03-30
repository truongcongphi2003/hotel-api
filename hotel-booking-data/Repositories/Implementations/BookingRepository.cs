using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly HotelContext _context;
        public BookingRepository(HotelContext context) : base(context) {
            _context = context;
        }
        public async Task<bool> CheckBookingByBookingReference(string bookingReference)
        {
            return await _context.Bookings.AnyAsync(bk => bk.BookingReference == bookingReference);
        }
        public async Task<int> GetNoOfRoomAvailable(string roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var existingBookings = await _context.Bookings
                .Where(bk => bk.RoomId == roomId &&
                             !(bk.CheckOutDate <= checkInDate || bk.CheckInDate >= checkOutDate))
                .ToListAsync();

            var room = await _context.Rooms.Include(r => r.RoomType).FirstAsync(r => r.Id == roomId);
            int totalRooms = room.RoomType.RoomCount;

            int roomsBooked = existingBookings.Sum(bk => bk.QuantityRoom);

            return totalRooms - roomsBooked;
        }
        public async Task<Booking> GetById(string id)
        {
            return await _context.Bookings.FirstAsync(rb => rb.Id == id);
        }
        public async Task<Booking> GetCustomerBookingById(string id)
        {
            return await _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                .Include(b => b.Hotel)
                .OrderByDescending(b => b.CreatedAt)
                .FirstAsync(rb => rb.Id == id);
        }
        public IQueryable<Booking> GetAllCustomerBookings(string customerId, FilterBookingDto filterBookingDto)
        {
            var query = _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                .Include(b => b.Hotel)
                .AsNoTracking()
                .OrderByDescending(b => b.CreatedAt)

                .Where(b => b.CustomerId == customerId);


            // lọc theo khoảng thời gian
            if (filterBookingDto.StartDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt >= filterBookingDto.StartDate.Value);
            }

            if (filterBookingDto.EndDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt <= filterBookingDto.EndDate.Value);
            }

            // lọc theo tháng và năm
            if (filterBookingDto.Month.HasValue && filterBookingDto.Year.HasValue)
            {
                query = query.Where(b => b.CreatedAt.Month == filterBookingDto.Month.Value &&
                                         b.CreatedAt.Year == filterBookingDto.Year.Value);
            }
            else if (filterBookingDto.Year.HasValue)
            {
                query = query.Where(b => b.CreatedAt.Year == filterBookingDto.Year.Value);
            }
            if (filterBookingDto.PaymentStatus?.Any() == true)
            {
                var statusList = filterBookingDto.PaymentStatus.Select(s => s.ToLower()).ToList();
                query = query.Where(b => statusList.Contains(b.Payment.Status.ToLower()));
            }


            return query;
        }

    }
}
