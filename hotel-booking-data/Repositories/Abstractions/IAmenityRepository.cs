using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IAmenityRepository : IGenericRepository<Amenity>
    {
        Task<Amenity> GetByIdAsync(string id);
        IQueryable<Amenity> GetAll(SearchRequestDto searchRequestDto);
        IQueryable<Amenity> Search(SearchRequestDto request);
        IQueryable<Amenity> GetAllHotelAmenities(string hotelId, SearchRequestDto request);
        IQueryable<Amenity> GetAllRoomTypeAmenities(string roomTypeId);

    }
}
