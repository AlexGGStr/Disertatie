using System.ComponentModel.DataAnnotations;

namespace BookingService.DTOs;

public class CreateBookingDTO
{
    [Required]
    public Guid PropertyId { get; set; }

    [Required]
    public DateOnly From { get; set; }

    [Required]
    public DateOnly To { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Number of people must be at least 1.")]
    public int NoOfPeople { get; set; }
}