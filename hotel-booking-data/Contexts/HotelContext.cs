using hotel_booking_models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace hotel_booking_data.Contexts
{
    public class HotelContext : IdentityDbContext<AppUser>
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) 
        {
        }

        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<BedType> BedTypes { get; set; }
        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<RoomTypeAmenity> RoomTypeAmenities { get; set; }
        public DbSet<RoomTypeImage> RoomTypeImages { get; set; }
        public DbSet<ManagerRequest> ManagerRequests { get; set; }
        public DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TimeZoneInfo vietnamTimeZone = TZConvert.GetTimeZoneInfo("SE Asia Standard Time");
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            foreach (var item in ChangeTracker.Entries<BaseEntity>())
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        item.Entity.UpdatedAt = now;
                        break;
                    case EntityState.Added:
                        item.Entity.Id = Guid.NewGuid().ToString();
                        item.Entity.CreatedAt = now;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WishList>()
                .HasKey(bc => new { bc.CustomerId, bc.HotelId });
            modelBuilder.Entity<WishList>()
                .HasOne(bc => bc.Customer)
                .WithMany(b => b.WishLists)
                .HasForeignKey(bc => bc.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<WishList>()
                .HasOne(bc => bc.Hotel)
                .WithMany(c => c.WishLists)
                .HasForeignKey(bc => bc.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomTypeImage>()
                .HasKey(bc => new { bc.RoomTypeId, bc.ImageId });
            modelBuilder.Entity<RoomTypeImage>()
                .HasOne(bc => bc.RoomType)
                .WithMany(b => b.RoomTypeImages)
                .HasForeignKey(bc => bc.RoomTypeId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<RoomTypeImage>()
                .HasOne(bc => bc.Image)
                .WithMany(b => b.RoomTypeImages)
                .HasForeignKey(bc => bc.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HotelAmenity>()
                .HasKey(bc => new { bc.HotelId, bc.AmenityId });
            modelBuilder.Entity<HotelAmenity>()
                .HasOne(bc => bc.Hotel)
                .WithMany(b => b.HotelAmenities )
                .HasForeignKey(bc => bc.HotelId);
            modelBuilder.Entity<HotelAmenity>()
                .HasOne(bc => bc.Amenity)
                .WithMany(b => b.HotelAmenities)
                .HasForeignKey(bc => bc.AmenityId);

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasKey(bc => new { bc.RoomTypeId, bc.AmenityId });
            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne(bc => bc.RoomType)
                .WithMany(b => b.RoomTypeAmenities)
                .HasForeignKey(bc => bc.RoomTypeId);
            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne(bc => bc.Amenity)
                .WithMany(b => b.RoomTypeAmenities)
                .HasForeignKey(bc => bc.AmenityId);

            modelBuilder.Entity<RoomTypeBedType>()
                .HasKey(bc => new { bc.RoomTypeId, bc.BedTypeId });
            modelBuilder.Entity<RoomTypeBedType>()
                .HasOne(bc => bc.RoomType)
                .WithMany(b => b.RoomTypeBedTypes)
                .HasForeignKey(bc => bc.RoomTypeId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<RoomTypeBedType>()
                .HasOne(bc => bc.BedType)
                .WithMany(b => b.RoomTypeBedTypes)
                .HasForeignKey(bc => bc.BedTypeId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hotel)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
