using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto.ManagerRequestDtos;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities.EmailBodyHelper;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork, IMailService mailService, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _userManager = userManager;
        }
        public async Task<Response<PageResult<IEnumerable<HotelManagerDto>>>> GetAllHotelManagersAsync(PagingDto paging)
        {
            var hotelManagers = _unitOfWork.Managers.GetHotelManagersAsync();
            var response = await hotelManagers.PaginationAsync<Manager, HotelManagerDto>(paging.Items, paging.Page, _mapper);
            return Response<PageResult<IEnumerable<HotelManagerDto>>>.Success("Tất cả quản lý", response); ;
        }

        public async Task<Response<string>> AddManagerRequestAsync(AddManagerRequestDto managerRequest)
        {
            var getManager = await _unitOfWork.ManagerRequests.GetHotelManagerRequestByEmail(managerRequest.Email);
            var getUser = await _unitOfWork.Managers.GetAppUserByEmail(managerRequest.Email);

            if (getUser == null && getManager == null)
            {
                var addManager = _mapper.Map<ManagerRequest>(managerRequest);
                
                await _unitOfWork.ManagerRequests.InsertAsync(addManager);
                await _unitOfWork.Save();

                return Response<string>.Success("Cảm ơn bạn đã quan tâm, bạn sẽ sớm nhận được phản hồi từ chúng tôi",null);

            }
            return Response<string>.Fail("Yêu cầu Email này đã tồn tại", StatusCodes.Status409Conflict);
        }

        public async Task<Response<PageResult<IEnumerable<ManagerRequestResponseDto>>>> GetAllManagerRequest(PagingDto paging)
        {
            var managerRequests = _unitOfWork.ManagerRequests.GetManagerRequests();

            var response = await managerRequests.PaginationAsync<ManagerRequest, ManagerRequestResponseDto>(paging.Items, paging.Page, _mapper);

            return Response<PageResult<IEnumerable<ManagerRequestResponseDto>>>.Success("Tất cả yêu cầu của người quản lý", response, StatusCodes.Status200OK);
        }

        public async Task<Response<bool>> ApproveManagerRequestAsync(string email)
        {
            var request = await _unitOfWork.ManagerRequests.GetHotelManagerRequestByEmail(email);
            if (request == null)
                return Response<bool>.Fail("Yêu cầu này không tồn tại.");

            request.Token = Guid.NewGuid().ToString();

            _unitOfWork.ManagerRequests.Update(request);
            await _unitOfWork.Save();

            var resendMail = await SendManagerInvite(email);
            if (resendMail.success)
            {
                return Response<bool>.Success($"Phê duyệt thành công, đã gửi email đăng ký tới {email}", true);
            }
            return Response<bool>.Fail(resendMail.Message, resendMail.Code);
        }

        public async Task<Response<bool>> RegisterManagerAsync(RegisterManagerDto dto)
        {
            dto.Token = Decode(dto.Token).ToString();
            var getPotentialManager = await _unitOfWork.ManagerRequests.GetHotelManagerByEmailToken(email: dto.BusinessEmail, token: dto.Token);
            if (getPotentialManager != null)
            {
                var expired = getPotentialManager.ExpiresAt < DateTime.Now.AddMinutes(-5);
                if (expired)
                {
                    var resendMail = await SendManagerInvite(dto.BusinessEmail);
                    if (resendMail.success)
                    {
                        return Response<bool>.Fail("Liên kết đã hết hạn, một liên kết mới đã được gửi", StatusCodes.Status408RequestTimeout);
                    }
                    return Response<bool>.Fail(resendMail.Message, StatusCodes.Status408RequestTimeout);
                }
                var appUser = _mapper.Map<AppUser>(dto);
                var manager = _mapper.Map<Manager>(dto);
                var hotel = _mapper.Map<Hotel>(dto);
                manager.AppUserId = appUser.Id;
                hotel.ManagerId = manager.AppUserId;
                appUser.Manager = manager;
                appUser.IsActive = true;
                appUser.EmailConfirmed = true;
                manager.Hotels = new List<Hotel>() { hotel };
                var result = await _userManager.CreateAsync(appUser, dto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Manager");
                    await _unitOfWork.Save();
                    return Response<bool>.Success($"Khách hàng: {manager.CompanyName} có ID Quản lý: {manager.AppUserId}: đã đăng ký thành công", true);
                }
                return Response<bool>.Fail(GetErrors(result), StatusCodes.Status400BadRequest);
            }
            return Response<bool>.Fail("Mã Token không hợp lệ", StatusCodes.Status400BadRequest);
        }

        public async Task<Response<bool>> SendManagerInvite(string email)
        {
            var existingRequest = await _unitOfWork.ManagerRequests.GetHotelManagerRequestByEmail(email);
            var existingUser = await _unitOfWork.Managers.GetAppUserByEmail(email);

            if (existingUser != null && existingRequest == null)
            {
                return Response<bool>.Fail($"{email} là người dùng đã đăng ký (đã là Manager)", StatusCodes.Status409Conflict);
            }

            if (existingUser == null && existingRequest != null)
            {
                var token = Encode(Guid.Parse(existingRequest.Token));
                var mailBody = await EmailBodyBuilder.GetEmailBody(
                    emailTempPath: "StaticFiles/Html/ManagerInvite.html",
                    token: token,
                    email: email
                );

                var mailRequest = new MailRequest
                {
                    Subject = "Yêu cầu đã được chấp thuận",
                    Body = mailBody,
                    ToEmail = email
                };

                var emailSent = await _mailService.SendEmailAsync(mailRequest);
                if (!emailSent)
                {
                    return Response<bool>.Fail("Gửi thư đã thất bại", StatusCodes.Status400BadRequest);
                }

                existingRequest.ConfirmtionFlag = true;
                existingRequest.ExpiresAt = DateTime.UtcNow.AddHours(24);
                _unitOfWork.ManagerRequests.Update(existingRequest);
                await _unitOfWork.Save();

                return Response<bool>.Success("Thư đã được gửi thành công", true, StatusCodes.Status200OK);
            }

            return Response<bool>.Fail("Không tìm thấy yêu cầu đăng ký", StatusCodes.Status404NotFound);
        }

        private string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-").TrimEnd('=');
            return encoded;
        }
        public async Task<Response<bool>> CheckTokenExpiring(string email, string token)
        {
            token = Decode(token).ToString();
            var managerRequest = await _unitOfWork.ManagerRequests.GetHotelManagerByEmailToken(email, token);
            var getUser = await _unitOfWork.Managers.GetAppUserByEmail(email);

            if (managerRequest != null)
            {
                if (getUser == null)
                {
                    var expired = managerRequest.ExpiresAt < DateTime.Now;
                    
                    return Response<bool>.Success("Đang chuyển hướng đến trang đăng ký", true, StatusCodes.Status200OK);
                }
                return Response<bool>.Fail("Người dùng này đã được đăng ký rồi", StatusCodes.Status409Conflict);
            }
            return Response<bool>.Fail("Email hoặc mã Token không hợp lệ", StatusCodes.Status404NotFound);
        }



        private Guid Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            string paddedValue = value.Replace("_", "/").Replace("-", "+");
            // Thêm padding đúng theo yêu cầu Base64
            int paddingNeeded = (4 - (paddedValue.Length % 4)) % 4;
            paddedValue += new string('=', paddingNeeded);

            try
            {
                byte[] bytes = Convert.FromBase64String(paddedValue);
                return new Guid(bytes);
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid encoded GUID format", ex);
            }
        }
        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }
    }
}
