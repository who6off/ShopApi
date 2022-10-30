using HelloApi.Models;

namespace HelloApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetById(int id);
        public Task<User?> FindByEmail(string email);
        public Task<User[]> GetAll();
        public Task<User> Add(User user);
    }
}
