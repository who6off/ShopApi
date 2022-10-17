using HelloApi.Models;

namespace HelloApi.Authentication
{
    public class RegistrationResponce
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
