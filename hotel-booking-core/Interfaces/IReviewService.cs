using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewService
    {
        Task<Response<ReviewDto>> AddReviewAsync(AddReviewDto dto, string customerId);
        Task<Response<ReviewDto>> UpdateReviewAsync(string reviewId, string customerId, ReviewUpdateDto dto);
        Task<Response<string>> DeleteReviewAsync(string customerId, string reviewId);
    }
}
