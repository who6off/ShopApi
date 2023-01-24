using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopApi.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public bool IsRequestedForDelivery { get; set; } = false;

        public int? BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        [JsonIgnore]
        public virtual User? Buyer { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
