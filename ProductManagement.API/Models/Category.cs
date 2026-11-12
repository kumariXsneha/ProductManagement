using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductManagement.API.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Navigation: one category → many products
        [JsonIgnore]   // don't include in JSON (explained below)
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}