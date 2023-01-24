using ShopApi.Models;

namespace ShopApi.Authentication
{
    public interface ITokenGenerator
    {
        public string Generate(User user);
    }
}
