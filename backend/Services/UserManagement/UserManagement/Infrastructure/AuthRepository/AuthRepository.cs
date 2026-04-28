using Microsoft.EntityFrameworkCore;
using Shared;
using UserManagement.Infrastructure.UserKafkaProducer;
using UserManagement.Models;

namespace UserManagement.Infrastructure.IAuthRepository;

public class AuthRepository(UserManagementDbContext context, IUserEventProducer producer) : IAuthRepository
{
    public async Task<Guid> InsertUser(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        producer.SendUserRegisteredAsync(new UserRegisteredEvent { Email = user.Email, UserId = user.Id, UserName = user.Name });
        return user.Id;
    }

    public Task<string> GetUserById(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<string> MakeUserAdmin(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return "User not Found";

        if (user.Role == UserRole.Admin) return "User Already Admin";

        user.Role = UserRole.Admin;
        user.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        return "User Updated";
    }
}