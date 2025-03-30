using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.HotelAmenityDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ImageDtos;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IAmenityService _amenityService;
        
        public HotelController(IHotelService hotelService, IAmenityService amenityService)
        {
            _hotelService = hotelService;
            _amenityService = amenityService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchHotel([FromQuery]SearchHotelDto searchHotelDto)
        {
            return Ok();
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetHotel(string id)
        {
            var response = await _hotelService.GetDetailAsync(id);
            return StatusCode(response.Code, response);
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetHotelsByLocation([FromQuery] LocationRequestDto locationRequestDto, [FromQuery] PagingDto pagingDto )
        {
            var response = await _hotelService.GetHotelsByLocation(locationRequestDto, pagingDto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetHotelImages(string id)
        {
            var response = await _hotelService.GetHotelImagesAsync(id);
            return StatusCode(response.Code, response);
        }
        [HttpGet("amenities/{id}")]
        public async Task<IActionResult> GetHotelAmenities(string id,[FromQuery] SearchRequestDto searchRequestDto,[FromQuery] PagingDto pagingDto)
        {
            var response = await _amenityService.GetAllAmenitiesByHotelIdAsync(id, searchRequestDto, pagingDto);
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetHotel()
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.GetHotelAsync(hotelId);
            return StatusCode(response.Code, response);
        }
        [HttpGet("images")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetHotelImages()
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.GetHotelImages(hotelId);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto dto)
        {
            var managerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _hotelService.CreateHotelAsync(managerId, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPatch("update")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UpdateHotel([FromForm] UpdateHotelDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.UpdateHotelAsync(hotelId, dto);

            return StatusCode(response.Code, response);
        }

        [HttpPost("upload-thumbnail")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UploadThumbnail([FromForm]AddThumbnailDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.UpdateHotelThumbnail(hotelId, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPatch("images/update")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UploadImages([FromForm]AddImageDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.UpdateHotelImages(hotelId, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPost("update-amenities")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UpdateHotelAmenities([FromBody]HotelAmenityUpdateDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _hotelService.UpdateHotelAmenities(hotelId, dto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("amenities")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetAllAmenities([FromQuery] SearchRequestDto requestDto, [FromQuery] PagingDto pagingDto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _amenityService.GetAllAmenitiesByHotelIdAsync(hotelId, requestDto, pagingDto);
            return StatusCode(response.Code, response);
        }

    }
}
