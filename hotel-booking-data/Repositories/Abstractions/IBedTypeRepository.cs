using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IBedTypeRepository : IGenericRepository<BedType>
    {
        IQueryable<BedType> GetAllByHotelId(string hotelId);
        Task<BedType> GetById(string id);
        IQueryable<BedType> Search(SearchRequestDto request);
    }
}
