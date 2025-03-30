using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_models
{
    public class AdministrativeRegion
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string NameEn { get; set; }

        [MaxLength(255)]
        public string CodeName { get; set; }

        [MaxLength(255)]
        public string CodeNameEn { get; set; }

        public ICollection<Province> Provinces { get; set; }
    }
}