using hotel_booking_dto.VnPayDtos;
using hotel_booking_utilities.VNPay;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_core.Interfaces;

namespace hotel_booking_core.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public VnPayService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public string CreatePaymentUrl(PaymentInformationDto DTO, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)DTO.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", DTO.TransactionReference);
            pay.AddRequestData("vnp_OrderType", "HotelBooking");
            pay.AddRequestData("vnp_ReturnUrl", $"{urlCallBack}?bookingId={DTO.BookingId}");
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<Response<PaymentResponseDto>> PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            var bookingId = collections["bookingId"].ToString();
            var result = new PaymentResponseDto();
            if (response.VnPayResponseCode == "00")
            {
                await UpdatePaymentStatus(response.TransactionReference, PaymentStatus.Success);
                result = new PaymentResponseDto
                {
                    Success = true,
                    TransactionReference = response.TransactionReference,
                    PaymentMethod = response.PaymentMethod,
                    TransactionId = response.TransactionId,
                    PaymentId = response.PaymentId,
                    Token = response.Token,
                    VnPayResponseCode = response.VnPayResponseCode,
                    BookingId = bookingId
                };

                return Response<PaymentResponseDto>.Success("Thanh toán VnPay thành công", result);
            }

            await UpdatePaymentStatus(response.TransactionReference, PaymentStatus.Failed);
            result = new PaymentResponseDto { Success = false };
            return Response<PaymentResponseDto>.Fail("Thanh toán thất bại");
        }

        private async Task UpdatePaymentStatus(string transactionReference, string status)
        {
            var payment = await _unitOfWork.Payments.GetPaymentByReference(transactionReference);
            if (payment != null)
            {
                payment.Status = status;
                payment.UpdatedAt = DateTime.Now;
                payment.MethodOfPayment = Payments.VnPay;

                    var booking = await _unitOfWork.Bookings.GetById(payment.BookingId);
                    if (booking != null)
                    {
                        booking.PaymentStatus = (status == PaymentStatus.Success) ? true : false;
                        _unitOfWork.Bookings.Update(booking);
                    }
                
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();
            }
        }
    }
}
