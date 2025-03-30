using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IBookingService
    {
        Task<Response<BookingResponseDto>> CreateRoomBookingAsync(string? customerId, CreateBookingDto dto);
        Task<Response<PageResult<IEnumerable<GetAllBookingDto>>>> GetAllCustomerBookingsAsync(string customerId, PagingDto pagingDto, FilterBookingDto filterBookingDto);
        Task<Response<GetBookingDto>> GetCustomerBookingByIdAsync(string id);
    }
}
