using System.ComponentModel.DataAnnotations;

namespace ShopApi.Data.Models
{
	public class Role
	{
		public int Id { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 1)]
		public string Name { get; set; } = string.Empty;

		public virtual ICollection<User> Users { get; set; }
	}
}
