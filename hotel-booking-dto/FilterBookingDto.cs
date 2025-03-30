using System;

namespace hotel_booking_dto
{
    public class FilterBookingDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PastDays { get; set; }
        public int? Month { get; set; } 
        public int? Year { get; set; }
        public List<string>? PaymentStatus { get; set; }
    }
}
