using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Customer
    {
        [Key]
        public string AppUserId { get; set; }
        public string? Address { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
