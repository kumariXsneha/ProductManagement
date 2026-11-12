using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the student's name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be 2 to 50 characters")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please enter a course")]
        public string Course { get; set; } = "";

        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
        [Display(Name = "Marks (out of 100)")]
        public int Marks { get; set; }

        public string? City { get; set; }
    }
}