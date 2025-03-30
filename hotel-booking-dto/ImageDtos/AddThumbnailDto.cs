using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ImageDtos
{
    public class AddThumbnailDto
    {
        public IFormFile Thumbnail { get; set; }
    }
}
