using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using Microsoft.EntityFrameworkCore.Storage;


namespace hotel_booking_data.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelContext _context;
        private IAmenityRepository _amenities;
        private IHotelAmenityRepository _hotelAmenities;
        private ICustomerRepository _customers;
        private IHotelRepository _hotels;
        private IManagerRepository _managers;
        private IBedTypeRepository _bedTypes;
        private IRoomTypeRepository _roomTypes;
        private IRoomTypeAmenityRepository _roomTypeAmenitys;
        private IRoomTypeImageRepository _roomTypesImages;
        private ICancellationPolicyRepository _cancellationPolicys;
        private IPaymentRepository _payments;
        private IRoomRepository _rooms;
        //private IWishListRepository _wishLists;
        //private IRoomTypeRepository _roomType;
        private IBookingRepository _bookings;
        private IReviewRepository _review;
        //private ITransactionRepository _transaction;
        //private IRatingRepository _rating;
        
        private IManagerRequestRepository _managerRequests;

        public UnitOfWork(HotelContext context)
        {
            _context = context;
        }

        public IAmenityRepository Amenities => _amenities ??= new AmenityRepository(_context);
        public IHotelAmenityRepository HotelAmenities => _hotelAmenities ??= new HotelAmenityReposiory(_context);

        //public ITransactionRepository Transactions => _transaction ??= new TransactionRepository(_context);

        public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);
        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
        public IHotelRepository Hotels => _hotels ??= new HotelRepository(_context);
        public IManagerRepository Managers => _managers ??= new ManagerRepository(_context);
        public IBedTypeRepository BedTypes => _bedTypes ??= new BedTypeRepository(_context);
        public IRoomTypeRepository RoomTypes => _roomTypes ??= new RoomTypeRepository(_context);
        public IRoomTypeAmenityRepository RoomTypeAmenitys => _roomTypeAmenitys ??= new RoomTypeAmenityRepository(_context);
        public IRoomTypeImageRepository RoomTypeImages => _roomTypesImages ??= new RoomTypeImageRepository(_context);
        public ICancellationPolicyRepository CancellationPolicys => _cancellationPolicys ??= new CancellatitonPolicyRepository(_context);

        public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);

        public IRoomRepository Rooms => _rooms ??= new RoomRepository(_context);

        //public IWishListRepository WishLists => _wishLists ??= new WishListRepository(_context);

        public IReviewRepository Reviews => _review ??= new ReviewRepository(_context);
        //public IRoomTypeRepository RoomType => _roomType ??= new RoomTypeRepository(_context);
        public IManagerRequestRepository ManagerRequests => _managerRequests ??= new ManagerRequestRepository(_context);
        //public IRatingRepository Rating => _rating ??= new RatingRepository(_context);


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
