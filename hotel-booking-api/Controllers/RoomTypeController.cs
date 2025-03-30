using hotel_booking_dto.RoomTypeDtos;
using hotel_booking_dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_dto.ImageDtos;
using hotel_booking_dto.HotelAmenityDtos;
using hotel_booking_dto.RoomTypeAmenityDtos;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IAmenityService _amenityService;

        public RoomTypeController(IRoomTypeService roomTypeService, IAmenityService amenityService)
        {
            _roomTypeService = roomTypeService;
            _amenityService = amenityService;
        }
        [HttpGet("room/search")]
        public async Task<IActionResult> SearchAvailableRooms([FromQuery] SearchHotelDto searchHotelDto)
        {
            var response = await _roomTypeService.SearchAvailableRooms(searchHotelDto);
            return StatusCode(response.Code, response);
        }
        [HttpGet]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetAllRoomType([FromQuery] SearchRequestDto searchRequestDto, [FromQuery] PagingDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _roomTypeService.GetAllRoomTypes(hotelId, searchRequestDto, dto);
            return StatusCode(response.Code, response);
        }

        [HttpGet("read/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetRoomType(string Id)
        {
            var response = await _roomTypeService.GetRoomTypeById(Id);
            return StatusCode(response.Code, response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchRoomTypeAsync([FromQuery] SearchRequestDto requestDto)
        {
            var response = await _roomTypeService.SearchRoomTypeAsync(requestDto);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> CreateRoomType([FromForm] RoomTypeCreateDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _roomTypeService.CreateRoomTypeAsync(hotelId, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPatch("update/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UpdateRoomType(string id,[FromForm] RoomTypeUpdateDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _roomTypeService.UpdateRoomTypeAsync(hotelId, id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> DeleteRoomType(string id)
        {
            var response = await _roomTypeService.DeleteRoomTypeAsync(id);
            return StatusCode(response.Code, response);
        }

        [HttpPost("upload-thumbnail/{id}")]
        [Authorize(Policy = Policies.Manager)]

        public async Task<IActionResult> UploadThumbnail(string id, [FromForm] AddThumbnailDto dto)
        {
            var response = await _roomTypeService.UpdateRoomTypeThumbnail(id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPost("upload-images/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UploadImages(string id, [FromForm] AddImageDto dto)
        {
            var response = await _roomTypeService.UpdateRoomTypeImages(id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpGet("images/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetImageRoomType(string Id)
        {
            var response = await _roomTypeService.GetImageRoomTypeAsync(Id);
            return StatusCode(response.Code, response);
        }

        [HttpGet("all-images/{id}")]
        public async Task<IActionResult> GetImages(string Id)
        {
            var response = await _roomTypeService.GetImageListAsync(Id);
            return StatusCode(response.Code, response);
        }

        [HttpPost("update-amenities/{id}")]
        public async Task<IActionResult> UpdateRoomTypeAmenities(string id, [FromBody] RoomTypeAmenityUpdateDto dto)
        {
            var response = await _roomTypeService.UpdateRoomTypeAmenities(id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpGet("amenities/{id}")]
        public async Task<IActionResult> GetAllAmenities(string id, [FromQuery] PagingDto pagingDto)
        {
            var response = await _amenityService.GetAllAmenitiesByRoomTypeIdAsync(id, pagingDto);
            return StatusCode(response.Code, response);
        }
    }
}
