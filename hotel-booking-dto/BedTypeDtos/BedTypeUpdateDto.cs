﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BedTypeDtos
{
    public class BedTypeUpdateDto
    {
        public string BedName { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
}
