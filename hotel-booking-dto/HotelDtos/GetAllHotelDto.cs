using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class GetAllHotelDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double StarRating { get; set; }
        public string? Thumbnail { get; set; }
        public double? ReviewRating { get; set; }
        public int? ReviewCount { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
    }
}
