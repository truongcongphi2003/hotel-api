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
    public class TokenRepository : ITokenRepository
    {
        private readonly HotelContext _context;
        public TokenRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByRefreshToken(Guid token, string userId)
        {
            //kiểm tra user Id
            var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == token && u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException($"Người dùng với Id {userId} không tồng tại");
            }

            return user;
        }
    }
}
