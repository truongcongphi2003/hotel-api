using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.VnPayDtos
{
    public class PaymentInformationDto
    {
        public double Amount { get; set; }
        public string BookingDescription { get; set; }
        public string BookingId { get; set; }
        public string TransactionReference { get; set; }
    }
}
