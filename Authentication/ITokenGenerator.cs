using HelloApi.Models;

namespace HelloApi.Authentication
{
    public interface ITokenGenerator
    {
        public string Generate(User user);
    }
}
