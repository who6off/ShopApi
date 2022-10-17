using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelloApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(320, MinimumLength = 3)]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string SecondName { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
