using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class RoomTypeBedType
    {
        public string RoomTypeId { get; set; }
        public string BedTypeId { get; set; }
        public int Quantity { get; set; }
        public RoomType RoomType { get; set; }
        public BedType BedType { get; set; }
    }
}
