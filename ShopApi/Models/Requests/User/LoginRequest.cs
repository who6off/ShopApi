using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models.Requests.User
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
