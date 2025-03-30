using hotel_booking_dto.RoomTypeDtos;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_dto.ImageDtos;
using hotel_booking_dto.RoomTypeAmenityDtos;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_core.Interfaces
{
    public interface IRoomTypeService
    {
        Task<Response<RoomTypeResponseDto>> CreateRoomTypeAsync(string hotelId, RoomTypeCreateDto dto);

        Task<Response<RoomTypeResponseDto>> UpdateRoomTypeAsync(string hotelId, string id, RoomTypeUpdateDto dto);

        Task<Response<bool>> DeleteRoomTypeAsync(string id);

        Task<Response<PageResult<IEnumerable<RoomTypeDto>>>> GetAllRoomTypes(string hotelId, SearchRequestDto searchRequestDto, PagingDto pagingDto);
        Task<Response<RoomTypeResponseDto>> GetRoomTypeById(string id);
        Task<Response<string>> UpdateRoomTypeThumbnail(string id, AddThumbnailDto dto);

        Task<Response<IEnumerable<ImageResponseDto>>> UpdateRoomTypeImages(string id, AddImageDto dto);
        Task<Response<GetRoomTypeImageDto>> GetImageRoomTypeAsync(string id);
        Task<Response<IEnumerable<string>>> GetImageListAsync(string id);
        Task<Response<bool>> UpdateRoomTypeAmenities(string id, RoomTypeAmenityUpdateDto dto);
        Task<Response<IEnumerable<RoomTypeDto>>> SearchRoomTypeAsync(SearchRequestDto request);
        Task<Response<IEnumerable<RoomTypeRoomsDto>>> SearchAvailableRooms(SearchHotelDto searchDto);
    }
}
