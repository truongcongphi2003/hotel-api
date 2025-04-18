﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ManagerDtos
{
    public class ManagerDetailsResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ManagerPhone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CompanyName { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
