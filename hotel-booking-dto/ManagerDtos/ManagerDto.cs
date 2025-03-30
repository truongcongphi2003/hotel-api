using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ManagerDtos
{
    public class ManagerDto
    {
        public string CompanyName { get; set; }
        public string BusinessEmail { get; set; }
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string HotelEmail { get; set; }
        public string HotelPhone { get; set; }
        public string HotelAddress { get; set; }
        public string HotelCity { get; set; }
        public string BusinessPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TokenCode { get; set; }
    }
}
