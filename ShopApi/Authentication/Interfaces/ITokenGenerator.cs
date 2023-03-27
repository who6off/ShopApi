using ShopApi.Data.Models;

namespace ShopApi.Authentication.Interfaces
{
    public interface ITokenGenerator
    {
        public string Generate(User user);
    }
}
