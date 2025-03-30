using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto.ManagerRequestDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpPost("aprove-request")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> ApproveManagerRequest([FromBody] string email)
        {
            var result = await _managerService.ApproveManagerRequestAsync(email);
            return StatusCode(result.Code, result);
        }

        [HttpPost("request")]
        public async Task<IActionResult> AddManagerRequest([FromBody] AddManagerRequestDto managerRequestDto)
        {
            var newManagerRequest = await _managerService.AddManagerRequestAsync(managerRequestDto);
            return StatusCode(newManagerRequest.Code, newManagerRequest);
        }

        [HttpGet("all-request")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> GetAllRequests([FromQuery] PagingDto paging)
        {
            var getAll = await _managerService.GetAllManagerRequest(paging);

            return Ok(getAll);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterManagerDto registerDto)
        {
            var result = await _managerService.RegisterManagerAsync(registerDto);
            return StatusCode(result.Code, result);
        }
        [HttpGet("validate-email")]
        public async Task<IActionResult> TokenExpiring([FromQuery]string email,[FromQuery] string token)
        {
            var confirmToken = await _managerService.CheckTokenExpiring(email, token);
            if(!confirmToken.success)
                return BadRequest("Token không hợp lệ hoặc đã hết hạn.");
            
            return Redirect($"register?token={token}");
        }

        [HttpGet("send-invite")]
        public async Task<IActionResult> SendManagerInvite(string email)
        {
            var result = await _managerService.SendManagerInvite(email);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> GetAllHotelManagers([FromQuery] PagingDto paging)
        {
            var response = await _managerService.GetAllHotelManagersAsync(paging);
            return StatusCode(response.Code, response);
        }

      
    }
}
