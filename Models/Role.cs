using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopApi.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; } = String.Empty;

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
