using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Payment
    {
        [Key]
        public string BookingId { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string MethodOfPayment { get; set; }
        // Thời gian yêu cầu hoàn tiền
        public DateTime? RefundRequestedDate { get; set; }
        // Ngày hoàn tiền (nếu đã hoàn)
        public DateTime? RefundDate { get; set; }
        public string? RefundReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Booking Booking { get; set; }
    } 
}
