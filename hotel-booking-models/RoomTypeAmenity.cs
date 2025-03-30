namespace hotel_booking_models
{
    public class RoomTypeAmenity
    {
        public string RoomTypeId { get; set; }
        public string AmenityId { get; set; }
        public bool IsMain { get; set; }
        public RoomType RoomType { get; set; }
        public Amenity Amenity { get; set; }
    }
}
