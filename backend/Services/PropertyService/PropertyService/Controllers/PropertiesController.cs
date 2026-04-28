using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.DTOs;
using PropertyService.Models;
using PropertyService.Services.PropertyService;

namespace PropertyService.Controllers;

[Route("api/[controller]")]
public class PropertiesController(IPropertyManagementService _propertyManagementService) : Controller
{
    [HttpGet("private")]
    [Authorize]
    public IActionResult GetPrivateData()
    {
        return Ok(new { message = "You accessed a protected endpoint!", user = User.Identity?.Name });
    }

    [HttpPost("addProperty")]
    [Authorize]
    public async Task<IActionResult> AddProperty([FromForm] AddPropertyDTO propertyDto)
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        
        var result = await _propertyManagementService
            .AddProperty(new PropertyOwner {Id = userId, Name = userName, Email = email}, propertyDto);
        
        return Ok(result);
    }

    [HttpGet("bookings")]
    [Authorize]
    public async Task<IActionResult> GetBookingsForProperty(Guid propertyId)
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        var result = await _propertyManagementService.GetBookingsForProperty(userId, propertyId);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("searchProperties")]
    public async Task<IActionResult> SearchProperties([FromQuery] SearchPropertiesDto search)
    {
        var result = await _propertyManagementService.SearchPropertiesByLocationAndDatesAsync(search);

        return Ok(result);
    }

    [HttpGet("GetById/{propertyId}")]
    public async Task<IActionResult> GetPropertyById(Guid propertyId)
    {
        var result = await _propertyManagementService.GetPropertyById(propertyId);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCity(string city, bool onlyCity = true)
    {
        return Ok(await _propertyManagementService.SearchCity(city, onlyCity));
    }

    [HttpGet("getMyProperties")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetProperties()
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return Ok(await _propertyManagementService.GetPropertiesForOwnerAsync(userId));
    }
}