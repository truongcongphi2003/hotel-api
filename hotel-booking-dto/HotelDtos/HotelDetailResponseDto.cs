using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class HotelDetailResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double? StarRating { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward{ get; set; }
        public string? MapLocation { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public string? Thumbnail { get; set; }
    }
}
