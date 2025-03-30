using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BookingDtos
{
    public class GetAllBookingDto
    {
        public string Id { get; set; }
        public string HotelName { get; set; }
        public string HotelThumbnail { get; set; }
        public string RoomTypeName {  get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
