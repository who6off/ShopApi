using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public bool IsForAdults { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
