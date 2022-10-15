using HelloApi.Models;

namespace HelloApi.Authorization
{
    public interface ITokenGenerator
    {
        public string Generate(User user);
    }
}
