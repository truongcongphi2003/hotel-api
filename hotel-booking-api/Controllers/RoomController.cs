using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_dto;
using hotel_booking_dto.RoomDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet("read/{id}")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> GetRoom(string Id)
        {
            var response = await _roomService.GetRoomByIdAsync(Id);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody]RoomCreateDto dto)
        {
            var response = await _roomService.CreateRoomAsync(dto);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoom(string id, [FromBody] RoomUpdateDto dto)
        {
            var response = await _roomService.UpdateRoomAsync(id, dto);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var response = await _roomService.DeleteRoomAsync(id);
            return StatusCode(response.Code, response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoom([FromQuery]PagingDto pagingDto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;

            var response = await _roomService.GetAllRoomsAsync(hotelId,pagingDto);
            return StatusCode(response.Code, response);
        }
    }
}
