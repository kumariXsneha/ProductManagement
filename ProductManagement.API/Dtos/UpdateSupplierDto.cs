using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.Dtos
{
    // INPUT for PUT: full update (no Id in body, it comes from the URL)
    public class UpdateSupplierDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public bool IsActive { get; set; }
    }
}