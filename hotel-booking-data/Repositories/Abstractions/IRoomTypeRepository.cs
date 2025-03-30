using hotel_booking_data.Contexts;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        IQueryable<RoomType> GetAllHotelRoomType(string hotelId, SearchRequestDto request);        
        IQueryable<RoomType> GetAllRoomTypeRooms(string hotelId);
        Task<RoomType> GetRoomTypeById(string id);
        IQueryable<RoomType> Search(SearchRequestDto request);
        Task<IEnumerable<RoomType>> SearchAvailableRooms(SearchHotelDto searchDto);
    }
}
