using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_dto;
using hotel_booking_dto.BedTypeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BedTypeController : ControllerBase
    {
        private readonly IBedTypeService _bedTypeService;

        public BedTypeController(IBedTypeService bedTypeService)
        {
            _bedTypeService = bedTypeService;
        }

        [HttpGet]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> GetAllBedType( [FromQuery]PagingDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _bedTypeService.GetAllBedType(hotelId,dto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchRedTypeAsync([FromQuery] SearchRequestDto requestDto)
        {
            var response = await _bedTypeService.SearchBedTypeAsync(requestDto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> GetBedType(string Id)
        {
            var response = await _bedTypeService.GetBedTypeById(Id);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create")]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> CreateBedType([FromBody] BedTypeCreateDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _bedTypeService.CreateBedTypeAsync(hotelId, dto);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> UpdateBedType(string id,[FromBody] BedTypeUpdateDto dto)
        {
            var hotelId = HttpContext.User.FindFirst("HotelId")?.Value;
            var response = await _bedTypeService.UpdateBedTypeAsync(hotelId,id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = Policies.AdminAndManager)]
        public async Task<IActionResult> DeleteBedType(string id)
        {
            var response = await _bedTypeService.DeleteBedTypeAsync(id);
            return StatusCode(response.Code, response);
        }
    }
}
