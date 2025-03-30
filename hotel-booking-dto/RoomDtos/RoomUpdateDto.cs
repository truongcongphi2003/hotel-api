using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomDtos
{
    public class RoomUpdateDto
    {
        public string? RoomTypeName { get; set; }
        public string Name { get; set; }
        public int? MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxAndFee { get; set; }
        public string? Gift { get; set; }
        public string? GiftDescription { get; set; }
        public bool? IsCancelable { get; set; }
        public bool? IsPayAtHotel { get; set; }
        public IEnumerable<CancellationPolicyUpdateDto>? CancellationPolicies { get; set; }
    }
}
