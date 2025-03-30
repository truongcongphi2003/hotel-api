using hotel_booking_dto.BedTypeDtos;
using hotel_booking_dto.ImageDtos;
using hotel_booking_dto.RoomDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomTypeDtos
{
    public class RoomTypeRoomsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public bool? IsNoSmoking { get; set; }
        public double? Size { get; set; }
        public IEnumerable<ImageResponseDto>? Images { get; set; }
        public IEnumerable<RoomDto>? Rooms { get; set; }
        public IEnumerable<RoomTypeBedTypeDto>? BedTypes { get; set; }

    }
}