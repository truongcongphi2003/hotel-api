using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("provinces/search")]
        public async Task<IActionResult> GetProvinces([FromQuery] SearchRequestDto searchRequestDto)
        {
            var response = await _addressService.SearchProvincesAsync(searchRequestDto);
            return Ok(response);
        }

        [HttpGet("districts/search")]
        public async Task<IActionResult> GetDistricts([FromQuery] SearchRequestDto searchRequestDto)
        {
            var result = await _addressService.SearchDistrictsByProvinceAsync(searchRequestDto);
            return Ok(result);
        }

        [HttpGet("wards/search")]
        public async Task<IActionResult> GetWards([FromQuery] SearchRequestDto searchRequestDto)
        {
            var result = await _addressService.SearchWardsByDistrictAsync(searchRequestDto);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetLocations([FromQuery] string keyword)
        {
            var result = await _addressService.SearchLocationsAsync(keyword);
            return Ok(result);
        }
    }
}
