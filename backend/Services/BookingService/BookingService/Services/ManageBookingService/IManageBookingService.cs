using BookingService.DTOs;
using BookingService.Models;
using Shared;

namespace BookingService.Services.ManageBookingService;

public interface IManageBookingService
{
    Task<ServiceResponse<Guid>> CreateBooking(User user, CreateBookingDTO booking);

    Task<ServiceResponse<bool>> AreDatesAvailable(Guid propertyId, DateOnly start, DateOnly end);

    Task<ServiceResponse<List<GetBookingsDTO>>> GetBookings(Guid UserId);
}