using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class LocationSuggestionDto
    {
        public string Type { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? FullNameEn { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? HotelCount { get; set; }
    }
}
