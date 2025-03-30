namespace hotel_booking_models
{
    public class RoomTypeImage
    {
        public string RoomTypeId { get; set; }
        public string ImageId { get; set; }
        public RoomType RoomType { get; set; }
        public Image Image { get; set; }
    }
}
