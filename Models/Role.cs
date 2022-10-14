using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; } = String.Empty;

        public ICollection<User> Users { get; set; }
    }
}
