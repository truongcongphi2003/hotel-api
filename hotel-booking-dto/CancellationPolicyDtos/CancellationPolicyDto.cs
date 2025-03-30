using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.CancellationPolicyDtos
{
    public class CancellationPolicyDto
    {
        public string RoomId { get; set; }
        public int? MinDaysBefore { get; set; }
        public Decimal? FeePercentage { get; set; }
        public Decimal? FixedFee { get; set; }
        public TimeSpan CutoffTime { get; set; }
        public bool IsRefundable { get; set; }
    }
}
