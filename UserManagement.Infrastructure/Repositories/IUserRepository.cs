using UserManagement.Domain.Entities;

namespace UserManagement.Application.Repositories
{
    public interface IUserRepository
    {
        User? GetByLogin(string login);
        User? GetByLoginAndPasswordHash(string login, string passwordHash);
        IEnumerable<User> GetAllActive();
        IEnumerable<User> GetUsersOlderThan(int age);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        void SaveChanges();
    }
}
