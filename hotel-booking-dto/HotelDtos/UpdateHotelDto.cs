using hotel_booking_dto.ImageDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class UpdateHotelDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double? StarRating { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string WardCode { get; set; }
        public string? MapLocation { get; set; }
        public IFormFile? Thumbnail {  get; set; }
        public bool IsDeleteThumbnail { get; set; }
    }
}
