using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using UserManagement.DTOs;
using UserManagement.Infrastructure.IAuthRepository;
using UserManagement.Models;

namespace UserManagement.Services.RegisterService;

public class AuthService(IOptions<JwtSettings> jwtSettings, IAuthRepository authRepository) : IAuthService
{
    public async Task<ServiceResponse<string>> Login(UserLoginDTO user)
    {
        var response = new ServiceResponse<string>();
        var userToLogin = await authRepository.GetUserByEmail(user.Email);
        
        if (userToLogin == null)
        {
            response.Success = false;
            response.Message = "User not Found";
        }
        else if (!VerifyPasswordHash(user.Password, userToLogin.PasswordHash, userToLogin.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong Password";
        }
        else
        {
            response.Data = GenerateJwtToken(userToLogin);
            response.Message = "Login Successful";
        }
        
        return response;
    }

    public async Task<ServiceResponse<Guid>> RegisterUser(UserRegisterDTO user)
    {
        if (await authRepository.GetUserByEmail(user.Email) != null)
        {
            return new ServiceResponse<Guid> { Success = false, Message = "User Exists" };
        }
        
        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

        return new ServiceResponse<Guid>
        {
            Data = await authRepository.InsertUser(new User
            {
                Name = user.Name,
                Email = user.Email,
                Role = UserRole.User,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            })
        };
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }

    private string GenerateJwtToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = Encoding.UTF8.GetBytes(jwtSettings.Value.Key);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Value.Issuer,
            audience: jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}