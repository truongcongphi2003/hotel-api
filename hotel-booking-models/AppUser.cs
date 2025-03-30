using Microsoft.AspNetCore.Identity;

namespace hotel_booking_models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public bool IsActive { get; set; }
        public string? PublicId { get; set; } = Guid.NewGuid().ToString();
        public string? Avatar {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool LockoutEnabled = false;
        public Guid RefreshToken { get; set; }
        public DateTime RefreshExpiryTime { get; set; }
        public Manager Manager { get; set; }
        public Customer Customer { get; set; }
    }
}
