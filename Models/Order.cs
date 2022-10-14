using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required]
        public User Buyer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
