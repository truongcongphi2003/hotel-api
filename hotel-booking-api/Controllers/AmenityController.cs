using hotel_booking_core.Interfaces;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService Amenity)
        {
            _amenityService = Amenity;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] SearchRequestDto searchRequestDto, [FromQuery]PagingDto pagingDto)
        {
            var response = await _amenityService.GetAllAmenitiesAsync(searchRequestDto, pagingDto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchAmenitiesAsync([FromQuery] SearchRequestDto requestDto)
        {
            var response = await _amenityService.SearchAmenitiesAsync(requestDto);
            return StatusCode(response.Code, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _amenityService.GetAmenityByIdAsync(id);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AmenityCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _amenityService.CreateAmenityAsync(dto);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AmenityUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _amenityService.UpdateAmenityAsync(id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _amenityService.DeleteAmenityAsync(id);
            return StatusCode(response.Code, response);
        }
    }
}
