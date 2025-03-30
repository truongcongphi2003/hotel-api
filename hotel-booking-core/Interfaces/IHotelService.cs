using hotel_booking_dto;
using hotel_booking_dto.HotelAmenityDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ImageDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        Task<Response<HotelDetailResponseDto>> GetDetailAsync(string id);
        Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetHotelsByLocation(LocationRequestDto locationRequestDto, PagingDto pagingDto);
        Task<Response<HotelResponseDto>> GetHotelAsync(string hotelId);
        Task<Response<HotelResponseDto>> CreateHotelAsync(string managerId, CreateHotelDto dto);
        Task<Response<HotelResponseDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto dto);
        Task<Response<string>> UpdateHotelThumbnail(string hotelId, AddThumbnailDto dto);
        Task<Response<IEnumerable<ImageResponseDto>>> UpdateHotelImages(string hotelId, AddImageDto dto);
        Task<Response<bool>> UpdateHotelAmenities(string hotelId, HotelAmenityUpdateDto dto);
        Task<Response<IEnumerable<ImageResponseDto>>> GetHotelImages(string hotelId);
        Task<Response<IEnumerable<ImageResponseDto>>> GetHotelImagesAsync(string id);

    }
}