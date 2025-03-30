using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.TokenDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hotel_booking_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<LoginResponseDto>>> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            return StatusCode(result.Code, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            return StatusCode(result.Code, result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không tìm thấy thông tin người dùng");

            var result = await _authService.Logout(userId);

            return StatusCode(result.Code, result);
        }
        [HttpGet("confirm-email")]
        public async Task<ActionResult<Response<string>>> ConfirmEmail([FromQuery] ConfirmEmailDto confirmEmailDto)
        {
            var result = await _authService.ConfirmEmail(confirmEmailDto);
            return StatusCode(result.Code, result);
        }


        [HttpPatch("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPassword(resetPasswordDto);
            return StatusCode(result.Code, result);
        }

        [Authorize]
        [HttpPatch("update-password")]
        public async Task<ActionResult<Response<string>>> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var result = await _authService.UpdatePassword(updatePasswordDto);
            return StatusCode(result.Code, result);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword([FromBody] ForgotPassworDto forgotPassworDto)
        {
            var result = await _authService.ForgotPassword(forgotPassworDto.Email);
            return StatusCode(result.Code, result);
        }

        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _authService.ChangePassword(userId, changePasswordDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<Response<RefreshTokenResponseDto>>> RefreshToken([FromQuery] RefreshTokenRequestDto model)
        {
            var result = await _authService.RefreshToken(model);
            return StatusCode(result.Code, result);
        }

        [HttpPost("validate-user")]
        [Authorize]
        public async Task<IActionResult> ValidateUserRole([FromBody] ValidateUserRoleDto userRoleDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.ValidateUserRole(userId, userRoleDto.Roles);
            return result ? Ok() : BadRequest();
        }
    }
}
