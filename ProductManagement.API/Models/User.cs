using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // We NEVER store the real password, only its hash
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // "Admin" or "User"
        [Required, MaxLength(20)]
        public string Role { get; set; } = "User";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}