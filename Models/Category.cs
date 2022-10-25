using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
