using UserManagement.Models;

namespace UserManagement.Infrastructure.IAuthRepository;

public interface IAuthRepository
{
    Task<Guid> InsertUser(User user);

    Task<string> GetUserById(Guid userId);

    Task<User?> GetUserByEmail(string email);

    Task<string> MakeUserAdmin(Guid userId);
}