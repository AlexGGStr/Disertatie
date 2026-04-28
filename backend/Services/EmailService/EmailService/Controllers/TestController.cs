using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers;

[Route("api/[controller]")]

public class TestController : Controller
{
    [HttpGet("Ping")]
    public IActionResult GetTest()
    {
        return Ok("Pong");
    }
}