using CloudinaryDotNet;

namespace hotel_booking_api.Extensions
{
    public static class CloudinaryServiceExtension
    {
        public static IServiceCollection AddCloudinary(this IServiceCollection services,
            Account account, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.Add(new ServiceDescriptor(typeof(Cloudinary), c => new Cloudinary(account), lifetime));
            return services;
        }

        public static Account GetAccount(IConfiguration Configuration)
        {
            Account account = new(
                                Configuration["CloudinarySettings:CloudName"],
                                Configuration["CloudinarySettings:ApiKey"],
                                Configuration["CloudinarySettings:ApiSecret"]);
            return account;
        }
    }
}
