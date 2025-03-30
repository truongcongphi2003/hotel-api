using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.TokenDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<string>> Register(RegisterDto registerDto);
        Task<Response<LoginResponseDto>> Login(LoginDto loginDto);
        Task<Response<string>> Logout(string userId);
        Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<Response<string>> ForgotPassword(string email);
        Task<Response<string>> UpdatePassword(UpdatePasswordDto updatePasswordDto);
        Task<Response<bool>> ChangePassword(string id, ChangePasswordDto changePasswordDto);
        Task<Response<string>> ResetPassword(ResetPasswordDto model);
        Task<Response<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto token);
        Task<bool> ValidateUserRole(string userId, string[] roles);

    }
}
