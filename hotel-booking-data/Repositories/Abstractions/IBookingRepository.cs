using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<bool> CheckBookingByBookingReference(string bookingReference);
        Task<int> GetNoOfRoomAvailable(string roomId, DateTime checkInDate, DateTime checkOutDate);
        Task<Booking> GetById(string id);
        IQueryable<Booking> GetAllCustomerBookings(string customerId, FilterBookingDto filterBookingDto);
        Task<Booking> GetCustomerBookingById(string id);
    }
}
