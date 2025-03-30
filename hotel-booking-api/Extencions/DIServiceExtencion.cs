using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.UnitOfWork.Implementation;
using thda.Services;

namespace hotel_booking_api.Extencions
{
    public static class DIServiceExtencion
    {
        public static void AddDependencyInjection (this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IBedTypeService, BedTypeService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IBookingService, BookingService>();

            // Add Repository Injections Here
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
