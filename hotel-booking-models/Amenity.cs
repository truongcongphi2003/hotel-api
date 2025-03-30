namespace hotel_booking_models
{
    public class Amenity : BaseEntity
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? ParentId { get; set; }
        public ICollection<HotelAmenity> HotelAmenities { get; set; }
        public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; }
    }
}
