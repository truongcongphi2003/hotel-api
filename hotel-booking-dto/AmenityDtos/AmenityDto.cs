using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.AmenityDtos
{
    public class AmenityDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? ParentId { get; set; }
        public IEnumerable<AmenityDto>? Children { get; set; }
    }
}
