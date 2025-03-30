using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ICancellationPolicyRepository : IGenericRepository<CancellationPolicy>
    {
        IQueryable<CancellationPolicy> GetAllByRoomId(string roomId);
        Task<CancellationPolicy> GetCancellationPolicyAsync(string roomId);
    }
}
