namespace hotel_booking_models
{
    public class HotelAmenity
    {
        public string HotelId { get; set; }
        public string AmenityId { get; set; }
        public Hotel Hotel { get; set; }
        public Amenity Amenity { get; set; }
    }
}
