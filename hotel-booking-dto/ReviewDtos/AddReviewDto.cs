using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ReviewDtos
{
    public class AddReviewDto
    {
        public double Rating;
        [DataType(DataType.Text)]
        public string Commment;
        public string HotelId { get; set; }
    }
}
