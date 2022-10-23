using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelloApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public bool IsRequestedForDelivery { get; set; } = false;

        [Required]
        public int BuyerId { get; set; }

        [Required]
        [JsonIgnore]
        public User Buyer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
