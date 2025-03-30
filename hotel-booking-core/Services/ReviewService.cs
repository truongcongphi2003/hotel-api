using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<ReviewDto>> AddReviewAsync(AddReviewDto dto, string customerId)
        {
            var existingHotel = _unitOfWork.Hotels.GetByIdAsync(dto.HotelId);
            if (existingHotel == null)
                return Response<ReviewDto>.Fail("khách sạn không tồn tại");

            var review = _mapper.Map<Review>(dto);
            review.CustomerId = customerId;
            
            await _unitOfWork.Reviews.InsertAsync(review);
            await _unitOfWork.Save();

            var reviewRespone = _mapper.Map<ReviewDto>(review);

            return Response<ReviewDto>.Success("Thêm đánh giá thành công",reviewRespone,StatusCodes.Status201Created);
        }

        public async Task<Response<ReviewDto>> UpdateReviewAsync(string reviewId, string customerId, ReviewUpdateDto dto)
        {
            var existingReview = await _unitOfWork.Reviews.GetById(reviewId);

            if (existingReview == null)
                return Response<ReviewDto>.Fail("Đánh giá không tồn tại");

            _mapper.Map(dto,existingReview);
            _unitOfWork.Reviews.Update(existingReview);
            await _unitOfWork.Save();

            var dataResult = _mapper.Map<ReviewDto>(existingReview); 

            return Response<ReviewDto>.Success("Cập nhật đánh giá thành công",dataResult);
        }

        public async Task<Response<string>> DeleteReviewAsync(string customerId ,string reviewId)
        {
            var existingReview = await _unitOfWork.Reviews.GetById(reviewId);

            if (existingReview == null)
                return Response<string>.Fail("Đánh giá không tồn tại");

            if(existingReview.CustomerId != customerId)
                return Response<string>.Fail("Bạn không có quyền xóa đánh giá này");

            _unitOfWork.Reviews.Delete(existingReview);
            await _unitOfWork.Save();

            return Response<string>.Success("Xóa đánh giá thành công", null);
        }
    }
}
