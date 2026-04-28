using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using UserManagement.DTOs;
using UserManagement.Services.RegisterService;

namespace UserManagement.Controllers;


[Route("api/[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
    {
        var result = await authService.Login(request);

        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
    {
        return Ok(await authService.RegisterUser(userRegisterDto));
    }

    [HttpGet("CheckToken")]
    [Authorize]
    public IActionResult CheckToken()
    {
        return Ok("Avem Token " + User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
    }
}