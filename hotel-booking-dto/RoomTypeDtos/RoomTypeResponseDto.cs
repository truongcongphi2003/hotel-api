﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomTypeDtos
{
    public class RoomTypeResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsNoSmoking { get; set; }
        public double? Size { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int RoomCount { get; set; }
        public IEnumerable<AddRoomTypeBedTypeDto> BedTypes { get; set; }
        public string? Thumbnail { get; set; }

    }
}
