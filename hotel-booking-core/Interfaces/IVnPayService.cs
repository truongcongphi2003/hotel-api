using hotel_booking_dto;
using hotel_booking_dto.VnPayDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationDto dto, HttpContext context);
        Task<Response<PaymentResponseDto>> PaymentExecute(IQueryCollection collections);
    }
}
