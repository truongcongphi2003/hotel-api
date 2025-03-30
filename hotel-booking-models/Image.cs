namespace hotel_booking_models
{
    public class Image : BaseEntity
    {
        public string HotelId { get; set; }
        public string ImageUrl { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<RoomTypeImage> RoomTypeImages { get; set; }
    }
}
