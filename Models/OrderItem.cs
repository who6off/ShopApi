using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelloApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        [JsonIgnore]
        public virtual Order? Order { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        public uint Amount { get; set; }
    }
}
