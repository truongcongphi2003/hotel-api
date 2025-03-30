using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.TokenDtos
{
    public class RefreshTokenResponseDto
    {
        public string NewJwtAccessToken { get; set; }
        public Guid NewRefreshToken { get; set; }
    }
}
