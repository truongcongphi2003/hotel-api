using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class BedType : BaseEntity
    {
        public string BedName {  get; set; }
        public string? Description {  get; set; }
        public string? Icon { get; set; }
        public string HotelId { get; set; }
        public ICollection<RoomTypeBedType> RoomTypeBedTypes { get; set; }
        public Hotel Hotel { get; set; }
    }
}
