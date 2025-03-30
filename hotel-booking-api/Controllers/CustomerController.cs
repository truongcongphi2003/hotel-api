using hotel_booking_core.Interfaces;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto;
using hotel_booking_models.Cloudinary;
using hotel_booking_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        //private readonly IBookingService _bookingService;
        //private readonly IWishListService _wishListService;
        private readonly ILogger<CustomerController> _logger;
        private readonly UserManager<AppUser> _userManager;
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, UserManager<AppUser> userManager)
        {
            _customerService = customerService;
            //_bookingService = bookingService;
            //_wishListService = wishListService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = Policies.Customer)]
        public async Task<ActionResult<Response<string>>> Update([FromBody] UpdateCustomerDto model)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation($"Update Attempt for user with id = {userId}");
            var result = await _customerService.UpdateCustomer(userId, model);
            return StatusCode(result.Code, result);
        }


        [HttpPatch("update-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateImage([FromForm] AddImageDto imageDto)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation($"Update Image Attempt for user with id = {userId}");
            var result = await _customerService.UpdatePhoto(imageDto, userId);
            return StatusCode(result.Code, result);
        }

        //[HttpGet("bookings")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Policy = Policies.ManagerAndCustomer)]
        //public async Task<IActionResult> GetCustomerBooking([FromQuery] PagingDto paging)
        //{
        //    string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        //    var result = await _bookingService.GetCustomerBookings(userId, paging);
        //    return StatusCode(result.StatusCode, result);
        //}

        [HttpGet("AllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> GetAllCustomersAsync([FromQuery] PagingDto pagenator)
        {
            var result = await _customerService.GetAllCustomersAsync(pagenator);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = Policies.Customer)]
        public async Task<IActionResult> GetCustomerDetailsAsync()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _customerService.GetCustomerDetails(customerId);
            return StatusCode(result.Code, result);
        }
        //[HttpGet("wishlist")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Policy = Policies.Customer)]
        //public async Task<IActionResult> GetCustomerWishList([FromQuery] PagingDto paging)
        //{
        //    string customerId = _userManager.GetUserId(User);
        //    _logger.LogInformation($"Retrieving the paginated wishlist for the customer with ID {customerId}");
        //    var result = await _customerService.GetCustomerWishList(customerId, paging);
        //    _logger.LogInformation($"Retrieved the paginated wishlist for the customer with ID {customerId}");
        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpDelete("clear-wishlist")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Policy = Policies.Customer)]
        //public async Task<IActionResult> ClearWishList()
        //{
        //    string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        //    var result = await _wishListService.ClearWishList(userId);
        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpPost("{hotelId}/add-wishlist")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Policy = Policies.Customer)]
        //public async Task<IActionResult> AddToWishlist([FromRoute] string hotelId)
        //{
        //    string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        //    var result = await _wishListService.AddToWishList(hotelId, userId);
        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpDelete("{hotelId}/remove-wishlist")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Policy = Policies.Customer)]
        //public async Task<IActionResult> RemoveFromWishList([FromRoute] string hotelId)
        //{
        //    string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        //    var result = await _wishListService.RemoveFromWishList(hotelId, userId);
        //    return StatusCode(result.StatusCode, result);
        //}


    }
}
