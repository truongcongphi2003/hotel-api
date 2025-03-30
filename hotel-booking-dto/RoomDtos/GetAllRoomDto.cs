using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomDtos
{
    public class GetAllRoomDto
    {
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public IEnumerable<RoomDto>? Rooms { get; set; }
    }
}
