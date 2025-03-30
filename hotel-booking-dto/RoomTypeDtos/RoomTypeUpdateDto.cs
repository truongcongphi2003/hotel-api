using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomTypeDtos
{
    public class RoomTypeUpdateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsNoSmoking { get; set; }
        public double? Size { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int RoomCount { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public bool IsDeleteThumbnail { get; set; }
        public IEnumerable<AddRoomTypeBedTypeDto>? BedTypes { get; set; }

    }
}
