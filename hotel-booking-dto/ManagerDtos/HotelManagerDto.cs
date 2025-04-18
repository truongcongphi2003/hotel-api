﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ManagerDtos
{
    public class HotelManagerDto
    {
        public string ManagerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerEmail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CompanyName { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int TotalHotels { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<string> HotelNames { get; set; }
        public ICollection<string> HotelStreet { get; set; }
        public ICollection<string> HotelCity { get; set; }
        public string Avatar { get; set; }
    }
}
