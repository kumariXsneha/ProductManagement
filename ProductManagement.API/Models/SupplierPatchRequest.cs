using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.Models
{
    // Used ONLY for PATCH: every field is optional (null = "don't change")
    public class SupplierPatchRequest
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public bool? IsActive { get; set; }
    }
}