using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomTypeAmenityDtos
{
    public class RoomTypeAmenityUpdateDto
    {
        public IEnumerable<AmenityIdUpdateDto>? AddAmenityIds { set; get; }
        public IEnumerable<string>? RemoveAmenityIds { set; get; }
    }
}
