using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Booking : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string HotelId { get; set; }
        public string RoomId { get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults {  get; set; }
        public int? Children {  get; set; }
        public int QuantityRoom { get; set; }
        public string Email { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string? FullName { get; set; }
        [StringLength(100)]
        public string? FullNameOthers { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxAndFee { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime? CancelledDate;
        public string? CancellationReason { get; set; }
        public Hotel Hotel { get; set; }
        public Customer Customer { get; set; }
        public Room Room { get; set; } 
        public Payment Payment { get; set; }
    }
}
