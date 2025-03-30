using hotel_booking_models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_booking_models
{
    public class Hotel : BaseEntity
    {
        public string ManagerId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double? StarRating { get; set; }
        public string? Email {get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string? MapLocation { get; set; }
        public string? Thumbnail { get; set; }
        public Manager Manager { get; set; }
        [MaxLength(20)]
        public string? ProvinceCode { get; set; }
        [ForeignKey("ProvinceCode")]
        public Province? Province { get; set; }
        [MaxLength(20)]
        public string? DistrictCode { get; set; }
        [ForeignKey("DistrictCode")]
        public District? District { get; set; }
        [MaxLength(20)]
        public string? WardCode { get; set; }
        [ForeignKey("WardCode")]
        public Ward? Ward { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<RoomType> RoomTypes { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<HotelAmenity> HotelAmenities { get; set;}

    }
}