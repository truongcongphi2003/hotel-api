using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoomBooking([FromBody] CreateBookingDto createBookingDto)
        {
            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var response = await _bookingService.CreateRoomBookingAsync(customerId,createBookingDto);
            return StatusCode(response.Code, response);
            //return CreatedAtAction(nameof(GetRoomBookingById), new { id = response.Data?.Id }, response.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerBookings([FromQuery] PagingDto pagingDto, [FromQuery] FilterBookingDto filterBookingDto)
        {

            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _bookingService.GetAllCustomerBookingsAsync(customerId, pagingDto,filterBookingDto);

            return StatusCode(response.Code, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerBookings(string id)
        {
            var response = await _bookingService.GetCustomerBookingByIdAsync(id);

            return StatusCode(response.Code, response);
        }
    }

}
