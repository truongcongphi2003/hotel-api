using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomDtos
{
    public class AddCancellationPolicyDto
    {
        public int? MinDaysBefore { get; set; }
        public decimal? FeePercentage { get; set; }
        public decimal? FixedFee { get; set; }
        public TimeSpan CutoffTime { get; set; }
        public bool IsRefundable { get; set; }
    }
}
