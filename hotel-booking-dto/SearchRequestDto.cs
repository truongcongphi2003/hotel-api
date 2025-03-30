using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class SearchRequestDto
    {
        public string? SearchQuery { get; set; } = "";
        public string[]? Fields { get; set; }
        public string? Code { get; set; }
     
    }
}
