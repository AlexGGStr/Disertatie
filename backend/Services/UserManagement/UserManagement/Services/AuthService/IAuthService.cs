using Shared;
using UserManagement.DTOs;

namespace UserManagement.Services.RegisterService;

public interface IAuthService
{
    Task<ServiceResponse<string>> Login(UserLoginDTO user);

    Task<ServiceResponse<Guid>> RegisterUser(UserRegisterDTO user);
}