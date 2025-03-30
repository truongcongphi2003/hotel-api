
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        IQueryable<Hotel> GetAll();
        Task<Hotel> GetHotelByIdAsync(string id);
        Task<IEnumerable<Image>> GetImages(string id);
        Task<Hotel> GetByIdAsync(string id);
        Task<bool> CheckHotelManager(string hotelId, string managerId);
        IQueryable<Hotel> GetByLocation(LocationRequestDto requestDto);
    }
}
