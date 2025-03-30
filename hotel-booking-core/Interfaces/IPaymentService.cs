using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> InitializePayment(decimal amount, string bookingId, string transactionRef, string paymentService);
    }
}
