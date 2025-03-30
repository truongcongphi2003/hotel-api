namespace hotel_booking_dto.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
