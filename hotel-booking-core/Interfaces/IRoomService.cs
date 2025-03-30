using hotel_booking_dto;
using hotel_booking_dto.RoomDtos;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IRoomService
    {
        Task<Response<RoomDto>> CreateRoomAsync(RoomCreateDto roomCreateDto);
        Task<Response<RoomDto>> UpdateRoomAsync(string id, RoomUpdateDto roomUpdateDto);

        Task<Response<bool>> DeleteRoomAsync(string id);

        Task<Response<PageResult<IEnumerable<GetAllRoomDto>>>> GetAllRoomsAsync(string hotelId, PagingDto pagingDto);
        Task<Response<RoomDto>> GetRoomByIdAsync(string id);
    } 
}
