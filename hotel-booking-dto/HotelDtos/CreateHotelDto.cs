using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class CreateHotelDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double? StarRating { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string? MapLocation { get; set; }
    }
}
