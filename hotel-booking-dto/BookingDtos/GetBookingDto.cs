using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BookingDtos
{
    public class GetBookingDto
    {
        public string CustomerId { get; set; }
        public string HotelName { get; set; }
        public string HotelThumbnail { get; set; }
        public string RoomTypeName { get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int? Children { get; set; }
        public int QuantityRoom { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? FullNameOthers { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxAndFee { get; set; }
        public decimal TotalAmount { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime? CancelledDate;
        public string? CancellationReason { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
