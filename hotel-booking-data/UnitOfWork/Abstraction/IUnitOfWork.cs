using hotel_booking_data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace hotel_booking_data.UnitOfWork.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IAmenityRepository Amenities { get; }
        IHotelAmenityRepository HotelAmenities { get; }
        IHotelRepository Hotels { get; }
        IManagerRepository Managers { get; }
        IBedTypeRepository BedTypes { get; }
        IRoomTypeRepository RoomTypes { get; }
        IRoomTypeAmenityRepository RoomTypeAmenitys { get; }
        IRoomTypeImageRepository RoomTypeImages { get; }
        ICancellationPolicyRepository CancellationPolicys { get; }
        IPaymentRepository Payments { get; }
        IRoomRepository Rooms { get; }
        //IWishListRepository WishLists { get; }
        //IRoomTypeRepository RoomType { get; }
        IBookingRepository Bookings {  get; }
        IReviewRepository Reviews { get; }
        IManagerRequestRepository ManagerRequests { get; }
        //ITransactionRepository Transactions { get; }
        //IRatingRepository Rating { get; }
        Task Save();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
