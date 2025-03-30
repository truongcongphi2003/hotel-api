using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ReviewDtos
{
    public class ReviewUpdateDto
    {
        public string Rating { get; set; }
        [DataType(DataType.Text)]
        public string Comment { get; set; }
    }
}
