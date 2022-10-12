using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public Category Category { get; set; }
        public string? Image { get; set; }
    }
}
