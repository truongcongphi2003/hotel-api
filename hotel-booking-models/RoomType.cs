using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class RoomType : BaseEntity
    {
        public string HotelId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public bool? IsNoSmoking { get; set; }
        public double? Size { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int RoomCount { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<RoomTypeBedType> RoomTypeBedTypes { get; set; } = new List<RoomTypeBedType>();
        public ICollection<Room> Rooms { get; set; }
        public ICollection<RoomTypeImage> RoomTypeImages { get; set; }
        public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set;}
    }
}
