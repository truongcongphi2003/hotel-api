using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_models;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> InitializePayment(decimal amount, string bookingId, string transactionRef, string paymentService)
        {

            Payment payment = new()
            {
                Amount = amount,
                TransactionReference = transactionRef,
                Status = PaymentStatus.Pending,
                MethodOfPayment = paymentService,
                BookingId = bookingId
            };
            await _unitOfWork.Payments.InsertAsync(payment);
            await _unitOfWork.Save();

            return true;
        }
    }
}
