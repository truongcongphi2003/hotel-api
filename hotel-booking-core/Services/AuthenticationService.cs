using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.TokenDtos;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities;
using hotel_booking_utilities.EmailBodyHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace hotel_booking_core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly ITokenRepository _tokenRepository;
        private readonly HotelContext _dbContext;

        public AuthenticationService(HotelContext context,UserManager<AppUser> userManager, IMapper mapper, IMailService mailService, IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger, ITokenGeneratorService tokenGeneratorService, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mailService = mailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _tokenGeneratorService = tokenGeneratorService;
            _tokenRepository = tokenRepository;
            _dbContext = context;
        }

        // Đăng ký người dùng mới và gửi liên kết xác nhận đến email của người dùng
        //public async Task<Response<string>> Register(RegisterDto registerDto)
        //{
        //    var user = _mapper.Map<AppUser>(registerDto);
        //    user.IsActive = true;

        //    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        var result = await _userManager.CreateAsync(user, registerDto.Password);
        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(user, UserRoles.Customer);
        //            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            var encodedToken = TokenConverter.EncodeToken(token);
        //            var userRole = await _userManager.GetRolesAsync(user);
        //            var mailBody = await EmailBodyBuilder.GetEmailBody(user, userRole.ToList(), emailTempPath: "StaticFiles/Html/ConfirmEmail.html", linkName: "ConfirmEmail", encodedToken, controllerName: "Authencation");
        //            var mailRequest = new MailRequest()
        //            {
        //                Subject = "Xác nhận đăng ký của bạn",
        //                Body = mailBody,
        //                ToEmail = registerDto.Email,
        //            };

        //            bool emailResult = await _mailService.SendEmailAsync(mailRequest);
        //            if (emailResult)
        //            {
        //                _logger.LogInformation("Gửi mail thành công");
        //                var customer = new Customer
        //                {
        //                    AppUser = user,
        //                };
        //                await _unitOfWork.Customers.InsertAsync(customer);
        //                await _unitOfWork.Save();

        //                transaction.Complete();
        //                return Response<string>.Success("Tạo tài khoản thành công! Vui lòng kiểm tra email để xác minh tài khoản của bạn", user.Id, (int)HttpStatusCode.Created);
        //            }
        //            _logger.LogInformation("Dịch vụ email thất bại");
        //            transaction.Dispose();
        //            return Response<string>.Fail("Đăng ký thất bại, Vui lòng thử lại");
        //        }

        //        transaction.Complete();
        //        return Response<string>.Fail(GetErrors(result), (int)HttpStatusCode.BadRequest);
        //    }
        //}

        public async Task<Response<string>> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<AppUser>(registerDto);
            user.IsActive = true;
            user.UserName = registerDto.Email;
            user.CreatedAt = DateTime.UtcNow;

            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (!result.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return Response<string>.Fail(GetErrors(result), (int)HttpStatusCode.BadRequest);
                    }

                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = TokenConverter.EncodeToken(token);
                    var userRole = await _userManager.GetRolesAsync(user);
                    var mailBody = await EmailBodyBuilder.GetEmailBody(user, userRole.ToList(),
                        emailTempPath: "StaticFiles/Html/ConfirmEmail.html",
                        linkName: "ConfirmEmail",
                        encodedToken,
                        controllerName: "Authentication");
                    var mailRequest = new MailRequest()
                    {
                        Subject = "Xác nhận đăng ký của bạn",
                        Body = mailBody,
                        ToEmail = registerDto.Email,
                    };

                    bool emailResult = await _mailService.SendEmailAsync(mailRequest);
                    if (!emailResult)
                    {
                        _logger.LogInformation("Dịch vụ email thất bại");
                        await transaction.RollbackAsync();
                        return Response<string>.Fail("Đăng ký thất bại, Vui lòng thử lại");
                    }

                    _logger.LogInformation("Gửi mail thành công");
                    var customer = new Customer
                    {
                        AppUser = user,
                    };
                    await _unitOfWork.Customers.InsertAsync(customer);
                    await _unitOfWork.Save();

                    await transaction.CommitAsync();
                    return Response<string>.Success("Tạo tài khoản thành công! Vui lòng kiểm tra email để xác minh tài khoản của bạn",user.Id,(int)HttpStatusCode.Created);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi trong quá trình đăng ký");
                    return Response<string>.Fail("Đã xảy ra lỗi hệ thống, vui lòng thử lại sau", (int)HttpStatusCode.InternalServerError);
                }
            });
        }

        //xác nhận email của user đã đăng ký
        public async Task<Response<string>> ConfirmEmail(ConfirmEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Response<string>.Fail("Người dùng không tồn tại", (int)HttpStatusCode.NotFound);

            var decodedToken = TokenConverter.DecodeToken(dto.Token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
                return Response<string>.Success("Xác nhận email thành công", user.Id, (int)HttpStatusCode.OK);

            return Response<string>.Fail(GetErrors(result), (int)HttpStatusCode.BadRequest);
        }

        //Đăng nhập
        public async Task<Response<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var validityResult = await ValidateUser(loginDto);
            if (!validityResult.success)
                return Response<LoginResponseDto>.Fail("Đăng nhập thất bại", validityResult.Code);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var refreshToken = _tokenGeneratorService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var result = new LoginResponseDto()
            {
                Id = user.Id,
                Token = await _tokenGeneratorService.GenerateToken(user),
                Email = user.Email,
                RefreshToken = refreshToken,
                Avatar = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles,
               

                
            };

            return Response<LoginResponseDto>.Success("Đăng nhập thành công",result,StatusCodes.Status200OK);
        }

        public async Task<Response<string>> Logout(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Response<string>.Fail("Người dùng không tồn tại", (int)HttpStatusCode.NotFound);

            // Reset refresh token và thời gian hết hạn
            user.RefreshToken = Guid.Empty;
            user.RefreshExpiryTime = DateTime.MinValue;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Response<string>.Fail("Đăng xuất thất bại. Vui lòng thử lại");

            _logger.LogInformation($"Người dùng {user.Email} đã đăng xuất thành công");

            return Response<string>.Success("Đăng xuất thành công", null);
        }

        //Xác thực người dùng
        private async Task<Response<bool>> ValidateUser(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var response = new Response<bool>();
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Response<bool>.Fail("Thông tin không hợp lệ", (int)HttpStatusCode.BadRequest);
            
            if (!await _userManager.IsEmailConfirmedAsync(user) && user.IsActive)
            {
                return Response<bool>.Fail("Tài khoản chưa được kích hoạt", (int)HttpStatusCode.Forbidden);
            }
            else
            {
                response.success = true;
                return response;
            }
        }

        //Quên mật khẩu
        public async Task<Response<string>> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
                return Response<string>.Fail($"Một email đã được gửi đến {email} nếu nó tồn tại",(int)HttpStatusCode.OK);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = TokenConverter.EncodeToken(token);
            var userRole = await _userManager.GetRolesAsync(user);

            var mailBody = await EmailBodyBuilder.GetEmailBody(user,userRole.ToList(), emailTempPath: "StaticFiles/Html/ForgotPassword.html", linkName: "resetpassword", encodedToken, controllerName: "authentication");

            var mailRequest = new MailRequest()
            {
                Subject = "Quên mật khẩu",
                Body = mailBody,
                ToEmail = email
            };

            var emailResult = await _mailService.SendEmailAsync(mailRequest);
            if (emailResult)
                return Response<string>.Success($"Một email đã được gửi đến {email} nếu nó tồn tại",email,(int)HttpStatusCode.OK);

            return Response<string>.Fail("Có gì đó không ổn. Vui lòng thử lại", (int)HttpStatusCode.ServiceUnavailable);
        }

        //Đổi mật khẩu
        public async Task<Response<bool>> ChangePassword(string id, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Response<bool>.Fail("Người dùng không tồn tại", StatusCodes.Status404NotFound);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Response<bool>.Success("Đổi mật khẩu thành công", true, StatusCodes.Status200OK);
            }
            return Response<bool>.Fail("Đảm bảo mật khẩu cũ của bạn là chính xác", StatusCodes.Status400BadRequest);
        }

        public async Task<Response<string>> UpdatePassword(UpdatePasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Response<string>.Fail("Người dùng không tồn tại", StatusCodes.Status404NotFound);

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return Response<string>.Fail("Ồ! Có gì đó không ổn rồi", StatusCodes.Status400BadRequest);

            return Response<string>.Success("Đổi mật khẩu thành công", user.Id, StatusCodes.Status200OK);
        }

        //Đặt lại mật khẩu
        public async Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = new Response<string>();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null)
                return Response<string>.Fail("Người dùng không hợp lệ",StatusCodes.Status404NotFound);
            
            var decodedToken = TokenConverter.DecodeToken(resetPasswordDto.Token);

            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;

            var isValidToken = await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, decodedToken);
            if (isValidToken)
            {
                _mapper.Map<AppUser>(resetPasswordDto);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);

                return Response<string>.Success("Mật khẩu đã được đặt lại thành công",user.Id,StatusCodes.Status200OK);
            }

            return Response<string>.Fail("Token không hợp lệ", StatusCodes.Status400BadRequest);
        }

        //làm mới token
        public async Task<Response<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto token)
        {
            var refreshToken = token.RefreshToken;

            var user = await _tokenRepository.GetUserByRefreshToken(refreshToken, token.UserId);

            if (user.RefreshToken != refreshToken || user.RefreshExpiryTime <= DateTime.Now)

                return Response<RefreshTokenResponseDto>.Fail("Yêu cầu không hợp lệ",StatusCodes.Status400BadRequest);

            var result = new RefreshTokenResponseDto
            {
                NewJwtAccessToken = await _tokenGeneratorService.GenerateToken(user),
                NewRefreshToken = _tokenGeneratorService.GenerateRefreshToken()
            };
            user.RefreshToken = result.NewRefreshToken;
            await _userManager.UpdateAsync(user);

            return Response<RefreshTokenResponseDto>.Success("Làm mới token thành công", result, StatusCodes.Status200OK);
        }

        //xác thực vai trò người dùng
        public async Task<bool> ValidateUserRole(string userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool result = false;
            foreach (var role in userRoles)
            {
                if (roles.Contains(role))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }

    }
}
