using HelloApi.Models;

namespace HelloApi.Authorization
{
    public class RegistrationResponce
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
