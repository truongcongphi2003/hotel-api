using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAmenityService
    {
        Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesAsync(SearchRequestDto searchRequestDto,PagingDto pagingDto);
        Task<Response<AmenityResponseDto>> GetAmenityByIdAsync(string id);
        Task<Response<IEnumerable<AmenityResponseDto>>> SearchAmenitiesAsync(SearchRequestDto request);
        Task<Response<AmenityResponseDto>> CreateAmenityAsync(AmenityCreateDto dto);
        Task<Response<AmenityUpdateDto>> UpdateAmenityAsync(string id, AmenityUpdateDto dto);
        Task<Response<string>> DeleteAmenityAsync(string id);
        Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesByHotelIdAsync(string hotelId, SearchRequestDto requestDto, PagingDto pagingDto);
        Task<Response<PageResult<IEnumerable<AmenityDto>>>> GetAllAmenitiesByRoomTypeIdAsync(string roomTypeId, PagingDto pagingDto);
    }
}
