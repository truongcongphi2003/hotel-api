using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class CancellatitonPolicyRepository : GenericRepository<CancellationPolicy>, ICancellationPolicyRepository
    {
        private HotelContext _context;
        public CancellatitonPolicyRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<CancellationPolicy> GetAllByRoomId(string roomId)
        {
            return _context.CancellationPolicies.Where(c => c.RoomId == roomId);
        }

        public async Task<CancellationPolicy> GetCancellationPolicyAsync(string roomId)
        {
            return await _context.CancellationPolicies.FirstOrDefaultAsync(c => c.RoomId == roomId);
        }
    }
}
