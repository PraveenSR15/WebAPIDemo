using System.ComponentModel.DataAnnotations;
using WebApp.Models.Validations;

namespace WebApp.Models
{
    public class Shirt
    {
        public int ShirtId { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        [Shirt_EnsureCorrectSize]
        public char? Size { get; set; }

        [Required]
        public string? Gender { get; set; }
        public decimal? Price { get; set; }
    }
}
