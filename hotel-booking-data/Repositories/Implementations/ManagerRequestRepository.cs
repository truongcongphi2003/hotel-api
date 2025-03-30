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
    public class ManagerRequestRepository : GenericRepository<ManagerRequest>,IManagerRequestRepository
    {
        private readonly HotelContext _context;

        public ManagerRequestRepository(HotelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ManagerRequest> GetHotelManagerRequestByEmail(string email)
        {
            var managerRequest = await _context.ManagerRequests.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return managerRequest;
        }

        public async Task<ManagerRequest> GetHotelManagerByEmailToken(string email, string token)
        {
            var managerRequest = await _context.ManagerRequests.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            var checkToken = managerRequest != null && managerRequest.Token == token;
            return checkToken ? managerRequest : null;
        }

        public IQueryable<ManagerRequest> GetManagerRequests()
        {
            var query = _context.ManagerRequests.Select(x => x)
                .OrderByDescending(recent => recent.CreatedAt)
                .OrderBy(recent => recent.ConfirmtionFlag);
            return query;
        }
    }
}
