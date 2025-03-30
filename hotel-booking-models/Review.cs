using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Review : BaseEntity
    {
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public string HotelId { get; set; }
        public string CustomerId { get; set; }
        public Hotel Hotel { get; set; }
        public Customer Customer { get; set; }
    }
}
