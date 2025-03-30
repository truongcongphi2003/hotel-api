using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IHotelAmenityRepository : IGenericRepository<HotelAmenity>
    {
        IQueryable<HotelAmenity> GetHotelAmenities();
        Task<HotelAmenity> CheckHotelAmenityAsync(string hotelId, string amenityId);

    }
}
