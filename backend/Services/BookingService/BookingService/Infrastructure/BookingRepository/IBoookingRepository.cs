using BookingService.DTOs;
using BookingService.Models;

namespace BookingService.Infrastructure.BookingRepository;

public interface IBoookingRepository
{
    Task<bool> AreDatesAvailable(Guid propertyId, DateOnly from, DateOnly to);

    Task<Guid> InsertBooking(Booking booking);

    Task<bool> ConfirmBooking(Guid bookingId);

    Task<List<GetBookingsDTO>> GetBookingsForUser(Guid userId);

}