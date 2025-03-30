using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    /// <summary>
    /// This specifies the roles available within the application
    /// </summary>
    public class UserRoles
    {
        public const string Admin = "Admin";
  
        public const string HotelManager = "Manager";
  
        public const string Customer = "Customer";
    }
    public class Payments
    {
        public const string VnPay = "vnpay";
        public const string AtHotel = "athotel";
    }

    public class PaymentStatus
    {
        public const string Success = "Success";
        public const string Pending = "Pending";
        public const string Failed = "Failed";
        public const string PendingRefund = "Pending-Refund";
        public const string Refunded = "Refunded";
        public const string NotRefunded = "NotRefunded";
    }
    public class BookingStatus
    {
        public const string Success = "Success";
        public const string Pending = "Pending";
        public const string Cancelled = "Cancelled";
        public const string Failed = "Failed";

    }
}
