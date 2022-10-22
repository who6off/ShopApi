using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HelloApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [JsonIgnore]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public uint Amount { get; set; }
    }
}
