using hotel_booking_dto.ImageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomTypeDtos
{
    public class GetRoomTypeImageDto
    {
        public string? Thumbnail { get; set; }
        public IEnumerable<ImageResponseDto>? Images { get; set; }
    }
}
