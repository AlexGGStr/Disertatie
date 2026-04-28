using System.Security.Claims;
using BookingService.DTOs;
using BookingService.Models;
using BookingService.Services.ManageBookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Route("api/[controller]")]
public class BookingController(IManageBookingService _bookingService) : Controller
{
    [HttpGet("private")]
    [Authorize]
    public IActionResult GetPrivateData()
    {
        return Ok(new { message = "You accessed a protected endpoint!", user = User.Identity?.Name });
    }
    
    [HttpPost("addBooking")]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO bookingDto)
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

        var response = await _bookingService.CreateBooking(new User { Id = userId, Email = email, Name = userName }, bookingDto);

        return Ok(response);
    }

    [HttpGet("areDatesAvailable")]
    public async Task<IActionResult> CheckDates(Guid propertyId, DateOnly startDate, DateOnly endDate)
    {
        return Ok(await _bookingService.AreDatesAvailable(propertyId, startDate, endDate));
    }

    [HttpGet("myBookings")]
    [Authorize]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        
        return Ok(await _bookingService.GetBookings(userId));
    }
}