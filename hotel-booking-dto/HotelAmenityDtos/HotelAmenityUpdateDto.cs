using hotel_booking_dto.AmenityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelAmenityDtos
{
    public class HotelAmenityUpdateDto
    {
        public IEnumerable<string> UpdateAmenityIds { set; get; }
    }
}
