using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    [Index(nameof(AdministrativeRegionId))]
    [Index(nameof(AdministrativeUnitId))]
    public class Province
    {
        [Key, MaxLength(20)]
        public string Code { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string NameEn { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [Required, MaxLength(255)]
        public string FullNameEn { get; set; }

        [MaxLength(255)]
        public string CodeName { get; set; }

        public int? AdministrativeUnitId { get; set; }
        [ForeignKey("AdministrativeUnitId")]
        public AdministrativeUnit AdministrativeUnit { get; set; }

        public int? AdministrativeRegionId { get; set; }
        [ForeignKey("AdministrativeRegionId")]
        public AdministrativeRegion AdministrativeRegion { get; set; }

        public ICollection<District> Districts { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}
