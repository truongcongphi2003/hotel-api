using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ImageDtos
{
    public class AddImageDto
    {
        public IEnumerable<IFormFile>? Files { get; set; }
        public List<string>? ImageRemoveIds { get; set; }
    }
}
