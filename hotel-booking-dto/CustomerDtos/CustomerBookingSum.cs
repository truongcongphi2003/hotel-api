using hotel_booking_models;

namespace hotel_booking_dto.CustomerDtos
{
    public class CustomerBookingSum
    {
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
    }
}
