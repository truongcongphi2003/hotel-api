using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IManagerRequestRepository : IGenericRepository<ManagerRequest>
    {
        Task<ManagerRequest> GetHotelManagerRequestByEmail(string email);
        Task<ManagerRequest> GetHotelManagerByEmailToken(string email, string token);
        IQueryable<ManagerRequest> GetManagerRequests();
    }
}
