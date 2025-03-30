using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto;
using hotel_booking_models.Cloudinary;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Transactions;
using thda.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using hotel_booking_data.Contexts;

namespace hotel_booking_core.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        private readonly HotelContext _dbContext;

        public CustomerService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IImageService imageService, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<string>> UpdateCustomer(string customerId, UpdateCustomerDto updateCustomer)
        {
            var customer = _unitOfWork.Customers.GetCustomer(customerId);
            if (customer == null)
                return Response<string>.Fail("Khách hàng không tồn tại", StatusCodes.Status404NotFound);

            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    var user = await _userManager.FindByIdAsync(customerId);
                    var userUpdateResult = await UpdateUser(user, updateCustomer);

                    if (!userUpdateResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return Response<string>.Fail("Có lỗi xảy ra khi cập nhật bảng AppUser. Vui lòng thử lại sau",
                            StatusCodes.Status400BadRequest);
                    }

                    customer.Address = updateCustomer.Address;
                    _unitOfWork.Customers.Update(customer);
                    await _unitOfWork.Save();

                    await transaction.CommitAsync();
                    return Response<string>.Success("Cập nhật thành công", customerId, StatusCodes.Status200OK);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi trong quá trình cập nhật khách hàng {CustomerId}", customerId);
                    return Response<string>.Fail("Đã xảy ra lỗi hệ thống, vui lòng thử lại sau",
                        StatusCodes.Status500InternalServerError);
                }
            });
        }

        public async Task<Response<UpdateImageDto>> UpdatePhoto(AddImageDto imageDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var upload = await _imageService.UploadAsync(imageDto.Image);
                string url = upload.Url.ToString();
                user.Avatar = url;
                user.PublicId = upload.PublicId;
                await _userManager.UpdateAsync(user);

                return Response<UpdateImageDto>.Success("image upload successful", new UpdateImageDto { Url = url });
            }
            return Response<UpdateImageDto>.Fail("user not found");

        }

        private async Task<IdentityResult> UpdateUser(AppUser user, UpdateCustomerDto model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<Response<PageResult<IEnumerable<GetUsersResponseDto>>>> GetAllCustomersAsync(PagingDto pagenator)
        {
            var customers = _unitOfWork.Customers.GetAllUsers();
            var customersList = await customers.PaginationAsync<Customer, GetUsersResponseDto>(pagenator.Items, pagenator.Page, _mapper);
            var response = new Response<PageResult<IEnumerable<GetUsersResponseDto>>>();

            if (customersList != null)
                return Response<PageResult<IEnumerable<GetUsersResponseDto>>>.Success("Lấy danh sách khách hàng thành công",customersList,StatusCodes.Status200OK);

            return Response<PageResult<IEnumerable<GetUsersResponseDto>>>.Fail("Danh sách khách hàng trống", StatusCodes.Status404NotFound);
        }

        //public async Task<Response<PageResult<IEnumerable<CustomerWishListDto>>>> GetCustomerWishList(string customerId, PagingDto paging)
        //{
        //    var customer = await _userManager.FindByIdAsync(customerId);
        //    var response = new Response<PageResult<IEnumerable<CustomerWishListDto>>>();

        //    if (customer != null)
        //    {
        //        var wishList = _unitOfWork.WishLists.GetCustomerWishList(customerId);
        //        var pageResult = await wishList.PaginationAsync<WishList, CustomerWishListDto>(paging.PageSize, paging.PageNumber, _mapper);

        //        response.StatusCode = (int)HttpStatusCode.OK;
        //        response.Succeeded = true;
        //        response.Data = pageResult;
        //        response.Message = $"are the Wishlists for the customer with Id {customerId}";
        //        return response;
        //    }

        //    response.StatusCode = (int)HttpStatusCode.BadRequest;
        //    response.Succeeded = false;
        //    response.Message = $"Customer with Id = {customerId} doesn't exist";
        //    return response;
        //}

        public async Task<Response<CustomerDetailsToReturnDto>> GetCustomerDetails(string userId)
        {
            var response = new Response<CustomerDetailsToReturnDto>();
            var user = await _unitOfWork.Customers.GetCustomerDetails(userId);

            if (user == null)
                return Response<CustomerDetailsToReturnDto>.Fail("Người dùng không tồn tại", StatusCodes.Status404NotFound);

            var result = _mapper.Map<CustomerDetailsToReturnDto>(user);

            return Response<CustomerDetailsToReturnDto>.Success("Lấy thông tin người dùng thành công",result, StatusCodes.Status200OK);
        }
    }
}
