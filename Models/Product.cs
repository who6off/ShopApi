using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        public User Seller { get; set; }

        public string Image { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
