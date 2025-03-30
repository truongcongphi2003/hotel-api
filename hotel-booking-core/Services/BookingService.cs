using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPaymentService _paymentService;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _paymentService = paymentService;
        }
        public async Task<Response<BookingResponseDto>> CreateRoomBookingAsync(string? customerId, CreateBookingDto dto)
        {
            try
            {
                // Lấy thông tin phòng
                var room = await _unitOfWork.Rooms.GetById(dto.RoomId);
                if (room == null)
                    return Response<BookingResponseDto>.Fail("Phòng không tồn tại",404);

                // Kiểm tra trùng lịch đặt
                var roomsAvailable = await _unitOfWork.Bookings.GetNoOfRoomAvailable(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);

                if (roomsAvailable - dto.QuantityRoom < 0)
                    return Response<BookingResponseDto>.Fail("Số lượng phòng trống hiện tại không đủ. Vui lòng thay đổi hoặc lựa chọn loại phòng khác", 400);

                var bookingReference = await GenerateUniqueBookingCodeAsync();

                var booking = _mapper.Map<Booking>(dto);
                booking.BookingReference = bookingReference;

                if (!string.IsNullOrEmpty(customerId))
                {
                    booking.CustomerId = customerId;
                }

                //Xử lý giá
                var unitPrice = room.Discount?? room.Price;

                var totalPrice = dto.Nights * dto.QuantityRoom * unitPrice;
                var totalTaxAndFee = dto.Nights * dto.QuantityRoom * room.TaxAndFee??0;

                var totalAmount = totalPrice + totalTaxAndFee;

                await _unitOfWork.Bookings.InsertAsync(booking);
                await _unitOfWork.Save();
                string transactionRef = $"{Guid.NewGuid()}";

                string paymenService = Payments.VnPay;
                if (room.IsPayAtHotel == true)
                {
                    paymenService = Payments.AtHotel;
                }

                // Xử lý thanh toán
                await _paymentService.InitializePayment(totalAmount, booking.Id, transactionRef, paymenService );

                await _unitOfWork.Save();

                var bookingResponse = _mapper.Map<BookingResponseDto>(booking);

                return Response<BookingResponseDto>.Success("Tạo đặt phòng thành công", bookingResponse, 201);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(ex, "Lỗi xảy ra khi tạo đặt phòng");
            }

        }
        private async Task<string> GenerateUniqueBookingCodeAsync()
        {
            string bookingCode;
            bool isDuplicate;
            do
            {
                bookingCode = new Random().Next(100000, 999999).ToString();

                isDuplicate = await _unitOfWork.Bookings
                    .CheckBookingByBookingReference(bookingCode);
            }
            while (isDuplicate);

            return bookingCode;
        }

        public async Task<Response<PageResult<IEnumerable<GetAllBookingDto>>>> GetAllCustomerBookingsAsync(string customerId, PagingDto pagingDto, FilterBookingDto filterBookingDto)
        {
            try
            {
                var bookings = _unitOfWork.Bookings.GetAllCustomerBookings(customerId,filterBookingDto);

                var result = await bookings.PaginationAsync<Booking, GetAllBookingDto>(pagingDto.Items, pagingDto.Page, mapper: _mapper);

                return Response<PageResult<IEnumerable<GetAllBookingDto>>>.Success("Lấy tất cả đơn đặt của khách hàng",result);
            }
            catch (Exception ex)
            {
                return Response<PageResult<IEnumerable<GetAllBookingDto>>>.Fail(ex);
            }
        }
        public async Task<Response<GetBookingDto>> GetCustomerBookingByIdAsync(string id)
        {
            try
            {
                var bookings = await _unitOfWork.Bookings.GetCustomerBookingById(id);

                if(bookings == null) Response<GetBookingDto>.Fail("Đơn đặt không tồn tại");

                var result = _mapper.Map<GetBookingDto>(bookings);

                return Response<GetBookingDto>.Success("Lấy đơn đặt của khách hàng", result);
            }
            catch (Exception ex)
            {
                return Response<GetBookingDto>.Fail(ex);
            }
        }
    }
}
