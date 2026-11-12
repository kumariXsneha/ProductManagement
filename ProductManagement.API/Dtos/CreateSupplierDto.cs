using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.Dtos
{
    // INPUT: what the client is ALLOWED to send when creating
    public class CreateSupplierDto
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

        // ❌ No Id: the database creates it
        // ❌ No CreatedDate: the server sets it
        // ❌ No IsActive: new suppliers are always active
    }
}